#region References

using Server.Misc;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki maga minotaura")]
	public class MinotaurMage : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Orc; } }

		[Constructable]
		public MinotaurMage() : base(AIType.AI_Mage, FightMode.Closest, 12, 2, 0.2, 0.4)
		{
			Name = "mag minotaur";
			Body = 262;
			BaseSoundID = 0x45A;

			SetStr(316, 350);
			SetDex(91, 115);
			SetInt(261, 285);

			SetHits(270, 290);

			SetDamage(8, 14);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 25, 55);
			SetResistance(ResistanceType.Fire, 30, 50);
			SetResistance(ResistanceType.Cold, 20, 50);
			SetResistance(ResistanceType.Poison, 30, 50);
			SetResistance(ResistanceType.Energy, 30, 50);

			SetSkill(SkillName.EvalInt, 70.1, 82.5);
			SetSkill(SkillName.Magery, 70.1, 82.5);
			SetSkill(SkillName.MagicResist, 60.1, 75.0);
			SetSkill(SkillName.Tactics, 50.1, 65.0);
			SetSkill(SkillName.Wrestling, 40.1, 50.0);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 30;
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Average);
			AddLoot(LootPack.LowScrolls);
			AddLoot(LootPack.MageryRegs, 6);
		}

		public override double AttackMasterChance { get { return 0.2; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override int TreasureMapLevel { get { return 1; } }
		public override int Meat { get { return 1; } }

		public MinotaurMage(Serial serial) : base(serial)
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
