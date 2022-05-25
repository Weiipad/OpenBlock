using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OpenBlock.GUI
{
    [RequireComponent(typeof(RectTransform))]
    public class ScreenRatioFitter : MonoBehaviour
    {
        public enum FitMode
        {
            Width, Height
        }

        public FitMode property;
        public float ratio;

        private RectTransform rect;
        private RectTransform rectTransform
        {
            get
            {
                if (rect == null) rect = GetComponent<RectTransform>();
                return rect;
            }
        }

        private void OnEnable()
        {
            rectTransform.sizeDelta.Set(rectTransform.sizeDelta.x, Screen.height * ratio);
        }
    }
}