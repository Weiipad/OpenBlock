using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject contentContainer;
    private System.Action onClose = () => { };
    public void SetOnCloseListener(System.Action action)
    {
        onClose = action;
    }

    public void SetContent(GameObject content)
    {
        content.transform.SetParent(contentContainer.transform);
    }

    public void CloseButtonEvent()
    {
        onClose();
        Destroy(gameObject);
    }
}
