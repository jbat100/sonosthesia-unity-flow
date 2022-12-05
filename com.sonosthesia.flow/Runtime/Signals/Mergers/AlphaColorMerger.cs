using System;
using UniRx;
using UnityEngine;

namespace Sonosthesia.Flow.Mergers
{
    public class AlphaColorMerger : Signal<Color>
    {
        [SerializeField] private Signal<float> _alphaSignal;

        [SerializeField] private Signal<Color> _colorSignal;

        private IDisposable _subscription;

        protected void OnEnable()
        {
            _subscription?.Dispose();
            if (_alphaSignal && _colorSignal)
            {
                _alphaSignal.SignalObservable
                    .CombineLatest(_colorSignal.SignalObservable, (a, c) => new Color(c.r, c.g, c.b, a))
                    .Subscribe(Broadcast);
            }
        }

        protected void OnDisable()
        {
            _subscription?.Dispose();
            _subscription = null;
        }
    }
}