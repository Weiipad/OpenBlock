using OpenBlock;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class GameManager : Singleton<GameManager>
{
    public Text debugText;
    public Settings settings;
    public Material wireframeMaterial;

    public enum GameStage
    {
        MainMenu, Game, Pause
    }


    protected override void Awake()
    {
        base.Awake();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnPostRender()
    {
        debugText.text = "";
    }
}
