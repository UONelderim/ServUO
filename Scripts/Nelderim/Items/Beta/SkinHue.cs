using System;
using System.Text;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class SkinHue : Item
	{
		[Constructable]
		public SkinHue() : base( 5360 )
		{
			Weight = 1.0;
            Name = "Zwoj zmiany skory";
            Hue = 1177;
		}

        public SkinHue(Serial serial): base(serial)
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); 
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 1 ) )
			{
				from.CloseGump( typeof( SkinHueGump ) );
				from.SendGump( new SkinHueGump( this ) );
			}
			else
			{
				from.LocalOverheadMessage( MessageType.Regular, 906, 1019045 ); 
			}	
		}
	}

	public class SkinHueGump : Gump
	{
		private SkinHue m_SkinHue;

		private class SkinHueEntry
		{
			private string m_Name;
			private int m_HueStart;
			private int m_HueCount;

			public string Name
			{
				get
				{
					return m_Name;
				}
			}

			public int HueStart
			{
				get
				{
					return m_HueStart;
				}
			}

			public int HueCount
			{
				get
				{
					return m_HueCount;
				}
			}

			public SkinHueEntry( string name, int hueStart, int hueCount )
			{
				m_Name = name;
				m_HueStart = hueStart;
				m_HueCount = hueCount;
			}
		}

		private static SkinHueEntry[] m_Entries = new SkinHueEntry[]
			{
                //The first number is the starting hue, The second is how
                //many hues after the starting hue will show up on the page.
				new SkinHueEntry( "*****", 1602, 26 ),
				new SkinHueEntry( "*****", 1628, 27 ),
				new SkinHueEntry( "*****", 1502, 32 ),
				new SkinHueEntry( "*****", 1302, 32 ),
				new SkinHueEntry( "*****", 1402, 32 ),
				new SkinHueEntry( "*****", 1202, 24 ),
				new SkinHueEntry( "*****", 2402, 29 ),
				new SkinHueEntry( "*****", 2213, 6 ),
				new SkinHueEntry( "*****", 1102, 8 ),
				new SkinHueEntry( "*****", 1110, 8 ),
				new SkinHueEntry( "*****", 1118, 16 ),
				new SkinHueEntry( "*****", 1134, 16 )
			};

		public SkinHueGump( SkinHue dye ) : base( 50, 50 )
		{
			m_SkinHue = dye;

			AddPage( 0 );

			AddBackground( 100, 10, 350, 355, 2600 );
			AddBackground( 120, 54, 110, 270, 5100 );

            AddLabel(150, 25, 400, @"Skin Hue Selection Menu");

			AddButton( 149, 328, 4005, 4007, 1, GumpButtonType.Reply, 0 );
            AddLabel(185, 329, 250, @"Hue my skin this color!");

			for ( int i = 0; i < m_Entries.Length; ++i )
			{
				AddLabel( 130, 59 + (i * 22), m_Entries[i].HueStart - 1, m_Entries[i].Name );
				AddButton( 207, 60 + (i * 22), 5224, 5224, 0, GumpButtonType.Page, i + 1 );
			}

			for ( int i = 0; i < m_Entries.Length; ++i )
			{
				SkinHueEntry e = m_Entries[i];

				AddPage( i + 1 );

				for ( int j = 0; j < e.HueCount; ++j )
				{
					AddLabel( 278 + ((j / 16) * 80), 52 + ((j % 16) * 17), e.HueStart + j - 1, "*****" );
					AddRadio( 260 + ((j / 16) * 80), 52 + ((j % 16) * 17), 210, 211, false, (i * 100) + j );
				}
			}
		}

		public override void OnResponse( NetState from, RelayInfo info )
		{
			if ( m_SkinHue.Deleted )
				return;

			Mobile m = from.Mobile;
			int[] switches = info.Switches;

			if ( !m_SkinHue.IsChildOf( m.Backpack ) ) 
			{
				m.SendLocalizedMessage( 1042010 );
				return;
			}

			if ( info.ButtonID != 0 && switches.Length > 0 )
			{
				Item backpack = m.Backpack;

				if ( backpack == null )
				{
					m.SendMessage( "You must have a backpack to use this!" );// Had to add this so the script would work properly.
                }                                                            // You can't use it unless it is in your backpack anyways.... So it doesn't matter.
                                                                             // I'm sure there is an easier way.
				else
				{

					int entryIndex = switches[0] / 100;
					int hueOffset = switches[0] % 100;

					if ( entryIndex >= 0 && entryIndex < m_Entries.Length )
					{
						SkinHueEntry e = m_Entries[entryIndex];

						if ( hueOffset >= 0 && hueOffset < e.HueCount )
						{
							int hue = e.HueStart + hueOffset;

                            m.Hue = hue;
							m.SendMessage( "You hue your skin!" );
							m_SkinHue.Delete();
							m.PlaySound( 0x4E );
						}
					}
				}
			}
			else
			{
                m.SendMessage("You decide not to hue your skin");
			}
		}
	}
}