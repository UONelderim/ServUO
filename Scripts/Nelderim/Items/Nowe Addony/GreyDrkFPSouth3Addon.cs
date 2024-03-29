////////////////////////////////////////
//                                     //
//   Generated by CEO's YAAAG - Ver 2  //
// (Yet Another Arya Addon Generator)  //
//    Modified by Hammerhand for       //
//      SA & High Seas content         //
//                                     //
////////////////////////////////////////

namespace Server.Items
{
	public class GreyDrkFPSouth3Addon : BaseAddon
	{
		private static readonly int[,] m_AddOnSimpleComponents =
		{
			{ 1315, 2, 1, 0 }, { 1315, 0, 1, 0 }, { 1315, 1, 1, 0 } // 1	2	3	
			,
			{ 1822, 2, 0, 0 }, { 1822, 2, -1, 0 }, { 1822, 1, -1, 0 } // 8	9	10	
			,
			{ 1822, 0, -1, 0 }, { 1993, 0, 0, 10 }, { 1993, 1, 0, 10 } // 11	12	13	
			,
			{ 1993, 2, 0, 10 }, { 1315, 1, -1, 0 }, { 1315, 2, -1, 0 } // 14	15	16	
			,
			{ 2598, 2, 0, 11 }, { 1315, 2, 0, 0 }, { 1822, 2, -1, 11 } // 17	20	21	
			,
			{ 1822, 1, -1, 11 }, { 1822, 2, -1, 5 }, { 1822, 2, 0, 5 } // 22	23	24	
			,
			{ 1822, 0, -1, 5 }, { 1822, 1, -1, 5 }, { 1993, 2, -1, 10 } // 25	26	27	
			,
			{ 1993, 0, -1, 10 }, { 1315, 0, -1, 0 }, { 1993, 1, -1, 10 } // 28	29	30	
			,
			{ 1822, 0, -1, 11 }, { 1315, -2, 1, 0 }, { 1315, -1, 1, 0 } // 31	32	33	
			,
			{ 1822, -1, -1, 0 }, { 1822, -2, -1, 0 }, { 1822, -2, -1, 5 } // 36	37	38	
			,
			{ 1822, -2, 0, 0 }, { 1315, -2, 0, 0 }, { 1315, -2, -1, 0 } // 39	40	41	
			,
			{ 1315, -1, -1, 0 }, { 1993, -2, 0, 10 }, { 2598, -2, 0, 11 } // 42	43	45	
			,
			{ 1822, -2, 0, 5 }, { 1822, -1, -1, 11 }, { 1993, -2, -1, 10 } // 46	47	48	
			,
			{ 1822, -2, -1, 11 }, { 1993, -1, -1, 10 }, { 1822, -1, -1, 5 } // 49	50	51	
			,
			{ 1993, -1, 0, 10 } // 52	
		};


		public override BaseAddonDeed Deed
		{
			get
			{
				return new GreyDrkFPSouth3AddonDeed();
			}
		}

		[Constructable]
		public GreyDrkFPSouth3Addon()
		{
			for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
				AddComponent(new AddonComponent(m_AddOnSimpleComponents[i, 0]), m_AddOnSimpleComponents[i, 1],
					m_AddOnSimpleComponents[i, 2], m_AddOnSimpleComponents[i, 3]);


			AddComplexComponent(this, 3555, 1, 0, 0, 0, 29, "", 1); // 4
			AddComplexComponent(this, 3555, 1, 0, 3, 0, 29, "", 1); // 5
			AddComplexComponent(this, 3555, 0, 0, 0, 0, 29, "", 1); // 6
			AddComplexComponent(this, 3555, 0, 0, 3, 0, 29, "", 1); // 7
			AddComplexComponent(this, 1315, 0, 0, 0, 1147, -1, "", 1); // 18
			AddComplexComponent(this, 1315, 1, 0, 0, 1147, -1, "", 1); // 19
			AddComplexComponent(this, 3555, -1, 0, 0, 0, 29, "", 1); // 34
			AddComplexComponent(this, 3555, -1, 0, 3, 0, 29, "", 1); // 35
			AddComplexComponent(this, 1315, -1, 0, 0, 1147, -1, "", 1); // 44
		}

		public GreyDrkFPSouth3Addon(Serial serial) : base(serial)
		{
		}

		private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset,
			int hue, int lightsource)
		{
			AddComplexComponent(addon, item, xoffset, yoffset, zoffset, hue, lightsource, null, 1);
		}

		private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset,
			int hue, int lightsource, string name, int amount)
		{
			AddonComponent ac;
			ac = new AddonComponent(item);
			if (name != null && name.Length > 0)
				ac.Name = name;
			if (hue != 0)
				ac.Hue = hue;
			if (amount > 1)
			{
				ac.Stackable = true;
				ac.Amount = amount;
			}

			if (lightsource != -1)
				ac.Light = (LightType)lightsource;
			addon.AddComponent(ac, xoffset, yoffset, zoffset);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // Version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class GreyDrkFPSouth3AddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new GreyDrkFPSouth3Addon();
			}
		}

		[Constructable]
		public GreyDrkFPSouth3AddonDeed()
		{
			Name = "GreyDrkFPSouth3";
		}

		public GreyDrkFPSouth3AddonDeed(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // Version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
