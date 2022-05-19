using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTest : MonoBehaviour, IDragHandler
{
    private Vector2 deltaPosition;
    private bool dragFlag;
    public Vector2 Delta => deltaPosition;

    public void OnDrag(PointerEventData eventData)
    {
        dragFlag = true;
        deltaPosition = eventData.delta;
    }

    private void LateUpdate()
    {
        if (dragFlag)
        {
            deltaPosition = Vector2.zero;
            dragFlag = false;
        }
    }
}
