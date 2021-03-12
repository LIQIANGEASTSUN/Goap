using UnityEngine;
using UIFrameWork;
using UnityEngine.UI;

#pragma warning disable 0219
public class LoginControllerPanel : UIBase
{
    private LoginView m_loginView = null;
    private LoginModel m_loginModel = null;

    protected override void Awake()
    {
        base.Awake();
        EventManage.Instance.AddEventListener((int)EventHandlerID.ShowLogin, ShowLogin);
    }

    /// <summary>
    /// 初始化 View
    /// </summary>
    protected override void GetView()
    {
        m_loginView = new LoginView(M_TR);
    }

    /// <summary>
    /// 初始化 Data
    /// </summary>
    protected override void GetData()
    {
        m_loginModel = new LoginModel();
    }

    /// <summary>
    /// 刷新面板
    /// </summary>
    public override void RefreshFixed()
    {
        m_loginView.m_loginButton.onClick.AddListener(LoginOnClick);
        m_loginView.m_clearButton.onClick.AddListener(ClearOnClick);

        SetUid();

        m_loginView.m_languageDropDown.options.Clear();

        SetDropDown(m_loginView.m_languageDropDown, m_loginModel.m_languageArr);
        m_loginView.m_languageDropDown.onValueChanged.AddListener(LanguageDropDownChange);

        SetDropDown(m_loginView.m_serverDropDown, m_loginModel.m_ipPortArr);
        m_loginView.m_serverDropDown.onValueChanged.AddListener(ServerDropDownChange);
    }

    private void SetUid()
    {
        m_loginView.m_UidInputField.text = m_loginModel.m_userName;
    }

    private void LoginOnClick()
    {
        if (string.IsNullOrEmpty(m_loginModel.m_userName) || string.IsNullOrEmpty(m_loginModel.m_ip))
        {
            Debug.LogError("请输入IP和端口");  // UIManager.Instance.AddHudText("[ff0000]请输入IP和端口[-]");
            return;
        }

        OnLoginOk(new byte[] { });
    }

    private void OnLoginOk(byte[] data)
    {
        ManagerScene.LoadScene(SceneEnum.Battle);
        Show(false);
    }

    // 清空数据
    private void ClearOnClick()
    {
        PlayerPrefs.DeleteAll();

        m_loginModel.ResetUserName();
        SetUid();
    }

    private void SetDropDown(Dropdown p_dropDown, string[] dataArr)
    {
        if (p_dropDown == null)
        {
            return;
        }
        p_dropDown.options.Clear();
        for (int i = 0; i < dataArr.Length; ++i)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = dataArr[i];
            p_dropDown.options.Add(optionData);
        }
    }

    private void LanguageDropDownChange(int value)
    {
        LanguageString.languageType = (value == 0) ? LANGUAGE_TYPE.CN : LANGUAGE_TYPE.EN;
    }

    private void ServerDropDownChange(int value)
    {
        string server = m_loginView.m_serverDropDown.options[value].text;
        string[] content = server.Split(':');
        m_loginModel.SetIpPort(server);
    }

    private void ShowLogin(int type, object data)
    {
        bool value = (bool)data;
        m_loginView.m_bg.gameObject.SetActive(value);
        Show(value);
    }

    private void Show(bool value)
    {
        if (gameObject.activeInHierarchy != value)
        {
            gameObject.SetActive(value);
        }
    }
}