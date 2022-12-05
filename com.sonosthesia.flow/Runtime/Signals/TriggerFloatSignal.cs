using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sonosthesia.Flow
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(TriggerFloatSignal))]
    public class TriggerFloatSignalEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TriggerFloatSignal signal = (TriggerFloatSignal)target;
            if(GUILayout.Button("Trigger"))
            {
                signal.Trigger(1f);
            }
        }
    }
#endif
    
    public enum AccumulationMode
    {
        None,
        Sum,
        Max,
        Min
    }
    
    public class TriggerFloatSignal : Signal<float>
    {
        [SerializeField] private FloatEnvelope _envelope;
        
        [SerializeField] private AccumulationMode _acumulationMode = AccumulationMode.Max;

        [SerializeField] private float _fade = 1f;

        [SerializeField] private float _rest;
        
        public void Trigger(float scale)
        {
            _entries.Add(new TriggerEntry(Time.time, scale, _fade, _envelope));
        }
        
        protected void Update()
        {
            _entries.ExceptWith(_entries.Where(entry => entry.Ended(Time.time)).ToList());
            Broadcast(_entries.Aggregate(_rest, (current, entry) => entry.Accumulate(Time.time, _acumulationMode, current)));
        }
        
        private class TriggerEntry
        {
            private float _startTime;
            private float _scale;
            private float _fade;
            private FloatEnvelope _envelope;
            private float _fadeTime;
            private float _endTime;
            
            public TriggerEntry(float startTime, float scale, float fade, FloatEnvelope envelope)
            {
                _startTime = startTime;
                _scale = scale;
                _fade = fade;
                _envelope = envelope;
                _fadeTime = _startTime + envelope.Duration();
                _endTime = _fadeTime + envelope.End() * _fade;
            }

            public bool Ended(float time) => time > _endTime;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="time">Absolute unity time</param>
            /// <returns></returns>
            public float GetValue(float time)
            {
                if (time > _endTime)
                {
                    return 0f;
                }
                
                float result = _envelope.Evaluate(time - _startTime);

                if (time > _fadeTime)
                {
                    result *= (_fadeTime - time) / (_endTime - _fadeTime);
                }

                return result;
            }

            public float Accumulate(float time, AccumulationMode accumulationMode, float current)
            {
                float value = GetValue(time);
                return accumulationMode switch
                {
                    AccumulationMode.Sum => current + value,
                    AccumulationMode.Max => Mathf.Max(current, value),
                    AccumulationMode.Min => Mathf.Min(current, value),
                    _ => throw new NotImplementedException()
                };
            }
        }
        
        private readonly HashSet<TriggerEntry> _entries = new HashSet<TriggerEntry>();
    }
}