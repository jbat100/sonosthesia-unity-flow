using UnityEngine;

namespace Sonosthesia.Flow
{
    public class LightColorTarget : Target<Color>
    {
        [SerializeField] private Light _light;

        protected void Awake()
        {
            if (!_light)
            {
                _light = GetComponent<Light>();
            }
        }

        protected override void Apply(Color value)
        {
            _light.color = value;
        }
    }
}