namespace Server.Items.Crops
{
	public class ZrodloKonopia : WeedPlantZbieractwo
	{
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack(new SurowiecKonopia(count)); }

		public override SkillName SkillRequired { get { return SkillName.Herbalism; } }
		//public override int CropAmount { get{ return 5; } }

		public override bool GivesSeed { get { return false; } }

		[Constructable]
		public ZrodloKonopia() : base(0x0CC3) //3271
		{
			//Hue = 263;
			Name = "Krzak konopi"; // 1032612
		}

		public ZrodloKonopia(Serial serial) : base(serial)
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

	public class SurowiecKonopia : WeedCropZbieractwo
	{
		public override int AmountOfReagent(double skill) { return 12; }
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack(new CannabisFiber(count)); }
		public override SkillName SkillRequired { get { return SkillName.Fletching; } }

		[Constructable]
		public SurowiecKonopia(int amount) : base(amount, 0x0C5F)
		{
			//Hue = 0;
			Name = "Lodyga konopi"; // 1032615
		}

		[Constructable]
		public SurowiecKonopia() : this(1)
		{
		}

		public SurowiecKonopia(Serial serial) : base(serial)
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

	public class CannabisFiber : Item
	{
		[Constructable]
		public CannabisFiber(int amount) : base(3166)
		{
			Stackable = true;
			Amount = amount;
			//Hue = 0;
			Name = "Konopne wlokno"; // 1032616
			Weight = 0.15;
		}

		[Constructable]
		public CannabisFiber() : this(1)
		{
		}

		public CannabisFiber(Serial serial) : base(serial)
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
