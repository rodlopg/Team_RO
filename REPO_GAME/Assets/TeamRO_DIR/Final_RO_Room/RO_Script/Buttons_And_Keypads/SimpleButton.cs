using UnityEngine;

namespace KeypadSystem
{
    public class SimpleButton : MonoBehaviour
    {
        // Identificador único del botón
        [SerializeField] private string buttonId;

        // Referencia al keypad al que pertenece este botón
        [SerializeField] private SimpleKeypadChecker keypad;

        // Propiedad que indica si el botón está presionado
        public bool IsPressed { get; private set; }

        private void Update()
        {
            // Detectar clic derecho del mouse
            if (Input.GetMouseButtonDown(0)) // Right-click
            {
                // Lanzar un rayo desde la cámara hacia la posición del mouse
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // Verificar si el rayo golpeó este botón específico
                    if (hit.transform == transform)
                    {
                        PressButton();
                    }
                }
            }
        }

        // Método para registrar este botón con un keypad específico
        public void RegisterWithKeypad(SimpleKeypadChecker targetKeypad)
        {
            keypad = targetKeypad;
        }

        // Método para simular la presión del botón
        public void PressButton()
        {
            // Si ya está presionado, no hacer nada
            if (IsPressed) return;

            // Marcar como presionado y notificar al keypad
            IsPressed = true;
            keypad.UpdateKeypadDisplay();
            keypad.NotifyButtonPressedFromButton();
        }

        // Método para reiniciar el estado del botón
        public void ResetButton()
        {
            IsPressed = false;
        }
    }
}