using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.GUI.Panels
{
    public class OptionsPanel : BasePanel
    {
        [SerializeField]
        private BasePanel controlOptPanel, displayOptPanel, aboutPanel;
        [SerializeField]
        private PanelManager optPanelManager;

        public override void OnPause()
        {
            base.OnPause();
            GameManager.Instance.settings.Save();
        }

        public void OnBackClick()
        {
            manager.CloseCurrentPanel();
        }

        public void OnControlTab(bool isChecked)
        {
            if (isChecked)
            {
                optPanelManager.OpenPanel(controlOptPanel, PanelManager.OpenMode.Replace);
            }
        }

        public void OnDisplayTab(bool isChecked)
        {
            if (isChecked)
            {
                optPanelManager.OpenPanel(displayOptPanel, PanelManager.OpenMode.Replace);
            }
        }

        public void OnAboutTab(bool isChecked)
        {
            if (isChecked)
            {
                optPanelManager.OpenPanel(aboutPanel, PanelManager.OpenMode.Replace);
            }
        }
    }
}
