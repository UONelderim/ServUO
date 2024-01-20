using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarArmysPaeonSpell : AvatarSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
			"Witalność Armii",
			"Vitalium Engrevo Maxi",
			//SpellCircle.First,
			212,
			9041
		);

		public override SpellCircle Circle => SpellCircle.First;
		public override double CastDelay => 2;
		public override int RequiredTithing => 50;
		public override double RequiredSkill => 60.0;
		public override int RequiredMana => 15;

		public AvatarArmysPaeonSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			Scroll?.Consume();
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				var targets = new List<Mobile>();

				IPooledEnumerable eable = Caster.GetMobilesInRange(3);
				foreach (Mobile m in eable)
				{
					if (Caster.CanBeBeneficial(m, false, true) && !(m is Golem))
						targets.Add(m);
				}

				eable.Free();

				foreach (var m in targets)
				{
					int rounds = (int)(Caster.Skills[SkillName.Anatomy].Value * 0.5);

					new ExpireTimer(m, rounds, TimeSpan.FromSeconds(5)).Start();

					m.FixedParticles(0x376A, 9, 32, 5030, 0x21, 3, EffectLayer.Waist);
				}
			}

			FinishSequence();
		}

		private class ExpireTimer : Timer
		{
			private Mobile m_Mobile;
			private int m_Round;
			private int m_Totalrounds;

			public ExpireTimer(Mobile m, int totalrounds, TimeSpan delay) : base(delay, delay)
			{
				m_Mobile = m;
				m_Round = 0;
				m_Totalrounds = totalrounds;
			}

			protected override void OnTick()
			{
				if (m_Mobile == null || m_Mobile.Deleted)
				{
					Stop();
					return;
				}

				m_Mobile.Hits += 5;

				if (m_Round >= m_Totalrounds)
				{
					m_Mobile.SendMessage("Efekt modlitwy wygasa.");
					Stop();
				}
				else
				{
					m_Round += 1;
				}
			}
		}
	}
