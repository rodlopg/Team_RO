using System.Collections;
using UnityEngine;

public class AxisMover : MonoBehaviour
{
    // Enumerado para seleccionar el eje de movimiento
    public enum MoveAxis { X, Y, Z }

    [Header("Target Settings")]
    [Tooltip("Assign the object you want to move.")]
    public Transform target; // Objeto que se moverá

    [Header("Movement Settings")]
    public MoveAxis axis = MoveAxis.Y; // Eje seleccionado para el movimiento
    public float offset = 0.25f; // Distancia del movimiento
    public float moveSpeed = 0.1f; // Velocidad del movimiento
    public float holdTime = 0.1f; // Tiempo de espera antes de volver (si no es toggle)
    public bool toggle = false; // Modo toggle (alternar posición)

    // Variables de estado
    private bool moving = false; // Indica si está en movimiento
    private bool moved = false; // Indica si está en posición movida (para modo toggle)
    private Vector3 originalPos; // Guarda la posición original

    private void Start()
    {
        // Deshabilitar si no hay target asignado
        if (target == null)
        {
            enabled = false;
            return;
        }

        // Guardar posición original
        originalPos = target.localPosition;
    }

    private void Update()
    {
        // Detectar clic derecho
        if (Input.GetMouseButtonDown(0)) // Right-click
        {
            // Lanzar rayo desde la cámara
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Verificar si se hizo click en este objeto
                if (hit.transform == transform)
                {
                    Debug.Log("Right-clicked on: " + gameObject.name);
                    // Iniciar movimiento si no está en proceso
                    if (!moving)
                    {
                        StartCoroutine(MoveSmooth());
                    }
                }
            }
        }
    }

    // Obtiene el vector de desplazamiento según el eje seleccionado
    private Vector3 GetOffsetVector()
    {
        switch (axis)
        {
            case MoveAxis.X: return Vector3.right * offset;
            case MoveAxis.Y: return Vector3.up * offset;
            case MoveAxis.Z: return Vector3.forward * offset;
            default: return Vector3.zero;
        }
    }

    // Corrutina para movimiento suave
    private IEnumerator MoveSmooth()
    {
        moving = true;

        Vector3 startPos = target.localPosition;
        Vector3 endPos;

        // Determinar dirección del movimiento (toggle o no)
        if (toggle && moved)
        {
            Debug.Log("Moving back");
            endPos = originalPos; // Volver a posición original
            moved = false;
        }
        else
        {
            Debug.Log("Moving forward");
            endPos = originalPos + GetOffsetVector(); // Mover a nueva posición
            moved = true;
        }

        // Animación de movimiento hacia adelante
        float elapsedTime = 0f;
        while (elapsedTime < moveSpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveSpeed);
            target.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        target.localPosition = endPos;

        // Si no es toggle, esperar y volver a posición original
        if (!toggle)
        {
            yield return new WaitForSeconds(holdTime);

            // Animación de movimiento de regreso
            startPos = target.localPosition;
            endPos = originalPos;

            elapsedTime = 0f;
            while (elapsedTime < moveSpeed)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / moveSpeed);
                target.localPosition = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }

            target.localPosition = endPos;
        }

        moving = false;
    }
}