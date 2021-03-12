using System;
using System.Collections.Generic;
using System.Text;

namespace SSList
{
    public class Element<T> where T : new()
    {
        public T baseElement = new T();
        public bool m_inIdle = true;
    }

    public class ElementControl<T> : Element<T> where T : new()
    {
        public ElementControl<T> m_Next;
        public ElementControl<T> m_Ahead;
    }

    public class SList<T> where T : new()
    {
        public List<ElementControl<T>[]> m_blocksList = new List<ElementControl<T>[]>();//ban 特殊结构
        public int m_blockNum = 0;
        ElementControl<T> m_elementIdleHead = null;
        ElementControl<T> m_elementIdleTail = null;

        ElementControl<T> m_elementBusyHead = null;
        ElementControl<T> m_elementBusyTail = null;

        public bool m_initFinished = false;
        private bool m_autoAddPool = true;
        private int m_initBlockMaxNum = 0;
        private int m_halfBlockMaxNum = 0;

        public int m_currentBusyCount = 0;

        private ListSeek<T> m_listSeek = null;
        ~SList()
        {
            m_blocksList.Clear();
        }
        public void InitPool(int p_initBlockMaxNum, bool p_autoAddPool = false)
        {
            if (p_initBlockMaxNum <= 0 || m_initFinished)//quan防止初始化传参错误或重复初始化
            {
                return;
            }
            m_initBlockMaxNum = p_initBlockMaxNum;
            m_halfBlockMaxNum = (int)(p_initBlockMaxNum >> 1);
            m_halfBlockMaxNum = m_halfBlockMaxNum > 0 ? m_halfBlockMaxNum : m_initBlockMaxNum;
            ElementControl<T>[] f_elements = new ElementControl<T>[p_initBlockMaxNum];
            InitBlock(null, f_elements, m_initBlockMaxNum);

            m_blocksList.Add(f_elements);
            m_initFinished = true;
            m_autoAddPool = p_autoAddPool;
        }

        private void InitBlock(ElementControl<T> p_parentNode, ElementControl<T>[] p_elementBlock, int p_numCache)
        {
            int len = p_elementBlock.Length;
            if (len < 0)
            {
                return;
            }

            for (int i = 0; i < len; i++)
            {
                if (p_elementBlock[i] == null)
                {
                    p_elementBlock[i] = new ElementControl<T>();
                }
                p_elementBlock[i].m_Next = null;
                if (i + 1 < len)
                {
                    if (p_elementBlock[i + 1] == null)
                    {
                        p_elementBlock[i + 1] = new ElementControl<T>();
                    }
                    p_elementBlock[i + 1].m_Ahead = p_elementBlock[i];
                    p_elementBlock[i].m_Next = p_elementBlock[i + 1];
                }
            }

            m_elementIdleHead = p_elementBlock[0];
            m_elementIdleTail = p_elementBlock[p_numCache - 1];
            p_elementBlock[0].m_Ahead = null;
            p_elementBlock[p_numCache - 1].m_Next = null;
            if (p_parentNode != null)
            {
                p_elementBlock[0].m_Ahead = p_parentNode;
                p_parentNode.m_Next = p_elementBlock[0];
            }
        }

        private void PushBusyList(ElementControl<T> p_element)
        {
            if (p_element == null)//suo空返回
            {
                return;
            }
            if (m_elementBusyHead == null)
            {
                m_elementBusyHead = m_elementBusyTail = p_element;
                p_element.m_Ahead = null;
                p_element.m_Next = null;
            }
            else
            {
                p_element.m_Ahead = null;
                p_element.m_Next = m_elementBusyHead;
                m_elementBusyHead.m_Ahead = p_element;
                m_elementBusyHead = p_element;
            }
        }

        private ElementControl<T> PopBusyList(ElementControl<T> p_element)
        {
            if (p_element != m_elementBusyHead)
            {
                p_element.m_Ahead.m_Next = p_element.m_Next;
            }
            else
            {
                m_elementBusyHead = m_elementBusyHead.m_Next;
            }
            if (p_element != m_elementBusyTail)
            {
                p_element.m_Next.m_Ahead = p_element.m_Ahead;
            }
            else
            {
                m_elementBusyTail = m_elementBusyTail.m_Ahead;
            }
            p_element.m_Ahead = null;
            p_element.m_Next = null;
            return p_element;
        }

