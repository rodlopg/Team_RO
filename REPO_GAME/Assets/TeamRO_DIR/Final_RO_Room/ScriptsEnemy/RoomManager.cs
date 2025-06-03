using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script se encarga de gestionar los cuartos y los enemigos secundarios
public class RoomManager : MonoBehaviour
{
    [System.Serializable]
    public class Room
    {
        public string roomName;
        public List<Transform> waypoints = new List<Transform>();
        public bool hasSecondaryEnemy = false;
        public float timeSinceLastEnemy = 0f;
        public float cooldownTimer = 0f;
        public bool inCooldown = false;
    }
    
    // Referencia al enemigo principal
    public EnemyNavigation mainEnemy;
    
    // Prefab del enemigo secundario
    public GameObject secondaryEnemyPrefab;
    
    // Lista de cuartos en el mapa
    public List<Room> rooms = new List<Room>();
    
    // Ajustes para aparición de enemigos secundarios
    public float secondaryEnemyDuration = 60f;      // Cuánto tiempo permanece el enemigo secundario (en segundos)
    public float roomCooldownDuration = 90f;        // Cooldown después de que desaparece un enemigo (en segundos)
    public float autoSpawnTimer = 300f;             // Tiempo para aparición automática si no hay enemigos (en segundos)
    public float mainEnemyDetectionRange = 20f;     // Rango más allá del cual el enemigo principal no detecta ruido
    
    // Variables de rastreo interno
    private Dictionary<GameObject, Room> activeSecondaryEnemies = new Dictionary<GameObject, Room>();
    
    // Debugging
    public bool showDebug = true;
    
    void Start()
    {
        // Verificar que el enemigo principal esté asignado
        if (mainEnemy == null)
        {
            Debug.LogError("¡No se ha asignado el enemigo principal en RoomManager!");
        }
        
        // Verificar que el prefab del enemigo secundario esté asignado
        if (secondaryEnemyPrefab == null)
        {
            Debug.LogError("¡No se ha asignado el prefab del enemigo secundario en RoomManager!");
        }
        
        // Verificar que cada cuarto tenga al menos un waypoint
        foreach (Room room in rooms)
        {
            if (room.waypoints.Count == 0)
            {
                Debug.LogWarning("El cuarto " + room.roomName + " no tiene waypoints asignados.");
            }
        }
        
        // Inicializar tiempos para cada cuarto
        foreach (Room room in rooms)
        {
            room.timeSinceLastEnemy = Random.Range(0f, autoSpawnTimer * 0.5f); // Escalonar apariciones iniciales
        }
    }
    
    void Update()
    {
        // Actualizar timers para cada cuarto
        foreach (Room room in rooms)
        {
            // Si no hay enemigo secundario en este cuarto
            if (!room.hasSecondaryEnemy)
            {
                // Incrementar tiempo desde el último enemigo
                room.timeSinceLastEnemy += Time.deltaTime;
                
                // Si está en cooldown, reducir el timer de cooldown
                if (room.inCooldown)
                {
                    room.cooldownTimer -= Time.deltaTime;
                    
                    // Si el cooldown ha terminado
                    if (room.cooldownTimer <= 0f)
                    {
                        room.inCooldown = false;
                        if (showDebug) Debug.Log("Cooldown terminado para cuarto: " + room.roomName);
                    }
                }
                // Si no está en cooldown y ha pasado suficiente tiempo, considerar spawn automático
                else if (room.timeSinceLastEnemy >= autoSpawnTimer)
                {
                    // Intentar hacer spawn automático basado en tiempo
                    TryAutoSpawnSecondaryEnemy(room);
                }
            }
        }
        
        // Comprobar ruidos que no puede detectar el enemigo principal
        CheckForNoiseOutsideMainEnemyRange();
        
        // Comprobar ruido de players fuera del rango - SIN INTERVALO
        CheckForPlayerNoiseOutsideMainEnemyRange();
    }
    
    // Intenta hacer spawn de un enemigo secundario basado en tiempo
    private void TryAutoSpawnSecondaryEnemy(Room room)
    {
        // No hacer spawn si ya hay un enemigo en el cuarto
        if (room.hasSecondaryEnemy || room.inCooldown)
            return;
            
        // Seleccionar un waypoint aleatorio en el cuarto
        if (room.waypoints.Count > 0)
        {
            int randomIndex = Random.Range(0, room.waypoints.Count);
            Transform spawnWaypoint = room.waypoints[randomIndex];
            
            // Hacer spawn del enemigo secundario
            SpawnSecondaryEnemy(room, spawnWaypoint.position);
            
            if (showDebug) Debug.Log("Auto-spawn de enemigo secundario en el cuarto " + room.roomName + " por tiempo");
        }
    }
    
