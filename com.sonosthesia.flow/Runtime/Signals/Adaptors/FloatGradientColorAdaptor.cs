using UnityEngine;

namespace Sonosthesia.Flow
{
    public class FloatGradientColorAdaptor : SimpleFloatAdaptor<Color>
    {
        [SerializeField] private Gradient _gradient;

        protected override Color Map(float value) => _gradient.Evaluate(value);
    }
}