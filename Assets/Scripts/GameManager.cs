using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Text debugText;
    public VisionController vc;
    public DragTest drag;

    [System.Serializable]
    public struct ControlBindings
    {
        public UnityEvent OnMoveForwardButton;
        public UnityEvent OnMoveBackButton;
        public UnityEvent OnMoveLeftButton;
        public UnityEvent OnMoveRightButton;
        public UnityEvent OnRiseButton;
        public UnityEvent OnDownButton;
        public UnityEvent OnDigButton;
        public UnityEvent OnPlaceButton;
    }
    public ControlBindings controlBindings;


    public Settings settings;

    public enum GameStage
    {
        MainMenu, Game, Pause
    }


    protected override void Awake()
    {
        base.Awake();
        #region Init Settings
        settings = new Settings();
        settings.OnSetControlMode += (ctrlMode) =>
        {
            if (ctrlMode == Settings.ControlMode.KeyboardAndMouse)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }


            vc.GetMovementDelta = ctrlMode switch
            {
                Settings.ControlMode.TouchScreen => () => drag.Delta,
                Settings.ControlMode.KeyboardAndMouse => () => 10 * new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")),
                _ => () => { 
                    Debug.Log("Non ControlMode setted");
                    return Vector2.zero;
                },
            };
        };
        settings.controlMode = Settings.ControlMode.KeyboardAndMouse;
        #endregion
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W)) controlBindings.OnMoveForwardButton?.Invoke();
        if (Input.GetKey(KeyCode.S)) controlBindings.OnMoveBackButton?.Invoke();
        if (Input.GetKey(KeyCode.A)) controlBindings.OnMoveLeftButton?.Invoke();
        if (Input.GetKey(KeyCode.D)) controlBindings.OnMoveRightButton?.Invoke();
        if (Input.GetKey(KeyCode.Space)) controlBindings.OnRiseButton?.Invoke();
        if (Input.GetKey(KeyCode.LeftShift)) controlBindings.OnDownButton?.Invoke();
        if (Input.GetKey(KeyCode.Mouse0)) controlBindings.OnDigButton?.Invoke();
        if (Input.GetKeyDown(KeyCode.Mouse1)) controlBindings.OnPlaceButton?.Invoke();
    }
}
