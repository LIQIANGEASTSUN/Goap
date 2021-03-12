using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UIFrameWork
{
    /// <summary>
    /// 面板枚举
    /// </summary>
    public enum EnumUIType
    {
        #region C# 1 - 1000
        None = 0,
        
        LoginControllerPanel                     = 1, // 登录面板
        HpControllerPanel                        = 2, // 血条面板

        #endregion

        #region Lua 1001 - 2000


        #endregion
    }
}

namespace UIFrameWork
{
    #region 委托
    public delegate void StateChangedEvent(object sender, UIPanelState state);
    #endregion

    #region 枚举  
    public enum UIPanelState
    {
        None         = 0,
        InitialUI    = 1 << 1,   // 初始化 UI
        InitialData  = 1 << 2,   // 初始化 数据
    }
    #endregion

    public class UIInfoData
    {
        public int    m_type;  // 面板枚举
        public string m_name;  // 面板预设名
        public object m_data;
    }

    public class UIManage
    {
        public static readonly UIManage Instance = new UIManage();
        private Dictionary<int, GameObject> m_dicOpenedUIs = null;

        private UIManage()
        {
            m_dicOpenedUIs = new Dictionary<int, GameObject>();
        }

        #region Get
        public T GetUI<T>(int _type) where T : UIBase
        {
            GameObject _retObj = GetUIGameObject(_type);
            if (_retObj != null)
            {
                return _retObj.GetComponent<T>();
            }
            return null;
        }

        public GameObject GetUIGameObjectEnum(int _type)
        {
            return GetUIGameObject((int)_type);
        }

        public GameObject GetUIGameObject(int _type)
        {
            GameObject p_retObj = null;
            if (!m_dicOpenedUIs.TryGetValue(_type, out p_retObj))
            {
                return p_retObj;
            }
            return null;
        }
        #endregion

        #region Open
        public void OpenUI(int type, string name, object data)
        {
            UIInfoData uiInfoData = new UIInfoData() { m_type = type, m_name = name, m_data =data};
            OpenUI(uiInfoData);
        }

        public void OpenUI(UIInfoData _uiInfoData)
        {
            UIInfoData[] uiInfoDataArr = new UIInfoData[1];
            uiInfoDataArr[0] = _uiInfoData;
            OpenUI(uiInfoDataArr, false);
        }

        public void OpenUICloseOthers(UIInfoData _uiInfoData)
        {
            UIInfoData[] uiInfoDataArr = new UIInfoData[1];
            uiInfoDataArr[0] = new UIInfoData() { m_type = _uiInfoData.m_type, m_name = _uiInfoData.m_name, m_data = _uiInfoData.m_data };
            OpenUI(uiInfoDataArr, true);
        }

        private void OpenUI(UIInfoData[] _UIInfoDataArr, bool _isCloseOthers)
        {
            // Close Others UI.
            if (_isCloseOthers)
            {
                CloseAllUI();
            }

            // push _uiTypes in Stack.  
            for (int i = 0; i < _UIInfoDataArr.Length; i++)
            {
                UIInfoData uiInfoData = _UIInfoDataArr[i];
                int p_uiType = uiInfoData.m_type;
                if (!m_dicOpenedUIs.ContainsKey(p_uiType))
                {
                    LoadUIType(p_uiType, uiInfoData.m_name, uiInfoData.m_data);
                }
            }
        }
        #endregion

        #region Close
        public void CloseUI(int _uiType)
        {
            GameObject p_uiObj = null;
            if (!m_dicOpenedUIs.TryGetValue((int)_uiType, out p_uiObj))
            {
                Debug.Log("dicOpenedUIs TryGetValue Failure! _uiType :" + _uiType.ToString());
                return;
            }
            CloseUI(_uiType, p_uiObj);
        }

        public void CloseUI(int[] _uiTypes)
        {
            for (int i = 0; i < _uiTypes.Length; i++)
            {
                CloseUI(_uiTypes[i]);
            }
        }

        public void CloseAllUI()
        {
            List<int> _keyList = new List<int>(m_dicOpenedUIs.Keys);
            for (int i = 0; i < _keyList.Count; ++i)
            {
                int _uiType = _keyList[i];
                GameObject _uiObj = m_dicOpenedUIs[_uiType];
                CloseUI(_uiType, _uiObj);
            }
            m_dicOpenedUIs.Clear();
        }

        private void CloseUI(int _uiType, GameObject _uiObj)
        {
            if (_uiObj == null)
            {
                m_dicOpenedUIs.Remove(_uiType);
                return;
            }
            UIBase p_baseUI = _uiObj.GetComponent<UIBase>();
            if (p_baseUI != null)
            {
                p_baseUI.Release();
            }

            m_dicOpenedUIs.Remove(_uiType);
        }
        #endregion

        private void LoadUIType(int _enumUIType, string _name, object _uiParams)
        {
            // 加载UI 
            Action<GameObject> CallBack = (go) =>
            {
                if (go == null)
                {
                    return;
                }

                GameObject p_prefabObj = go as GameObject;
                if (p_prefabObj == null)
                {
                    return;
                }

                if (UGUICanvas.Instance != null)
                {
                    p_prefabObj.gameObject.SetActive(true);
                    p_prefabObj.transform.SetParent(UGUICanvas.Instance.RootPanel);
                    p_prefabObj.transform.localPosition = Vector3.zero;
                    p_prefabObj.transform.localScale = Vector3.one;
                }

                UIBase p_baseUI = p_prefabObj.GetComponent<UIBase>();
                if (null == p_baseUI)
                {
                    Debug.LogError("UIRoot is null ..." + _name);
                    return;
                }

                p_baseUI.SetData(_enumUIType, _uiParams);
                m_dicOpenedUIs.Add(_enumUIType, p_prefabObj);
            };

            LoadUI(_name, CallBack);
        }

        private void LoadUI(string _name, Action<GameObject> CallBack)
        {
            // 加载UI
            LoadCallBackHandler LoadCallBack = delegate (HandlerParam p_handleParam)
            {
                if (p_handleParam.assetObj == null)
                {
                    return;
                }
                GameObject go = GameObject.Instantiate( p_handleParam.assetObj) as GameObject;
                if (CallBack != null)
                {
                    CallBack(go);
                }
            };

            AssetPool.Instance.UI.LoadAsset<GameObject>(_name, LoadCallBack, null);
        }
    }
}