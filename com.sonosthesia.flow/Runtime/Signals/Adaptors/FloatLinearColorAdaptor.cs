using UnityEngine;

namespace Sonosthesia.Flow
{
    public class FloatLinearColorAdaptor : SimpleFloatAdaptor<Color>
    {
        [SerializeField] [ColorUsage(true, true)] private Color _start;
        [SerializeField] [ColorUsage(true, true)] private Color _end;

        protected override Color Map(float value) => Color.Lerp(_start, _end, value);
    }
}