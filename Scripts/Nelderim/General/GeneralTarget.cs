using System;

namespace Server.Targeting
{
    public class GeneralTarget : Target
    {
        private Action<Mobile, object> _onTarget;

        protected override void OnTarget( Mobile from, object targeted )
        {
            _onTarget(from, targeted);
        }

        public GeneralTarget( int range, bool allowGround, TargetFlags flags, Action<Mobile, object> onTarget )
            : base(range, allowGround, flags)
        {
            _onTarget = onTarget;
        }
    }
}