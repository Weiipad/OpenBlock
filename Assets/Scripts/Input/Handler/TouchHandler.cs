using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Utilities;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace OpenBlock.Input.Handler
{
    public class TouchHandler : IInputHandler
    {
        private const float PLACE_DEC_TIME = 0.2f;
        private const float DIG_DEC_TIME = 0.5f;
        private List<int> touchIdOnUI = new List<int>();
        private List<int> touchTriggeredDigStart = new List<int>();
        private Vector2 smoothDelta = Vector2.zero;
        private Vector2 deltaTarget;

        public void HandleInputs(ref InputActions actions)
        {
            InputManager input = InputManager.Instance;

            #region Handle Fingers
            foreach (var touch in Touch.activeTouches)
            {
                bool uiTouch = touchIdOnUI.Contains(touch.touchId);
                bool digTrig = touch.inProgress && (Time.realtimeSinceStartup - touch.startTime) > DIG_DEC_TIME;

                if (digTrig && !uiTouch && !touchTriggeredDigStart.Contains(touch.touchId))
                {
                    actions.digStart?.Invoke();
                    touchTriggeredDigStart.Add(touch.touchId);
                }

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (EventSystem.current.IsPointerOverGameObject())
                        {
                            touchIdOnUI.Add(touch.touchId);
                        }
                        break;

                    case TouchPhase.Moved:
                        if (!uiTouch)
                        {
                            deltaTarget = touch.delta;
                        }
                        break;

                    case TouchPhase.Stationary:
                        if (!uiTouch)
                        {
                            smoothDelta *= 0.4f;
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (uiTouch)
                        {
                            var touchIdx = touchIdOnUI.Find(i => i == touch.touchId);
                            var last = touchIdOnUI.Count - 1;
                            touchIdOnUI[touchIdx] = touchIdOnUI[last];
                            touchIdOnUI.RemoveAt(last);
                        }
                        else
                        {
                            if ((touch.time - touch.startTime) < PLACE_DEC_TIME)
                            {
                                actions.place?.Invoke();
                            }
                            else
                            {
                                actions.digEnd?.Invoke();
                                touchTriggeredDigStart.Remove(touch.touchId);
                            }
                            smoothDelta *= 0.05f;
                            deltaTarget = Vector2.zero;
                        }
                        break;

                    
                    case TouchPhase.None:
                        break;
                }
            }

            if (Vector2.Distance(smoothDelta, deltaTarget) > 0.01f)
            {
                smoothDelta += 8.0f * Time.deltaTime * (deltaTarget - smoothDelta);
                actions.look?.Invoke(GameManager.Instance.settings.input.sensitivity * smoothDelta);
            }

            deltaTarget = Vector2.zero;
            #endregion

            var keys = Keyboard.current;
            var gamepad = Gamepad.current;

            actions.move?.Invoke(gamepad.leftStick.ReadValue());
            if (gamepad.buttonSouth.isPressed) actions.jump?.Invoke();
            if (gamepad.buttonEast.isPressed) actions.descend?.Invoke();
            if (gamepad.selectButton.wasPressedThisFrame || keys.escapeKey.wasPressedThisFrame) actions.menu?.Invoke();

        }
    }
}
