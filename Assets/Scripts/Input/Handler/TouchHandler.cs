using OpenBlock.Core;
using OpenBlock.Core.Event.PlayerControl;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
            var eventQueue = GameManager.Instance.eventQueue;

            #region Handle Fingers
            foreach (var touch in Touch.activeTouches)
            {
                bool uiTouch = touchIdOnUI.Contains(touch.touchId);
                bool digTrig = touch.inProgress && (Time.realtimeSinceStartup - touch.startTime) > DIG_DEC_TIME;

                if (digTrig && !uiTouch && !touchTriggeredDigStart.Contains(touch.touchId))
                {
                    eventQueue.SendEvent(new DigEvent(DigEvent.Phase.Start));
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
                                eventQueue.SendEvent(new PlaceEvent());
                            }
                            else
                            {
                                eventQueue.SendEvent(new DigEvent(DigEvent.Phase.End));
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
                eventQueue.SendEvent(new LookEvent(GameManager.Instance.settings.input.sensitivity * smoothDelta));
            }

            deltaTarget = Vector2.zero;
            #endregion

            var keys = Keyboard.current;
            var gamepad = Gamepad.current;

            eventQueue.SendEvent(new MoveEvent(gamepad.leftStick.ReadValue()));
            if (gamepad.buttonSouth.isPressed) eventQueue.SendEvent(new JumpEvent());
            if (gamepad.buttonEast.isPressed) eventQueue.SendEvent(new DescendEvent());
            if (gamepad.selectButton.wasPressedThisFrame || keys.escapeKey.wasPressedThisFrame) actions.menu?.Invoke();

        }
    }
}
