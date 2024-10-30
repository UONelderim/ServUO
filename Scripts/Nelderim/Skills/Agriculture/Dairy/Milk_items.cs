namespace Server.Items
{
	public abstract class MilkBottle : Item
	{
		public virtual Item EmptyItem => null;

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Poisoner { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Poison Poison { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int FillFactor { get; set; }

		public MilkBottle(int itemID) : base(itemID)
		{
			FillFactor = 4;
			Stackable = true;
		}

		public MilkBottle(Serial serial) : base(serial)
		{
		}

		public void Drink(Mobile from)
		{
			if (ReduceThirst(from, FillFactor))
			{
				from.PlaySound(Utility.Random(0x30, 2));

				if (from.Body.IsHuman && !from.Mounted)
					from.Animate(34, 5, 1, true, false, 0);

				if (Poison != null)
					from.ApplyPoison(Poisoner, Poison);

				Consume();

				Item item = EmptyItem;

				if (item != null)
					from.AddToBackpack(item);
			}
		}

		private bool ReduceThirst(Mobile from, int fillFactor)
		{
			if (from.Thirst >= 20)
			{
				from.SendMessage("Nie mozesz juz nic wypic!");
				return false;
			}

			int iThirst = from.Thirst + fillFactor;
			if (iThirst >= 20)
			{
				from.Thirst = 20;
				from.SendMessage("Jestes juz pelen!");
			}
			else
			{
				from.Thirst = iThirst;

				if (iThirst < 5)
					from.SendMessage("Wypiles mleko ale wciaz jestes bardzo spragniony.");
				else if (iThirst < 10)
					from.SendMessage("Wypiles mleko i poczules sie troche lepiej.");
				else if (iThirst < 15)
					from.SendMessage("Po wypiciu mleka czujesz znacznie mniejsze pragnienie.");
				else
					from.SendMessage("Po wypiciu mleka czujesz sie pelen.");
			}

			return true;
		}


		public override void OnDoubleClick(Mobile from)
		{
			if (!Movable)
				return;

			if (from.InRange(GetWorldLocation(), 1))
				Drink(from);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // version

			writer.Write(Poisoner);

			Poison.Serialize(Poison, writer);
			writer.Write(FillFactor);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
				{
					Poisoner = reader.ReadMobile();

					goto case 0;
				}
				case 0:
				{
					Poison = Poison.Deserialize(reader);
					FillFactor = reader.ReadInt();
					break;
				}
			}
		}
	}

	public class BottleCowMilk : MilkBottle
	{
		public override Item EmptyItem => new Bottle();

		[Constructable]
		public BottleCowMilk() : base(0x0f09)
		{
			Weight = 0.2;
			FillFactor = 4;
			Name = "Butelka krowiego mleka";
		}

		public BottleCowMilk(Serial serial) : base(serial)
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

	public class BottleGoatMilk : MilkBottle
	{
		public override Item EmptyItem => new Bottle();

		[Constructable]
		public BottleGoatMilk() : base(0x0f09)
		{
			Weight = 0.2;
			Name = "Butelka koziego mleka";
		}

		public BottleGoatMilk(Serial serial) : base(serial)
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

	public class BottleSheepMilk : MilkBottle
	{
		public override Item EmptyItem => new Bottle();

		[Constructable]
		public BottleSheepMilk() : base(0x0f09)
		{
			Weight = 0.2;
			Name = "Butelka owczego mleka";
		}

		public BottleSheepMilk(Serial serial) : base(serial)
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

	public class FromageDeVache : Food, ICarvable
	{
		public bool Carve(Mobile from, Item item)
		{
			if (!Movable)
				return false;

			if (Amount > 1) // workaround because I can't call scissorhelper twice?
			{
				from.SendMessage("You can only cut up one wheel at a time.");
				return false;
			}

			base.ScissorHelper(from, new FromageDeVacheWedge(), 1);

			from.AddToBackpack(new FromageDeVacheWedgeSmall());

			from.SendMessage("You cut a wedge out of the wheel.");
			return true;
		}

		[Constructable]
		public FromageDeVache() : this(1)
		{
		}

		[Constructable]
		public FromageDeVache(int amount) : base(amount, 0x97E)
		{
			Weight = 0.4;
			FillFactor = 12;
			Name = "Emmental (krowi ser)";
			Hue = 0x481;
			Stackable = true;
		}
		
		public FromageDeVache(Serial serial) : base(serial)
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

	public class FromageDeVacheWedge : Food, ICarvable
	{
		public bool Carve(Mobile from, Item item)
		{
			if (!Movable)
				return false;

			base.ScissorHelper(from, new FromageDeVacheWedgeSmall(), 3);
			from.SendMessage("You cut the wheel into 3 wedges.");
			return true;
		}

		[Constructable]
		public FromageDeVacheWedge() : this(1)
		{
		}

		[Constructable]
		public FromageDeVacheWedge(int amount) : base(amount, 0x97D)
		{
			Weight = 0.3;
			FillFactor = 9;
			Name = "Emmental (krowi ser)";
			Hue = 0x481;
			Stackable = true;
		}

		public FromageDeVacheWedge(Serial serial) : base(serial)
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

	public class FromageDeVacheWedgeSmall : Food
	{
		[Constructable]
		public FromageDeVacheWedgeSmall() : this(1)
		{
		}

		[Constructable]
		public FromageDeVacheWedgeSmall(int amount) : base(amount, 0x97C)
		{
			Weight = 0.1;
			FillFactor = 3;
			Name = "Emmental (krowi ser)";
			Hue = 0x481;
			Stackable = true;
		}

		public FromageDeVacheWedgeSmall(Serial serial) : base(serial)
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
	
	public class FromageDeBrebis : Food, ICarvable
	{
		public bool Carve(Mobile from, Item item)
		{
			if (!Movable)
				return false;

			if (Amount > 1) // workaround because I can't call scissorhelper twice?
			{
				from.SendMessage("You can only cut up one wheel at a time.");
				return false;
			}

			base.ScissorHelper(from, new FromageDeBrebisWedge(), 1);

			from.AddToBackpack(new FromageDeBrebisWedgeSmall());

			from.SendMessage("You cut a wedge out of the wheel.");
			return true;
		}

		[Constructable]
		public FromageDeBrebis() : this(1)
		{
		}

		[Constructable]
		public FromageDeBrebis(int amount) : base(amount, 0x97E)
		{
			Weight = 0.4;
			FillFactor = 12;
			Name = "Perail de Brebis (owczy ser)";
			Hue = 0x481;
			Stackable = true;
		}

		public FromageDeBrebis(Serial serial) : base(serial)
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

	public class FromageDeBrebisWedge : Food, ICarvable
	{
		public bool Carve(Mobile from, Item item)
		{
			if (!Movable)
				return false;

			base.ScissorHelper(from, new FromageDeBrebisWedgeSmall(), 3);
			from.SendMessage("You cut the wheel into 3 wedges.");
			return true;
		}

		[Constructable]
		public FromageDeBrebisWedge() : this(1)
		{
		}

		[Constructable]
		public FromageDeBrebisWedge(int amount) : base(amount, 0x97D)
		{
			Weight = 0.3;
			FillFactor = 9;
			Name = "Perail de Brebis (owczy ser)";
			Hue = 0x481;
			Stackable = true;
		}

		public FromageDeBrebisWedge(Serial serial) : base(serial)
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

	public class FromageDeBrebisWedgeSmall : Food
	{
		[Constructable]
		public FromageDeBrebisWedgeSmall() : this(1)
		{
		}

		[Constructable]
		public FromageDeBrebisWedgeSmall(int amount) : base(amount, 0x97C)
		{
			Weight = 0.1;
			FillFactor = 3;
			Name = "Perail de Brebis (owczy ser)";
			Hue = 0x481;
			Stackable = true;
		}

		public FromageDeBrebisWedgeSmall(Serial serial) : base(serial)
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

	public class FromageDeChevre : Food, ICarvable
	{
		public bool Carve(Mobile from, Item item)
		{
			if (!Movable)
				return false;

			if (Amount > 1) // workaround because I can't call scissorhelper twice?
			{
				from.SendMessage("You can only cut up one wheel at a time.");
				return false;
			}

			base.ScissorHelper(from, new FromageDeChevreWedge(), 1);

			from.AddToBackpack(new FromageDeChevreWedgeSmall());

			from.SendMessage("You cut a wedge out of the wheel.");
			return true;
		}

		[Constructable]
		public FromageDeChevre() : this(1)
		{
		}

		[Constructable]
		public FromageDeChevre(int amount) : base(amount, 0x97E)
		{
			Weight = 0.4;
			FillFactor = 12;
			Name = "Chevreton du Bourbonnais (kozi ser)";
			Hue = 0x481;
			Stackable = true;
		}

		public FromageDeChevre(Serial serial) : base(serial)
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

	public class FromageDeChevreWedge : Food, ICarvable
	{
		public bool Carve(Mobile from, Item item)
		{
			if (!Movable)
				return false;

			base.ScissorHelper(from, new FromageDeChevreWedgeSmall(), 3);
			from.SendMessage("You cut the wheel into 3 wedges.");
			return true;
		}

		[Constructable]
		public FromageDeChevreWedge() : this(1)
		{
		}

		[Constructable]
		public FromageDeChevreWedge(int amount) : base(amount, 0x97D)
		{
			Weight = 0.3;
			FillFactor = 9;
			Name = "Chevreton du Bourbonnais (kozi ser)";
			Hue = 0x481;
			Stackable = true;
		}

		public FromageDeChevreWedge(Serial serial) : base(serial)
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

	public class FromageDeChevreWedgeSmall : Food
	{
		[Constructable]
		public FromageDeChevreWedgeSmall() : this(1)
		{
		}

		[Constructable]
		public FromageDeChevreWedgeSmall(int amount) : base(amount, 0x97C)
		{
			Weight = 0.1;
			FillFactor = 3;
			Name = "Chevreton du Bourbonnais (kozi ser)";
			Hue = 0x481;
			Stackable = true;
		}

		public FromageDeChevreWedgeSmall(Serial serial) : base(serial)
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
