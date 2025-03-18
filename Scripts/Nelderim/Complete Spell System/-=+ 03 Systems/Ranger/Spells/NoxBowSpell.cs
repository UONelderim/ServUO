#region References

using System;
using Server.Items;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerNoxBowSpell : RangerSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Wężowy Łuk", "Agnu Kshapsa Cu",
			//SpellCircle.Fifth,
			212,
			9041,
			CReagent.Kindling,
			Reagent.Nightshade
		);

		public override SpellCircle Circle => SpellCircle.Fifth;

		public override double CastDelay => 7.0;
		public override double RequiredSkill => 95.0;
		public override int RequiredMana => 30;

		public RangerNoxBowSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				Item weap = new RangerNoxBow(Caster);

				Caster.AddToBackpack(weap);
				Caster.SendMessage("Tworzysz magiczny łuk w plecaku.");

				Caster.PlaySound(481);

				Effects.SendLocationParticles(
					EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 29, 1278, 2,
					9962, 0);
				Effects.SendLocationParticles(
					EffectItem.Create(new Point3D(Caster.X, Caster.Y, Caster.Z - 7), Caster.Map,
						EffectItem.DefaultDuration), 0x37C4, 1, 29, 1278, 2, 9502, 0);
			}
		}

		[FlipableAttribute(0x13B2, 0x13B1)]
		public class RangerNoxBow : BaseRanged
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;

			public override int EffectID => 0xF42;
			public override Type AmmoType => typeof(Arrow);
			public override Item Ammo => new Arrow();

			public override WeaponAbility PrimaryAbility => WeaponAbility.InfectiousStrike;
			public override WeaponAbility SecondaryAbility => WeaponAbility.MortalStrike;

			public override int StrengthReq => 30;
			public override int MinDamage => 16;
			public override int MaxDamage => 18;
			public override float Speed => 4.25f;
			public override int DefMaxRange => 10;

			public override int InitMinHits => 31;
			public override int InitMaxHits => 60;

			public override WeaponAnimation DefAnimation => WeaponAnimation.ShootBow;

			[Constructable]
			public RangerNoxBow(Mobile owner) : base(0x13B2)

			{
				WeaponAttributes.HitPoisonArea = 50;
				WeaponAttributes.HitHarm = 50;
				WeaponAttributes.HitLowerAttack = 20;
				Attributes.WeaponDamage = 25;
				Attributes.WeaponSpeed = 15;
				m_Owner = owner;
				Weight = 6.0;
				Layer = Layer.TwoHanded;
				Hue = 1272;
				BlessedFor = owner;
				Name = "Łuk Wężowy";

				double time = (owner.Skills[SkillName.Archery].Value / 20.0);
				m_Expire = DateTime.Now + TimeSpan.FromMinutes((int)time);
				m_Timer = new InternalTimer(this, m_Expire);

				m_Timer.Start();
			}

			public override void OnHit(Mobile attacker, IDamageable damageable, double damageBonus)
			{
				Mobile defender = damageable as Mobile;

				if (defender != null && 0.1 > Utility.RandomDouble())
					defender.ApplyPoison(defender, Poison.Lesser);

				base.OnHit(attacker, defender, damageBonus);
			}

			public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
				out int nrgy, out int chaos, out int direct)
			{
				phys = 0;
				fire = 0;
				cold = 0;
				pois = 100;
				chaos = 0;
					direct = 0;
				nrgy = 0;
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
				m_Owner.SendMessage("Twój łuk rozpuszcza się.");
				Delete();
			}

			public RangerNoxBow(Serial serial) : base(serial)
			{
			}

			public override void AddNameProperties(ObjectPropertyList list)
			{
				base.AddNameProperties(list);
				list.Add(1049644, "Tymaczosowo wzmocniony czarami");
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
			private readonly RangerNoxBow m_Bow;
			private readonly DateTime m_Expire;

			public InternalTimer(RangerNoxBow bow, DateTime expire) : base(TimeSpan.Zero, TimeSpan.FromSeconds(0.1))
			{
				m_Bow = bow;
				m_Expire = expire;
			}

			protected override void OnTick()
			{
				if (DateTime.Now >= m_Expire)
				{
					m_Bow.Remove();
					Stop();
				}
			}
		}
	}
}
