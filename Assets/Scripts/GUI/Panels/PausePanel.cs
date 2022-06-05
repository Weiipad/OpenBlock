using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.GUI.Panels
{
    public class PausePanel : BasePanel
    {
        [SerializeField]
        private BasePanel mainMenuPanel, optionsPanel;
        public void OnOptionsClick()
        {
            OpenPanel(optionsPanel, PanelManager.OpenMode.Additive);
        }

        public void OnQuitClick()
        {
            GameManager.Instance.BackToMainMenu();
        }

        public void OnBackClick()
        {
            manager.CloseCurrentPanel();
            GameManager.Instance.SetGameStage(GameManager.GameStage.Game);
        }
    }
}
