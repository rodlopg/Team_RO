using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EnemyNavigation : MonoBehaviour
{
    // Enemy states
    public enum EnemyState
    {
        Patrolling,
        Investigating,
        Idle,
        Pursuing,
        Attacking
    }

    // Main variables
    public List<Transform> wayPoint;
    public int waypointIndex = 0;
    private NavMeshAgent navMeshAgent;
    private AudioSource audioSource;
    private string microphoneDevice;
    private float currentVolume = 0f;
    private Vector3 lastNoisePosition;
    
    // NUEVO: Variables estáticas para micrófono compartido
    public static AudioSource sharedMicrophoneAudioSource = null;
    public static bool isMicrophoneInitialized = false;
    
    // Target tracking variables
    private Transform currentTarget = null;
    private float currentTargetScore = 0f;
    private bool currentTargetIsGlobal = false;
    public float scoreImprovementThreshold = 0.01f; // Nuevo sonido debe ser 1% mejor para cambiar objetivo
    
    // State variables
    private EnemyState currentState = EnemyState.Patrolling;
    private float idleTimer = 0f;
    public float idleDuration = 3.0f;
    public float noiseThreshold = 0.1f;
    
    // NUEVA: Referencia al RoomManager para obtener el rango de detección
    private RoomManager roomManager;
    private float detectionRange = 20f; // Valor por defecto si no hay RoomManager
    
    // Death sentence mode tracking
    private bool inDeathSentenceMode = false;
    private Transform targetPlayer = null; // Only used in death sentence mode
    
    // Animation references
    private Animator animator;
    
    // Animation parameters
    private readonly string ANIM_IS_WALKING = "IsWalking";
    private readonly string ANIM_IS_RUNNING = "IsRunning";
    private readonly string ANIM_IS_IDLE = "IsIdle";
    private readonly string ANIM_ATTACK = "Attack";
    
    // Debug
    public bool showDebug = true;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        // Buscar el RoomManager en la escena
        roomManager = FindObjectOfType<RoomManager>();
        if (roomManager != null)
        {
            detectionRange = roomManager.mainEnemyDetectionRange;
            if (showDebug) Debug.Log("Enemy using RoomManager detection range: " + detectionRange);
        }
        else
        {
            Debug.LogWarning("No se encontró RoomManager. Usando rango por defecto: " + detectionRange);
        }
        
        // Microphone setup - Solo el enemigo principal inicializa el micrófono
        if (Microphone.devices.Length > 0 && !isMicrophoneInitialized)
        {
            microphoneDevice = Microphone.devices[0];
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = Microphone.Start(microphoneDevice, true, 10, 44100);
            audioSource.loop = true;
            while (!(Microphone.GetPosition(microphoneDevice) > 0)) { }
            audioSource.Play();
            
            // Hacer disponible para otros enemigos
            sharedMicrophoneAudioSource = audioSource;
            isMicrophoneInitialized = true;
            
            if (showDebug) Debug.Log("Enemigo principal inicializó el micrófono");
        }
        else
        {
            Debug.LogWarning("No microphones connected or already initialized by main enemy.");
        }
        
        // Start in patrol state
        StartPatrolling();
    }

    void Update()
    {
        // Actualizar rango de detección del RoomManager si está disponible
        if (roomManager != null)
        {
            detectionRange = roomManager.mainEnemyDetectionRange;
        }
        
        // Constant volume calculation
        currentVolume = CalculateVolume();
        
        // Logic based on current state
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
                // Attack animation is handled by events
                break;
        }
    }
    
    #region State Update Methods
    
    private void UpdatePatrolling()
    {
        // Check if reached current waypoint
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            // Advance to next waypoint
            waypointIndex = (waypointIndex + 1) % wayPoint.Count;
            navMeshAgent.SetDestination(wayPoint[waypointIndex].position);
        }
        
        // If detects noise above threshold, investigate
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
        // If near the noise position
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            // When reaching the noise position, enter idle mode
            StartIdle();
        }
        
        // Check for new noise while investigating
        if (currentVolume > noiseThreshold)
        {
            // Evaluar fuentes de sonido con lógica de priorización y umbral de mejora
            // Durante la investigación, usamos un umbral para evitar cambios constantes
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
        // Increment idle counter
        idleTimer += Time.deltaTime;
        
        // If idle time is over
        if (idleTimer >= idleDuration)
        {
            // Return to patrol route
            ReturnToNearestWaypoint();
        }
        
        // If hears noise while in idle mode
        if (currentVolume > noiseThreshold)
        {
            // Evaluar fuentes de sonido con priorización desde estado idle
            EvaluateNoiseSourcesWithPriority();
            
            // Si encontramos un objetivo válido
            if (currentTarget != null)
            {
                // Verificar si el objetivo es un jugador para death sentence
                if (!currentTargetIsGlobal && currentTarget.CompareTag("Player"))
                {
                    Debug.Log("In idle: Player is making noise, activating death sentence");
                    targetPlayer = currentTarget;
                    inDeathSentenceMode = true;
                    StartDeathSentence();
                }
                else
                {
                    // Si es GlobalSoundSource, investigar
                    Debug.Log("In idle: Investigating noise source");
                    StartInvestigating();
                }
            }
        }
    }
    
    private void UpdatePursuing()
    {
        // If in death sentence mode with a specific player target
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
            // Un sonido debe ser MUCHO mejor para cambiar durante persecución
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
        
        // Reset target tracking
        inDeathSentenceMode = false;
        targetPlayer = null;
        currentTarget = null;
        currentTargetScore = 0f;
        currentTargetIsGlobal = false;
        
        // Set animation
        SetAnimationState(true, false, false);
        
        if (wayPoint.Count > 0)
        {
            navMeshAgent.speed = 2.0f; // Normal walking speed
            navMeshAgent.SetDestination(wayPoint[waypointIndex].position);
        }
    }
    
    private void StartInvestigating()
    {
        currentState = EnemyState.Investigating;
        
        // Set animation to running
        SetAnimationState(false, true, false);
        
        navMeshAgent.speed = 3.5f; // Faster running speed
        navMeshAgent.SetDestination(lastNoisePosition);
        
        if (currentTargetIsGlobal)
            Debug.Log("Investigating GlobalSoundSource at: " + lastNoisePosition + " with score: " + currentTargetScore);
        else
            Debug.Log("Investigating player noise at: " + lastNoisePosition + " with score: " + currentTargetScore);
    }
    
    private void StartIdle()
    {
        currentState = EnemyState.Idle;
        
        // Set animation to idle
        SetAnimationState(false, false, true);
        
        navMeshAgent.ResetPath();
        idleTimer = 0f;
        
        Debug.Log("Entering idle state");
    }
    
    private void StartDeathSentence()
    {
        currentState = EnemyState.Pursuing;
        
        // Set animation to running (fastest pursuit)
        SetAnimationState(false, true, false);
        
        // Maximum speed for death sentence
        navMeshAgent.speed = 5.0f; 
        
        // Debug message
        Debug.Log("DEATH SENTENCE MODE ACTIVATED - Pursuing player until caught!");
        
        // Set initial destination
        if (targetPlayer != null)
        {
            navMeshAgent.SetDestination(targetPlayer.position);
        }
    }
    
    private void StartAttacking()
    {
        currentState = EnemyState.Attacking;
        
        // Trigger attack animation
        if (animator != null)
        {
            animator.SetTrigger(ANIM_ATTACK);
        }
        
        navMeshAgent.ResetPath();
        
        // After attack animation, return to patrol
        StartCoroutine(ReturnToPatrolAfterAttack(1.5f)); // Adjust time based on attack animation length
    }
    
    private IEnumerator ReturnToPatrolAfterAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToNearestWaypoint();
    }
    
    private void ReturnToNearestWaypoint()
    {
        // Clear all targets
        inDeathSentenceMode = false;
        targetPlayer = null;
        currentTarget = null;
        currentTargetScore = 0f;
        currentTargetIsGlobal = false;
        
        if (wayPoint.Count == 0)
        {
            currentState = EnemyState.Patrolling;
            return;
        }
        
        // Find nearest waypoint
        int nearestIndex = 0;
        float minDistance = float.MaxValue;
        
        for (int i = 0; i < wayPoint.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, wayPoint[i].position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestIndex = i;
            }
        }
        
        // Set index and route
        waypointIndex = nearestIndex;
        StartPatrolling();
    }
    
    #endregion
    
    #region Helper Methods
    
    // Method to set all animation states
    private void SetAnimationState(bool walking, bool running, bool idle)
    {
        if (animator != null)
        {
            animator.SetBool(ANIM_IS_WALKING, walking);
            animator.SetBool(ANIM_IS_RUNNING, running);
            animator.SetBool(ANIM_IS_IDLE, idle);
        }
    }
    
    // ACTUALIZADO: Método que usa el rango de detección del RoomManager
    private void EvaluateNoiseSourcesWithPriority(float improvementThreshold = 0f)
    {
        bool foundBetterTarget = false;
        
        // Primero evaluar GlobalSoundSources DENTRO DEL RANGO
        Transform bestGlobalSource = null;
        float bestGlobalScore = 0f;
        
        GlobalSoundSource[] soundSources = FindObjectsOfType<GlobalSoundSource>();
        
        foreach (GlobalSoundSource source in soundSources)
        {
            if (source.isActive)
            {

                // ignora la musica con tag ignorar
                if (source.CompareTag("ignorar"))
                {
                    if (showDebug) Debug.Log("Ignoring music source: " + source.name);
                    continue;
                }

                float distance = Vector3.Distance(transform.position, source.transform.position);
                
                // NUEVO: Solo considerar fuentes dentro del rango de detección
                if (distance > detectionRange)
                {
                    if (showDebug) Debug.Log("GlobalSoundSource fuera de rango: " + distance + " > " + detectionRange);
                    continue;
                }
                
                if (distance < 0.1f) distance = 0.1f;
                
                float score = source.volumeIntensity * (1.0f / distance);
                
                if (bestGlobalSource == null || score > bestGlobalScore)
                {
                    bestGlobalScore = score;
                    bestGlobalSource = source.transform;
                }
            }
        }
        
        // Si encontramos un GlobalSoundSource activo dentro del rango
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
                    Debug.Log("Found better GlobalSoundSource with score: " + bestGlobalScore);
            }
        }
        // Si NO hay GlobalSoundSources activos DENTRO DEL RANGO, evaluar jugadores
        else
        {
            Transform bestPlayer = null;
            float bestPlayerScore = 0f;
            
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            
            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                
                // NUEVO: Solo considerar jugadores dentro del rango de detección
                if (distance > detectionRange)
                {
                    if (showDebug) Debug.Log("Player fuera de rango: " + distance + " > " + detectionRange);
                    continue;
                }
                
                if (distance < 0.1f) distance = 0.1f;
                
                float score = currentVolume * (1.0f / distance);
                
                if (bestPlayer == null || score > bestPlayerScore)
                {
                    bestPlayerScore = score;
                    bestPlayer = player.transform;
                }
            }
            
            // Si encontramos un jugador haciendo ruido dentro del rango
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
                        Debug.Log("Found better player source with score: " + bestPlayerScore);
                }
            }
        }
        
        // Si no encontramos un mejor objetivo, mantener el actual
        if (!foundBetterTarget && currentTarget == null)
        {
            // No se encontró ninguna fuente de sonido dentro del rango
            if (showDebug)
                Debug.Log("No valid noise sources found within detection range");
        }
    }
    
    // ACTUALIZADO: CalculateVolume ahora también respeta el rango de detección
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
        
        // Then check global sound sources DENTRO DEL RANGO
        float globalVolume = 0f;
        
        // Look for any active global sound sources WITHIN RANGE
        GlobalSoundSource[] soundSources = FindObjectsOfType<GlobalSoundSource>();
        foreach (GlobalSoundSource source in soundSources)
        {
            if (source.isActive)
            {
                float distance = Vector3.Distance(transform.position, source.transform.position);
                // NUEVO: Solo incluir fuentes dentro del rango para el cálculo de volumen
                if (distance <= detectionRange)
                {
                    globalVolume += source.volumeIntensity;
                }
            }
        }
        
        // Return the greater of the two values
        return Mathf.Max(micVolume, globalVolume);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("You died!");
            
            // Clear the target player
            targetPlayer = null;
            inDeathSentenceMode = false;
            
            // Attack animation when colliding with player
            StartAttacking();
        }
    }
    
    // NUEVO: Método para visualizar el rango de detección en el editor
    void OnDrawGizmos()
    {
        if (showDebug)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
    
    // Limpiar cuando el enemigo se destruye
    void OnDestroy()
    {
        // Limpiar cuando el enemigo principal se destruye
        if (audioSource == sharedMicrophoneAudioSource)
        {
            if (microphoneDevice != null)
            {
                Microphone.End(microphoneDevice);
            }
            isMicrophoneInitialized = false;
            sharedMicrophoneAudioSource = null;
            
            if (showDebug) Debug.Log("Enemigo principal destruido - micrófono liberado");
        }
    }
    
    #endregion

    // Optional public method for other classes to get the current volume
    public float GetCurrentVolume()
    {
        return currentVolume;
    }
}
