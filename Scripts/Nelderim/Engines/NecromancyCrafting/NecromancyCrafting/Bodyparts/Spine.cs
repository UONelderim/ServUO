#region References

using Server.Network;

#endregion

namespace Server.Items
{
	public class Spine : Item
	{
		public override string DefaultName
		{
			get { return "kręgosłup"; }
		}

		[Constructable]
		public Spine() : base(0x1B1B)
		{
			Weight = 1.0;
			Stackable = true;
		}

		public Spine(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(this.GetWorldLocation(), 3))
				from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
			else
				from.SendAsciiMessage("The spine of a skeleton.");
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
