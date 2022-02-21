namespace Server.Engines.BulkOrders
{
	public class HuntingBOBFilter
	{
		//private int m_Quality;
		//private int m_Material;

		public bool IsDefault
		{
			get { return (Type == 0 && Class == 0 && Quantity == 0); }
		}

		public void Clear()
		{
			Type = 0;
			//m_Quality = 0;
			Class = 0;
			Quantity = 0;
		}

		public int Type { get; set; }

		public int Class // Level
		{
			get;
			set;
		}

		public int Quantity { get; set; }

		public HuntingBOBFilter()
		{
		}

		public HuntingBOBFilter(GenericReader reader)
		{
			int version = reader.ReadEncodedInt();

			switch (version)
			{
				case 1:
				{
					Type = reader.ReadEncodedInt();
					Class = reader.ReadEncodedInt();
					Quantity = reader.ReadEncodedInt();

					break;
				}
			}
		}

		public void Serialize(GenericWriter writer)
		{
			if (IsDefault)
			{
				writer.WriteEncodedInt(0); // version
			}
			else
			{
				writer.WriteEncodedInt(1); // version

				writer.WriteEncodedInt(Type);
				writer.WriteEncodedInt(Class);
				writer.WriteEncodedInt(Quantity);
			}
		}
	}
}
