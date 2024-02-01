using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("zwloki czarnej jaszczurki")]
	public class BlackLizard : BaseCreature
	{
		[Constructable]
		public BlackLizard() : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			Name = "czarna jaszczurka";
			Body = 206;
			Hue = 2406;
			BaseSoundID = 0x5A;

			SetStr(131, 152);
			SetDex(48, 61);
			SetInt(29, 32);

			SetHits(128, 145);
			SetMana(100);

			SetDamage(2, 5);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 30);

			SetSkill(SkillName.MagicResist, 68.9, 79.3);
			SetSkill(SkillName.Tactics, 45.6, 54.4);
			SetSkill(SkillName.Wrestling, 50.7, 59.6);

			Tamable = false;
			PackGold(3, 29);

			SetSpecialAbility(SpecialAbility.DragonBreath); //Custom definition?
		}

		public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
		{
			if (from == null || from == this) return;

			PlayerMobile pm = from as PlayerMobile;
			if (pm == null) return;

			Item weapon1 = pm.FindItemOnLayer(Layer.OneHanded);
			Item weapon2 = pm.FindItemOnLayer(Layer.TwoHanded);

			if (weapon1 is BaseKnife || weapon2 is BaseKnife)
			{
				damage *= 2;
			}
			else if (weapon1 is BaseSpear || weapon2 is BaseSpear)
			{
				damage *= 4;
			}
		}

		public override FoodType FavoriteFood => FoodType.Meat | FoodType.Fish;
		public override int Hides => 12;
		public override HideType HideType => HideType.Spined;

		public override void OnDamagedBySpell(Mobile from)
		{
			if (from != null && from.Alive && 0.4 > Utility.RandomDouble())
			{
				ShootGrassWind(from);
			}
		}

		public void ShootGrassWind(Mobile to)
		{
			MovingEffect(to, 0x0C4E, 10, 0, false, false);
			DoHarmful(to);
			PlaySound(0x32F); // f_shush
			AOS.Damage(to, this, 8, 100, 0, 0, 0, 0);
		}

		private static ResistanceMod _fanMod = new ResistanceMod(ResistanceType.Physical, -50);

		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGotMeleeAttack(attacker);
			if (_fannedMobiles.Contains(attacker)) return;

			if (0.05 > Utility.RandomDouble())
			{
				attacker.SendMessage("Czarna jaszczurka rozpyla gaz, ktory redukuje twoja odpornosc fizyczna.");

				attacker.FixedParticles(0x3779, 10, 30, 0x34, EffectLayer.RightFoot);
				attacker.PlaySound(0x1E6);
				attacker.AddResistanceMod(_fanMod);

				Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>ExpireFan(attacker));
				_fannedMobiles.Add(attacker);
			}
		}

		private static List<Mobile> _fannedMobiles = new List<Mobile>();

		private void ExpireFan(Mobile m)
		{
			m?.RemoveResistanceMod(_fanMod);
			_fannedMobiles.Remove(m);
		}

		public BlackLizard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
