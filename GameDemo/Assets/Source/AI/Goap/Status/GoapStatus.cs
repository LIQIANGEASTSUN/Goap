using System.Collections.Generic;

namespace Goap
{
    public class GoapStatus
    {
        private Dictionary<GoapCondition, object> statusDic = new Dictionary<GoapCondition, object>();

        public GoapStatus()
        {
            statusDic = new Dictionary<GoapCondition, object>();
        }

        public Dictionary<GoapCondition, object> Status()
        {
            return statusDic;
        }

        public void AddState(GoapCondition key, object value)
        {
            statusDic[key] = value;
        }

        public object GetState(GoapCondition key)
        {
            object value = null;
            if (statusDic.TryGetValue(key, out value))
            {
                return value;
            }
            return value;
        }

        public void RemoveState(GoapCondition key)
        {
            if (statusDic.ContainsKey(key))
            {
                statusDic.Remove(key);
            }
        }

        public GoapStatus Clone()
        {
            GoapStatus goapStatus = new GoapStatus();
            foreach (var pair in statusDic)
            {
                goapStatus.AddState(pair.Key, pair.Value);
            }

            return goapStatus;
        }

        /// <summary>
        /// 包含于 a
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool IsContainIn(GoapStatus a)
        {
            Dictionary<GoapCondition, object> aStatusDic = a.Status();

            bool isContentIn = true;
            foreach (var pair in statusDic)
            {
                object aValue = null;
                if (aStatusDic.TryGetValue(pair.Key, out aValue))
                {
                    if (Equals(pair.Value, aValue))
                    {
                        continue;
                    }
                }

                isContentIn = false;
                break;
            }

            return isContentIn;
        }

        /// <summary>
        /// 有任何一个值包含在 a 中
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool IsAnyContainIn(GoapStatus a)
        {
            Dictionary<GoapCondition, object> aStatusDic = a.Status();

            foreach (var pair in aStatusDic)
            {
                object aValue = null;
                if (aStatusDic.TryGetValue(pair.Key, out aValue))
                {
                    if (Equals(pair.Value, aValue))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 将 a 的值 加到自身
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public void AddFromStatus(GoapStatus a)
        {
            foreach (var pair in a.Status())
            {
                AddState(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// 将 a 和 自己 key 相同且值也相同的元素移除
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public void RemoveFromStatus(GoapStatus a)
        {
            foreach (var pair in a.Status())
            {
                object value = null;
                if (statusDic.TryGetValue(pair.Key, out value))
                {
                    if (Equals(pair.Value, value))
                    {
                        RemoveState(pair.Key);
                    }
                }
            }
        }
    }
}