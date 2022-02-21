#region References

using System;

#endregion

namespace Server.Engines.BulkOrders
{
	public class HuntingBOBLargeSubEntry
	{
		public Type ItemType { get; }
		public int AmountCur { get; }
		public int Number { get; }
		public int Graphic { get; }
		public int Level { get; }

		public HuntingBOBLargeSubEntry(LargeBulkEntry lbe)
		{
			ItemType = lbe.Details.Type;
			AmountCur = lbe.Amount;
			Number = lbe.Details.Number;
			Graphic = lbe.Details.Graphic;
			Level = lbe.Details.Graphic;
		}

		public HuntingBOBLargeSubEntry(GenericReader reader)
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
					Number = reader.ReadEncodedInt();
					Graphic = reader.ReadEncodedInt();
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
			writer.WriteEncodedInt(Number);
			writer.WriteEncodedInt(Graphic);
			writer.WriteEncodedInt(Level);
		}
	}
}
