using System.Collections;
using UnityEngine;

namespace FIMSpace.Basics
{
    /// <summary>
    /// FM: Class to detect mouse dragging object to change value by moving mouse
    /// </summary>
    public class FBasic_Pullable : FBasic_InteractionAreaCanvas
    {
        [Header(" --- Pullable Parameters --- ")]
        [Space(5f)]
        public UnityEngine.Events.UnityEvent EventOnKeyUpInteraction;
        [Space(10f)]
        [Tooltip("If value for dragging should increase / decrease on moving with mouse on x axis")]
        public bool XAxis = false;
        [Tooltip("If value for dragging should increase / decrease on moving with mouse on y axis")]
        public bool YAxis = true;

        [Tooltip("If we stop dragging, value comes back to 0")]
        public bool ResetValue = false;

        [Tooltip("How much value should be changed by mouse movement")]
        public float Sensitivity = 0.25f;

        [Range(0f, 1f)]
        public float StartValueY = 0f;
        [Range(0f, 1f)]
        public float StartValueX = 0f;

        public float LeverPullThreshold = 90f;
        public bool IsPulledDown { get; private set; } = false;

        public float YValue { get; protected set; }
        public float YValueUnclamped { get; private set; }
        public float XValue { get; protected set; }
        public float XValueUnclamped { get; private set; }

        private Vector2 holdStartPosition;
        private float holdStartValueY;
        private float holdStartValueX;

        private CursorLockMode previousCursorLockMode;
        private bool previousCursorVisibility;

        private bool startedByMouseClick = false;

        public bool Holding { get; protected set; }

        public bool CanBePulledByMouse = true;
        [Tooltip("If you want be able to pool it without need to be as player in object's range using mouse (pulling lever just using mouse from any distance)")]
        public bool OverrideCanBePulledByMouse = false;

        protected bool mouseEntered;

        protected override void Start()
        {
            base.Start();
            Holding = false;
            YValue = StartValueY * 100f;
            YValueUnclamped = YValue;
            holdStartValueY = YValue;

            XValue = StartValueX * 100f;
            XValueUnclamped = XValue;
            holdStartValueX = XValue;
            mouseEntered = false;

            EventOnInteraction.AddListener(StartHolding);
        }

        protected virtual void UpdatePullableOrientation() { }

        protected virtual void OnMouseEnter()
        {
            if (!CanBePulledByMouse) return;

            if (OverrideCanBePulledByMouse)
                if (!Entered)
                {
                    StartCoroutine(UpdateIfInRange());
                    OnEnter();
                    Entered = true;
                }

            if (Entered)
            {
                MouseEnter();
            }
        }

        protected virtual void MouseEnter()
        {
            if (!CanBePulledByMouse) return;
            mouseEntered = true;
        }

        protected virtual void OnMouseDown()
        {
            if (!CanBePulledByMouse) return;
            if (Input.GetMouseButtonDown(1)) // Right-click
            {
                StartHolding();
                startedByMouseClick = true;
            }
        }

        protected virtual void StartHolding()
        {
            if (!Holding)
            {
                previousCursorLockMode = Cursor.lockState;
                previousCursorVisibility = Cursor.visible;

                Holding = true;
                holdStartPosition = Input.mousePosition;
                holdStartValueY = YValue;
                holdStartValueX = XValue;
                mouseEntered = true;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        protected override void UpdateIn()
        {
            base.UpdateIn();

            if (!startedByMouseClick)
            {
                if (Input.GetKeyUp(InteractionKey))
                {
                    Holding = false;
                }
            }

            if (CanBePulledByMouse && Input.GetMouseButtonDown(1) && mouseEntered)
            {
                StartHolding();
                startedByMouseClick = true;
            }

            if (Holding)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    Holding = false;
                }

                float differenceValue = 0f;

                if (YAxis) differenceValue = holdStartPosition.y - Input.mousePosition.y;
                YValueUnclamped = holdStartValueY - differenceValue * Sensitivity;
                YValue = Mathf.Clamp(YValueUnclamped, 0f, 100f);

                differenceValue = 0f;
                if (XAxis) differenceValue = Input.mousePosition.x - holdStartPosition.x;
                XValueUnclamped = holdStartValueX - differenceValue * Sensitivity;
                XValue = Mathf.Clamp(XValueUnclamped, 0f, 100f);

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None;

                CheckPullState();
            }
            else
            {
                if (mouseEntered)
                {
                    StopHolding();
                }
            }
        }

        private void CheckPullState()
        {
            if (YValue >= LeverPullThreshold)
            {
                if (!IsPulledDown)
                {
                    IsPulledDown = true;
                    Debug.Log("Lever pulled down!");
                }
            }
            else
            {
                if (IsPulledDown)
                {
                    IsPulledDown = false;
                    Debug.Log("Lever released.");
                }
            }
        }

        protected override void OnExit()
        {
            StopHolding();
            base.OnExit();
        }

        protected virtual void StopHolding()
        {
            EventOnKeyUpInteraction.Invoke();
            if (ResetValue) YValue = 0f;

            Cursor.lockState = previousCursorLockMode;
            Cursor.visible = previousCursorVisibility;

            startedByMouseClick = false;
            mouseEntered = false;
            Holding = false;

            if (!Entered)
            {
                if (CanBePulledByMouse)
                    if (OverrideCanBePulledByMouse)
                    {
                        StopAllCoroutines();
                        OnExit();
                        Entered = false;
                    }
            }
        }
    }
}
