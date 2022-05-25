using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenBlock
{
    public class WindowManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject touchBlockPanel;
        [SerializeField]
        private List<GameObject> windows;
        private int current;
        private void Awake()
        {
            current = 0;
            Close();
        }

        public void ShowWindow(int index)
        {
            if (index >= windows.Count)
            {
                Debug.LogError("show window out of range");
            }
            windows[current].SetActive(false);
            windows[index].SetActive(true);
            current = index;
            touchBlockPanel.SetActive(true);
        }

        public void Close()
        {
            foreach (var window in windows) window.SetActive(false);
            touchBlockPanel.SetActive(false);
        }
    }
}