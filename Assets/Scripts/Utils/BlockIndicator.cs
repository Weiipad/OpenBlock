using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIndicator : MonoBehaviour
{
    public void SetPosition(Vector3Int blockPos)
    {
        transform.position = blockPos + 0.5f * Vector3.one;
    }
}
