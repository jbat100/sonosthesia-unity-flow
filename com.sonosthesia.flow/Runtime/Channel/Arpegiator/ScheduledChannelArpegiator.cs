using System;
using UnityEngine;
using UniRx;

namespace Sonosthesia.Flow
{
    public abstract class ScheduledChannelArpegiator<T> : DynamicChannelArpegiator<T> where T : struct
    {
        [SerializeField] private ArpegiatorScheduler _scheduler;
        
        [SerializeField] private ArpegiatorModulator<T> _modulator;

        // Designed to handle a single 'note' stream from the source channel
        // TODO : use pooling
        protected abstract class StreamArpegiator
        {
            private T _startValue;
            private T _currentValue;
            private IDisposable _streamSubscription;
            private IDisposable _schedulerSubscription;
            private bool _first;
            private Subject<IObservable<T>> _arpegiations = new();

            // return observable fires when arpegiator is done
            public IObservable<IObservable<T>> Setup(IObservable<T> stream, 
                ArpegiatorScheduler scheduler, 
                ArpegiatorModulator<T> modulator,
                Func<T, T, T, T> follow,
                float duration)
            {
                Teardown();
                _schedulerSubscription = scheduler.Session().Subscribe(offset => Arpegiate(stream, offset, modulator, follow, duration));
                _arpegiations = new Subject<IObservable<T>>();
                _streamSubscription = stream.Subscribe(value =>
                {
                    if (!_first)
                    {
                        _first = true;
                        _startValue = value;
                    }
                    _currentValue = value;
                }, Teardown);
                return _arpegiations.AsObservable();
            }

            public void Teardown()
            {
                _first = false;
                _arpegiations?.OnCompleted();
                _arpegiations?.Dispose();
                _streamSubscription?.Dispose();
                _schedulerSubscription?.Dispose();
            }

            private void Arpegiate(IObservable<T> stream, float offset, ArpegiatorModulator<T> modulator, Func<T, T, T, T> follow, float duration)
            {
                T original = _currentValue;
                T arpegiated = modulator.Modulate(_startValue, offset);
                BehaviorSubject<T> subject = new BehaviorSubject<T>(arpegiated);
                _arpegiations.OnNext(subject);
                if (follow != null)
                {
                    stream.Select(current => follow(original, current, arpegiated)).Subscribe(subject);    
                }
                Observable.Timer(TimeSpan.FromSeconds(duration)).Subscribe(_ =>
                {
                    subject.OnCompleted();
                    subject.Dispose();
                });
            }

            // defaults to auto-destruction following source duration
            protected abstract float? Duration(float timeOffset);
            
        }

        protected override void HandleStream(IObservable<T> stream)
        {
            StreamArpegiator arpegiator = GetArpegiator();
            //arpegiator.Setup(stream).Subscribe(Pipe, () => ReleaseArpegiator(arpegiator));
        }

        protected abstract StreamArpegiator GetArpegiator();
        
        protected abstract void ReleaseArpegiator(StreamArpegiator arpegiator);

    }
}