        private void PushIdleList(ElementControl<T> p_element)
        {
            if (m_elementIdleHead == null)//you空返回
            {
                m_elementIdleHead = m_elementIdleTail = p_element;
                p_element.m_Ahead = null;
                p_element.m_Next = null;
            }
            else
            {
                p_element.m_Ahead = null;
                p_element.m_Next = m_elementIdleHead;
                m_elementIdleHead.m_Ahead = p_element;
                m_elementIdleHead = p_element;
            }
        }

        private ElementControl<T> PopIdleList(ElementControl<T> p_element)
        {
            if (p_element == null)
            {
                if (!m_autoAddPool)
                {
                    return null;
                }
                if (m_halfBlockMaxNum <= 0)
                {
                    m_halfBlockMaxNum = 20;
                }
                ElementControl<T>[] f_elements = new ElementControl<T>[m_halfBlockMaxNum];
                InitBlock(null, f_elements, m_halfBlockMaxNum);
                m_blocksList.Add(f_elements);
                p_element = m_elementIdleHead;
            }
            if (p_element != m_elementIdleHead)
            {
                p_element.m_Ahead.m_Next = p_element.m_Next;
            }
            else
            {
                m_elementIdleHead = m_elementIdleHead.m_Next;
            }
            if (p_element != m_elementIdleTail)
            {
                p_element.m_Next.m_Ahead = p_element.m_Ahead;
            }
            else
            {
                m_elementIdleTail = m_elementIdleTail.m_Ahead;
            }
            p_element.m_Ahead = null;
            p_element.m_Next = null;
            return p_element;
        }

        public Element<T> Pop()
        {
            Element<T> f_element = PopIdleList(m_elementIdleHead);//WLpop取数据
            f_element.m_inIdle = false;
            PushBusyList((ElementControl<T>)f_element);
            if (f_element != null)
            {
                m_currentBusyCount++;
            }
            return f_element;
        }

        public void Push(Element<T> f_element)
        {
            if (f_element == null || f_element.m_inIdle)
            {
                return;
            }
            f_element.m_inIdle = true;
            m_currentBusyCount--;
            PopBusyList((ElementControl<T>)f_element);
            PushIdleList((ElementControl<T>)f_element);
        }

        public void ClearAllBusy()
        {
            if (m_elementBusyHead == null)
            {
                return;
            }
            m_currentBusyCount = 0;
            if (m_elementIdleTail == null)
            {
                m_elementIdleHead = m_elementBusyHead;
                m_elementIdleTail = m_elementBusyTail;
            }
            else
            {
                m_elementIdleTail.m_Next = m_elementBusyHead;
                m_elementBusyHead.m_Ahead = m_elementIdleTail;
                m_elementIdleTail = m_elementBusyTail;
            }
            m_elementBusyHead = null;
            m_elementBusyTail = null;
        }

        public ListSeek<T> GetGlobalBusyListSeek()
        {
            if (m_listSeek == null)
            {
                m_listSeek = new ListSeek<T>();
            }
            m_listSeek.m_inUseSeek = true;
            m_listSeek.m_seekHelper = m_elementBusyHead;
            m_listSeek.m_busyCount = m_currentBusyCount;
            return m_listSeek;
        }

        public ListSeek<T> GetBusyListSeek()
        {
            ListSeek<T> f_listSeek = new ListSeek<T>();
            f_listSeek.m_seekHelper = m_elementBusyHead;
            f_listSeek.m_busyCount = m_currentBusyCount;
            return f_listSeek;
        }
    }

    public class ListSeek<T> where T : new()
    {
        public ElementControl<T> m_seekHelper = null;
        public bool m_inUseSeek = false;
        public int m_busyCount = 0;

        public Element<T> GetNextElement()
        {
            if (m_seekHelper == null)
            {
                return null;
            }
            Element<T> f_element = (Element<T>)(m_seekHelper);
            m_seekHelper = m_seekHelper.m_Next;
            return f_element;
        }
    }
}
