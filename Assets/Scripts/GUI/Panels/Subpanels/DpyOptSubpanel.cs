using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OpenBlock.GUI.Panels
{
    public class DpyOptSubpanel : BasePanel
    {
        [SerializeField]
        private RangeOptItem leftOffset, rightOffset;
        public override void OnResume()
        {
            base.OnResume();
            var settings = GameManager.Instance.settings;
            leftOffset.SetValue(settings.ui.leftOffset);
            rightOffset.SetValue(settings.ui.rightOffset);
        }
        public void OnLeftOffsetValueChanged(float value)
        {
            var gm = GameManager.Instance;
            gm.settings.ui.leftOffset = value;
        }

        public void OnRightOffsetValueChanged(float value)
        {
            var gm = GameManager.Instance;
            gm.settings.ui.rightOffset = value;
        }
    }
}
