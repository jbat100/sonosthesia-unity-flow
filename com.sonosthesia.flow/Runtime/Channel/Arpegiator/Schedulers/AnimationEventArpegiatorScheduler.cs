using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sonosthesia.Flow
{
    public class AnimationEventArpegiatorScheduler : ArpegiatorScheduler
    {
        [SerializeField] private AnimationClip _clip;
        
        protected override void InternalTimeOffsets(List<float> buffer)
        {
            buffer.AddRange(_clip.events.Select(e => e.time));
        }
    }
}