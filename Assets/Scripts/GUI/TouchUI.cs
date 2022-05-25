using OpenBlock.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.GUI
{
    public class TouchUI : MonoBehaviour
    {
        private void Start()
        {
            var input = InputManager.Instance;
            Debug.Assert(input != null);
            input.onControlModeChanged += HandleControlModeChange;
            HandleControlModeChange(input.ctrlMode);
        }

        private void HandleControlModeChange(ControlMode ctrlMode)
        {
            if (ctrlMode == ControlMode.Touch)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
