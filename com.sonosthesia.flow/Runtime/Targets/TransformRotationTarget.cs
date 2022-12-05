using UnityEngine;

namespace Sonosthesia.Flow
{
    public class TransformRotationTarget : Target<Quaternion>
    {
        protected override void Apply(Quaternion value) => transform.rotation = value;
    }
}