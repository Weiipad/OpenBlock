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
                itemSlots[i] = Instantiate(itemSlotPrefab, layout.transform).GetComponent<ItemSlot>();

                itemSlots[i].idx = i;
                itemSlots[i].onTap += SetIndex;
            }
            
        }

        private void Start()
        {
            aspect.aspectRatio = count;
            StartCoroutine(RefreshLayout());
        }

        private void Update()
        {
            GameManager.Instance.debugText.text = $"Shortcut [{idx}] selecting";
        }

        private IEnumerator RefreshLayout()
        {
            layout.gameObject.SetActive(false);
            for (int i = 0; i < 3; i++) yield return null;
            layout.gameObject.SetActive(true);
            for (int i = 0; i < 3; i++) yield return null;
            layout.gameObject.SetActive(false);
            for (int i = 0; i < 3; i++) yield return null;
            layout.gameObject.SetActive(true);
        }

        public void SetIndex(int index)
        {
            index += count;
            index %= count;
            if (idx != -1) itemSlots[idx].Deselect();
            idx = index;
            itemSlots[idx].Select();
        }
    }
}