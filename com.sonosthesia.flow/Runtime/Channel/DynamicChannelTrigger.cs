using System;
using UniRx;
using UnityEngine;

namespace Sonosthesia.Flow
{
    public class DynamicChannelTrigger<T> : MonoBehaviour where T : struct
    {
        [SerializeField] private TriggerFloatSignal _triggerFloatSignal;

        [SerializeField] private DynamicChannel<T> _channel;

        [SerializeField] private Selector<T> _scaleSelector;

        private IDisposable _subscription;
    
        protected void OnEnable()
        {
            _subscription?.Dispose();
            _subscription = _channel.StreamObservable
                .SelectMany(stream => stream.First())
                .Subscribe(note =>
                {
                    float scale = _scaleSelector.Select(note);
                    _triggerFloatSignal.Trigger(scale);
                });
        }

        protected void OnDisable() => _subscription?.Dispose();

    }
}


