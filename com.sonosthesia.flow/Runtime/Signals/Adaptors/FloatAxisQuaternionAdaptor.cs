using UnityEngine;

namespace Sonosthesia.Flow
{
    public class FloatAxisQuaternionAdaptor : SimpleFloatAdaptor<Quaternion>
    {
        [SerializeField] private Vector3 _axis = Vector3.up;

        protected override Quaternion Map(float value) => Quaternion.AngleAxis(value, _axis);
    }
}