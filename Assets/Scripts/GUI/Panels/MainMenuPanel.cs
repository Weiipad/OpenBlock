using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.GUI.Panels
{
    public class MainMenuPanel : BasePanel
    {
        [SerializeField]
        private BasePanel optionsPanel;

        public override void OnResume()
        {
            base.OnResume();
            GameManager.Instance.SetGameStage(GameManager.GameStage.MainMenu);
        }

        public void OnPlayClick()
        {
            manager.CloseCurrentPanel();
            GameManager.Instance.LoadWorld();
        }

        public void OnOptionsClick()
        {
            OpenPanel(optionsPanel, PanelManager.OpenMode.Additive);
        }

        public void OnExitClick()
        {
            GameManager.Instance.ExitGame();
        }
    }
}
