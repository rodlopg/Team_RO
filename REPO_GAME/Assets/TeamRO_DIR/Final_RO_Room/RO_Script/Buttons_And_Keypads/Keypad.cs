using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NavKeypad
{
    public class Keypad : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted; // Evento cuando se concede acceso
        [SerializeField] private UnityEvent onAccessDenied;  // Evento cuando se deniega acceso
        [Header("Combination Code (9 Numbers Max)")]
        [SerializeField] private int keypadCombo = 12345;   // Código de combinación

        // Propiedades públicas para acceder a los eventos
        public UnityEvent OnAccessGranted => onAccessGranted;
        public UnityEvent OnAccessDenied => onAccessDenied;

        [Header("Settings")]
        [SerializeField] private string accessGrantedText = "Granted"; // Texto acceso concedido
        [SerializeField] private string accessDeniedText = "Denied";  // Texto acceso denegado

        [Header("Visuals")]
        [SerializeField] private float displayResultTime = 1f; // Tiempo que se muestra el resultado
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f; // Intensidad de la pantalla

        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); // Color normal (naranja)
        [SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f); // Color denegado (rojo)
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f); // Color concedido (verde)

        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx; // Sonido al presionar botón
        [SerializeField] private AudioClip accessDeniedSfx;  // Sonido acceso denegado
        [SerializeField] private AudioClip accessGrantedSfx; // Sonido acceso concedido

        [Header("Component References")]
        [SerializeField] private Renderer panelMesh;         // Renderer del panel
        [SerializeField] private TMP_Text keypadDisplayText; // Texto de visualización
        [SerializeField] private AudioSource audioSource;    // Fuente de audio

        // Variables de estado
        private string currentInput;         // Entrada actual del usuario
        private bool displayingResult = false; // Si se está mostrando un resultado
        private bool accessWasGranted = false; // Si el acceso fue concedido

        private void Awake()
        {
            ClearInput(); // Limpiar entrada al inicio
            // Configurar color inicial de la pantalla
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
        }

        // Método para agregar entrada desde los botones
        public void AddInput(string input)
        {
            audioSource.PlayOneShot(buttonClickedSfx); // Reproducir sonido de botón

            // Si se está mostrando resultado o ya se concedió acceso, ignorar
            if (displayingResult || accessWasGranted) return;

            switch (input)
            {
                case "enter":
                    CheckCombo(); // Verificar combinación al presionar Enter
                    break;
                default:
                    // Limitar a 9 dígitos máximo
                    if (currentInput != null && currentInput.Length == 9)
                    {
                        return;
                    }
                    currentInput += input; // Agregar dígito
                    keypadDisplayText.text = currentInput; // Actualizar display
                    break;
            }
        }

        // Verifica si la combinación ingresada es correcta
        public void CheckCombo()
        {
            if (int.TryParse(currentInput, out var currentKombo))
            {
                bool granted = currentKombo == keypadCombo; // Comparar con código guardado
                if (!displayingResult)
                {
                    StartCoroutine(DisplayResultRoutine(granted)); // Mostrar resultado
                }
            }
            else
            {
                Debug.LogWarning("Couldn't process input for some reason..");
            }
        }

        // Corrutina para mostrar el resultado temporalmente
        private IEnumerator DisplayResultRoutine(bool granted)
        {
            displayingResult = true;

            if (granted) AccessGranted(); // Acción para acceso concedido
            else AccessDenied();          // Acción para acceso denegado

            yield return new WaitForSeconds(displayResultTime); // Esperar tiempo de visualización

            displayingResult = false;
            if (granted) yield break; // Si fue concedido, terminar

            // Restablecer para intentar nuevamente
            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
        }

        // Acciones cuando se deniega el acceso
        private void AccessDenied()
        {
            keypadDisplayText.text = accessDeniedText; // Mostrar texto denegado
            onAccessDenied?.Invoke(); // Disparar evento
            panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity); // Cambiar color
            audioSource.PlayOneShot(accessDeniedSfx); // Reproducir sonido
        }

        // Limpiar la entrada actual
        private void ClearInput()
        {
            currentInput = "";
            keypadDisplayText.text = currentInput;
        }

        // Acciones cuando se concede el acceso
        private void AccessGranted()
        {
            accessWasGranted = true;
            keypadDisplayText.text = accessGrantedText; // Mostrar texto concedido
            onAccessGranted?.Invoke(); // Disparar evento
            GameManager.Instance.PuzzleSolved(); // Notificar al GameManager
            panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity); // Cambiar color
            audioSource.PlayOneShot(accessGrantedSfx); // Reproducir sonido
        }
    }
}