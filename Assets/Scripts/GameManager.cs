using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenBlock.Input;
using OpenBlock.GUI;
using UnityEngine.SceneManagement;
using OpenBlock.Utils;
using OpenBlock.File;

namespace OpenBlock
{
    public class GameManager : Singleton<GameManager>
    {
        public const float LOADING_MIN_TIME = 0.618f;

        public const int MAIN_MENU_SCENE_INDEX = 1;
        public const int WORLD_SCENE_INDEX = 2;

        public Text debugText;
        public Dialog dialog;
        public Settings settings;
        public Material wireframeMaterial;

        public WindowManager windowManager;

        public GameObject loading;

        [System.Serializable]
        public enum GameStage
        {
            MainMenu, Game, Pause
        }
#if UNITY_EDITOR
        [SerializeField]
#endif
        private GameStage gameStage;

        public List<GameObject> layouts;

        protected override void Awake()
        {
            base.Awake();
            Settings.GetInstance();
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

            SetGameStage(gameStage);
            input.actions.menu += OnMenu;
            LoadScene(MAIN_MENU_SCENE_INDEX);
            windowManager.ShowWindow(0);
        }

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
            if (gameStage == GameStage.Game) SetGameStage(GameStage.Pause);
            else if (gameStage == GameStage.Pause) SetGameStage(GameStage.Game);
        }

        public void QuitGame()
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
            SetGameStage(GameStage.Game);
            StartCoroutine(CoLoadWorld());
        }

        private IEnumerator CoLoadWorld()
        {
            loading.SetActive(true);
            var startTime = Time.realtimeSinceStartup;

            yield return SwitchScene(MAIN_MENU_SCENE_INDEX, WORLD_SCENE_INDEX);

            yield return new WaitForSecondsRealtime(LOADING_MIN_TIME - (Time.realtimeSinceStartup - startTime));
            loading.SetActive(false);
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
            SetGameStage(GameStage.MainMenu);
            var mainCam = Camera.main.GetComponent<MainCamera>();
            mainCam.Trace(gameObject);
            mainCam.StopTrace();
        }

        private IEnumerator TryUnloadScene(int index)
        {
            var scene = SceneManager.GetSceneByBuildIndex(index);
            if (scene.isLoaded) yield return SceneManager.UnloadSceneAsync(index);
        }

        private IEnumerator LoadScene(int index)
        {
            yield return TryUnloadScene(index);
            yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        }

        private IEnumerator SwitchScene(int from, int to)
        {
            yield return TryUnloadScene(from);
            yield return TryUnloadScene(to);
            yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        }

        private void SetGameStage(GameStage stage)
        {
            if (stage == GameStage.Game)
            {
                Cursor.lockState = CursorLockMode.Locked;
                windowManager.Close();
            }
            else if (stage == GameStage.Pause)
            {
                Cursor.lockState = CursorLockMode.None;
                windowManager.ShowWindow(1);
            }
            else if (stage == GameStage.MainMenu)
            {
                Cursor.lockState = CursorLockMode.None;
                windowManager.ShowWindow(0);
            }

            Debug.Log($"Gamestage set to {gameStage}");
            gameStage = stage;
        }

        private void OnPostRender()
        {
            debugText.text = "";
        }
    }

}