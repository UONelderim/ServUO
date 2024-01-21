#region References

using System;
using System.Collections.Generic;
using Server.Spells;
using Server.Targeting;

#endregion

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadSwarmOfInsectsSpell : UndeadSpell
	{
		private static readonly SpellInfo m_Info = new(
			"Chmara Insektów",
			"Ess Laah Ohm En Sec Tia",
			263,
			9032,
			false,
			Reagent.Garlic,
			Reagent.Nightshade,
			CReagent.DestroyingAngel
		);

		public override SpellCircle Circle => SpellCircle.Seventh;
		public override double CastDelay => 2.0;
		public override double RequiredSkill => 85.0;
		public override int RequiredMana => 10;

		public UndeadSwarmOfInsectsSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
		}

		public override void OnCast()
		{
			Caster.BeginTarget(12,
				true,
				TargetFlags.None,
				(_, o) =>
				{
					if (o is Mobile m)
						Target(m);
				});
		}

		public void Target(Mobile m)
		{
			if (CheckHSequence(m))
			{
				SpellHelper.Turn(Caster, m);

				SpellHelper.CheckReflect(this, Caster, ref m);

				CheckResisted(m);

				m.FixedParticles(0x91B, 1, 240, 9916, 38, 3, EffectLayer.Head);
				m.PlaySound(0x1E5);

				double damage = ((Caster.Skills[CastSkill].Value - m.Skills[CastSkill].Value) / 10) + 3;

				if (damage < 1)
					damage = 1;

				if (m_Table.ContainsKey(m))
				{
					damage /= 10;
				}
				else
				{
					m_Table[m] = new InternalTimer(m, damage);
					m_Table[m].Start();
				}
				SpellHelper.Damage(this, m, damage);
			}

			FinishSequence();
		}

		private static readonly Dictionary<Mobile, Timer> m_Table = new();

		private class InternalTimer : Timer
		{
			private readonly Mobile target;
			private readonly int m_ToRestore;
			private DateTime Expire;
			private static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(1);

			public InternalTimer(Mobile m, double toRestore) : base(CheckInterval, CheckInterval)
			{
				target = m;
				m_ToRestore = (int)toRestore;
				Expire = DateTime.UtcNow + TimeSpan.FromSeconds(20);
			}

			protected override void OnTick()
			{
				if (target.Deleted || !target.Alive)
				{
					m_Table.Remove(target);
					Stop();
				}

				if(DateTime.UtcNow >= Expire && target.Alive)
					target.Hits += m_ToRestore;
				
				m_Table.Remove(target);
				Stop();
			}
		}
	}
}
