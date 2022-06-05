using OpenBlock.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenBlock.GUI
{
    [RequireComponent(typeof(AspectRatioFitter))]
    public class ItemShortcuts : MonoBehaviour
    {
        public int count;
        [SerializeField]
        private HorizontalLayoutGroup layout;
        [SerializeField]
        private GameObject itemSlotPrefab;
        public Sprite[] tempItemIcons;

        private AspectRatioFitter aspect;

        private ItemSlot[] itemSlots;
        private int idx = -1;

        public int Index
        {
            get => idx;
            set
            {
                SetIndex(value % count);
            }
        }
        private void Awake()
        {
            aspect = GetComponent<AspectRatioFitter>();

            itemSlots = new ItemSlot[count];
            for (int i = 0; i < itemSlots.Length; i++)
            {
                itemSlots[i] = Instantiate(itemSlotPrefab).GetComponent<ItemSlot>();
                itemSlots[i].transform.SetParent(layout.transform);
                itemSlots[i].idx = i;
                itemSlots[i].onTap += SetIndex;
                itemSlots[i].SetSprite(tempItemIcons[i]);
            }

            SetIndex(0);
        }

        private void Start()
        {
            aspect.aspectRatio = count;

            var input = InputManager.Instance;
            input.actions.select += OnSelect;
            input.actions.selectItem += OnSelectItem;
        }

        private void OnSelect(float dir)
        {
            if (dir > 0)
            {
                Index--;
            }
            else if (dir < 0)
            {
                Index++;
            }
        }

        private void OnSelectItem(int idx)
        {
            Index = idx;
        }

        public void SetIndex(int index)
        {
            index += count;
            index %= count;
            if (idx != -1) itemSlots[idx].Deselect();
            idx = index;
            itemSlots[idx].Select();
        }

        private void OnDisable()
        {
            var input = InputManager.Instance;
            input.actions.select -= OnSelect;
            input.actions.selectItem -= OnSelectItem;
        }
    }
}