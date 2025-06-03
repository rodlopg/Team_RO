using UnityEngine;

public class MCMovement : MonoBehaviour
{
    // Componente que maneja el movimiento y las colisiones del personaje
    private CharacterController controller;
    
    // Velocidad de movimiento del personaje (modificable en el Inspector)
    private float playerSpeed = 5.0f;
    
    // Referencia al Transform de la cámara (debe asignarse en el Inspector)
    public Transform cameraTransform;

    
    void Start()
    {
        // Añade automáticamente el componente CharacterController al objeto
        controller = gameObject.AddComponent<CharacterController>();
    }

    
    void Update()
    {
        // Obtiene el input horizontal (A/D o ←/→) en un rango de -1 a 1
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // Obtiene el input vertical (W/S o ↑/↓) en un rango de -1 a 1
        float verticalInput = Input.GetAxis("Vertical");

        // Crea un vector de movimiento inicial basado en los inputs
        // El componente Y es 0 para mantener el movimiento horizontal
        Vector3 move = new Vector3(horizontalInput, 0, verticalInput);

        // Solo procesa el movimiento si hay algún input del jugador
        if (move != Vector3.zero)
        {
            // Obtiene el vector que representa "adelante" desde la cámara
            Vector3 forward = cameraTransform.forward;
            
            // Obtiene el vector que representa "derecha" desde la cámara
            Vector3 right = cameraTransform.right;

            // Elimina cualquier inclinación vertical para mantener el movimiento horizontal
            forward.y = 0;
            right.y = 0;
            
            // Normaliza los vectores para asegurar una velocidad consistente
            // sin importar la dirección o la inclinación de la cámara
            forward.Normalize();
            right.Normalize();

            // Calcula la dirección final del movimiento combinando:
            // - La dirección "adelante" multiplicada por el input vertical (W/S)
            // - La dirección "derecha" multiplicada por el input horizontal (A/D)
            Vector3 moveDirection = (forward * verticalInput + right * horizontalInput);

            // Mueve al personaje usando el CharacterController:
            // - moveDirection: la dirección del movimiento
            // - Time.deltaTime: hace el movimiento independiente de los FPS
            // - playerSpeed: controla la velocidad del movimiento
            controller.Move(moveDirection * Time.deltaTime * playerSpeed);
        }
    }
}
