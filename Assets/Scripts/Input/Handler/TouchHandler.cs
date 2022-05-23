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
        private float placeDigDivTime = 0.5f;
        private List<int> touchIdOnUI = new List<int>();
        public void HandleInputs(ref InputActions actions)
        {
            InputManager input = InputManager.Instance;

            #region Handle Fingers
            foreach (var touch in Touch.activeTouches)
            {
                bool uiTouch = touchIdOnUI.Contains(touch.touchId);

                if (!uiTouch && (Time.realtimeSinceStartup - touch.startTime) > placeDigDivTime && touch.inProgress) actions.digStart?.Invoke();

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (EventSystem.current.IsPointerOverGameObject())
                        {
                            touchIdOnUI.Add(touch.touchId);
                        }
                        break;

                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        if (!uiTouch)
                        {
                            actions.look?.Invoke(touch.delta);
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (uiTouch)
                        {
                            touchIdOnUI.Remove(touch.touchId);
                        }
                        else
                        {
                            if ((touch.time - touch.startTime) < placeDigDivTime) actions.place?.Invoke();
                            else actions.digEnd?.Invoke();
                        }
                        break;

                    
                    case TouchPhase.None:
                        break;
                }
            }
            #endregion

            var gamepad = Gamepad.current;
            actions.move?.Invoke(gamepad.leftStick.ReadValue());
            if (gamepad.buttonSouth.isPressed) actions.jump?.Invoke();
            if (gamepad.buttonEast.isPressed) actions.descend?.Invoke();
        }
    }
}
