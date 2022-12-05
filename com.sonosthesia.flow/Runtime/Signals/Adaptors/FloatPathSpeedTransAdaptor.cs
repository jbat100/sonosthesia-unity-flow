using Sonosthesia.Utils;
using UnityEngine;

namespace Sonosthesia.Flow
{
    public class FloatPathSpeedTransAdaptor : SpeedFloatAdaptor<Trans>
    {
        [SerializeField] private Path _path;
        
        protected override Trans Map(float value)
        {
            return new Trans(_path.Position(value, true), _path.Rotation(value, true), Vector3.one);
        }
    }
}