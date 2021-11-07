using Server.Mobiles;

namespace Server.Items
{
	[Flipable(0x9AA, 0xE7D)]
	public class UnearthedJewelryBox : LockableContainer
	{
		Mobile m_From = null;

		[Constructable]
		public UnearthedJewelryBox()
			: base(0x9AA)
		{
			Weight = 4.0;

			int num = Utility.RandomMinMax(1, 10);

			for (int i = 0; i < num; i++)
			{
				switch (Utility.Random(4))
				{
					case 0:
						AddBracelet();
						break;
					case 1:
						AddRing();
						break;
					case 2:
						AddNecklace();
						break;
					case 3:
						AddEarrings();
						break;
				}
			}
		}

		[Constructable]
		public UnearthedJewelryBox(Mobile from)
			: base(0x9AA)
		{
			Weight = 4.0;
			m_From = from;
			int num = Utility.RandomMinMax(1, 10);

			for (int i = 0; i < num; i++)
			{
				switch (Utility.Random(3))
				{
					case 0:
						AddBracelet();
						break;
					case 1:
						AddRing();
						break;
					case 2:
						AddNecklace();
						break;
					// case 3:
					//   AddEarrings();
					//   break;
				}
			}
		}

		public UnearthedJewelryBox(Serial serial)
			: base(serial)
		{
		}

		public void AddBracelet()
		{
			switch (Utility.Random(2))
			{
				case 0:
					GoldBracelet gold = new GoldBracelet();
					ApplyAttrib(gold);
					DropItem(gold);
					break;
				case 1:
					SilverBracelet silver = new SilverBracelet();
					ApplyAttrib(silver);
					DropItem(silver);
					break;
			}
		}

		public void AddEarrings()
		{
			switch (Utility.Random(2))
			{
				case 0:
					GoldEarrings gold = new GoldEarrings();
					ApplyAttrib(gold);
					DropItem(gold);
					break;
				case 1:
					SilverEarrings silver = new SilverEarrings();
					ApplyAttrib(silver);
					DropItem(silver);
					break;
			}
		}

		public void AddRing()
		{
			switch (Utility.Random(2))
			{
				case 0:
					GoldRing gold = new GoldRing();
					ApplyAttrib(gold);
					DropItem(gold);
					break;
				case 1:
					SilverRing silver = new SilverRing();
					ApplyAttrib(silver);
					DropItem(silver);
					break;
			}
		}

		public void AddNecklace()
		{
			switch (Utility.Random(2))
			{
				case 0:
					GoldNecklace gold = new GoldNecklace();
					ApplyAttrib(gold);
					DropItem(gold);
					break;
				case 1:
					SilverNecklace silver = new SilverNecklace();
					ApplyAttrib(silver);
					DropItem(silver);
					break;
			}
		}

		public void ApplyAttrib(Item item)
		{
			int props = 1 + (Utility.RandomMinMax(0, 2));
			//int luckChance = (int)(Utility.RandomDouble() * 10000);

			int luckChance;

			if (m_From != null)
				luckChance = m_From is PlayerMobile ? ((PlayerMobile)m_From).Luck : m_From.Luck;
			else
				luckChance = (int)(Utility.RandomDouble() * 100);

			int min = 1;
			int max = 100;

			BaseRunicTool.ApplyAttributesTo((BaseJewel)item, false, luckChance, props, min, max);
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
