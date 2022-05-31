using OpenBlock.Input.Handler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace OpenBlock.Input
{
    public class InputManager : Singleton<InputManager>
    {
        private static readonly RuntimePlatform[] touchscreenPlatforms = 
        {
            RuntimePlatform.IPhonePlayer, RuntimePlatform.Android
        };

        private static readonly RuntimePlatform[] gamepadPlatforms =
        {
            RuntimePlatform.GameCoreXboxSeries, RuntimePlatform.PS4, RuntimePlatform.Switch, RuntimePlatform.PS5
        };

        [Range(0, 1)]
        public float lookSensitivity;

        public System.Action<ControlMode> onControlModeChanged;

        public InputActions actions;

        public ControlMode ctrlMode { get; private set; }
        private IInputHandler handler = null;

        protected override void Awake()
        {
            base.Awake();
        }

        public static ControlMode GetDefaultControlMode()
        {
            foreach (var platform in touchscreenPlatforms)
            {
                if (platform == Application.platform) return ControlMode.Touch;
            }

            foreach (var platform in gamepadPlatforms)
            {
                if (platform == Application.platform) return ControlMode.Gamepad;
            }

            return ControlMode.KeyboardAndMouse;
        }

        public bool SetControlMode(ControlMode mode)
        {
            switch (mode)
            {
                case ControlMode.KeyboardAndMouse:
                    if (Keyboard.current == null || Mouse.current == null) return false;
                    handler = new KeyboardHandler();
                    break;
                case ControlMode.Touch:
                    if (Touchscreen.current == null) return false;
                    handler = new TouchHandler();
                    break;
                case ControlMode.Gamepad:
                    if (Gamepad.current == null) return false;
                    handler = null;
                    break;
            }

            ctrlMode = mode;
            onControlModeChanged?.Invoke(ctrlMode);
            return true;
        }

        private void Update()
        {
            if (handler != null) handler.HandleInputs(ref actions);
        }

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }
    }
}
