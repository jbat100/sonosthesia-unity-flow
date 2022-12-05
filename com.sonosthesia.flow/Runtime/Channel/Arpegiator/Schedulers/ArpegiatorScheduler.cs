using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Sonosthesia.Flow
{
    public abstract class ArpegiatorScheduler : MonoBehaviour
    {
        private enum TimingMode
        {
            None,
            Seconds,
            Beats
        }

        private static List<float> _buffer = new ();

        [SerializeField] private bool _loop;

        // note this does not allow warping during session
        public IObservable<float> Session()
        {
            _buffer.Clear();
            float duration = _buffer[^1];
            TimeOffsets(_buffer);
            IObservable<float> session = _buffer
                .Select(ProcessOffset)
                .Select(offset => Observable.Timer(TimeSpan.FromSeconds(offset)).Select(_ => offset / duration))
                .Merge();
            if (_loop)
            {
                session = session.Repeat();
            }
            return session;
        }

        // add randomization or whatever
        protected virtual float ProcessOffset(float offset)
        {
            return offset;
        }

        // note : no need for duration, it's the last value in the buffer
        protected void TimeOffsets(List<float> buffer)
        {
            InternalTimeOffsets(buffer);
        }
        
        protected abstract void InternalTimeOffsets(List<float> buffer);
    }
}