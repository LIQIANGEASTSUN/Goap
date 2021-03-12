using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace UIFrameWork
{
    [RequireComponent(typeof(RectTransformExtend))]
    public abstract class UIBase : ViewController
    {
        private Transform m_Tr;
        private Button m_closeBtn = null;

        /// <summary>
        /// 是否是直接放在UI 上的
        /// </summary>
        [SerializeField]
        private bool isRootPanel = false;

        #region Type && State
        protected int m_uiType = 0;
        protected object m_data = null;
        protected UIPanelState m_state = UIPanelState.None;
        public event StateChangedEvent StateChangedEvent;

        public Transform M_TR { get { return m_Tr; } }

        public int GetUIType()
        {
            return m_uiType;
        }

        #endregion
        protected virtual void Awake()
        {
            // 获取 UI
            m_Tr = transform;
            m_closeBtn = ToolsComponent.FindChildCom<Button>(m_Tr, "Close");
            if (m_closeBtn != null)
            {
                m_closeBtn.onClick.AddListener(CloseOnClick);
            }

            GetView();
            State |= UIPanelState.InitialUI;

            if (isRootPanel)
            {
                SetData(-1, null);
            }
        }

        protected virtual void OnDestroy()
        {
            State = UIPanelState.None;
        }

        public virtual void SetData(int _uiType, object _data)
        {
            // 处理数据
            this.m_uiType = _uiType;
            m_data = _data;
            GetData();
            State |= UIPanelState.InitialData;
        }
        public UIPanelState State
        {
            get { return this.m_state; }
            set
            {
                m_state = value;
                StateChange(m_state);
            }
        }

        private void StateChange(UIPanelState _state)
        {
            if (null != StateChangedEvent)
            {
                StateChangedEvent(this, _state);
            }

            // UI、数据 都初始化刷新面板 
            if (IsInitComplete())
            {
                InitComplete();
            }
        }

        /// <summary>
        /// 面板：UI、数据 初始化完成
        /// </summary>
        /// <returns></returns>
        private bool IsInitComplete()
        {
            bool p_isInitialUI = ((m_state & UIPanelState.InitialUI) == UIPanelState.InitialUI);
            bool p_isInitialData = ((m_state & UIPanelState.InitialData) == UIPanelState.InitialData);
            return p_isInitialUI && p_isInitialData;
        }

        //void Update()
        //{
        //    if (EnumObjectState.Ready == State)
        //        OnUpdate(Time.deltaTime);
        //}

        private void CloseOnClick()
        {
            Close();
        }

        public void Close()
        {
            UIManage.Instance.CloseUI(m_uiType);
        }

        public void Release()
        {
            OnRelease();
            State = UIPanelState.None;
        }

        protected virtual void OnRelease()
        {
            Destroy(gameObject);
        }

        public virtual void InitComplete()
        {
            RefreshFixed();
        }
    }
}