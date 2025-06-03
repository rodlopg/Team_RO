using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

// Script para el enemigo secundario - hereda la mayoría del comportamiento del enemigo principal
public class EnemySecondary : MonoBehaviour
{
    // Enemy states - igual que el enemigo principal
    public enum EnemyState
    {
        Patrolling,
        Investigating,
        Idle,
        Pursuing,
        Attacking
    }

    // Main variables
    private List<Transform> wayPoints = new List<Transform>();
    private int waypointIndex = 0;
    private NavMeshAgent navMeshAgent;
    private AudioSource audioSource;
    private string microphoneDevice;
    private float currentVolume = 0f;
    private Vector3 lastNoisePosition;
    
    // Target tracking variables
    private Transform currentTarget = null;
    private float currentTargetScore = 0f;
    private bool currentTargetIsGlobal = false;
    public float scoreImprovementThreshold = 0.01f;
    
    // State variables
    private EnemyState currentState = EnemyState.Patrolling;
    private float idleTimer = 0f;
    public float idleDuration = 3.0f;
    public float noiseThreshold = 0.1f;
    
    // Death sentence mode tracking - RESTAURADO IGUAL QUE EL PRINCIPAL
    private bool inDeathSentenceMode = false;
    private Transform targetPlayer = null; // Only used in death sentence mode
    
    // Animation references
    private Animator animator;
    
    // Animation parameters
    private readonly string ANIM_IS_WALKING = "IsWalking";
    private readonly string ANIM_IS_RUNNING = "IsRunning";
    private readonly string ANIM_IS_IDLE = "IsIdle";
    private readonly string ANIM_ATTACK = "Attack";
    
    // Variables específicas del enemigo secundario
    private float lifetime = 60f;         // Tiempo de vida del enemigo secundario
    private float lifetimeRemaining;      // Contador de tiempo restante
    private RoomManager roomManager;      // Referencia al RoomManager
    private RoomManager.Room assignedRoom; // Cuarto asignado
    
    // Debug
    public bool showDebug = true;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        // Microphone setup - Usar el micrófono compartido del enemigo principal
        if (EnemyNavigation.sharedMicrophoneAudioSource != null)
        {
            // Usar el AudioSource compartido del enemigo principal
            audioSource = EnemyNavigation.sharedMicrophoneAudioSource;
            microphoneDevice = Microphone.devices[0]; // Solo para referencia
            
            if (showDebug) Debug.Log("Enemigo secundario usando micrófono compartido");
        }
        else
        {
            Debug.LogWarning("No shared microphone available. Make sure main enemy is initialized first.");
        }
        
        // Inicializar tiempo de vida
        lifetimeRemaining = lifetime;
        
