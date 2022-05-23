using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenBlock.Input;

namespace OpenBlock
{
    public class GameManager : Singleton<GameManager>
    {
        public Text debugText;
        public Settings settings;
        public Material wireframeMaterial;

        public WindowManager windowManager;

        public enum GameStage
        {
            MainMenu, Game, Pause
        }
        private GameStage gameStage;

        protected override void Awake()
        {
            base.Awake();
            Settings.GetInstance();
            gameStage = GameStage.Game;
        }

        private void Start()
        {
            var input = InputManager.Instance;
            input.SetControlMode(Settings.Instance.input.controlMode);
            Cursor.lockState = CursorLockMode.Locked;
            input.actions.menu += OnOpenMenu;

            windowManager.onBack += () => SetGameStage(GameStage.Game);
        }

        public void OnOpenMenu()
        {
            if (gameStage == GameStage.Game) SetGameStage(GameStage.Pause);
            else if (gameStage == GameStage.Pause) SetGameStage(GameStage.Game);
        }

        private void SetGameStage(GameStage stage)
        {
            if (stage == GameStage.Game)
            {
                Cursor.lockState = CursorLockMode.Locked;
                windowManager.gameObject.SetActive(false);
            }
            else if (stage == GameStage.Pause)
            {
                Cursor.lockState = CursorLockMode.None;
                windowManager.gameObject.SetActive(true);
            }
            gameStage = stage;
        }

        private void OnPostRender()
        {
            debugText.text = "";
        }
    }

}