#region References

using System;
using Server.Misc;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

#endregion

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadLureStoneSpell : UndeadSpell
	{
		private static readonly SpellInfo m_Info = new(
			"Gnijące Zwłoki",
			"Ekhen Karyen Uus Corps",
			269,
			9020,
			false,
			Reagent.BlackPearl,
			CReagent.SpringWater
		);

		public override SpellCircle Circle => SpellCircle.Second;

		public UndeadLureStoneSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
		}

		public override double CastDelay => 1.0;
		public override double RequiredSkill => 15.0;
		public override int RequiredMana => 80;

		public override void OnCast()
		{
			Caster.BeginTarget(12,
				true,
				TargetFlags.None,
				((_, o) =>
				{
					if (o is IPoint3D p)
						Target(p);
				}));
		}

		public void Target(IPoint3D p)
		{
			if (!Caster.CanSee(p))
			{
				Caster.SendLocalizedMessage(500237); // Target can not be seen.
			}
			else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
			{
				SpellHelper.Turn(Caster, p);

				SpellHelper.GetSurfaceTop(ref p);

				Effects.PlaySound(p, Caster.Map, 0x243);

				Point3D loc = new Point3D(p.X, p.Y, p.Z);
				new LureStone(this, loc, Caster);
				loc.Y -= 1;
				new Decor(loc, Caster);
			}

			FinishSequence();
		}

		[DispellableField]
		private class LureStone : Item
		{
			private readonly Mobile _Owner;
			private readonly UndeadLureStoneSpell _Spell;

			public override bool BlocksFit => true;

			public LureStone(UndeadLureStoneSpell spell, Point3D loc, Mobile caster) : base(0x3D64)
			{
				_Spell = spell;
				_Owner = caster;
				Visible = true;
				Movable = false;
				Hue = 1429;
				Name = "gnijące zwłoki";
				MoveToWorld(loc, caster.Map);
				Timer.DelayCall(TimeSpan.FromSeconds(20.0), Delete);
			}

			public LureStone(Serial serial) : base(serial)
			{
			}

			public override bool HandlesOnMovement => true;

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);
				Delete();
			}

			public override void OnMovement(Mobile m, Point3D oldLocation)
			{
				if (m is BaseNelderimGuard || m is BaseVendor)
				{
					return;
				}

				if (_Owner == null) return;
				if (!m.InRange(this, Core.GlobalMaxUpdateRange)) return;
				
				double castValue = _Owner.Skills[_Spell.CastSkill].Value;
				double damageValue = _Owner.Skills[_Spell.DamageSkill].Value / 10;

				if (m is BaseCreature cret)
				{
					if (castValue >= 99.9 && (cret.Combatant == null || !cret.Combatant.Alive ||
					                      cret.Combatant.Deleted))
					{
						cret.TargetLocation = new Point2D(X, Y);
					}
					else if (cret.Tamable && (cret.Combatant == null || !cret.Combatant.Alive ||
					                          cret.Combatant.Deleted))
					{
						if (cret.MinTameSkill <= (castValue + damageValue) + 0.1)
							cret.TargetLocation = new Point2D(X, Y);
					}
				}
			}
		}

		[DispellableField]
		private class Decor : Item
		{
			public override bool BlocksFit => true;

			public Decor(Point3D loc, Mobile caster) : base(0x3D64)
			{
				Movable = false;
				MoveToWorld(loc, caster.Map);
				Timer.DelayCall(TimeSpan.FromSeconds(20.0), Delete);
			}

			public Decor(Serial serial) : base(serial)
			{
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);
				Delete();
			}
		}
	}
}
