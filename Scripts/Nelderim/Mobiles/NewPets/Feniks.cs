using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("zgliszcza feniksa")]
	public class Feniks : BaseCreature, IAuraCreature
	{
		[Constructable]
		public Feniks() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.3, 0.4)
		{
			Name = "feniks";
			Body = 832;
			Hue = 0x489;

			SetStr(605, 611);
			SetDex(391, 519);
			SetInt(669, 818);

			SetHits(670, 850);

			SetDamage(15, 25);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Fire, 50);

			SetResistance(ResistanceType.Physical, 65);
			SetResistance(ResistanceType.Fire, 72, 74);
			SetResistance(ResistanceType.Poison, 36, 41);
			SetResistance(ResistanceType.Energy, 50, 51);

			SetSkill(SkillName.Wrestling, 121.9, 130.6);
			SetSkill(SkillName.Tactics, 114.9, 117.4);
			SetSkill(SkillName.MagicResist, 147.7, 153.0);
			SetSkill(SkillName.Poisoning, 122.8, 124.0);
			SetSkill(SkillName.Magery, 121.8, 127.8);
			SetSkill(SkillName.EvalInt, 103.6, 117.0);

			Tamable = true;
			ControlSlots = 5;
			MinTameSkill = 115.0;

			SetWeaponAbility(WeaponAbility.ParalyzingBlow);
			SetWeaponAbility(WeaponAbility.BleedAttack);
			SetSpecialAbility(SpecialAbility.DragonBreath);
			SetAreaEffect(AreaEffect.AuraDamage);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.08)
					corpse.DropItem(new DragonsHeart());
				if (Utility.RandomDouble() < 0.20)
					corpse.DropItem(new DragonsBlood());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.UltraRich, 4);
		}

		public override bool AutoDispel => !Controlled;

		public override int TreasureMapLevel => 5;
		public override int Feathers => 36;
		public override int GetIdleSound() { return 0x2EF; }
		public override int GetAttackSound() { return 0x2EE; }
		public override int GetAngerSound() { return 0x2EF; }
		public override int GetHurtSound() { return 0x2F1; }
		public override int GetDeathSound() { return 0x2F2; }


		public Feniks(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public void AuraEffect(Mobile m)
		{
			m.FixedParticles(0x374A, 10, 30, 5052, 0x489, 0, EffectLayer.Waist);
			m.PlaySound(0x5C6);

			m.SendLocalizedMessage(1008112, false, Name); //  : The intense heat is damaging you!
		}
	}
}
