using System;
using UniRx;
using UnityEngine;

namespace Sonosthesia.Flow
{
    public abstract class SignalLerper<T> : Signal<T> where T : struct
    {
        [SerializeField] private Signal<T> _source;

        [SerializeField] private float _lerp;
        
        private T? _current;
        private T? _target;

        private IDisposable _subscription;
        
        protected abstract T Lerp(T current, T target, float lerp);

        protected void OnEnable()
        {
            _subscription?.Dispose();
            if (_source)
            {
                _subscription = _source.SignalObservable.Subscribe(value => _target = value);
            }
        }

        protected void OnDisable()
        {
            _subscription?.Dispose();
            _current = null;
            _target = null;
        }
        

        protected void Update()
        {
            if (_target.HasValue)
            {
                _current = _current.HasValue ? Lerp(_current.Value, _target.Value, _lerp) : _target.Value;
                Broadcast(_current.Value);
            }
        }
    }
}