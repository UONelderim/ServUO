namespace Server.Items
{
	public class LuskiStarozytnegoSmoka : DragonArms
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseColdResistance { get { return 15; } }
		public override int BaseEnergyResistance { get { return 15; } }
		public override int BasePhysicalResistance { get { return 5; } }
		public override int BasePoisonResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 5; } }

		[Constructable]
		public LuskiStarozytnegoSmoka()
		{
			Hue = 851;
			Name = "Luski Starozytnego Smoka";
			Attributes.RegenStam = 2;
			SkillBonuses.SetValues(0, SkillName.Chivalry, 5.0);
			LootType = LootType.Cursed;
		}

		public LuskiStarozytnegoSmoka(Serial serial)
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