    // Comprueba ruidos de GlobalSoundSources fuera del rango del enemigo principal
    private void CheckForNoiseOutsideMainEnemyRange()
    {
        // Encontrar todas las fuentes de sonido activas
        GlobalSoundSource[] soundSources = FindObjectsOfType<GlobalSoundSource>();
        
        foreach (GlobalSoundSource source in soundSources)
        {
            // Solo procesar fuentes activas
            if (source.isActive)
            {
                // Calcular distancia al enemigo principal
                float distanceToMainEnemy = Vector3.Distance(mainEnemy.transform.position, source.transform.position);
                
                // Si está fuera del rango de detección del enemigo principal
                if (distanceToMainEnemy > mainEnemyDetectionRange)
                {
                    // Encontrar en qué cuarto está la fuente de sonido
                    Room sourceRoom = FindRoomForPosition(source.transform.position);
                    
                    // Si encontramos un cuarto válido
                    if (sourceRoom != null && !sourceRoom.hasSecondaryEnemy && !sourceRoom.inCooldown)
                    {
                        // Encontrar el waypoint más cercano a la fuente de sonido
                        Transform nearestWaypoint = FindNearestWaypoint(sourceRoom, source.transform.position);
                        
                        if (nearestWaypoint != null)
                        {
                            // Hacer spawn del enemigo secundario
                            SpawnSecondaryEnemy(sourceRoom, nearestWaypoint.position);
                            
                            if (showDebug) Debug.Log("Spawn de enemigo secundario en el cuarto " + sourceRoom.roomName + " por ruido GlobalSoundSource fuera de rango");
                        }
                    }
                }
            }
        }
    }
    
