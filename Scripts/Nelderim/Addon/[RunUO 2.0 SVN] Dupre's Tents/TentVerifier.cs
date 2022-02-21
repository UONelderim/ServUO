// no longer used

//==============================================//
// Created by Dupre								//
//==============================================//

namespace Server.Items
{
	public class TentVerifier : Item
	{
		[Constructable]
		public TentVerifier()
		{
			ItemID = 6256;
			Stackable = false;
			Name = "Tent Verifier";
			Weight = 0;
			Movable = false;
			Visible = false;
			LootType = LootType.Blessed;
		}

		public TentVerifier(Serial serial) : base(serial)
		{
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

			ItemID = 6256;
			Stackable = false;
			Name = "Tent Verifier";
			Weight = 0;
			Movable = false;
			Visible = false;
			LootType = LootType.Blessed;
		}
	}
}
