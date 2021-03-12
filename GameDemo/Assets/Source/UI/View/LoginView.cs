using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginView
{
    private Transform m_tr = null;

    public Transform m_bg = null;
    public InputField m_UidInputField = null;
    public Button m_loginButton = null;
    public Button m_clearButton = null;
    public Dropdown m_languageDropDown = null;
    public Dropdown m_serverDropDown = null;

    public LoginView(Transform tr)
    {
        m_tr = tr;
        Init();
    }

    public void Init()
    {
        m_bg = ToolsComponent.FindChildCom<Transform>(m_tr, "BG");
        m_UidInputField = ToolsComponent.FindChildCom<InputField>(m_tr, "BG/UidInputField");
        m_loginButton = ToolsComponent.FindChildCom<Button>(m_tr, "BG/LoginButton");
        m_clearButton = ToolsComponent.FindChildCom<Button>(m_tr, "BG/ClearButton");
        m_languageDropDown = ToolsComponent.FindChildCom<Dropdown>(m_tr, "BG/LanguageDropdown");
        m_serverDropDown = ToolsComponent.FindChildCom<Dropdown>(m_tr, "BG/ServerDropdown");
    }
}
