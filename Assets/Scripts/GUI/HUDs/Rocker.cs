using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

[RequireComponent(typeof(RectTransform))]
public class Rocker : MonoBehaviour
{
    public OnScreenStick onScreenStick;
    public RectTransform stickRect;

    private RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        onScreenStick.movementRange = (rectTransform.rect.width - stickRect.rect.width) / 2.0f;
    }
}