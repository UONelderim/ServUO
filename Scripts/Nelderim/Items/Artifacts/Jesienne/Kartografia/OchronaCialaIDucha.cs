namespace Server.Items
{
	public class OchronaCialaIDucha : WoodlandGloves
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		public override int BaseFireResistance { get { return 20; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePhysicalResistance { get { return -10; } }
		public override int BaseEnergyResistance { get { return 25; } }
		public override int BasePoisonResistance { get { return 10; } }

		[Constructable]
		public OchronaCialaIDucha()
		{
			Hue = 2931;
			Name = "Ochrona Ciala I Ducha";
			Attributes.AttackChance = 10;
			Attributes.ReflectPhysical = 40;
			Attributes.RegenStam = 1;
			Attributes.BonusStam = 3;
			SkillBonuses.SetValues(0, SkillName.Macing, 5.0);
		}

		public OchronaCialaIDucha(Serial serial)
			: base(serial)
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
