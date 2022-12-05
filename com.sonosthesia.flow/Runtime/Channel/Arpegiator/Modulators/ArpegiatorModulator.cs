using System;
using UnityEngine;

namespace Sonosthesia.Flow
{
    [Serializable]
    public class FloatModulation
    {
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _randomization;
        [SerializeField] private float _scale;

        public float Modulate(float value, float offset)
        {
            float curve = _curve.Evaluate(offset);
            float randomization = (UnityEngine.Random.value - 0.5f) * _randomization * 2f;
            return (value + curve + randomization) * _scale;
        }
    }

    [Serializable]
    public class ColorModulation
    {
        [SerializeField] private Gradient _gradient;
        [SerializeField] private FloatModulation _r;
        [SerializeField] private FloatModulation _g;
        [SerializeField] private FloatModulation _b;
        [SerializeField] private FloatModulation _a;
        
        public Color Modulate(Color value, float offset)
        {
            Color gradient = _gradient.Evaluate(offset);
            float r = Mathf.Clamp(_r.Modulate(value.r, offset), 0f, 1f);
            float g = Mathf.Clamp(_g.Modulate(value.g, offset), 0f, 1f);
            float b = Mathf.Clamp(_g.Modulate(value.b, offset), 0f, 1f);
            float a = Mathf.Clamp(_g.Modulate(value.a, offset), 0f, 1f);
            return new Color(r, g, b, a);
        }
    }
    
    [Serializable]
    public class VectorModulation
    {
        [SerializeField] private float _randomization;
        [SerializeField] private FloatModulation _x;
        [SerializeField] private FloatModulation _y;
        [SerializeField] private FloatModulation _z;
        
        public Vector3 Modulate(Vector3 value, float offset)
        {
            Vector3 randomization = _randomization > 0 ? UnityEngine.Random.insideUnitSphere * _randomization : Vector3.zero;
            float x = _x.Modulate(value.x, offset);
            float y = _y.Modulate(value.y, offset);
            float z = _z.Modulate(value.z, offset);
            return new Vector3(x, y, z);
        }
    }
    
    public abstract class ArpegiatorModulator<T> : MonoBehaviour where T : struct
    {
        public abstract T Modulate(T original, float offset);
    }
}