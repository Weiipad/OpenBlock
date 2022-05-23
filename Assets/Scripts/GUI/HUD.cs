using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HUD : MonoBehaviour
{
    private RectTransform rectTransform;
    public int screenAdapt = 75;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        rectTransform.offsetMin = new Vector2(screenAdapt, rectTransform.offsetMin.y);
        rectTransform.offsetMax = new Vector2(-screenAdapt, rectTransform.offsetMax.y);
    }
}
