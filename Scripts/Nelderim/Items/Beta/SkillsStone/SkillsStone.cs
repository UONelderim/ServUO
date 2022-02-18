// ID 0000042

#region References

using Server.Gumps;
using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class SkillsStone : Item
	{
		[Constructable]
		public SkillsStone()
			: base(0xEDC)
		{
			Movable = false;
			Name = "kamien umiejetnosci";
			Hue = 2451;
		}

		public SkillsStone(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from is PlayerMobile)
			{
				from.SendGump(new PvPSkillsGump(from, @from));
			}
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
