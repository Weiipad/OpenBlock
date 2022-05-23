using OpenBlock.Input;
using OpenBlock.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock
{
    public class Settings : CommonSingleton<Settings>
    {
        public struct InputSettings
        {
            public ControlMode controlMode;
        }

        public struct UISettings
        {
            public int specScreenAdaptation;
        }

        public InputSettings input;
        public UISettings ui;

        public Settings()
        {
            input.controlMode = InputManager.GetDefaultControlMode();
            ui.specScreenAdaptation = 0;
        }
    }

}