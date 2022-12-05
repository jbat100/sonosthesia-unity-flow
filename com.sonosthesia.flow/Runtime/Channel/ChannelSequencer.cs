using System;
using UnityEngine;

namespace Sonosthesia.Flow
{
    public class ChannelSequencer<T> : MonoBehaviour where T : struct
    {
        [SerializeField] private DynamicChannel<T> _target;

        protected void Sequence(IObservable<T> stream)
        {
            _target.Pipe(stream);
        }
    }
}