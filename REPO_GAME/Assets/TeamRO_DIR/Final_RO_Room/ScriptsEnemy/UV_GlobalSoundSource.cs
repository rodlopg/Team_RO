using UnityEngine;
using System.Collections;

public class UV_GlobalSoundSource : MonoBehaviour
{
    // Audio settings
    [Range(0.1f, 10f)]
    public float volumeIntensity = 1f;
    public bool isActive = false;
    // Keyboard activation
    public KeyCode activationKey = KeyCode.None;
    
    // Visual settings for debugging
    public Color sourceColor = Color.yellow;
    
    // NUEVO: Variables para trampa
    [Header("Trampa Settings")]
    public bool isTrap = false;                   // Si es una trampa
    public float trapDuration = 3f;               // Duración del sonido cuando se activa la trampa
    public float trapCooldown = 10f;              // Cooldown antes de poder activar de nuevo
    private float cooldownTimer = 0f;             // Timer del cooldown actual
    private bool isCooldownActive = false;        // Si está en cooldown
    private Coroutine soundCoroutine;             // Referencia a la corrutina de sonido
    
    void Update()
    {
        // If a key is assigned and pressed, toggle the sound source
        if (activationKey != KeyCode.None && Input.GetKeyDown(activationKey))
        {
            ToggleSource();
            Debug.Log("Sound source at " + transform.position + " toggled: " + (isActive ? "ON" : "OFF") + " (Volume: " + volumeIntensity + ")");
        }
        
        // NUEVO: Actualizar cooldown de trampa
        if (isCooldownActive)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCooldownActive = false;
                Debug.Log("Trampa lista para activar de nuevo en: " + transform.position);
            }
        }
    }
    
    // Activate this sound source
    public void ActivateSource()
    {
        isActive = true;
    }
    
    // Deactivate this sound source
    public void DeactivateSource()
    {
        isActive = false;
    }
    
    // Toggle this sound source
    public void ToggleSource()
    {
        isActive = !isActive;
    }
    
    // NUEVO: Método para activar trampa
    public void ActivateTrap()
    {
        if (!isTrap || isCooldownActive) return;
        
        // Activar el sonido
        isActive = true;
        Debug.Log("¡TRAMPA ACTIVADA! en " + transform.position);
        
        // Parar corrutina anterior si existe
        if (soundCoroutine != null)
        {
            StopCoroutine(soundCoroutine);
        }
        
        // Iniciar nueva corrutina para desactivar después del tiempo
        soundCoroutine = StartCoroutine(DeactivateAfterTime());
        
        // Iniciar cooldown
        isCooldownActive = true;
        cooldownTimer = trapCooldown;
    }
    
    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(trapDuration);
        
        // Desactivar el sonido
        isActive = false;
        Debug.Log("Trampa desactivada en: " + transform.position);
    }
    
    // NUEVO: Detectar colisión con player SOLAMENTE
    void OnTriggerEnter(Collider other)
    {
        // Debug para ver qué está tocando las trampas
        Debug.Log("Algo tocó la trampa: " + other.name + " con tag: " + other.tag);
        
        // Solo activar si es una trampa Y el objeto que colisiona es un Player
        if (isTrap && other.CompareTag("UV_Tag"))
        {
            ActivateTrap();
            Debug.Log("¡Trampa activada por Luz UV!");
        }
        
        // Los enemigos NO activarán las trampas, aunque colisionen con ellas
        // porque no tienen el tag "Player"
    }
    
    // Visual indicator in scene view - MEJORADO
    void OnDrawGizmos()
    {
        // Color diferente para trampas
        if (isTrap)
        {
            if (isCooldownActive)
                Gizmos.color = Color.red;     // Rojo si está en cooldown
            else if (isActive)
                Gizmos.color = Color.green;   // Verde si está activa
            else
                Gizmos.color = Color.yellow;  // Amarillo si está lista
                
            // Dibujar área de trigger
            Collider col = GetComponent<Collider>();
            if (col != null && col.isTrigger)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                if (col is BoxCollider)
                {
                    BoxCollider box = col as BoxCollider;
                    Gizmos.DrawWireCube(box.center, box.size);
                }
                else if (col is SphereCollider)
                {
                    SphereCollider sphere = col as SphereCollider;
                    Gizmos.DrawWireSphere(sphere.center, sphere.radius);
                }
            }
        }
        else
        {
            // Color normal para fuentes de sonido regulares
            Gizmos.color = isActive ? sourceColor : Color.gray;
        }
        
        Gizmos.DrawSphere(transform.position, 0.5f);
        
        #if UNITY_EDITOR
        string labelText = "Volume: " + volumeIntensity + 
                          "\nActive: " + isActive +
                          "\nKey: " + activationKey;
        
        if (isTrap)
        {
            labelText += "\n--- TRAMPA ---";
            labelText += "\nDuración: " + trapDuration + "s";
            labelText += "\nCooldown: " + trapCooldown + "s";
            
            if (isCooldownActive)
            {
                labelText += "\nCooldown activo: " + cooldownTimer.ToString("F1") + "s";
            }
            else
            {
                labelText += "\nEstado: Lista";
            }
        }
        
        UnityEditor.Handles.Label(transform.position + Vector3.up, labelText);
        #endif
    }
}
