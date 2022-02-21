#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki pomniejszego bezimiennego demona")]
	public class LesserDaemon : BaseCreature
	{
		[Constructable]
		public LesserDaemon() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "pomniejszy bezimienny demon";
			Body = 39;
			Hue = 1570;
			BaseSoundID = 372;

			SetStr(326, 395);
			SetDex(76, 95);
			SetInt(131, 205);

			SetHits(128, 155);

			SetDamage(7, 14);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 50, 50);
			SetResistance(ResistanceType.Fire, 60, 60);
			SetResistance(ResistanceType.Cold, 25, 30);
			SetResistance(ResistanceType.Poison, 25, 35);
			SetResistance(ResistanceType.Energy, 25, 35);

			SetSkill(SkillName.EvalInt, 30.1, 40.0);
			SetSkill(SkillName.Magery, 30.1, 40.0);
			SetSkill(SkillName.MagicResist, 75.1, 85.0);
			SetSkill(SkillName.Tactics, 70.1, 80.0);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);

			Fame = 5500;
			Karma = -5500;

			VirtualArmor = 42;
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.05)
					corpse.DropItem(new Bloodspawn());
				if (Utility.RandomDouble() < 0.10)
					corpse.DropItem(new DaemonBone());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich);
		}

		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }
		public override bool BardImmune { get { return false; } }

		public LesserDaemon(Serial serial) : base(serial)
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
