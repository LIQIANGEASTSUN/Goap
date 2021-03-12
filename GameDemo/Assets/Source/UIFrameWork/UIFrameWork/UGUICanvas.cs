using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

public class UGUICanvas : MonoBehaviour {
    public static UGUICanvas Instance = null;
    private Camera m_uiCamera = null;
    private Transform m_tr = null;

    private Transform m_rootPanel = null;

    private Dictionary<string, Transform> m_otherPanelDic = new Dictionary<string, Transform>();

    private void Awake()
    {
        Instance = this;
        m_tr = transform;

        GetUICamera();
    }
    
    public Transform Transform
    {
        get { return m_tr; }
    }

    public Transform RootPanel
    {
        get { return m_rootPanel; }
    }

    private void GetUICamera()
    {
        m_uiCamera = ToolsComponent.FindChildCom<Camera>(m_tr, "Camera");
        m_rootPanel = ToolsComponent.FindChildCom<Transform>(m_tr, "RootPanel");
    }

    public Camera UICamera { get { return m_uiCamera; } }

    public Transform GetPanel(string panelName)
    {
        Transform panel = null;
        if (m_otherPanelDic.TryGetValue(panelName, out panel))
        {
            return panel;
        }

        panel = ToolsComponent.FindChildCom<Transform>(m_tr, panelName);
        return panel;
    }
}
