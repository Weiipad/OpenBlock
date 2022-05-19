using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public enum ControlMode
    {
        TouchScreen, KeyboardAndMouse
    }

    private ControlMode ctrlMode;
    public ControlMode controlMode
    {
        get => ctrlMode;
        set
        {
            OnSetControlMode?.Invoke(value);
            ctrlMode = value;
        }
    }
    public System.Action<ControlMode> OnSetControlMode;
}
