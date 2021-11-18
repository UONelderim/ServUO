using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using System;

namespace Server.SkillHandlers
{
    public class Anatomy
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Anatomy].Callback = OnUse;
        }

        public static TimeSpan OnUse(Mobile m)
        {

            if (m.HasGump(typeof(CreatureAnatomyGump)))
            {
                m.SendLocalizedMessage(500118); // You must wait a few moments to use another skill.
            }
	    else
	    {
		m.Target = new InternalTarget();
            	m.SendLocalizedMessage(500321); // Whom shall I examine?

	    }

            return TimeSpan.FromSeconds(1.0);
        }

        private class InternalTarget : Target
        {

            private static void SendGump(Mobile from, BaseCreature c)
            {
                from.CheckTargetSkill(SkillName.Anatomy, c, 0.0, 100.0);

                  if (from is PlayerMobile)
                  {
                      Timer.DelayCall(TimeSpan.FromSeconds(1), () => 
              {
                              BaseGump.SendGump(new CreatureAnatomyGump((PlayerMobile)from, c));
                          });
            }
        }

            private static void SendPlayerGump(Mobile from, PlayerMobile p)
                {
                    if (from.CheckTargetSkill(SkillName.Anatomy, p, 100.0, 120.0));
                    {
                         if (from is PlayerMobile)
                        {
                            Timer.DelayCall(TimeSpan.FromSeconds(1), () => 
                            {
                              BaseGump.SendGump(new PlayerAnatomyGump((PlayerMobile)from, p));
                            });
                        }
            
                    else
                    {


                        {
                    BaseGump.SendGump(new PlayerLesserAnatomyGump((PlayerMobile)from, p));
                        }

                    }
                }
        }

            private static void Check(Mobile from, BaseCreature c, double min)
            {
                if (from.CheckTargetSkill(SkillName.Anatomy, c, min, 100.0))
                    SendGump(from, c);
                else
                    from.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 1042666, from.NetState); // You can not quite get a sense of their physical characteristics.
            }


/////////////////////////////////////////
            public InternalTarget()
                : base(8, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (from == targeted)
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 500324); // You know yourself quite well enough already.
                }
                else if (targeted is TownCrier)
                {
                    ((TownCrier)targeted).PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500322, from.NetState); // This person looks fine to me, though he may have some news...
                }
                else if (targeted is BaseVendor && ((BaseVendor)targeted).IsInvulnerable)
                {
                    ((BaseVendor)targeted).PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500326, from.NetState); // That can not be inspected.
                }
                else if (targeted is Mobile)
                {
                    Mobile targ = (Mobile)targeted;

		    if (from.CheckTargetSkill(SkillName.Anatomy, targ, 0, 100))
                    {
			if(targeted is BaseCreature)
			{
				BaseCreature c = (BaseCreature)targeted;

				if(!c.Body.IsAnimal && c.Tamable == false)
					SendGump(from, c);
				else
					from.SendMessage("Examaning this creature requires Animal Lore.");
			}
			else if( targeted is PlayerMobile)
			{
				PlayerMobile p = (PlayerMobile)targeted;
				SendPlayerGump(from, p);
			}
                    		//targ.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 1042666, from.NetState); // You can not quite get a sense of their physical characteristics.

                    }
                    else
                    {
                        targ.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 1042666, from.NetState); // You can not quite get a sense of their physical characteristics.
                    }
                }
                else if (targeted is Item)
                {
                    ((Item)targeted).SendLocalizedMessageTo(from, 500323, ""); // Only living things have anatomies!
                }
            }
        }
    }
}
