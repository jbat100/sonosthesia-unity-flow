using System.Collections.Generic;
using UnityEngine;

namespace Sonosthesia.Flow
{
    public class SignalGroup<T> : MonoBehaviour where T : struct
    {
        private Dictionary<string, Signal<T>> _signals;

        protected void Awake()
        {
            foreach (Transform child in transform)
            {
                Signal<T> signal = child.GetComponent<Signal<T>>();
                if (!signal)
                {
                    continue;
                }
                if (_signals.ContainsKey(child.name))
                {
                    Debug.LogError($"Unexpected multiple signals for key {child.name}");
                    continue;
                }
                _signals[child.name] = signal;
            }
        }

        public Signal<T> GetSignal(string key)
        {
            return _signals[key];
        }
    }    
}


