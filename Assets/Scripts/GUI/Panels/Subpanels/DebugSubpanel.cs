using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.GUI.Panels
{
    public class DebugSubpanel : BasePanel
    {
        [SerializeField]
        private ToggleOptItem debugTextOn;
        public override void OnResume()
        {
            base.OnResume();
            debugTextOn.SetChecked(GameManager.Instance.debugText.isActiveAndEnabled);
        }
        public void OnDebugModeChange(bool isDebugTextOn)
        {
            GameManager.Instance.SetDebugText(isDebugTextOn);
        }
    }
}
