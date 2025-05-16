#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki wielkiego bezimiennego demona")]
	public class GreaterDaemon : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.25; } }

		public override double DispelDifficulty { get { return 135.0; } }
		public override double DispelFocus { get { return 45.0; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }
		public override bool AutoDispel => false;
		public override bool BardImmune { get { return false; } }

		public override int TreasureMapLevel { get { return 4; } }
		public override int Meat { get { return 1; } }

		[Constructable]
		public GreaterDaemon() : base(AIType.AI_Mage, FightMode.Closest, 11, 1, 0.2, 0.4)
		{
			Name = "wielki bezimienny demon";
			Body = 9;
			Hue = 1569;
			BaseSoundID = 357;

			SetStr(952, 1010);
			SetDex(152, 190);
			SetInt(602, 650);

			SetHits(1144, 1212);

			SetDamage(15, 36);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 60, 80);
			SetResistance(ResistanceType.Fire, 65, 80);
			SetResistance(ResistanceType.Cold, 45, 60);
			SetResistance(ResistanceType.Poison, 35, 50);
			SetResistance(ResistanceType.Energy, 45, 60);

			SetSkill(SkillName.EvalInt, 80.1, 100.0);
			SetSkill(SkillName.Magery, 80.1, 100.0);
			SetSkill(SkillName.MagicResist, 95.1, 115.0);
			SetSkill(SkillName.Tactics, 80.1, 100.0);
			SetSkill(SkillName.Wrestling, 70.1, 100.0);

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 68;

			SetWeaponAbility(WeaponAbility.WhirlwindAttack);
			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public override double WeaponAbilityChance => 0.45;

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.10)
					corpse.DropItem(new Bloodspawn());
				if (Utility.RandomDouble() < 0.25)
					corpse.DropItem(new DaemonBone());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			// 07.01.2013 :: szczaw :: usuniecie PackGold
			//PackGold(1000, 2000 );
			AddLoot(LootPack.UltraRich);
			AddLoot(LootPack.FilthyRich);
		}

		public GreaterDaemon(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
