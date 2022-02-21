namespace Server.Items
{
	public class JewelryStone : Item
	{
		private int minProperties = 4;
		private int maxProperties = 5;

		public int NumberOfProperties = 25; // Ilosc propsow ktore sa mozliwe do wylosowania 

		#region [props

		[CommandProperty(AccessLevel.GameMaster)]
		public int MinProperties
		{
			get
			{
				return minProperties;
			}
			set
			{
				minProperties = (value <= NumberOfProperties ? value : NumberOfProperties);
				if (minProperties > maxProperties)
					MaxProperties = minProperties;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int MaxProperties
		{
			get
			{
				return maxProperties;
			}
			set
			{
				maxProperties = (value <= NumberOfProperties ? value : NumberOfProperties);
				if (maxProperties < minProperties)
					MinProperties = maxProperties;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int MinIntensity { get; set; } = 80;

		[CommandProperty(AccessLevel.GameMaster)]
		public int MaxIntensity { get; set; } = 100;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Cost { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Ring { get; set; } = true;

		#endregion

		[Constructable]
		public JewelryStone()
			: base(3796)
		{
			Name = "kamien bizuterii";
			Movable = false;
		}

		public JewelryStone(Serial serial)
			: base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			Container b = from.Backpack;
			if (b == null)
				return;

			BaseJewel jewel = this.CreateJewel();

			b.DropItem(jewel);
			from.SendMessage("Bizuteria znalazla sie w Twoim plecaku");
		}

		public BaseJewel CreateJewel()
		{
			BaseJewel jewel = (Ring ? new GoldRing() : (BaseJewel)(new GoldBracelet()));
			int props = Utility.Random(this.maxProperties - this.minProperties) + this.minProperties;

			BaseRunicTool.ApplyAttributesTo(jewel, props, this.MinIntensity, this.MaxIntensity);

			return jewel;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(3);

			// 3
			writer.Write(Ring);
			// 2
			writer.Write(Cost);
			// 1
			writer.Write(MinIntensity);
			writer.Write(MaxIntensity);
			writer.Write(minProperties);
			writer.Write(maxProperties);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 3:
				{
					Ring = reader.ReadBool();
					goto case 2;
				}
				case 2:
				{
					Cost = reader.ReadInt();
					goto case 1;
				}
				case 1:
				{
					MinIntensity = reader.ReadInt();
					MaxIntensity = reader.ReadInt();
					minProperties = reader.ReadInt();
					maxProperties = reader.ReadInt();
					goto case 0;
				}
				case 0:
				{
					break;
				}
			}
		}
	}
}
