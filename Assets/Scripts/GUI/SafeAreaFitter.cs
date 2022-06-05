using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.GUI
{
    public class SafeAreaFitter : MonoBehaviour
    {
        private RectTransform m_rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (m_rectTransform == null) m_rectTransform = GetComponent<RectTransform>();
                return m_rectTransform;
            }
        }

        private void Update()
        {
            ResetSize();
        }

        /*private void OnEnable()
        {
            ResetSize();
        }

        protected void OnRectTransformDimensionsChange()
        {
            ResetSize();
        }

        public void SetLeftOffset(float value)
        {
            leftOffset = value;
            ResetSize();
        }

        public void SetRightOffset(float value)
        {
            rightOffset = value;
            ResetSize();
        }*/

        private void ResetSize()
        {
            var settings = GameManager.Instance.settings;
            rectTransform.offsetMin = new Vector2(settings.ui.leftOffset, rectTransform.offsetMin.y);
            rectTransform.offsetMax = new Vector2(-settings.ui.rightOffset, rectTransform.offsetMax.y);
        }
    }
}
