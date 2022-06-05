using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OpenBlock.GUI
{
    public class ItemSlot : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private Sprite common, selected;
        [SerializeField]
        private Image itemImage, slotImage;

        public int idx;
        public System.Action<int> onTap;

        private void Awake()
        {
            Deselect();
        }

        public void Select()
        {
            slotImage.sprite = selected;
        }

        public void Deselect()
        {
            slotImage.sprite = common;
        }

        public void SetSprite(Sprite sprite)
        {
            itemImage.sprite = sprite;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onTap?.Invoke(idx);
        }
    }
}