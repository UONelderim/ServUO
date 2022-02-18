#region References

using Server.Network;

#endregion

namespace Server.Items
{
	public class UnsettlingPortraitE : Item
	{
		[Constructable]
		public UnsettlingPortraitE() : base(10855)
		{
			Name = "Lustrzany Portret";
			Weight = 10.0;
			Movable = true;
		}

		public UnsettlingPortraitE(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile m)
		{
			if (m.InRange(this, 3))
			{
				switch (ItemID)
				{
					//do swap or animation here 
					case 10855: //1
						this.ItemID = 10856;
						break;
					case 10856: //2
						this.ItemID = 10855;
						break;
				}
			}
			else
			{
				m.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that
			}
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
