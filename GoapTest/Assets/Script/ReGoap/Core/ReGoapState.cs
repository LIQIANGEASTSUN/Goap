using System;
using UnityEngine;
using System.Collections.Generic;

namespace ReGoap.Core
{
    public class ReGoapState
    {
        private Dictionary<string, object> values;

        private ReGoapState()
        {
            values = new Dictionary<string, object>();
        }

        private void Init(ReGoapState old)
        {
            values.Clear();
            if (old != null)
            {
                foreach (var pair in old.values)
                {
                    values[pair.Key] = pair.Value;
                }
            }
        }

        public void AddFromState(ReGoapState b)
        {
            foreach (var pair in b.values)
            {
                values[pair.Key] = pair.Value;
            }
        }

        public int Count
        {
            get { return values.Count; }
        }

        // a 中所有的值 b 中都有
        public static bool ProperlyInclude(ReGoapState a, ReGoapState b)
        {
            foreach(var pair in a.values)
            {
                if (!b.values.ContainsKey(pair.Key))
                {
                    return false;
                }

                if (!Equals(b.values[pair.Key], pair.Value))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool HasAny(ReGoapState effect, ReGoapState other)
        {
            foreach (var pair in other.values)
            {
                object effectValue;
                effect.GetValues().TryGetValue(pair.Key, out effectValue);
                if (Equals(effectValue, pair.Value))
                    return true;
            }
            return false;
        }


        // this method is more relaxed than the other, also accepts conflits that are fixed by "changes"
        public static bool HasAnyConflict( ReGoapState precond, ReGoapState other) // used only in backward for now
        {
            foreach (var pair in other.values)
            {
                object effectValue;
                precond.GetValues().TryGetValue(pair.Key, out effectValue);
                var otherValue = pair.Value;

                if (effectValue == null || Equals(otherValue, effectValue))
                {
                    continue;
                }

                return true;
            }
            return false;
        }

        // write differences in "difference"
        public ReGoapState MissingDifference(ReGoapState other)
        {
            ReGoapState difference = ReGoapState.Instantiate();

            foreach (var pair in values)
            {
                object otherValue;
                other.values.TryGetValue(pair.Key, out otherValue);
                if (!Equals(pair.Value, otherValue))
                {
                    difference.values[pair.Key] = pair.Value;
                }
            }

            return difference;
        }

        // keep only missing differences in values
        public void ReplaceWithMissingDifference(ReGoapState other)
        {
            Dictionary<string, object> buffer = values;
            values = new Dictionary<string, object>();
            values.Clear();

            foreach (var pair in buffer)
            {
                object otherValue;
                other.values.TryGetValue(pair.Key, out otherValue);
                if (!Equals(pair.Value, otherValue))
                {
                    values[pair.Key] = pair.Value;
                }
            }
        }

        public ReGoapState Clone()
        {
            return Instantiate(this);
        }
        
        public static ReGoapState Instantiate(ReGoapState old = null)
        {
            ReGoapState state = new ReGoapState();
           
            state.Init(old);
            return state;
        }

        public object Get(string key)
        {
            if (!values.ContainsKey(key))
                return default(object);
            return values[key];
        }

        public void Set(string key, object value)
        {
            values[key] = value;
        }

        public Dictionary<string, object> GetValues()
        {
            return values;
        }

        public void Clear()
        {
            values.Clear();
        }
    }
}


// this method is more relaxed than the other, also accepts conflits that are fixed by "changes"
//public bool HasAnyConflict(ReGoapState changes, ReGoapState other) // used only in backward for now
//{
//    foreach (var pair in other.values)
//    {
//        object thisValue;
//        values.TryGetValue(pair.Key, out thisValue);
//        object effectValue;
//        changes.values.TryGetValue(pair.Key, out effectValue);
//        var otherValue = pair.Value;

//        if (thisValue != null && !Equals(otherValue, thisValue) && !Equals(effectValue, thisValue))
//        {
//            return true;
//        }
//    }
//    return false;
//}
