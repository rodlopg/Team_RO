using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace KeypadSystem // Puedes cambiar este nombre si lo prefieres
{
    public class SimpleKeypadChecker : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted; // Evento que se dispara cuando se concede acceso

        [Header("Buttons to Press")]
        [SerializeField] private List<SimpleButton> buttons = new List<SimpleButton>(); // Lista de botones que deben presionarse

        [Header("Visuals")]
        [SerializeField] private string accessGrantedText = "OK"; // Texto mostrado cuando se concede acceso
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f; // Intensidad de la pantalla

        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); // Color normal
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f, 1f); // Color acceso concedido

        [Header("Components")]
        [SerializeField] private Renderer panelMesh; // Renderer del panel visual
        [SerializeField] private TMP_Text keypadDisplayText; // Texto de la pantalla

        private bool accessGranted = false; // Estado de acceso concedido

        private void Awake()
        {
            // Validación de componentes requeridos
            if (panelMesh == null)
                Debug.LogError("SimpleKeypadChecker: panelMesh is not assigned!");
            if (keypadDisplayText == null)
                Debug.LogError("SimpleKeypadChecker: keypadDisplayText is not assigned!");

            // Registrar cada botón con este keypad
            foreach (SimpleButton btn in buttons)
            {
                if (btn != null)
                    btn.RegisterWithKeypad(this);
                else
                    Debug.LogWarning("SimpleKeypadChecker: One of the buttons in the list is null!");
            }

            // Configurar color inicial de la pantalla
            if (panelMesh != null)
                panelMesh.material.SetColor("_EmissionColor", screenNormalColor * screenIntensity);

            // Actualizar visualización inicial
            UpdateKeypadDisplay();
        }

        // Método llamado por los botones cuando son presionados
        public void NotifyButtonPressedFromButton()
        {
            if (accessGranted)
                return; // Si ya se concedió acceso, no hacer nada

            // Verificar si todos los botones están presionados
            foreach (SimpleButton btn in buttons)
            {
                if (btn == null || !btn.IsPressed)
                {
                    UpdateKeypadDisplay();
                    return; // Si algún botón no está presionado, solo actualizar display
                }
            }

            // Si todos los botones están presionados, conceder acceso
            accessGranted = true;
            if (keypadDisplayText != null)
                keypadDisplayText.text = accessGrantedText;
            if (panelMesh != null)
                panelMesh.material.SetColor("_EmissionColor", screenGrantedColor * screenIntensity);

            // Notificar al GameManager que el puzzle fue resuelto
            GameManager.Instance.PuzzleSolved();

            // Disparar evento de acceso concedido
            onAccessGranted?.Invoke();
        }

        // Actualiza el display del keypad con el conteo actual de botones presionados
        public void UpdateKeypadDisplay()
        {
            int pressedCount = 0;
            foreach (SimpleButton btn in buttons)
            {
                if (btn != null && btn.IsPressed)
                    pressedCount++;
            }

            if (keypadDisplayText != null)
                keypadDisplayText.text = $"{pressedCount}/{buttons.Count}";
        }
    }
}