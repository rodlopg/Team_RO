using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // Velocidad de rotación de la cámara - modificable desde el Inspector
    // Valores más altos = rotación más rápida, valores más bajos = rotación más lenta
    public float speed = 10.0f;

    
    void Start()
    {
        // Bloquea el cursor al centro de la pantalla para que no se vea
        // y no pueda salir de la ventana del juego
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Modifica la rotación de la cámara basándose en el movimiento del mouse
        transform.eulerAngles += speed * new Vector3(-Input.GetAxis("Mouse Y"),Input.GetAxis("Mouse X"),0);
        // GetAxis("Mouse Y"): devuelve valores entre -1 y 1 basados en el movimiento vertical del mouse, el signo negativo invierte el movimiento para que se sienta natural
        // GetAxis("Mouse X"): devuelve valores entre -1 y 1 basados en el movimiento horizontal del mouse
        // No hay rotación en el eje Z (mantiene la cámara nivelada)
        // Los valores se multiplican por speed para ajustar la sensibilidad
    }
}
