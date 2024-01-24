namespace Server.Items
{
	public class HuntersWagonAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents =  {
			  {3647, 1, 1, 20}, {3647, 1, 0, 20}, {3645, 1, 1, 17}// 1	2	3	
			, {3645, 1, 0, 17}, {2861, 1, 3, 17}, {2861, 0, 3, 17}// 4	5	6	
			, {6448, 1, 5, 7}, {6448, -1, 5, 7}, {4273, 0, 6, 12}// 7	8	9	
			, {4272, 1, 6, 12}, {4271, 2, 6, 12}, {5482, 3, 2, 15}// 10	11	12	
			, {5482, 2, 2, 3}, {441, 1, 2, 17}, {441, -2, 2, 17}// 13	16	17	
			, {441, -1, 2, 17}, {6258, 3, 0, 21}, {39690, 1, 4, 45}// 18	21	22	
			, {39690, 1, 3, 45}, {39690, 1, 2, 45}, {39690, 1, 1, 45}// 23	24	25	
			, {39690, 1, 0, 45}, {39689, 2, 4, 42}, {39689, 2, 3, 42}// 26	32	33	
			, {39689, 2, 2, 42}, {39689, 2, 1, 42}, {39689, 2, 0, 42}// 34	35	36	
			, {39691, 0, 4, 42}, {39691, 0, 3, 42}, {39691, 0, 2, 41}// 37	38	39	
			, {39691, -1, 4, 38}, {39691, -1, 3, 38}, {39691, -1, 2, 38}// 40	41	42	
			, {11541, -2, 3, 18}, {11541, 2, 3, 18}, {39689, 3, 0, 38}// 43	44	46	
			, {39689, 3, 4, 38}, {39689, 3, 3, 38}, {445, -2, 0, 17}// 48	49	50	
			, {445, 2, 0, 17}, {439, -2, 2, 17}, {444, -1, 2, 17}// 51	54	55	
			, {1825, 0, 3, 13}, {1825, 0, 2, 13}, {1825, 0, 1, 13}// 72	73	74	
			, {1825, 0, 0, 13}, {1825, -1, 2, 13}, {1825, -1, 1, 13}// 75	78	79	
			, {1825, -1, 0, 13}, {39691, -1, 0, 38}, {39691, -1, 1, 38}// 80	82	86	
			, {39689, 3, 1, 38}, {39691, 0, 0, 41}, {2650, -1, 1, 20}// 87	91	92	
			, {39691, 0, 1, 41}, {39689, 3, 2, 38}, {438, 2, 2, 17}// 93	96	97	
			, {2651, -1, 0, 20}, {21, 1, 0, 3}, {11740, -1, -2, 17}// 98	101	102	
			, {7138, -1, -1, 17}, {3707, 1, -1, 25}, {3619, 1, -2, 24}// 104	105	106	
			, {7816, 1, -3, 25}, {7816, 1, -3, 23}, {2880, 1, -1, 17}// 107	108	109	
			, {2880, 1, -2, 17}, {2880, 1, -3, 17}, {441, 1, -5, 17}// 110	111	114	
			, {441, -1, -5, 17}, {39690, 1, -2, 45}, {39691, -1, -2, 38}// 115	116	117	
			, {39691, 0, -2, 41}, {39689, 2, -2, 42}, {39689, 3, -2, 39}// 118	119	120	
			, {21, 2, -2, 12}, {39690, 1, -1, 45}, {39690, 1, -4, 45}// 121	122	123	
			, {2650, -1, -3, 20}, {39689, 2, -4, 42}, {39689, 2, -1, 42}// 132	133	134	
			, {39689, 2, -3, 42}, {39691, 0, -4, 41}, {39689, 3, -1, 38}// 135	136	138	
			, {1981, 1, -5, 3}, {441, -2, -5, 17}, {440, 2, -5, 17}// 141	144	145	
			, {39691, -1, -3, 38}, {39691, -1, -1, 38}, {445, -2, -3, 17}// 147	148	149	
			, {445, -2, -4, 17}, {445, 2, -2, 17}, {444, -1, -5, 17}// 150	151	152	
			, {445, 2, -4, 17}, {445, 2, -1, 17}, {445, -2, -1, 17}// 153	154	155	
			, {39689, 3, -4, 38}, {1825, 0, -1, 13}, {1825, 0, -2, 13}// 159	168	169	
			, {1825, 0, -3, 13}, {1825, 0, -4, 13}, {1825, -1, -1, 13}// 170	171	174	
			, {1825, -1, -2, 13}, {1825, -1, -3, 13}, {39691, -1, -4, 38}// 175	176	181	
			, {39691, 0, -3, 41}, {2651, -1, -4, 20}, {39691, 0, -1, 41}// 182	183	184	
			, {5470, 3, -4, 17}, {7815, 3, -1, 24}, {39690, 1, -3, 45}// 185	187	189	
			, {7819, 3, -2, 25}, {39689, 3, -3, 38}, {1825, -1, -4, 13}// 190	194	195	
					};

 
            
		public override BaseAddonDeed Deed => new HuntersWagonAddonDeed();

		[ Constructable ]
		public HuntersWagonAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( this, 5029, -1, 0, 23, 2213, -1, "", 1);// 14
			AddComplexComponent( this, 2649, -1, 1, 22, 2213, -1, "", 1);// 15
			AddComplexComponent( this, 1825, 1, 4, 13, 2218, -1, "", 1);// 19
			AddComplexComponent( this, 16024, 1, 4, 16, 2218, -1, "", 1);// 20
			AddComplexComponent( this, 16024, 1, 0, 16, 2218, -1, "", 1);// 27
			AddComplexComponent( this, 16024, 1, 1, 16, 2218, -1, "", 1);// 28
			AddComplexComponent( this, 16024, 1, 2, 16, 2218, -1, "", 1);// 29
			AddComplexComponent( this, 1825, 2, 3, 13, 2218, -1, "", 1);// 30
			AddComplexComponent( this, 42330, 2, 4, 5, 2218, -1, "", 1);// 31
			AddComplexComponent( this, 15773, -1, 3, 18, 2213, -1, "", 1);// 45
			AddComplexComponent( this, 15773, 2, 3, 18, 2213, -1, "", 1);// 47
			AddComplexComponent( this, 455, -2, 1, 17, 0, 27, "", 1);// 52
			AddComplexComponent( this, 453, 2, 1, 17, 0, 27, "", 1);// 53
			AddComplexComponent( this, 16024, -1, 0, 16, 2218, -1, "", 1);// 56
			AddComplexComponent( this, 16024, 2, 1, 16, 2218, -1, "", 1);// 57
			AddComplexComponent( this, 16024, 0, 1, 16, 2218, -1, "", 1);// 58
			AddComplexComponent( this, 16024, -1, 1, 16, 2218, -1, "", 1);// 59
			AddComplexComponent( this, 16024, 2, 2, 16, 2218, -1, "", 1);// 60
			AddComplexComponent( this, 16024, 0, 2, 16, 2218, -1, "", 1);// 61
			AddComplexComponent( this, 16024, -1, 2, 16, 2218, -1, "", 1);// 62
			AddComplexComponent( this, 16024, 0, 0, 16, 2218, -1, "", 1);// 63
			AddComplexComponent( this, 42328, -1, 4, 5, 2218, -1, "", 1);// 64
			AddComplexComponent( this, 16024, 0, 4, 16, 2218, -1, "", 1);// 65
			AddComplexComponent( this, 16024, 0, 3, 16, 2218, -1, "", 1);// 66
			AddComplexComponent( this, 1825, 1, 3, 13, 2218, -1, "", 1);// 67
			AddComplexComponent( this, 1825, 1, 2, 13, 2218, -1, "", 1);// 68
			AddComplexComponent( this, 1825, 1, 1, 13, 2218, -1, "", 1);// 69
			AddComplexComponent( this, 1825, 1, 0, 13, 2218, -1, "", 1);// 70
			AddComplexComponent( this, 1825, 0, 4, 13, 2218, -1, "", 1);// 71
			AddComplexComponent( this, 16024, 2, 0, 16, 2218, -1, "", 1);// 76
			AddComplexComponent( this, 1825, -1, 3, 13, 2218, -1, "", 1);// 77
			AddComplexComponent( this, 1825, 2, 2, 13, 2218, -1, "", 1);// 81
			AddComplexComponent( this, 16035, -1, 3, 19, 2218, -1, "", 1);// 83
			AddComplexComponent( this, 1825, 2, 1, 13, 2218, -1, "", 1);// 84
			AddComplexComponent( this, 16024, 1, 3, 16, 2218, -1, "", 1);// 85
			AddComplexComponent( this, 15772, 0, 3, 18, 2213, -1, "", 1);// 88
			AddComplexComponent( this, 15772, 1, 3, 18, 2213, -1, "", 1);// 89
			AddComplexComponent( this, 3693, 2, 1, 5, 1175, -1, "", 1);// 90
			AddComplexComponent( this, 1825, 2, 0, 13, 2218, -1, "", 1);// 94
			AddComplexComponent( this, 16036, 2, 3, 19, 2218, -1, "", 1);// 95
			AddComplexComponent( this, 25490, 3, 1, 0, 2218, -1, "", 1);// 99
			AddComplexComponent( this, 19513, 2, 0, 3, 2724, -1, "", 1);// 100
			AddComplexComponent( this, 41473, -1, -2, 17, 0, 232, "", 1);// 103
			AddComplexComponent( this, 5029, -1, -4, 23, 2213, -1, "", 1);// 112
			AddComplexComponent( this, 2649, -1, -3, 22, 2213, -1, "", 1);// 113
			AddComplexComponent( this, 16024, 1, -4, 16, 2218, -1, "", 1);// 124
			AddComplexComponent( this, 16024, 1, -3, 16, 2218, -1, "", 1);// 125
			AddComplexComponent( this, 16024, 1, -2, 16, 2218, -1, "", 1);// 126
			AddComplexComponent( this, 16024, 1, -1, 16, 2218, -1, "", 1);// 127
			AddComplexComponent( this, 1825, 2, -4, 14, 2218, -1, "", 1);// 128
			AddComplexComponent( this, 1825, 2, -3, 13, 2218, -1, "", 1);// 129
			AddComplexComponent( this, 1825, 2, -2, 13, 2218, -1, "", 1);// 130
			AddComplexComponent( this, 1825, 2, -1, 13, 2218, -1, "", 1);// 131
			AddComplexComponent( this, 1981, 1, -5, 16, 2218, -1, "", 1);// 137
			AddComplexComponent( this, 1984, 2, -5, 16, 2218, -1, "", 1);// 139
			AddComplexComponent( this, 1983, -1, -5, 16, 2218, -1, "", 1);// 140
			AddComplexComponent( this, 15772, 0, -4, 17, 2213, -1, "", 1);// 142
			AddComplexComponent( this, 15772, 1, -4, 17, 2213, -1, "", 1);// 143
			AddComplexComponent( this, 453, 2, -3, 17, 0, 27, "", 1);// 146
			AddComplexComponent( this, 16024, 2, -4, 16, 2218, -1, "", 1);// 156
			AddComplexComponent( this, 16024, 0, -4, 16, 2218, -1, "", 1);// 157
			AddComplexComponent( this, 16024, -1, -4, 16, 2218, -1, "", 1);// 158
			AddComplexComponent( this, 1825, 1, -4, 13, 2218, -1, "", 1);// 160
			AddComplexComponent( this, 16024, 2, -2, 16, 2218, -1, "", 1);// 161
			AddComplexComponent( this, 16024, 0, -1, 16, 2218, -1, "", 1);// 162
			AddComplexComponent( this, 1825, 1, -1, 13, 2218, -1, "", 1);// 163
			AddComplexComponent( this, 1825, 1, -2, 13, 2218, -1, "", 1);// 164
			AddComplexComponent( this, 1825, 1, -3, 13, 2218, -1, "", 1);// 165
			AddComplexComponent( this, 16024, -1, -3, 16, 2218, -1, "", 1);// 166
			AddComplexComponent( this, 16024, 2, -1, 16, 2218, -1, "", 1);// 167
			AddComplexComponent( this, 16024, 0, -3, 16, 2218, -1, "", 1);// 172
			AddComplexComponent( this, 16024, -1, -2, 16, 2218, -1, "", 1);// 173
			AddComplexComponent( this, 16024, 2, -3, 16, 2218, -1, "", 1);// 177
			AddComplexComponent( this, 3693, 2, -3, 4, 1175, -1, "", 1);// 178
			AddComplexComponent( this, 15773, -1, -4, 17, 2213, -1, "", 1);// 179
			AddComplexComponent( this, 15773, 2, -4, 17, 2213, -1, "", 1);// 180
			AddComplexComponent( this, 455, -2, -2, 17, 0, 27, "", 1);// 186
			AddComplexComponent( this, 1981, 0, -5, 16, 2218, -1, "", 1);// 188
			AddComplexComponent( this, 16024, -1, -1, 16, 2218, -1, "", 1);// 191
			AddComplexComponent( this, 25490, 3, -3, 0, 2218, -1, "", 1);// 192
			AddComplexComponent( this, 16024, 0, -2, 16, 2218, -1, "", 1);// 193

		}

		public HuntersWagonAddon( Serial serial ) : base( serial )
		{
		}

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource)
        {
            AddComplexComponent(addon, item, xoffset, yoffset, zoffset, hue, lightsource, null, 1);
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource, string name, int amount)
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
                ac.Light = (LightType) lightsource;
            addon.AddComponent(ac, xoffset, yoffset, zoffset);
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class HuntersWagonAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon => new HuntersWagonAddon();

		[Constructable]
		public HuntersWagonAddonDeed()
		{
			Name = "HuntersWagon";
		}

		public HuntersWagonAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
