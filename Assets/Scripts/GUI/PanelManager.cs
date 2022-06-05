using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform panelRoot;
    [SerializeField]
    private BasePanel startPanelPrefab;

    private Stack<BasePanel> loadedPanels;

    public enum OpenMode
    {
        Replace, Additive
    }

    private void Awake()
    {
        loadedPanels = new Stack<BasePanel>();
    }

    private void OnEnable()
    {
        if (startPanelPrefab != null) OpenPanel(startPanelPrefab);
    }

    public void OpenPanel(BasePanel panelPrefab)
    {
        OpenPanel(panelPrefab, OpenMode.Replace);
    }

    public void OpenPanel(BasePanel panelPrefab, OpenMode mode)
    {
        if (loadedPanels.Count == 0)
        {
            LoadPanel(panelPrefab);
            return;
        }

        switch (mode)
        {
            case OpenMode.Replace:
                {
                    CloseCurrentPanel();
                    LoadPanel(panelPrefab);
                    break;
                }
            case OpenMode.Additive:
                {
                    if (loadedPanels.Count != 0)
                    {
                        var curPanel = loadedPanels.Peek();
                        curPanel.OnPause();
                        curPanel.gameObject.SetActive(false);
                    }
                    LoadPanel(panelPrefab);
                    break;
                }
        }
    }

    public void LoadPanel(BasePanel panelPrefab)
    {
        var panel = Instantiate(panelPrefab);
        panel.transform.SetParent(panelRoot);
        panel.OnCreate(this);
        panel.OnResume();
        loadedPanels.Push(panel);
    }

    public void CloseCurrentPanel()
    {
        if (loadedPanels.Count == 0) return;
        var curPanel = loadedPanels.Pop();
        curPanel.OnPause();
        curPanel.OnExit();
        Destroy(curPanel.gameObject);
        if (loadedPanels.Count != 0)
        {
            var p = loadedPanels.Peek();
            p.gameObject.SetActive(true);
            p.OnResume();
        }
    }
}
