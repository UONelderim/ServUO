#region References

using Server.Network;

#endregion

namespace Server.Items
{
	public class WrappedBod : Item
	{
		public override string DefaultName
		{
			get { return "Zmumifikowany tułów"; }
		}

		[Constructable]
		public WrappedBod() : base(0x1D8A)
		{
			Weight = 1.0;
			Stackable = true;
		}

		public WrappedBod(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(this.GetWorldLocation(), 3))
				from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
			else
				from.SendAsciiMessage("A corpse wrapped in bandages.");
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
