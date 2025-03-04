#region References
using Server.Mobiles;
using System;
#endregion

namespace Server
{
    public static class SpeedInfo
    {
	    public static readonly double MinDelay = 0.1;
        public static readonly double MaxDelay = 0.5;
        public static readonly double MinDelayWild = 0.15;
        public static readonly double MaxDelayWild = 0.8;

        public static bool GetSpeeds(BaseCreature bc, ref double activeSpeed, ref double passiveSpeed)
        {
	        if (bc is BaseNelderimGuard)
		        return true;

	        if (bc.IsMonster)
	        {
		        activeSpeed = Math.Clamp(activeSpeed * 1.3f, MinDelayWild, MaxDelayWild);
		        passiveSpeed = Math.Clamp(passiveSpeed * 1.3f, MinDelayWild, MaxDelayWild);
	        }

            return true;
        }
        

        private static int GetMaxMovementDex(BaseCreature bc)
        {
            return bc.IsMonster ? 150 : 190;
        }

        public static bool InActivePVPCombat(BaseCreature bc)
        {
            return bc.Combatant != null && bc.ControlOrder != OrderType.Follow && bc.Combatant is PlayerMobile;
        }

        public static double TransformMoveDelay(BaseCreature bc, double delay)
        {
            double max = bc.IsMonster ? MaxDelayWild : MaxDelay;

            if (!bc.IsDeadPet && (bc.ReduceSpeedWithDamage || bc.IsSubdued))
            {
                double offset = bc.Stam / (double)bc.StamMax;

                if (offset < 1.0)
                {
                    delay += ((max - delay) * (1.0 - offset));
                }

                var hitsScalar = bc.Hits < (bc.HitsMax * 0.25f) ? 1.2 : 1.0;
	            delay *= hitsScalar;
            }

            if (delay > max)
            {
                delay = max;
            }

            return delay;
        }
    }
}
