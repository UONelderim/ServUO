#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki wielkiego demona cienia")]
	public class GreaterArcaneDaemon : BaseCreature
	{
		[Constructable]
		public GreaterArcaneDaemon() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "wielki demon cienia";
			Body = 0x310;
			Hue = 2106;
			BaseSoundID = 0x47D;

			SetStr(800, 950);
			SetDex(190, 220);
			SetInt(390, 450);

			SetMana(1590, 1750);

			SetDamage(13, 19);

			SetDamageType(ResistanceType.Physical, 80);
			SetDamageType(ResistanceType.Fire, 20);

			SetResistance(ResistanceType.Physical, 80, 90);
			SetResistance(ResistanceType.Fire, 80, 90);
			SetResistance(ResistanceType.Cold, 20, 30);
			SetResistance(ResistanceType.Poison, 60, 70);
			SetResistance(ResistanceType.Energy, 40, 50);

			SetSkill(SkillName.EvalInt, 110.1, 135.0);
			SetSkill(SkillName.Magery, 110.1, 135.0);
			SetSkill(SkillName.MagicResist, 135.1, 145.0);
			SetSkill(SkillName.Tactics, 80.1, 85.0);
			SetSkill(SkillName.Wrestling, 80.1, 90.0);
			SetSkill(SkillName.Meditation, 110.1, 120.0);

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 65;

			SetWeaponAbility(WeaponAbility.ConcussionBlow);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.10)
					corpse.DropItem(new Bloodspawn());
				if (Utility.RandomDouble() < 0.30)
					corpse.DropItem(new DaemonBone());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			// 07.01.2013 :: szczaw :: usuniecie PackGold
			//PackGold(1000, 2000 );
			AddLoot(LootPack.UltraRich);
			AddLoot(LootPack.HighScrolls, 3);
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.25; } }
		public override bool AutoDispel { get { return true; } }
		public override bool BardImmune { get { return false; } }

		public GreaterArcaneDaemon(Serial serial) : base(serial)
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
