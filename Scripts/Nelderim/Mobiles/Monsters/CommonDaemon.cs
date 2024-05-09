#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki bezimiennego demona")]
	public class CommonDaemon : BaseCreature
	{
		public override double DispelDifficulty => 125.0;
		public override double DispelFocus => 45.0;

		[Constructable]
		public CommonDaemon() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "bezimienny demon"; //NameList.RandomName( "daemon" );
			Body = 9;
			BaseSoundID = 357;

			SetStr(476, 505);
			SetDex(76, 95);
			SetInt(301, 325);

			SetHits(286, 303);

			SetDamage(7, 14);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 45, 60);
			SetResistance(ResistanceType.Fire, 50, 60);
			SetResistance(ResistanceType.Cold, 30, 40);
			SetResistance(ResistanceType.Poison, 20, 30);
			SetResistance(ResistanceType.Energy, 30, 40);

			SetSkill(SkillName.EvalInt, 70.1, 80.0);
			SetSkill(SkillName.Magery, 70.1, 80.0);
			SetSkill(SkillName.MagicResist, 85.1, 95.0);
			SetSkill(SkillName.Tactics, 70.1, 80.0);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 58;
			ControlSlots = 4;
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.20)
					corpse.DropItem(new DaemonBone());
				if (Utility.RandomDouble() < 0.08)
					corpse.DropItem(new Bloodspawn());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich);
			AddLoot(LootPack.Average, 2);
		}

		public override bool CanRummageCorpses => true;
		public override Poison PoisonImmune => Poison.Regular;
		public override int TreasureMapLevel => 4;
		public override int Meat => 1;

		public CommonDaemon(Serial serial) : base(serial)
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
