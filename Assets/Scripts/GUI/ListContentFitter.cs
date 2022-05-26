using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OpenBlock.GUI
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class ListContentFitter : UIBehaviour, ILayoutSelfController
    {
        private int m_count;
        public int count
        {
            get => m_count;
            private set
            {
                m_count = value;
                ResetHeight();
            }
        }
        private RectTransform m_rect;
        public RectTransform rect
        {
            get
            {
                if (m_rect == null) m_rect = GetComponent<RectTransform>();
                return m_rect;
            }
        }
        [SerializeField]
        [Range(0, 1)]
        private float ratioToHeight;


        private float itemHeight;
        protected override void Start()
        {
            count = transform.childCount;
            itemHeight = ratioToHeight * Screen.height;
            
        }

        private void OnTransformChildrenChanged()
        {
            count = transform.childCount;
        }

        protected override void OnEnable()
        {
            ResetHeight();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            ResetHeight();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            ResetHeight();
        }
#endif
        private void ResetHeight()
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, itemHeight * m_count);
            LayoutRebuilder.MarkLayoutForRebuild(rect);
        }

        public void SetLayoutHorizontal()
        {
            ResetHeight();
        }

        public void SetLayoutVertical()
        {
            ResetHeight();
        }
    }
}