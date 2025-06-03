using System.Collections;
using UnityEngine;

namespace NavKeypad
{
    public class Counter_KeypadButton : MonoBehaviour
    {
        [Header("Button Value")]
        [SerializeField] private string value;

        [Header("Button Animation Settings")]
        [SerializeField] private float buttonSpeed = 0.1f;
        [SerializeField] private float moveDistance = 0.0025f;
        [SerializeField] private float buttonPressedTime = 0.1f;

        [Header("Component References")]
        [SerializeField] private Counter_Keypad keypad;

        private bool moving = false;

        public string Value => value;

        private void OnValidate()
        {
            // Auto-set the enter button value if it's the enter button
            if (gameObject.name.ToLower().Contains("enter"))
            {
                value = "enter";
            }
        }

        private void OnMouseDown()
        {
            PressButton();
        }

        public void PressButton()
        {
            Debug.Log("PressButton was called");

            if (!moving && keypad != null)
            {
                keypad.AddInput(value);
                StartCoroutine(MoveSmooth());
            }
        }

        private IEnumerator MoveSmooth()
        {
            moving = true;
            Vector3 startPos = transform.localPosition;
            Vector3 endPos = startPos + new Vector3(0, 0, moveDistance);

            // Press down animation
            float elapsedTime = 0f;
            while (elapsedTime < buttonSpeed)
            {
                transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / buttonSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = endPos;

            // Wait while pressed
            yield return new WaitForSeconds(buttonPressedTime);

            // Release animation
            startPos = transform.localPosition;
            endPos = startPos - new Vector3(0, 0, moveDistance);
            elapsedTime = 0f;

            while (elapsedTime < buttonSpeed)
            {
                transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / buttonSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = endPos;

            moving = false;
        }
    }
}