#region References

using System;

#endregion

namespace Server.Engines.BulkOrders
{
	public class HuntingBOBSmallEntry
	{
		public Type ItemType { get; }
		public int AmountCur { get; }
		public int AmountMax { get; }
		public int Number { get; }
		public int Graphic { get; }
		public int Level { get; }
		public int Price { get; set; }

		public Item Reconstruct()
		{
			SmallHunterBOD bod = null;

			bod = new SmallHunterBOD(AmountCur, AmountMax, ItemType, Number, Graphic, Level);

			return bod;
		}

		public HuntingBOBSmallEntry(SmallHunterBOD bod)
		{
			ItemType = bod.Type;
			AmountCur = bod.AmountCur;
			AmountMax = bod.AmountMax;
			Number = bod.Number;
			Graphic = bod.Graphic;
			Level = bod.Level;
		}

		public HuntingBOBSmallEntry(GenericReader reader)
		{
			int version = reader.ReadEncodedInt();

			switch (version)
			{
				case 0:
				{
					string type = reader.ReadString();

					if (type != null)
						ItemType = ScriptCompiler.FindTypeByFullName(type);
					AmountCur = reader.ReadEncodedInt();
					AmountMax = reader.ReadEncodedInt();
					Number = reader.ReadEncodedInt();
					Graphic = reader.ReadEncodedInt();
					Price = reader.ReadEncodedInt();
					Level = reader.ReadEncodedInt();

					break;
				}
			}
		}

		public void Serialize(GenericWriter writer)
		{
			writer.WriteEncodedInt(0); // version

			writer.Write(ItemType == null ? null : ItemType.FullName);
			writer.WriteEncodedInt(AmountCur);
			writer.WriteEncodedInt(AmountMax);
			writer.WriteEncodedInt(Number);
			writer.WriteEncodedInt(Graphic);
			writer.WriteEncodedInt(Price);
			writer.WriteEncodedInt(Level);
		}
	}
}
