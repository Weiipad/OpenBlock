using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenBlock.Input;
using OpenBlock.GUI;
using UnityEngine.SceneManagement;
using OpenBlock.Utils;
using OpenBlock.IO;
using OpenBlock.Core.Event;

namespace OpenBlock
{
    public class GameManager : Singleton<GameManager>
    {
        public const float LOADING_MIN_TIME = 0.618f;

        public const int MAIN_MENU_SCENE_INDEX = 1;
        public const int WORLD_SCENE_INDEX = 2;

        public Text debugText;
        public Dialog dialog;
        public Settings settings { get; private set; }
        public EventQueue eventQueue { get; private set; }
        public Material wireframeMaterial;

        public GameObject loading;

        public PanelManager menuPanelManager;
        public BasePanel mainMenuPanel, pausePanel;

        [System.Serializable]
        public enum GameStage
        {
            MainMenu, Game, Pause
        }
#if UNITY_EDITOR
        [SerializeField]
#endif
        private GameStage gameStage;

        #region Unity Events
        protected override void Awake()
        {
            base.Awake();
            settings = new Settings();
            eventQueue = new EventQueue();
            FileManager.GetInstance();
            MessageStorage.GetInstance();
        }

        private void Start()
        {
#if !UNITY_EDITOR
            gameStage = GameStage.MainMenu;
#endif
            var input = InputManager.Instance;
            input.SetControlMode(InputManager.GetDefaultControlMode());

            input.actions.menu += OnMenu;
            LoadScene(MAIN_MENU_SCENE_INDEX);
        }

        private void Update()
        {
            eventQueue.HandleEvents();
        }
        #endregion


        public void ShowDialog(string msg)
        {
            ShowDialog(msg, 0.8f);
        }

        public void ShowDialog(string msg, float seconds)
        {
            dialog.gameObject.SetActive(true);
            dialog.Show(msg, seconds);
        }

        public void OnMenu()
        {
            if (gameStage == GameStage.Game)
            {
                menuPanelManager.OpenPanel(pausePanel, PanelManager.OpenMode.Replace);
                SetGameStage(GameStage.Pause);
            }
            else if (gameStage == GameStage.Pause)
            {
                menuPanelManager.CloseCurrentPanel();
                SetGameStage(GameStage.Game);
            }
        }

        public void ExitGame()
        {
#if UNITY_EDITOR 
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        public void LoadWorld()
        {
            Debug.Log("Loading world");
            StartCoroutine(CoLoadWorld());
        }

        private IEnumerator CoLoadWorld()
        {
            loading.SetActive(true);
            var startTime = Time.realtimeSinceStartup;

            yield return SwitchScene(MAIN_MENU_SCENE_INDEX, WORLD_SCENE_INDEX);

            yield return new WaitForSecondsRealtime(LOADING_MIN_TIME - (Time.realtimeSinceStartup - startTime));
            loading.SetActive(false);
            SetGameStage(GameStage.Game);
        }

        public void BackToMainMenu()
        {
            StartCoroutine(CoBackToMainMenu());
        }

        private IEnumerator CoBackToMainMenu()
        {
            loading.SetActive(true);
            var startTime = Time.realtimeSinceStartup;

            yield return SwitchScene(WORLD_SCENE_INDEX, MAIN_MENU_SCENE_INDEX);

            yield return new WaitForSecondsRealtime(LOADING_MIN_TIME - (Time.realtimeSinceStartup - startTime));

            loading.SetActive(false);
            var mainCam = Camera.main.GetComponent<MainCamera>();
            mainCam.Trace(gameObject);
            mainCam.StopTrace();

            menuPanelManager.OpenPanel(mainMenuPanel);
        }

        private IEnumerator TryUnloadScene(int index)
        {
            var scene = SceneManager.GetSceneByBuildIndex(index);
            if (scene.isLoaded) yield return SceneManager.UnloadSceneAsync(index);
        }

        private IEnumerator CoLoadScene(int index)
        {
            yield return TryUnloadScene(index);
            yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(index));
        }

        private void LoadScene(int index)
        {
            StartCoroutine(CoLoadScene(index));
        }

        private IEnumerator SwitchScene(int from, int to)
        {
            yield return TryUnloadScene(from);
            yield return TryUnloadScene(to);
            yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        }

        public void SetDebugText(bool value)
        {
            debugText.gameObject.SetActive(value);
        }

        public GameStage GetGameStage() => gameStage;

        public void SetGameStage(GameStage stage)
        {
            if (stage == GameStage.Game)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if (stage == GameStage.Pause)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else if (stage == GameStage.MainMenu)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            gameStage = stage;
            Debug.Log($"Gamestage set to {gameStage}");
        }
    }

}