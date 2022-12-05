using System;
using UniRx;
using UnityEngine;

namespace Sonosthesia.Flow
{
    public abstract class SimpleFloatAdaptor<TTarget> : Adaptor<float, TTarget> where TTarget : struct
    {
        [SerializeField] private float _offset = 0f;
        
        [SerializeField] private float _scale = 1f;

        protected abstract TTarget Map(float value);

        protected override IDisposable Setup(Signal<float> source)
        {
            return source.SignalObservable.Subscribe(value =>
            {
                float eased = Mathf.Lerp(0f, 1f, value);
                TTarget mapped = Map(eased * _scale + _offset);
                Broadcast(mapped);
            });
        }
    }
}