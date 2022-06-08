using OpenBlock.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.GUI
{
    [RequireComponent(typeof(RectTransform))]
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private WorldObj world;

        private void Awake()
        {
        }

        private void Start()
        {
        }
    }
}