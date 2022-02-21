namespace Server.Engines.BulkOrders
{
	public class HuntingBOBLargeEntry
	{
		//private bool m_RequireExceptional;
		//private BODType m_DeedType;
		//private BulkMaterialType m_Material;

		//public bool RequireExceptional{ get{ return m_RequireExceptional; } }
		//public BODType DeedType{ get{ return m_DeedType; } }
		//public BulkMaterialType Material{ get{ return m_Material; } }
		public int AmountMax { get; }
		public int Price { get; set; }

		public HuntingBOBLargeSubEntry[] Entries { get; }

		public Item Reconstruct()
		{
			LargeHunterBOD bod = null;

			int level = Entries[0].Level;
			bod = new LargeHunterBOD(AmountMax, ReconstructEntries());

			for (int i = 0; bod != null && i < bod.Entries.Length; ++i)
				bod.Entries[i].Owner = bod;

			return bod;
		}

		private LargeBulkEntry[] ReconstructEntries()
		{
			LargeBulkEntry[] entries = new LargeBulkEntry[Entries.Length];

			for (int i = 0; i < Entries.Length; ++i)
			{
				entries[i] = new LargeBulkEntry(null,
					new SmallBulkEntry(Entries[i].ItemType, Entries[i].Number, Entries[i].Graphic, Entries[i].Level));
				entries[i].Amount = Entries[i].AmountCur;
			}

			return entries;
		}

		public HuntingBOBLargeEntry(LargeHunterBOD bod)
		{
			//m_RequireExceptional = bod.RequireExceptional;
/*
			if ( bod is LargeTailorBOD )
				m_DeedType = BODType.Tailor;
			else if ( bod is LargeSmithBOD )
				m_DeedType = BODType.Smith;
*/
			//m_Material = bod.Material;
			AmountMax = bod.AmountMax;

			Entries = new HuntingBOBLargeSubEntry[bod.Entries.Length];

			for (int i = 0; i < Entries.Length; ++i)
				Entries[i] = new HuntingBOBLargeSubEntry(bod.Entries[i]);
		}

		public HuntingBOBLargeEntry(GenericReader reader)
		{
			int version = reader.ReadEncodedInt();

			switch (version)
			{
				case 0:
				{
					//m_RequireExceptional = reader.ReadBool();

					//m_DeedType = (BODType)reader.ReadEncodedInt();

					//m_Material = (BulkMaterialType)reader.ReadEncodedInt();
					AmountMax = reader.ReadEncodedInt();
					Price = reader.ReadEncodedInt();

					Entries = new HuntingBOBLargeSubEntry[reader.ReadEncodedInt()];

					for (int i = 0; i < Entries.Length; ++i)
						Entries[i] = new HuntingBOBLargeSubEntry(reader);

					break;
				}
			}
		}

		public void Serialize(GenericWriter writer)
		{
			writer.WriteEncodedInt(0); // version

			//writer.Write( (bool) m_RequireExceptional );

			//writer.WriteEncodedInt( (int) m_DeedType );
			//writer.WriteEncodedInt( (int) m_Material );
			writer.WriteEncodedInt(AmountMax);
			writer.WriteEncodedInt(Price);

			writer.WriteEncodedInt(Entries.Length);

			for (int i = 0; i < Entries.Length; ++i)
				Entries[i].Serialize(writer);
		}
	}
}
