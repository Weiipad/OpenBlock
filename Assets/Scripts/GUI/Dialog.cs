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

        public void Show(string content)
        {
            contentText.text = content;
            StartCoroutine(CoAutoClose());
        }

        private IEnumerator CoAutoClose()
        {
            if (gameObject.activeSelf)
            {
                yield return new WaitForSeconds(0.8f);
                gameObject.SetActive(false);
            }
        }
    }
}