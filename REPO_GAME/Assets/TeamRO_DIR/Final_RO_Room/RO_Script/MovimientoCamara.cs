using UnityEngine;

public class MovimientoCamara: MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 100f;

    float pitch = 0f; // Rotación vertical
    float yaw = 0f;   // Rotación horizontal

    void Start()
    {
        // Oculta el cursor y lo bloquea al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S

        Vector3 direction = transform.forward * vertical + transform.right * horizontal;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f); // Limita la rotación vertical

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
