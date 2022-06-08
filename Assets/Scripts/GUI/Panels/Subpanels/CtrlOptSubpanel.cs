using OpenBlock.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.GUI.Panels
{
    public class CtrlOptSubpanel : BasePanel
    {
        [SerializeField]
        private RangeOptItem sensitivity;
        public override void OnResume()
        {
            base.OnResume();
            var settings = GameManager.Instance.settings;
            sensitivity.SetValue(settings.input.sensitivity);
        }

        public override void OnPause()
        {
            base.OnPause();
            GameManager.Instance.settings.input.sensitivity = sensitivity.GetValue();
        }

        public void OnSensitivityChanged(float value)
        {
            GameManager.Instance.settings.input.sensitivity = value;
        }

        
    }
}
