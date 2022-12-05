using System;
using UniRx;
using UnityEngine;

namespace Sonosthesia.Flow
{
    public abstract class DynamicChannelLink<TSource, TTarget> : MonoBehaviour where TSource : struct where TTarget : struct
    {
        [SerializeField] private bool _log;
        
        [SerializeField] private DynamicChannel<TSource> _source;

        [SerializeField] private DynamicChannel<TTarget> _target;

        private IDisposable _subscription;

        protected void OnEnable()
        {
            _subscription?.Dispose();
            if (_source)
            {
                _subscription = _source.StreamObservable.Subscribe(stream =>
                {
                    if (_log)
                    {
                        _target.Pipe(stream.Do(source => Debug.Log($"{this} mapping from {source}"))
                            .Select(Map)
                            .Do(mapped => Debug.Log($"{this} mapped to {mapped}")));
                    }
                    else
                    {
                        _target.Pipe(stream.Select(Map));
                    }

                });
            }
        }

        protected void OnDisable()
        {
            _subscription?.Dispose();
            _subscription = null;
        }

        protected abstract TTarget Map(TSource payload);
    }
}