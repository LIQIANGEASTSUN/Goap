using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoginModel
{
    public string m_ip = string.Empty;
    public string m_port = string.Empty;

    public string diviceID = string.Empty;
    // ip端口组
    public string[] m_ipPortArr;
    // 默认选择端口
    public const int m_defaultIpPortIndex = 0;

    // 语言组
    public string[] m_languageArr;
    // 语言默认选择
    public const int m_defalutLanguageIndex = 0;

    public string m_userName = string.Empty;
    private const string m_userNameKey = "lm_account_userName";
    public LoginModel()
    {
        Init();
    }

    public void Init()
    {
        m_ipPortArr = new string[]
        {
            "吕凯:10.1.6.242:8088",
            "江民:10.1.36.31:8088",
        };


        SetIpPort(m_ipPortArr[m_defaultIpPortIndex]);
        m_languageArr = new string[] { "简体中文", "English" };

        diviceID = LMGetDeviceUid();

        if (PlayerPrefs.HasKey(m_userNameKey))
        {
            m_userName = PlayerPrefs.GetString(m_userNameKey);
        }
        if (string.IsNullOrEmpty(m_userName))
        {
            ResetUserName();
        }
    }

    public void SetIpPort(string p_ipPort)
    {
        string[] content = p_ipPort.Split(':');
        m_ip = content[1];
        m_port = content[2];
    }

    public void ResetUserName()
    {
        //生成一个id 点击登录的时候写入到本地
        string ticks = DateTime.Now.Ticks.ToString();
        char[] deviceStr = diviceID.ToCharArray();
        char[] nameStr = ticks.ToCharArray();
        char[] createNameArr;
        if (nameStr.Length > 12)
        {
            createNameArr = new char[12];
            Array.Copy(nameStr, nameStr.Length - 12 - 1, createNameArr, 0, 12);
        }
        else
        {
            createNameArr = new char[nameStr.Length];
            Array.Copy(nameStr, createNameArr, nameStr.Length);
        }

        for (int i = 0; i < createNameArr.Length; i++)
        {
            createNameArr[i] |= deviceStr[i];
            if ((int)createNameArr[i] > (int)'z')
            {
                createNameArr[i] = (char)(((int)createNameArr[i]) / 2);
            }
        }

        m_userName = new string(createNameArr);
        PlayerPrefs.SetString(m_userNameKey, m_userName);
    }

    private string LMGetDeviceUid()
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
#if UNITY_ANDROID && !UNITY_EDITOR
            //using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            //{
            //    using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            //    {
            //        //LMGetDeviceUid
            //       deviceId = jo.CallStatic<string>("LMGetDeviceUid");
            //        Debug.Log("[ViewLogin] > LMGetDeviceUid > NativeUid: " + deviceId);
            //    }
            //}
#endif
        return deviceId;
    }
}
