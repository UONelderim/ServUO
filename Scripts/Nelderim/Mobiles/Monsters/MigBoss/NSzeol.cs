#region References

using System;
using System.Collections;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("Zgliszcza Pajaka")]
	public class NSzeol : BaseCreature
	{
		public override bool BardImmune { get { return true; } }
		public override double SwitchTargetChance { get { return 2.0; } }
		public override double DispelDifficulty { get { return 135.0; } }
		public override double DispelFocus { get { return 45.0; } }
		public override bool AutoDispel { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }

		public static Hashtable m_Table = new Hashtable();

		public static bool UnderWebEffect(Mobile m)
		{
			return m_Table.Contains(m);
		}

		[Constructable]
		public NSzeol() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Body = 173;
			Hue = 2835;
			Name = "Szeol - Przeklety Pajak";

			BaseSoundID = 0x183;

			SetStr(505, 800);
			SetDex(132, 160);
			SetInt(402, 600);
			SetHits(13000);
			SetStam(205, 300);

			SetDamage(21, 33);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Poison, 50);

			SetResistance(ResistanceType.Physical, 75, 80);
			SetResistance(ResistanceType.Fire, 60, 70);
			SetResistance(ResistanceType.Cold, 60, 70);
			SetResistance(ResistanceType.Poison, 100);
			SetResistance(ResistanceType.Energy, 60, 70);

			SetSkill(SkillName.MagicResist, 120.7, 140.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Wrestling, 97.6, 100.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 80;
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved)
			{
				corpse.DropItem(new Brimstone());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGotMeleeAttack(attacker);

			Mobile person = attacker;

			if (attacker is BaseCreature)
			{
				if (((BaseCreature)attacker).Summoned)
				{
					person = ((BaseCreature)attacker).SummonMaster;
				}
			}

			if (person == null)
			{
				return;
			}

			if (person is PlayerMobile) // && !UnderWebEffect( person ) )
			{
				Direction = GetDirectionTo(person);

				MovingEffect(person, 0xF7E, 10, 1, true, false, 0x496, 0);

				MephitisCocoon mCocoon = new MephitisCocoon((PlayerMobile)person);

				m_Table[person] = true;

				mCocoon.MoveToWorld(person.Location, person.Map);
				mCocoon.Movable = false;
			}
		}

		public NSzeol(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public class MephitisCocoon : Item
		{
			private readonly DelayTimer m_Timer;

			public MephitisCocoon(PlayerMobile target) : base(0x10da)
			{
				Weight = 1.0;
				int nCocoonID = (int)(3 * Utility.RandomDouble());
				ItemID = 4314 + nCocoonID; // is this correct itemid?

				target.Frozen = true;
				target.Hidden = true;

				target.SendLocalizedMessage(1042555); // You become entangled in the spider web.

				m_Timer = new DelayTimer(this, target);
				m_Timer.Start();
			}

			public MephitisCocoon(Serial serial) : base(serial)
			{
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.Write(0); // version
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();
			}
		}

		private class DelayTimer : Timer
		{
			private readonly PlayerMobile m_Target;
			private readonly MephitisCocoon m_Cocoon;

			private int m_Ticks;

			public DelayTimer(MephitisCocoon mCocoon, PlayerMobile target) : base(TimeSpan.FromSeconds(1.0),
				TimeSpan.FromSeconds(1.0))
			{
				m_Target = target;
				m_Cocoon = mCocoon;

				m_Ticks = 0;
			}

			protected override void OnTick()
			{
				m_Ticks++;

				if (!m_Target.Alive)
				{
					FreeMobile(true);
					return;
				}

				if (m_Ticks != 6)
				{
					return;
				}

				FreeMobile(true);
			}

			public void FreeMobile(bool Recycle)
			{
				if (!m_Target.Deleted)
				{
					m_Target.Frozen = false;
					m_Target.SendLocalizedMessage(1042532); // You free yourself from the web!

					NMephitis.m_Table.Remove(m_Target);

					if (m_Target.Alive)
					{
						m_Target.Hidden = false;
					}
				}

				if (Recycle)
				{
					m_Cocoon.Delete();
				}

				this.Stop();
			}
		}
	}
}
