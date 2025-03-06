#region References

using System;
using Server.Items;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadHammerOfFaithSpell : UndeadSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Sierp Wiary Smierci", "Helleus Terum",
			//SpellCircle.Fifth,
			269,
			9020,
			Reagent.PigIron,
			Reagent.Nightshade
		);

		public override SpellCircle Circle => SpellCircle.Fifth;

		public override double RequiredSkill => 40.0;

		public override int RequiredMana => 14;

		public UndeadHammerOfFaithSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				Item weap = new HammerOfFaith(Caster);

				Caster.AddToBackpack(weap);
				Caster.SendMessage("W Twym plecaku pojawia się magiczna broń.");

				Caster.PlaySound(0x212);
				Caster.PlaySound(0x206);

				Effects.SendLocationParticles(
					EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 29, 0x47D, 2,
					9962, 0);
				Effects.SendLocationParticles(
					EffectItem.Create(new Point3D(Caster.X, Caster.Y, Caster.Z - 7), Caster.Map,
						EffectItem.DefaultDuration), 0x37C4, 1, 29, 0x47D, 2, 9502, 0);
			}
		}

		[FlipableAttribute(0x13B0, 0x13B0)]
		private class HammerOfFaith : BaseBashing
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;

			public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
			public override WeaponAbility SecondaryAbility => WeaponAbility.CrushingBlow;

			public override int StrengthReq => 10;
			public override int MinDamage => 17;
			public override int MaxDamage => 18;
			public override float Speed => 3.25f;

			public override int InitMinHits => 255;
			public override int InitMaxHits => 255;

			public override WeaponAnimation DefAnimation => WeaponAnimation.Bash2H;

			[Constructable]
			public HammerOfFaith(Mobile owner) : base(9915)
			{
				m_Owner = owner;
				Weight = 10.0;
				Layer = Layer.OneHanded;
				Hue = 38;
				BlessedFor = owner;
				Slayer = SlayerName.Fey;
				Attributes.WeaponDamage = 30;
				WeaponAttributes.HitLeechStam = 20;
				WeaponAttributes.HitLeechHits = 20;
				Attributes.SpellChanneling = 1;
				WeaponAttributes.UseBestSkill = 1;
				Name = "Sierp Wiary Smierci";

				double time =
					(owner.Skills[SkillName.Necromancy].Value / 10.0) /* ClericDivineFocusSpell.GetScalar( owner )*/;
				m_Expire = DateTime.Now + TimeSpan.FromMinutes((int)time);
				m_Timer = new InternalTimer(this, m_Expire);

				m_Timer.Start();
			}

			public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
				out int nrgy, out int chaos, out int direct)
			{
				fire = cold = pois = nrgy = chaos = direct = 0;
				phys = 100;
			}

			public override void OnDelete()
			{
				if (m_Timer != null)
					m_Timer.Stop();

				base.OnDelete();
			}

			public override bool CanEquip(Mobile m)
			{
				if (m != m_Owner)
					return false;

				return true;
			}

			public void Remove()
			{
				m_Owner.SendMessage("Broń powoli się rozpływa.");
				Delete();
			}

			public HammerOfFaith(Serial serial) : base(serial)
			{
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.Write(0); // version
				writer.Write(m_Owner);
				writer.Write(m_Expire);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();
				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();

				m_Timer = new InternalTimer(this, m_Expire);
				m_Timer.Start();
			}
		}

		private class InternalTimer : Timer
		{
			private readonly HammerOfFaith m_Hammer;
			private readonly DateTime m_Expire;

			public InternalTimer(HammerOfFaith hammer, DateTime expire) : base(TimeSpan.Zero, TimeSpan.FromSeconds(0.1))
			{
				m_Hammer = hammer;
				m_Expire = expire;
			}

			protected override void OnTick()
			{
				if (DateTime.Now >= m_Expire)
				{
					m_Hammer.Remove();
					Stop();
				}
			}
		}
	}
}
