namespace Server.Mobiles
{
	[CorpseName("zwloki goblina wojownika")]
	public class GoblinWarrior : BaseCreature
	{
		[Constructable]
		public GoblinWarrior() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4) // AI_ melee or mage.
		{
			Name = "goblin wojownik";
			Body = 764; // Find Body types on the Distro mobile you want to use for your monster body.
			Hue = 2212;
			BaseSoundID = 1114; // Use the sound from any distro script.

			SetStr(344);
			SetDex(63);
			SetInt(128);

			SetHits(187);

			SetDamage(7, 9);

			SetDamageType(ResistanceType.Physical, 100);
			SetDamageType(ResistanceType.Cold, 0);
			SetDamageType(ResistanceType.Fire, 0);
			SetDamageType(ResistanceType.Energy, 0);
			SetDamageType(ResistanceType.Poison, 0);

			SetResistance(ResistanceType.Physical, 40);
			SetResistance(ResistanceType.Fire, 35);
			SetResistance(ResistanceType.Cold, 28);
			SetResistance(ResistanceType.Poison, 15);
			SetResistance(ResistanceType.Energy, 19);

			SetSkill(SkillName.Wrestling, 79.4, 89.6);

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 22;
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Potions);
		}

		public override bool CanRummageCorpses { get { return true; } }

		public GoblinWarrior(Serial serial) : base(serial)
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
