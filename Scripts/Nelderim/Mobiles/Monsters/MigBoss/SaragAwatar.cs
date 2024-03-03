#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zgliszcza saraga - awatara")]
	public class SaragAwatar : BaseCreature
	{
		public override bool BardImmune { get { return false; } }
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }
		public override double DispelDifficulty { get { return 135.0; } }
		public override double DispelFocus { get { return 45.0; } }
		public override bool AutoDispel { get { return false; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		[Constructable]
		public SaragAwatar() : base(AIType.AI_Boss, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Body = 308;
			Hue = 1455;
			Name = "sarag - awatar przedwiecznego nieumarłego";

			BaseSoundID = 0x48D;

			SetStr(700, 740);
			SetDex(100, 120);
			SetInt(300, 350);
			SetHits(5000);

			SetDamage(15, 18);

			SetDamageType(ResistanceType.Physical, 30);
			SetDamageType(ResistanceType.Cold, 70);

			SetResistance(ResistanceType.Physical, 60);
			SetResistance(ResistanceType.Fire, 60);
			SetResistance(ResistanceType.Cold, 60);
			SetResistance(ResistanceType.Poison, 60);
			SetResistance(ResistanceType.Energy, 60);

			SetSkill(SkillName.MagicResist, 90.0);
			SetSkill(SkillName.Tactics, 90.0);
			SetSkill(SkillName.Wrestling, 90.0);
			SetSkill(SkillName.EvalInt, 90.0);
			SetSkill(SkillName.Magery, 90.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 80;

			AddItem(new LightSource());

			if (Utility.RandomDouble() < 0.3)
				PackItem(new ObsidianStone());

			SetWeaponAbility(WeaponAbility.MortalStrike);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.40)
					corpse.DropItem(new Pumice());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.SuperBoss);
		}

		public SaragAwatar(Serial serial) : base(serial)
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
}