        // Comenzar en estado de patrullaje
        StartPatrolling();
    }

    void Update()
    {
        // Decrementar tiempo de vida
        lifetimeRemaining -= Time.deltaTime;
        
        // Si el tiempo de vida ha terminado, desaparecer
        if (lifetimeRemaining <= 0f)
        {
            DespawnEnemy();
            return;
        }
        
        // Calcular volumen actual
        currentVolume = CalculateVolume();
        
        // Lógica basada en el estado actual
        switch (currentState)
        {
            case EnemyState.Patrolling:
                UpdatePatrolling();
                break;
                
            case EnemyState.Investigating:
                UpdateInvestigating();
                break;
                
            case EnemyState.Idle:
                UpdateIdle();
                break;
                
            case EnemyState.Pursuing:
                UpdatePursuing();
                break;
                
            case EnemyState.Attacking:
                // La animación de ataque se maneja con eventos
                break;
        }
    }
    
    #region State Update Methods
    
    private void UpdatePatrolling()
    {
        // Verificar si ha llegado al waypoint actual
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            // Avanzar al siguiente waypoint
            waypointIndex = (waypointIndex + 1) % wayPoints.Count;
            navMeshAgent.SetDestination(wayPoints[waypointIndex].position);
            if (showDebug) Debug.Log("Moving to next waypoint: " + waypointIndex);
        }
        
        // Si detecta ruido por encima del umbral, investigar
        if (currentVolume > noiseThreshold)
        {
            // Evaluar fuentes de sonido con priorización
            EvaluateNoiseSourcesWithPriority();
            
            // Si encontramos un objetivo válido, investigar
            if (currentTarget != null)
            {
                StartInvestigating();
            }
        }
    }
    
    private void UpdateInvestigating()
    {
        // Si está cerca de la posición del ruido
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            // Al llegar a la posición del ruido, entrar en modo idle
            StartIdle();
        }
        
        // Comprobar nuevos ruidos mientras investiga
        if (currentVolume > noiseThreshold)
        {
            // Evaluar fuentes de sonido con lógica de priorización y umbral de mejora
            EvaluateNoiseSourcesWithPriority(scoreImprovementThreshold);
            
            // Si hay un nuevo objetivo válido que supera el umbral, actualizamos destino
            if (currentTarget != null)
            {
                navMeshAgent.SetDestination(lastNoisePosition);
            }
        }
    }
    
    private void UpdateIdle()
    {
        // Incrementar contador de idle
        idleTimer += Time.deltaTime;
        
        // Si el tiempo de idle ha terminado
        if (idleTimer >= idleDuration)
        {
            // Volver a la ruta de patrulla
            ReturnToNearestWaypoint();
        }
        
        // Si escucha ruido en modo idle - IGUAL QUE EL PRINCIPAL
        if (currentVolume > noiseThreshold)
        {
            // Evaluar fuentes de sonido con priorización
            EvaluateNoiseSourcesWithPriority();
            
            // Si encontramos un objetivo válido
            if (currentTarget != null)
            {
                // Verificar si el objetivo es un jugador para death sentence
                if (!currentTargetIsGlobal && currentTarget.CompareTag("Player"))
                {
                    if (showDebug) Debug.Log("In idle: Player is making noise, activating death sentence");
                    targetPlayer = currentTarget;
                    inDeathSentenceMode = true;
                    StartDeathSentence();
                }
                else
                {
                    // Si es GlobalSoundSource, investigar
                    if (showDebug) Debug.Log("In idle: Investigating noise source");
                    StartInvestigating();
                }
            }
        }
    }
    
    private void UpdatePursuing()
    {
        // If in death sentence mode with a specific player target - IGUAL QUE EL PRINCIPAL
        if (inDeathSentenceMode && targetPlayer != null)
        {
            // Directly pursue the player
            navMeshAgent.SetDestination(targetPlayer.position);
            
            // Check if close enough to attack
            float distanceToTarget = Vector3.Distance(transform.position, targetPlayer.position);
            if (distanceToTarget < 1.0f)
            {
                // Attack when close enough
                StartAttacking();
            }
            
            // Death sentence mode: focus only on this player
            return;
        }
        
        // Regular pursuit logic - just going to a noise position
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            // Has reached the pursuit position, enter idle mode
            StartIdle();
        }
        
        // Solo verificar mejores fuentes de sonido si no está en modo death sentence
        if (!inDeathSentenceMode && currentVolume > noiseThreshold)
        {
            // Usar umbral más alto durante persecución para estabilidad
            EvaluateNoiseSourcesWithPriority(scoreImprovementThreshold * 2.0f);
            
            // Si hay un nuevo objetivo válido que supera el umbral más alto, actualizamos
            if (currentTarget != null)
            {
                navMeshAgent.SetDestination(lastNoisePosition);
            }
        }
    }
    
    #endregion
    
    #region State Transition Methods
    
    private void StartPatrolling()
    {
        currentState = EnemyState.Patrolling;
        
        // Reset target tracking - IGUAL QUE EL PRINCIPAL
        inDeathSentenceMode = false;
        targetPlayer = null;
        currentTarget = null;
        currentTargetScore = 0f;
        currentTargetIsGlobal = false;
        
        // Configurar animación
        SetAnimationState(true, false, false);
        
        if (wayPoints.Count > 0)
        {
            navMeshAgent.speed = 2.0f; // Velocidad normal de caminata
            navMeshAgent.SetDestination(wayPoints[waypointIndex].position);
            if (showDebug) Debug.Log("Starting patrol at waypoint: " + waypointIndex);
        }
    }
    
    private void StartInvestigating()
    {
        currentState = EnemyState.Investigating;
        
        // Configurar animación de correr
        SetAnimationState(false, true, false);
        
        navMeshAgent.speed = 3.5f; // Velocidad más rápida para correr
        navMeshAgent.SetDestination(lastNoisePosition);
        
        if (currentTargetIsGlobal)
            Debug.Log("Investigating GlobalSoundSource at: " + lastNoisePosition + " with score: " + currentTargetScore);
        else
            Debug.Log("Investigating player noise at: " + lastNoisePosition + " with score: " + currentTargetScore);
    }
    
    private void StartIdle()
    {
        currentState = EnemyState.Idle;
        
        // Configurar animación de idle
        SetAnimationState(false, false, true);
        
        navMeshAgent.ResetPath();
        idleTimer = 0f;
        
        if (showDebug) Debug.Log("Entering idle state");
    }
    
    private void StartDeathSentence()
    {
        currentState = EnemyState.Pursuing;
        
        // Configurar animación de correr (persecución más rápida)
        SetAnimationState(false, true, false);
        
        // Maximum speed for death sentence
        navMeshAgent.speed = 5.0f; 
        
        // Debug message
        if (showDebug) Debug.Log("DEATH SENTENCE MODE ACTIVATED - Pursuing player until caught!");
        
        // Set initial destination
        if (targetPlayer != null)
        {
            navMeshAgent.SetDestination(targetPlayer.position);
        }
    }
    
    private void StartAttacking()
    {
        currentState = EnemyState.Attacking;
        
        // Activar animación de ataque
        if (animator != null)
        {
            animator.SetTrigger(ANIM_ATTACK);
        }
        
        navMeshAgent.ResetPath();
        
        // Después de la animación de ataque, volver a patrullar
        StartCoroutine(ReturnToPatrolAfterAttack(1.5f)); // Ajustar según la duración de la animación
    }
    
    private IEnumerator ReturnToPatrolAfterAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToNearestWaypoint();
    }
    
    private void ReturnToNearestWaypoint()
    {
        // Clear all targets - IGUAL QUE EL PRINCIPAL
        inDeathSentenceMode = false;
        targetPlayer = null;
        currentTarget = null;
        currentTargetScore = 0f;
        currentTargetIsGlobal = false;
        
        if (wayPoints.Count == 0)
        {
            currentState = EnemyState.Patrolling;
            return;
        }
        
        // Encontrar el waypoint más cercano
        int nearestIndex = 0;
        float minDistance = float.MaxValue;
        
        for (int i = 0; i < wayPoints.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, wayPoints[i].position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestIndex = i;
            }
        }
        
        // Set index and route - IGUAL QUE EL PRINCIPAL
        waypointIndex = nearestIndex;
        
        // Volver a estado de patrullaje
        StartPatrolling();
    }
    
    #endregion
    
    #region Helper Methods
    
    // Método para configurar estados de animación
    private void SetAnimationState(bool walking, bool running, bool idle)
    {
        if (animator != null)
        {
            animator.SetBool(ANIM_IS_WALKING, walking);
            animator.SetBool(ANIM_IS_RUNNING, running);
            animator.SetBool(ANIM_IS_IDLE, idle);
        }
    }
    
    // ACTUALIZADO: Detecta BOTH GlobalSoundSources and Players - IGUAL QUE EL PRINCIPAL
    private void EvaluateNoiseSourcesWithPriority(float improvementThreshold = 0f)
    {
        bool foundBetterTarget = false;
        
        // Primero evaluar GlobalSoundSources
        Transform bestGlobalSource = null;
        float bestGlobalScore = 0f;
        
        GlobalSoundSource[] soundSources = FindObjectsOfType<GlobalSoundSource>();
        
        foreach (GlobalSoundSource source in soundSources)
        {
            if (source.isActive)
            {
                // Verificar si la fuente está en el mismo cuarto (solo para enemigo secundario)
                if (!IsSourceInAssignedRoom(source.transform.position))
                {
                    if (showDebug) Debug.Log("GlobalSoundSource fuera del cuarto asignado");
                    continue;
                }
                
                float distance = Vector3.Distance(transform.position, source.transform.position);
                if (distance < 0.1f) distance = 0.1f;
                
                float score = source.volumeIntensity * (1.0f / distance);
                
                if (bestGlobalSource == null || score > bestGlobalScore)
                {
                    bestGlobalScore = score;
                    bestGlobalSource = source.transform;
                }
            }
        }
        
        // Si encontramos un GlobalSoundSource activo
        if (bestGlobalSource != null)
        {
            // Si no teníamos objetivo previo O es mejor que nuestro objetivo actual según el umbral
            // O teníamos un jugador como objetivo (siempre priorizar GlobalSoundSource sobre jugador)
            if (currentTarget == null || 
                bestGlobalScore > currentTargetScore * (1 + improvementThreshold) ||
                !currentTargetIsGlobal)
            {
                currentTarget = bestGlobalSource;
                currentTargetScore = bestGlobalScore;
                currentTargetIsGlobal = true;
                lastNoisePosition = bestGlobalSource.position;
                foundBetterTarget = true;
                
                if (showDebug)
                    Debug.Log("Secondary enemy found better GlobalSoundSource with score: " + bestGlobalScore);
            }
        }
        // Si NO hay GlobalSoundSources activos, evaluar jugadores - IGUAL QUE EL PRINCIPAL
        else
        {
            Transform bestPlayer = null;
            float bestPlayerScore = 0f;
            
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            
            foreach (GameObject player in players)
            {
                // Verificar si el jugador está en el mismo cuarto (solo para enemigo secundario)
                if (!IsSourceInAssignedRoom(player.transform.position))
                {
                    if (showDebug) Debug.Log("Player fuera del cuarto asignado");
                    continue;
                }
                    
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < 0.1f) distance = 0.1f;
                
                float score = currentVolume * (1.0f / distance);
                
                if (bestPlayer == null || score > bestPlayerScore)
                {
                    bestPlayerScore = score;
                    bestPlayer = player.transform;
                }
            }
            
            // Si encontramos un jugador haciendo ruido
            if (bestPlayer != null)
            {
                // Solo cambiar si no teníamos objetivo previo o si es significativamente mejor
                if (currentTarget == null || bestPlayerScore > currentTargetScore * (1 + improvementThreshold))
                {
                    currentTarget = bestPlayer;
                    currentTargetScore = bestPlayerScore;
                    currentTargetIsGlobal = false;
                    lastNoisePosition = bestPlayer.position;
                    foundBetterTarget = true;
                    
                    if (showDebug)
                        Debug.Log("Secondary enemy found better player source with score: " + bestPlayerScore);
                }
            }
        }
        
        // Si no encontramos un mejor objetivo, mantener el actual
        if (!foundBetterTarget && currentTarget == null)
        {
            // No se encontró ninguna fuente de sonido en el cuarto
            if (showDebug)
                Debug.Log("No valid noise sources found in assigned room");
        }
    }
    
    // Verifica si una posición está dentro del cuarto asignado
    private bool IsSourceInAssignedRoom(Vector3 position)
    {
        // Si no hay cuarto asignado, permitir cualquier posición
        if (assignedRoom == null || roomManager == null)
            return true;
            
        // Comprobar si la posición está más cerca de un waypoint de este cuarto que de cualquier otro
        float closestDistanceInRoom = float.MaxValue;
        
        // Encontrar la distancia al waypoint más cercano en el cuarto asignado
        foreach (Transform waypoint in assignedRoom.waypoints)
        {
            float distance = Vector3.Distance(position, waypoint.position);
            if (distance < closestDistanceInRoom)
            {
                closestDistanceInRoom = distance;
            }
        }
        
        // Comparar con otros cuartos
        foreach (RoomManager.Room otherRoom in roomManager.rooms)
        {
            if (otherRoom == assignedRoom)
                continue;
                
            foreach (Transform waypoint in otherRoom.waypoints)
            {
                float distance = Vector3.Distance(position, waypoint.position);
                // Si hay un waypoint en otro cuarto más cercano, la posición no está en nuestro cuarto
                if (distance < closestDistanceInRoom)
                {
                    return false;
                }
            }
        }
        
        // Si llegamos aquí, la posición está más cerca de un waypoint de nuestro cuarto
        return true;
    }
    
    // ACTUALIZADO: Función para calcular volumen - IGUAL QUE EL PRINCIPAL
    private float CalculateVolume()
    {
        // First check the microphone input (original method)
        float micVolume = 0f;
        
        if (audioSource != null)
        {
            float[] samples = new float[256];
            audioSource.GetOutputData(samples, 0);
            
            float sum = 0f;
            foreach (float sample in samples)
            {
                sum += sample * sample;
            }
            
            micVolume = Mathf.Sqrt(sum / samples.Length);
        }
        
        // Then check global sound sources
        float globalVolume = 0f;
        
        // Buscar fuentes de sonido global activas en el cuarto asignado
        GlobalSoundSource[] soundSources = FindObjectsOfType<GlobalSoundSource>();
        foreach (GlobalSoundSource source in soundSources)
        {
            if (source.isActive && IsSourceInAssignedRoom(source.transform.position))
            {
                globalVolume += source.volumeIntensity;
            }
        }
        
        // Return the greater of the two values
        return Mathf.Max(micVolume, globalVolume);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (showDebug) Debug.Log("You died!");
            
            // Clear the target player - IGUAL QUE EL PRINCIPAL
            targetPlayer = null;
            inDeathSentenceMode = false;
            
            // Animación de ataque al colisionar con el jugador
            StartAttacking();
        }
    }
    
    #endregion
    
    #region Secondary Enemy Specific Methods
    
    // Configurar el RoomManager
    public void SetRoomManager(RoomManager manager)
    {
        roomManager = manager;
    }
    
    // Configurar el cuarto asignado
    public void SetAssignedRoom(RoomManager.Room room)
    {
        assignedRoom = room;
    }
    
    // Configurar tiempo de vida
    public void SetLifetime(float duration)
    {
        lifetime = duration;
        lifetimeRemaining = duration;
    }
    
    // Configurar waypoints
    public void SetWaypoints(List<Transform> points)
    {
        wayPoints = points;
    }
    
    // Desaparecer el enemigo cuando se agota el tiempo
    private void DespawnEnemy()
    {
        if (roomManager != null && assignedRoom != null)
        {
            // Notificar al RoomManager
            roomManager.NotifySecondaryEnemyDespawned(gameObject, assignedRoom);
        }
        
        // Destruir el objeto
        Destroy(gameObject);
    }
    
    #endregion
}
