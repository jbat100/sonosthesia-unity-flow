using System;
using UnityEngine;

namespace Sonosthesia.Flow
{
    public abstract class FloatSignalOperator : Signal<float>
    {
        [SerializeField] private Signal<float> _source;

        private IDisposable _subscription;
        
        protected abstract IDisposable Setup(Signal<float> source);

        protected void OnEnable()
        {
            _subscription?.Dispose();
            _subscription = Setup(_source);
        }

        protected void OnDisable()
        {
            _subscription?.Dispose();
            _subscription = null;
        }
    }
}