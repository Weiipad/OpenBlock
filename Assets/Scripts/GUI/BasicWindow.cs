using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock
{
    public class BasicWindow : MonoBehaviour
    {
        public WindowManager windowManager;
        [SerializeField]
        private int prev;
        private System.Action onClose = () => { };

        public void SetOnCloseListener(System.Action action)
        {
            onClose = action;
        }

        public void CloseButtonEvent()
        {

            onClose?.Invoke();
            Destroy(gameObject);
        }
    }

}