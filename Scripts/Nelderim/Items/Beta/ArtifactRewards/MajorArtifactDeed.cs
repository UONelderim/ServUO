/////////////////////
//Crafted By Broze///
/////////////////////

using Server.Gumps;

namespace Server.Items
{
	public class MajorArtifactDeed : Item
	{
		[Constructable]
		public MajorArtifactDeed() : this(null)
		{
		}

		[Constructable]
		public MajorArtifactDeed(string name) : base(0x14F0)
		{
			Name = "zwoj artefaktu";
			LootType = LootType.Blessed;
			Hue = 1161;
		}

		public MajorArtifactDeed(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
			}
			else
			{
				from.SendGump(new MajorArtifactGump(from, this));
			}
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