    // MODIFICADO: Comprueba ruido de players fuera del rango del enemigo principal
    private void CheckForPlayerNoiseOutsideMainEnemyRange()
    {
        // Encontrar todos los jugadores
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject player in players)
        {
            // Calcular distancia al enemigo principal
            float distanceToMainEnemy = Vector3.Distance(mainEnemy.transform.position, player.transform.position);
            
            // Si está fuera del rango de detección del enemigo principal
            if (distanceToMainEnemy > mainEnemyDetectionRange)
            {
                // USAR EL VOLUMEN CALCULADO POR EL ENEMIGO PRINCIPAL DIRECTAMENTE
                float currentVolume = mainEnemy.GetCurrentVolume();
                
                // Verificar si hay suficiente ruido (usar el mismo threshold que el enemigo principal)
                if (currentVolume > mainEnemy.noiseThreshold)
                {
                    // Encontrar en qué cuarto está el jugador
                    Room playerRoom = FindRoomForPosition(player.transform.position);
                    
                    // Si encontramos un cuarto válido
                    if (playerRoom != null && !playerRoom.hasSecondaryEnemy && !playerRoom.inCooldown)
                    {
                        // Encontrar el waypoint más cercano al jugador
                        Transform nearestWaypoint = FindNearestWaypoint(playerRoom, player.transform.position);
                        
                        if (nearestWaypoint != null)
                        {
                            // Hacer spawn del enemigo secundario
                            SpawnSecondaryEnemy(playerRoom, nearestWaypoint.position);
                            
                            if (showDebug) Debug.Log("Spawn de enemigo secundario en el cuarto " + playerRoom.roomName + " por ruido de player fuera de rango. Volume: " + currentVolume);
                        }
                    }
                }
            }
        }
    }
    
    // Encuentra el cuarto que corresponde a una posición
    public Room FindRoomForPosition(Vector3 position)
    {
        Room bestRoom = null;
        float closestDistance = float.MaxValue;
        
        foreach (Room room in rooms)
        {
            foreach (Transform waypoint in room.waypoints)
            {
                float distance = Vector3.Distance(position, waypoint.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestRoom = room;
                }
            }
        }
        
        return bestRoom;
    }
    
    // Encuentra el waypoint más cercano a una posición en un cuarto específico
    private Transform FindNearestWaypoint(Room room, Vector3 position)
    {
        if (room.waypoints.Count == 0)
            return null;
            
        Transform nearest = null;
        float minDistance = float.MaxValue;
        
        foreach (Transform waypoint in room.waypoints)
        {
            float distance = Vector3.Distance(position, waypoint.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = waypoint;
            }
        }
        
        return nearest;
    }
    
    // Hace spawn de un enemigo secundario
    public void SpawnSecondaryEnemy(Room room, Vector3 spawnPosition)
    {
        // No hacer spawn si ya hay un enemigo en el cuarto o está en cooldown
        if (room.hasSecondaryEnemy || room.inCooldown)
        {
            if (showDebug) Debug.LogWarning("No se puede hacer spawn en cuarto " + room.roomName + 
                                          ": tiene enemigo = " + room.hasSecondaryEnemy + 
                                          ", en cooldown = " + room.inCooldown);
            return;
        }
            
        // Crear el enemigo secundario
        GameObject newEnemy = Instantiate(secondaryEnemyPrefab, spawnPosition, Quaternion.identity);
        EnemySecondary secondaryScript = newEnemy.GetComponent<EnemySecondary>();
        
        if (secondaryScript != null)
        {
            // Configurar el enemigo secundario
            secondaryScript.SetRoomManager(this);
            secondaryScript.SetAssignedRoom(room);
            secondaryScript.SetLifetime(secondaryEnemyDuration);
            
            // Asignar waypoints al enemigo secundario
            secondaryScript.SetWaypoints(new List<Transform>(room.waypoints));
            
            // Marcar el cuarto como ocupado
            room.hasSecondaryEnemy = true;
            room.timeSinceLastEnemy = 0f;
            
            // Registrar el enemigo activo
            activeSecondaryEnemies[newEnemy] = room;
            
            if (showDebug) Debug.Log("Enemigo secundario creado en cuarto " + room.roomName);
        }
        else
        {
            if (showDebug) Debug.LogError("El prefab del enemigo secundario no tiene el componente EnemySecondary!");
            Destroy(newEnemy);
        }
    }
    
    // Notifica que un enemigo secundario ha desaparecido
    public void NotifySecondaryEnemyDespawned(GameObject enemy, Room room)
    {
        // Marcar el cuarto como libre pero en cooldown
        if (room != null)
        {
            room.hasSecondaryEnemy = false;
            room.inCooldown = true;
            room.cooldownTimer = roomCooldownDuration;
            if (showDebug) Debug.Log("Enemigo secundario despawn del cuarto " + room.roomName + ". Entrando en cooldown por " + roomCooldownDuration + " segundos");
        }
        
        // Eliminar de la lista de enemigos activos
        if (activeSecondaryEnemies.ContainsKey(enemy))
        {
            activeSecondaryEnemies.Remove(enemy);
        }
    }
    
    // Método para dibujar gizmos en el editor
    void OnDrawGizmos()
    {
        // Dibujar el rango de detección del enemigo principal
        if (mainEnemy != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(mainEnemy.transform.position, mainEnemyDetectionRange);
        }
        
        // Dibujar los waypoints de cada cuarto con colores diferentes
        Color[] roomColors = new Color[] { 
            Color.red, Color.green, Color.blue, Color.cyan, 
            Color.magenta, Color.yellow, Color.white, Color.grey 
        };
        
        for (int i = 0; i < rooms.Count; i++)
        {
            Room room = rooms[i];
            Color roomColor = roomColors[i % roomColors.Length];
            
            // Dibujar waypoints
            foreach (Transform waypoint in room.waypoints)
            {
                if (waypoint != null)
                {
                    Gizmos.color = roomColor;
                    Gizmos.DrawSphere(waypoint.position, 0.5f);
                    
                    // Dibujar líneas entre waypoints consecutivos
                    if (room.waypoints.Count > 1)
                    {
                        for (int j = 0; j < room.waypoints.Count; j++)
                        {
                            if (room.waypoints[j] != null)
                            {
                                int nextIdx = (j + 1) % room.waypoints.Count;
                                if (room.waypoints[nextIdx] != null)
                                {
                                    Gizmos.DrawLine(room.waypoints[j].position, room.waypoints[nextIdx].position);
                                }
                            }
                        }
                    }
                    
                    // Mostrar nombre del cuarto en el primer waypoint
                    if (waypoint == room.waypoints[0])
                    {
                        #if UNITY_EDITOR
                        UnityEditor.Handles.Label(waypoint.position + Vector3.up * 1.5f, room.roomName);
                        #endif
                    }
                }
            }
        }
    }
}
