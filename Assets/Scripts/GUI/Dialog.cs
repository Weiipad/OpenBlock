using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OpenBlock.GUI
{
    public class Dialog : MonoBehaviour
    {
        [SerializeField]
        private RectTransform backgroundArea;
        [SerializeField]
        private Text contentText;
        [SerializeField]
        private Image progress;

        public void Show(string content, float seconds)
        {
            contentText.text = content;
            StartCoroutine(CoAutoClose(seconds));
        }

        private IEnumerator CoAutoClose(float seconds)
        {
            if (gameObject.activeSelf)
            {
                var time = Time.realtimeSinceStartup;
                var elapsed = 0.0f;
                while (elapsed < seconds)
                {
                    progress.fillAmount = 1.0f - Mathf.Clamp01(elapsed / seconds);
                    yield return null;
                    elapsed = Time.realtimeSinceStartup - time;
                }
                gameObject.SetActive(false);
            }
        }
    }
}