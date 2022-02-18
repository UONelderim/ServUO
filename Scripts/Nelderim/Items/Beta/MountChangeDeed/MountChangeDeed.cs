// ID 0000066

#region References

using Server.Gumps;

#endregion

namespace Server.Items
{
	public class MountChangeDeed : Item
	{
		[Constructable]
		public MountChangeDeed() : base(0xED4)
		{
			Movable = true;
			Hue = 1000;
			Name = "Mount change deed";
			ItemID = 5360;
		}

		public override void OnDoubleClick(Mobile player)
		{
			if (IsChildOf(player.Backpack))
			{
				player.SendGump(new MountChangeGump(player));
			}
			else
			{
				player.SendLocalizedMessage(1042001);
			}
		}

		public MountChangeDeed(Serial serial) : base(serial)
		{
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
