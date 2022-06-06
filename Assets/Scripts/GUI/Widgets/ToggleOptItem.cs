using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace OpenBlock.GUI
{
    public class ToggleOptItem : MonoBehaviour
    {
        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Toggle toggle;

        [SerializeField]
        private bool isChecked;

        [SerializeField]
        private string itemName;


        [SerializeField]
        private UnityEvent<bool> onValueChanged;

        private void OnEnable()
        {
            nameText.text = itemName;
            UpdateChildren();
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            nameText.text = itemName;
            UpdateChildren();
        }
#endif
        public void SetChecked(bool isChecked)
        {
            this.isChecked = isChecked;
            UpdateChildren();
        }

        public void OnValueChanged(bool val)
        {
            onValueChanged?.Invoke(val);
        }

        private void UpdateChildren()
        {
            toggle.isOn = isChecked;
        }
    }
}
