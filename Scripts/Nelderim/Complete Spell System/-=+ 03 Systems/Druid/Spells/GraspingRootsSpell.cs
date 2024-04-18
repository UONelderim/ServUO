#region References

using System;
using Server.Spells;
using Server.Targeting;

#endregion

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidGraspingRootsSpell : DruidSpell
	{
		private static readonly SpellInfo m_Info = new(
			"Szalone Korzenie", "En Ohm Sepa Tia Kes",
			//SpellCircle.Fifth,
			218,
			9012,
			false,
			CReagent.SpringWater,
			Reagent.Bloodmoss,
			Reagent.SpidersSilk
		);

		public override SpellCircle Circle => SpellCircle.Fifth;
		public override double CastDelay => 1.5;
		public override double RequiredSkill => 40.0;
		public override int RequiredMana => 40;

		public DruidGraspingRootsSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			Scroll?.Consume();
		}

		public override void OnCast()
		{
			Caster.BeginTarget(12,
				true,
				TargetFlags.None,
				(from, targeted) =>
				{
					if (targeted is Mobile m)
						Target(m);
				}
			);
		}

		public void Target(Mobile m)
		{
			if (!Caster.CanSee(m))
			{
				Caster.SendLocalizedMessage(500237); // Target can not be seen.
			}
			else if (CheckHSequence(m))
			{
				SpellHelper.Turn(Caster, m);

				double duration;

				duration = 7.0 + (Caster.Skills[DamageSkill].Value * 0.2);
				if ( Caster.Skills[CastSkill].Value < m.Skills[SkillName.MagicResist].Value )
					duration *= 0.5;
				
				Utility.Clamp(ref duration, 0.0, 4.0);
				var delay = TimeSpan.FromSeconds(duration);
				
				m.PlaySound(0x2A1);

				m.Paralyze(delay);
				m.FixedParticles(0x375A, 2, 10, 5027, 0x3D, 2, EffectLayer.Waist);

				var roots = new RootsItem(new Point3D(m.X, m.Y, m.Z), Caster.Map);
				Timer.DelayCall(delay, () => roots.Delete());
			}

			FinishSequence();
		}

		private class RootsItem : Item
		{
			public RootsItem(Point3D loc, Map map) : base(0xC5F)
			{
				MoveToWorld(loc, map);
				Movable = false;
			}

			public RootsItem(Serial serial) : base(serial)
			{
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);
				Delete();
			}
		}
	}
}
