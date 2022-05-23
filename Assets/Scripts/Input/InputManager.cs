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

        public InputActions actions;
        public GameObject touchscreenSpecUI;
        public Player player;

        private ControlMode ctrlMode;
        private IInputHandler handler = null;

        protected override void Awake()
        {
            base.Awake();
            actions.look += player.Look;
            actions.move += player.Move;
            actions.place += player.Place;
            actions.digStart += player.DigStart;
            actions.digEnd += player.DigEnd;
            actions.jump += player.Jump;
            actions.descend += player.Descend;
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
            if (mode == ControlMode.Touch) touchscreenSpecUI.SetActive(true);
            else touchscreenSpecUI.SetActive(false);
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
