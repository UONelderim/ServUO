namespace Server.Items
{
	public class PetBondingDeed : Item
	{
		[Constructable]
		public PetBondingDeed() : base(0x14F0)
		{
			Weight = 1.0;
			Name = "zwoj oswajacza";
			LootType = LootType.Blessed;
			Hue = 572;
		}

		public PetBondingDeed(Serial serial) : base(serial)
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
			LootType = LootType.Blessed;

			int version = reader.ReadInt();
		}

		public override bool DisplayLootType => false;

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it. 
			}
			else
			{
				from.SendLocalizedMessage(1152922); // Wskaz zwierze do uwiernienia
				from.Target = new BondingTarget(this); // Call our target 
			}
		}
	}
}
