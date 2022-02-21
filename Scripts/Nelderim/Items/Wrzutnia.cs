#region References

using Server.Network;

#endregion

namespace Server.Items
{
	public class Wrzutnia : Container
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public string Label3Filter { get; set; }

		[Constructable]
		public Wrzutnia(int itemID) : base(itemID)
		{
		}

		public override bool CheckLift(Mobile from, Item item, ref LRReason reject)
		{
			if (from.AccessLevel > AccessLevel.Player)
				return true;
			reject = LRReason.CannotLift;
			return false;
		}

		public override bool CheckItemUse(Mobile from, Item item)
		{
			if (from.AccessLevel > AccessLevel.Player)
				return true;
			return false;
		}

		public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, bool checkWeight,
			int plusItems, int plusWeight)
		{
			if (Label3Filter != null && Label3Filter != item.Label3)
			{
				if (message)
					m.SendMessage("Nie mozesz tego wrzucic");

				return false;
			}

			return base.CheckHold(m, item, message, checkItems, checkWeight, plusItems, plusWeight);
		}

		public Wrzutnia(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);

			writer.Write(Label3Filter);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			Label3Filter = reader.ReadString();
		}
	}
}
