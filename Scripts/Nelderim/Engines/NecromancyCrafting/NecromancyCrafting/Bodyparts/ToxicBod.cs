using Server.Network;

namespace Server.Items
{
	public class ToxicBod : Item
	{
		public override string DefaultName
		{
			get { return "toksyczne ciało"; }
		}

		[Constructable]
		public ToxicBod() : base(0x1CDE)
		{
			Weight = 1.0;
			Stackable = true;
		}

		public ToxicBod(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(this.GetWorldLocation(), 3))
				from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
			else
				from.SendAsciiMessage("The rotting remains of a poisoned corpse.");
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
