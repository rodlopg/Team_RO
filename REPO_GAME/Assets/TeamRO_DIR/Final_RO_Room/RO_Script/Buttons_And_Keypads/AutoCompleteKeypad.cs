using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace KeypadSystem
{
    public class AutoCompleteKeypad : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted;

        [Header("Buttons to Press")]
        [Tooltip("Assign all the buttons that must be clicked")]
        //[SerializeField] private List<SimpleButton> buttons = new List<SimpleButton>();

        [Header("Visuals")]
        [SerializeField] private string accessGrantedText = "OK";
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f;

        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f);
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f, 1f);

        [Header("Component References")]
        [SerializeField] private Renderer panelMesh;
        [SerializeField] private TMP_Text keypadDisplayText;

        private bool accessGranted = false;
    }
}

        /*

        private void Awake()
        {
            
            foreach (SimpleButton btn in buttons)
            {
                btn.RegisterKeypad(this);
            }
            

            panelMesh.material.SetColor("_EmissionColor", screenNormalColor * screenIntensity);
            UpdateDisplay();
        }

        public void NotifyButtonPressed()
        {
            if (accessGranted) return;

            // Check if all buttons have been pressed
            foreach (SimpleButton btn in buttons)
            {
                if (!btn.IsPressed)
                    return;
            }

            // All buttons pressed
            accessGranted = true;
            keypadDisplayText.text = accessGrantedText;
            panelMesh.material.SetColor("_EmissionColor", screenGrantedColor * screenIntensity);
            onAccessGranted?.Invoke();
        }

        private void UpdateDisplay()
        {
            int count = 0;
            foreach (SimpleButton btn in buttons)
            {
                if (btn.IsPressed) count++;
            }
            keypadDisplayText.text = $"{count}/{buttons.Count}";
        }

        public void RefreshDisplay() => UpdateDisplay();
    }
        
}
        */
