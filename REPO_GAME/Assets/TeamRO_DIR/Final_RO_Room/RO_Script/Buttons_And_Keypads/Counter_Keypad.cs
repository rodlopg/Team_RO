using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NavKeypad
{
    public class Counter_Keypad : MonoBehaviour
    {
        // Eventos que se disparan cuando se concede o deniega el acceso
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted;
        [SerializeField] private UnityEvent onAccessDenied;

        // Propiedades públicas para acceder a los eventos
        public UnityEvent OnAccessGranted => onAccessGranted;
        public UnityEvent OnAccessDenied => onAccessDenied;

        // Configuración de los botones del keypad
        [Header("Button Config")]
        [Tooltip("Number of unique buttons that must be pressed")]
        [SerializeField] private int requiredUniquePresses = 6; // Número de botones únicos que deben presionarse

        [Tooltip("List of valid button references")]
        [SerializeField] private List<Counter_KeypadButton> validButtons = new List<Counter_KeypadButton>(); // Lista de botones válidos

        // Configuración del botón Enter
        [Header("Enter Button")]
        [SerializeField] private Counter_KeypadButton enterButton;

        // Configuración visual del keypad
        [Header("Visuals")]
        [SerializeField] private string accessGrantedText = "OK"; // Texto cuando se concede acceso
        [SerializeField] private string accessDeniedText = "X"; // Texto cuando se deniega acceso
        [SerializeField] private string waitingForEnterText = "Press Enter"; // Texto mientras espera Enter
        [SerializeField] private float displayResultTime = 1f; // Tiempo que se muestra el resultado
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f; // Intensidad de la pantalla

        // Colores para diferentes estados del keypad
        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); // Color normal
        [SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f); // Color acceso denegado
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f, 1f); // Color acceso concedido
        [SerializeField] private Color screenWaitingColor = new Color(0.5f, 0.5f, 1f, 1f); // Color esperando Enter

        // Efectos de sonido
        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx; // Sonido al presionar botón
        [SerializeField] private AudioClip accessDeniedSfx; // Sonido acceso denegado
        [SerializeField] private AudioClip accessGrantedSfx; // Sonido acceso concedido
        [SerializeField] private AudioClip enterPressedSfx; // Sonido al presionar Enter

        // Referencias a componentes
        [Header("Component References")]
        [SerializeField] private Renderer panelMesh; // Renderer del panel
        [SerializeField] private TMP_Text keypadDisplayText; // Texto de la pantalla
        [SerializeField] private AudioSource audioSource; // Fuente de audio

        // Variables de estado
        private HashSet<string> pressedButtons = new HashSet<string>(); // Botones presionados
        private bool accessWasGranted = false; // Indica si el acceso fue concedido
        private bool displayingResult = false; // Indica si se está mostrando un resultado
        private bool waitingForEnter = false; // Indica si está esperando que se presione Enter

        private void Awake()
        {
            // Inicializa el keypad al cargar el script
            InitializeKeypad();
        }

        private void InitializeKeypad()
        {
            // Verifica que haya suficientes botones válidos
            if (validButtons.Count < requiredUniquePresses)
            {
                Debug.LogWarning($"Not enough valid buttons assigned. Need {requiredUniquePresses}, have {validButtons.Count}");
            }

            // Establece el color inicial de la pantalla
            panelMesh.material.SetColor("_EmissionColor", screenNormalColor * screenIntensity);
        }

        private void Start()
        {
            // Limpia el estado inicial del keypad
            ClearState();
        }

        // Método para agregar una entrada (se llama cuando se presiona un botón)
        public void AddInput(string input)
        {
            Debug.Log("Current input: " + input);

            // Si ya se está mostrando un resultado o el acceso fue concedido, no hacer nada
            if (displayingResult || accessWasGranted) return;

            // Manejar presión del botón Enter
            if (input == "enter")
            {
                HandleEnterPress();
                return;
            }

            // Manejar presión de botones normales
            if (!waitingForEnter && !pressedButtons.Contains(input))
            {
                audioSource?.PlayOneShot(buttonClickedSfx);
                pressedButtons.Add(input);
                UpdateDisplay();

                // Verificar si ya se presionaron suficientes botones únicos
                if (pressedButtons.Count >= requiredUniquePresses)
                {
                    ReadyForEnter();
                }
            }
        }

        // Maneja la presión del botón Enter
        private void HandleEnterPress()
        {
            if (!waitingForEnter) return;

            audioSource?.PlayOneShot(enterPressedSfx);

            // Verificar si se cumplió el requisito de botones presionados
            bool granted = pressedButtons.Count >= requiredUniquePresses;
            GameManager.Instance.PuzzleSolved();
            StartCoroutine(DisplayResultRoutine(granted));
        }

        // Prepara el keypad para esperar la presión del Enter
        private void ReadyForEnter()
        {
            waitingForEnter = true;
            keypadDisplayText.text = waitingForEnterText;
            panelMesh.material.SetColor("_EmissionColor", screenWaitingColor * screenIntensity);
        }

        // Actualiza el texto de la pantalla con el conteo actual
        private void UpdateDisplay()
        {
            keypadDisplayText.text = $"{pressedButtons.Count}/{requiredUniquePresses}";
        }

        // Corrutina para mostrar el resultado temporalmente
        private IEnumerator DisplayResultRoutine(bool granted)
        {
            displayingResult = true;

            if (granted)
                AccessGranted();
            else
                AccessDenied();

            yield return new WaitForSeconds(displayResultTime);

            displayingResult = false;

            // Si el acceso fue denegado, limpiar el estado para intentar nuevamente
            if (!granted)
            {
                ClearState();
            }
        }

        // Método que se ejecuta cuando se concede el acceso
        private void AccessGranted()
        {
            accessWasGranted = true;
            keypadDisplayText.text = accessGrantedText;
            panelMesh.material.SetColor("_EmissionColor", screenGrantedColor * screenIntensity);
            audioSource?.PlayOneShot(accessGrantedSfx);
            onAccessGranted?.Invoke();
        }

        // Método que se ejecuta cuando se deniega el acceso
        private void AccessDenied()
        {
            keypadDisplayText.text = accessDeniedText;
            panelMesh.material.SetColor("_EmissionColor", screenDeniedColor * screenIntensity);
            audioSource?.PlayOneShot(accessDeniedSfx);
            onAccessDenied?.Invoke();
        }

        // Limpia el estado del keypad (reinicia)
        private void ClearState()
        {
            Debug.Log("CLEAR STATE CALLED");
            pressedButtons.Clear();
            waitingForEnter = false;
            keypadDisplayText.text = $"0/{requiredUniquePresses}";
            keypadDisplayText.ForceMeshUpdate();
            panelMesh.material.SetColor("_EmissionColor", screenNormalColor * screenIntensity);
        }
    }
}