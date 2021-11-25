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
			if (m.HasGump(typeof(CreatureAnatomyGump)) ||
				m.HasGump(typeof(PlayerLesserAnatomyGump)) || m.HasGump(typeof(PlayerAnatomyGump)))
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
				else if (targeted is TownCrier tc)
				{
					tc.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500322, from.NetState); // This person looks fine to me, though he may have some news...
				}
				else if (targeted is BaseVendor bv && bv.IsInvulnerable)
				{
					bv.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500326, from.NetState); // That can not be inspected.
				}
				else if (targeted is Mobile m)
				{
					if (targeted is BaseCreature bc && (bc.Body.IsAnimal || bc.Tamable))
					{
						from.SendMessage("Examaning this creature requires Animal Lore.");
					}
					else if (from.CheckTargetSkill(SkillName.Anatomy, m, 0, 100))
					{
						SendGump(from, m);
					}
					else
					{
						m.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 1042666, from.NetState); // You can not quite get a sense of their physical characteristics.
					}
				}
				else if (targeted is Item item)
				{
					item.SendLocalizedMessageTo(from, 500323, ""); // Only living things have anatomies!
				}
			}

			private static void SendGump(Mobile from, Mobile targeted)
			{
				if (from is PlayerMobile)
					Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
					{
						if (targeted is BaseCreature bc)
							BaseGump.SendGump(new CreatureAnatomyGump((PlayerMobile)from, bc));
						else if (targeted is PlayerMobile tpm)
							if (from.Skills.Anatomy.Base >= 100.0)
								BaseGump.SendGump(new PlayerAnatomyGump((PlayerMobile)from, tpm));
							else
								BaseGump.SendGump(new PlayerLesserAnatomyGump((PlayerMobile)from, tpm));
					});
			}
		}
	}
}