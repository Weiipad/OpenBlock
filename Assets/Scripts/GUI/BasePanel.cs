using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PanelManager;

[DisallowMultipleComponent]
public class BasePanel : MonoBehaviour
{
    protected PanelManager manager;
    protected RectTransform rectTransform;
    public virtual void OnCreate(PanelManager manager)
    {
        this.manager = manager;
        rectTransform = GetComponent<RectTransform>();
        rectTransform.offsetMin = rectTransform.offsetMax = Vector2.zero;
    }
    public virtual void OnPause() { }
    public virtual void OnResume() { }
    public virtual void OnExit() { }

    protected void OpenPanel(BasePanel panel, OpenMode mode)
    {
        manager.OpenPanel(panel, mode);
    }
}
