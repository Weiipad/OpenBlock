using OpenBlock.Input;
using OpenBlock.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock
{
    public class Settings
    {
        public struct InputSettings
        {
            public ControlMode controlMode;
            public float sensitivity;
        }

        public struct UISettings
        {
            public float leftOffset;
            public float rightOffset;
        }

        public InputSettings input;
        public UISettings ui;

        public Settings()
        {
            input.controlMode = InputManager.GetDefaultControlMode();


            input.sensitivity = PlayerPrefs.GetFloat("input.sensitivity", 1.0f);
            ui.leftOffset = PlayerPrefs.GetFloat("ui.leftOffset", 75.0f);
            ui.rightOffset = PlayerPrefs.GetFloat("ui.rightOffset", 75.0f);
        }

        public void Save()
        {
            PlayerPrefs.SetFloat("input.sensitivity", input.sensitivity);
            PlayerPrefs.SetFloat("ui.leftOffset", ui.leftOffset);
            PlayerPrefs.SetFloat("ui.rightOffset", ui.rightOffset);
        }
    }

}