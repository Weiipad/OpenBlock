using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenBlock
{
    public class WindowManager : MonoBehaviour
    {
        public System.Action onBack;

        public void OnBackButton()
        {
            onBack?.Invoke();
        }
    }
}