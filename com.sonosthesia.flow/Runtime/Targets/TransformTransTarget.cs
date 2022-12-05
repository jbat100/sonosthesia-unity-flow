using Sonosthesia.Utils;

namespace Sonosthesia.Flow
{
    public class TransformTransTarget : Target<Trans>
    {
        protected override void Apply(Trans value)
        {
            value.ApplyTo(transform);
        }
    }
}