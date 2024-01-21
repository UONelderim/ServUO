using System;
using System.Collections.Generic;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.DeathKnight
{
	public class SuccubusSkinSpell : DeathKnightSpell
	{
		private static SpellInfo m_Info = new(
			"Skora Sukkuba",
			"Erinyes Carnem",
			236,
			9011
		);

		private static readonly Dictionary<Mobile, Timer> _Table = new();
		public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(3);
		public override int RequiredTithing => 49;
		public override int RequiredMana => 32;
		public override double RequiredSkill => 68.0;

		public SuccubusSkinSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
		}

		public static bool HasEffect(Mobile m)
		{
			return _Table[m] != null;
		}

		public static void RemoveEffect(Mobile m)
		{
			var t = _Table[m];
			if (t == null) return;
			
			t.Stop();
			_Table.Remove(m);
		}

		public override void OnCast()
		{
			Caster.BeginTarget(12,
				true,
				TargetFlags.None,
				(_, o) =>
				{
					if (o is Mobile)
					{
						Target((Mobile)o);
					}
				});
		}

		public void Target(Mobile m)
		{
			if (!Caster.CanSee(m))
			{
				Caster.SendLocalizedMessage(500237); // Target can not be seen.
			}
			if (_Table.ContainsKey(m))
			{
				Caster.LocalOverheadMessage(MessageType.Regular, 0x481, false, "Ten cel juz korzysta z tego efektu.");
			}
			else if (m.Poisoned || Items.MortalStrike.IsWounded(m))
			{
				Caster.LocalOverheadMessage(MessageType.Regular, 0x3B2, (Caster == m) ? 1005000 : 1010398);
			}
			else if (m.Hits >= m.HitsMax)
			{
				Caster.SendLocalizedMessage(500955); // "Jego stan zdrowia jest idealny!"
			}
			else if (m is BaseCreature { IsAnimatedDead: true })
			{
				Caster.SendLocalizedMessage(1061654); // "Ta istota nie jest zywa, nie mozesz jej leczyc."
			}
			else if (m.IsDeadBondedPet)
			{
				Caster.SendLocalizedMessage(1060177); // "Nie mozesz wyleczyc martwego stworzenia."
			}
			else if (CheckBSequence(m, false))
			{
				SpellHelper.Turn(Caster, m);

				var buffLength = GetKarmaPower(Caster);
				_Table[m] = new InternalTimer(m, buffLength);
				_Table[m].Start();
				m.PlaySound(0x202);
				m.FixedParticles(0x3779, 1, 46, 9502, 5, 3, EffectLayer.Waist);
				m.SendMessage("Twa skora zmienia sie, powodujac zasklepianie sie ran.");

				
				BuffInfo.AddBuff(m,
					new BuffInfo(BuffIcon.CorpseSkin, 1044123, 1044118, TimeSpan.FromSeconds(buffLength), m));
			}

			FinishSequence();
		}

		public static int PlayerLevelMod(int value, Mobile m)
		{
			// THIS MULTIPLIES AGAINST THE RAW STAT TO GIVE THE RETURNING HIT POINTS, MANA, OR STAMINA
			// SO SETTING THIS TO 2.0 WOULD GIVE THE CHARACTER HITS POINTS EQUAL TO THEIR STRENGTH x 2
			// THIS ALSO AFFECTS BENEFICIAL SPELLS AND POTIONS THAT RESTORE HEALTH, STAMINA, AND MANA

			double mod = 1.0;
			if (m is PlayerMobile) { mod = 1.25; } // ONLY CHANGE THIS VALUE

			value = (int)(value * mod);
			if (value < 0) { value = 1; }

			return value;
		}


		private class InternalTimer : Timer
		{
			public const int IntervalSeconds = 4;
			private Mobile target;
			private int maxTicks;
			private int ticks;

			public InternalTimer(Mobile m, double buffLength) : base(TimeSpan.Zero, TimeSpan.FromSeconds(IntervalSeconds))
			{
				target = m;
				maxTicks = (int)(buffLength / IntervalSeconds) + 1;
				ticks = 0;
			}

			protected override void OnTick()
			{
				if (target.Deleted || !target.CheckAlive())
				{
					RemoveEffect(target);
				}
				
				double heal = PlayerLevelMod(Utility.RandomMinMax(5, 10), target);
				target.Heal((int)heal);
				target.FixedParticles(0x3779, 1, 46, 9502, 5, 3, EffectLayer.Waist);
				ticks++;

				if (ticks >= maxTicks)
				{
					RemoveEffect(target);
				}
			}
		}
	}
}
