// 05.06.26 :: LogoS

using System;
using Server;
using System.Collections;
using Server.Network;
using Server.Gumps;
using Server.Commands;

namespace Server.Nelderim
{
	public class RegionsGump : Gump
	{
		public static void Initialize()
		{
            CommandSystem.Register( "Regions", AccessLevel.Counselor, new CommandEventHandler( RegionsList_OnCommand ) );
            CommandSystem.Register( "RegionsList", AccessLevel.Counselor, new CommandEventHandler( RegionsList_OnCommand ) );
		}

		[Usage( "RegionsList" )]
		[Aliases( "RegionsList" )]
		[Description( "Lists Nelderim Regions." )]
		private static void RegionsList_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendGump( new RegionsGump( e.Mobile ) );
		}

		public static bool OldStyle = PropsConfig.OldStyle;

		public static int GumpOffsetX = PropsConfig.GumpOffsetX;
		public static int GumpOffsetY = PropsConfig.GumpOffsetY;

		public static int TextHue = PropsConfig.TextHue;
		public static int TextOffsetX = PropsConfig.TextOffsetX;

		public static int OffsetGumpID = PropsConfig.OffsetGumpID;
		public static int HeaderGumpID = PropsConfig.HeaderGumpID;
		public static int  EntryGumpID = PropsConfig.EntryGumpID;
		public static int   BackGumpID = PropsConfig.BackGumpID;
		public static int    SetGumpID = PropsConfig.SetGumpID;

		public static int SetWidth = PropsConfig.SetWidth;
		public static int SetOffsetX = PropsConfig.SetOffsetX, SetOffsetY = PropsConfig.SetOffsetY;
		public static int SetButtonID1 = PropsConfig.SetButtonID1;
		public static int SetButtonID2 = PropsConfig.SetButtonID2;

		public static int PrevWidth = 24;
		public static int PrevOffsetX = PropsConfig.PrevOffsetX, PrevOffsetY = PropsConfig.PrevOffsetY;
		public static int PrevButtonID1 = PropsConfig.PrevButtonID1;
		public static int PrevButtonID2 = PropsConfig.PrevButtonID2;

		public static int NextWidth = 24; //PropsConfig.NextWidth;
		public static int NextOffsetX = PropsConfig.NextOffsetX, NextOffsetY = PropsConfig.NextOffsetY;
		public static int NextButtonID1 = PropsConfig.NextButtonID1;
		public static int NextButtonID2 = PropsConfig.NextButtonID2;

		public static int OffsetSize = PropsConfig.OffsetSize;

		public static int EntryHeight = PropsConfig.EntryHeight;
		public static int BorderSize = PropsConfig.BorderSize;

		private static bool PrevLabel = false, NextLabel = false;

		private static int PrevLabelOffsetX = PrevWidth + 1;
		private static int PrevLabelOffsetY = 0;

		private static int NextLabelOffsetX = -29;
		private static int NextLabelOffsetY = 0;

		private static int EntryWidth = 180;
		private static int EntryCount = 15;
		
		// LogoS - 13.06.05
		
		private static int ReturnWidth = 24;
		private static int ReturnButtonID1 = 2223; // 2224
		private static int ReturnButtonID2 = 2223;
		
		private static int NextRegWidth = 24;
		private static int NextRegButtonID1 = 2224; // 2224
		private static int NextRegButtonID2 = 2224;
		
		
		
		
		private class InternalComparer : IComparer
		{
			public static readonly IComparer Instance = new InternalComparer();

			public InternalComparer()
			{
			}

			public int Compare( object x, object y )
			{
				if ( x == null && y == null )
					return 0;
				else if ( x == null )
					return -1;
				else if ( y == null )
					return 1;

				RegionsEngineRegion a = x as RegionsEngineRegion;
				RegionsEngineRegion b = y as RegionsEngineRegion;

				if ( a == null || b == null )
					throw new ArgumentException();

				return Insensitive.Compare( a.Name, b.Name );
			}
		}
				
		private static int TotalWidth = OffsetSize + EntryWidth + OffsetSize + SetWidth  + OffsetSize + NextWidth + OffsetSize + PrevWidth + OffsetSize;
		private static int TotalHeight = OffsetSize + EntryHeight + OffsetSize + ((EntryHeight + OffsetSize) * (EntryCount + 1));

		private static int BackWidth = BorderSize + TotalWidth + BorderSize;
		private static int BackHeight = BorderSize + TotalHeight + BorderSize;

		private Mobile m_Owner;
		private string m_Parent;
		private ArrayList m_Regions_List;
		private int m_Page;


		public RegionsGump( Mobile owner ) : this( owner, "Default"  , 0 )
		{
		}

		public RegionsGump( Mobile owner, string parent , int page ) : base( GumpOffsetX, GumpOffsetY )
		{
			owner.CloseGump( typeof( RegionsGump ) );

			m_Owner = owner;

			Initialize( parent , page );
		}

		public static ArrayList BuildRegionList( string parent )
		{
			ArrayList list = new ArrayList();
			foreach( RegionsEngineRegion region in RegionsEngine.NelderimRegions ) 
			{
				if( region.Parent == parent && region.Name != "Default" )
				{
					list.Add( region );
				}
			}

			list.Sort( InternalComparer.Instance );

			return list;
		}

		public void Initialize( string parent , int page )
		{
			m_Parent = parent;
			m_Page = page;
			m_Regions_List = BuildRegionList( parent );
			
			int count = m_Regions_List.Count - (page * EntryCount);

			if ( count < 0 )
				count = 0;
			else if ( count > EntryCount )
				count = EntryCount;

			int totalHeight = OffsetSize + EntryHeight + OffsetSize + ((EntryHeight + OffsetSize) * (count + 1));

			AddPage( 0 );

			AddBackground( 0, 0, BackWidth, BorderSize + totalHeight + BorderSize, BackGumpID );
			AddImageTiled( BorderSize, BorderSize, TotalWidth - (OldStyle ? SetWidth + OffsetSize : 0), totalHeight, OffsetGumpID );

			int x = BorderSize + OffsetSize;
			int y = BorderSize + OffsetSize;

			int emptyWidth = TotalWidth - PrevWidth - NextWidth - (OffsetSize * 4) - (OldStyle ? SetWidth + OffsetSize : 0);

			if ( !OldStyle )
				AddImageTiled( x - (OldStyle ? OffsetSize : 0), y, emptyWidth + (OldStyle ? OffsetSize * 2 : 0), EntryHeight, EntryGumpID );

			AddLabel( x + TextOffsetX, y, TextHue, String.Format( "Page {0} of {1} ({2})", page+1, (m_Regions_List.Count + EntryCount - 1) / EntryCount, m_Regions_List.Count ) );

			x += emptyWidth + OffsetSize;

			if ( OldStyle )
				AddImageTiled( x, y, PrevWidth, EntryHeight, HeaderGumpID );
			else
				AddImageTiled( x, y, PrevWidth, EntryHeight, HeaderGumpID );

			if ( page > 0 )
			{
				AddButton( x + PrevOffsetX, y + PrevOffsetY, PrevButtonID1, PrevButtonID2, 1, GumpButtonType.Reply, 0 );

				if ( PrevLabel )
					AddLabel( x + PrevLabelOffsetX, y + PrevLabelOffsetY, TextHue, "Previous" );
			}
			
			x += PrevWidth + OffsetSize;

			if ( !OldStyle )
				AddImageTiled( x, y, NextWidth, EntryHeight, HeaderGumpID );

			if ( (page + 1) * EntryCount < m_Regions_List.Count )
			{
				AddButton( x + NextOffsetX, y + NextOffsetY, NextButtonID1, NextButtonID2, 2, GumpButtonType.Reply, 1 );

				if ( NextLabel )
					AddLabel( x + NextLabelOffsetX, y + NextLabelOffsetY, TextHue, "Next" );
			}

			x = BorderSize + OffsetSize;
			y += EntryHeight + OffsetSize;

			if ( SetGumpID != 0 )
				AddImageTiled( x, y, SetWidth, EntryHeight, SetGumpID );
				
			if( parent == "Default" )
				AddButton( x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, 4, GumpButtonType.Reply, 0 );
				
			x += SetWidth + OffsetSize;	
			
				
			AddImageTiled( x, y, EntryWidth , EntryHeight, SetGumpID );
			AddLabelCropped( x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, 0x58,  parent );
			
			x += EntryWidth + OffsetSize;
			

				if ( SetGumpID != 0 )
					AddImageTiled( x, y, ReturnWidth, EntryHeight, SetGumpID );
					
				if( CheckOverridingRegion( parent ) )							
					AddButton( x + SetOffsetX, y + SetOffsetY, ReturnButtonID1, ReturnButtonID2, 3, GumpButtonType.Reply, 0 );

				x += ReturnWidth + OffsetSize;
				
				if ( SetGumpID != 0 )
					AddImageTiled( x, y, NextRegWidth, EntryHeight, SetGumpID );
					
	
			
			
			for ( int i = 0, index = page * EntryCount; i < EntryCount && index < m_Regions_List.Count; ++i, ++index )
			{
				x = BorderSize + OffsetSize;
				y += EntryHeight + OffsetSize;
				
				RegionsEngineRegion reg = (RegionsEngineRegion)m_Regions_List[index];
				if ( SetGumpID != 0 )
					AddImageTiled( x, y, SetWidth, EntryHeight, SetGumpID );
					
				AddButton( x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, i + 5, GumpButtonType.Reply, 0 );
				x += SetWidth + OffsetSize;
				
				AddImageTiled( x, y, EntryWidth, EntryHeight, EntryGumpID );
				AddLabelCropped( x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, 0x58,  StyleText( reg.Name ) );

				x += EntryWidth + OffsetSize;
				
				if ( SetGumpID != 0 )
					AddImageTiled( x, y, ReturnWidth, EntryHeight, SetGumpID );
	
				x += ReturnWidth + OffsetSize;
				
				if ( SetGumpID != 0 )
					AddImageTiled( x, y, NextRegWidth, EntryHeight, SetGumpID );
				if( CheckSecondaryRegion( reg.Name ) )
					AddButton( x + SetOffsetX, y + SetOffsetY, NextRegButtonID1, NextRegButtonID2, i + 6 + ( EntryCount * 2 ), GumpButtonType.Reply, 0 );

					
			}
		
		}
		public static bool CheckOverridingRegion( string parent )
		{
			foreach( RegionsEngineRegion region in RegionsEngine.NelderimRegions ) 
			{
				if( region.Name == parent && region.Name != "Default" )
				{		
					return true;
				}
			}
			return false;
		}
		
		public static string FindOverridingRegion( string parent )
		{
			foreach( RegionsEngineRegion region in RegionsEngine.NelderimRegions ) 
			{
				if( region.Name == parent )
				{
					return region.Parent;
				}
			}
			return parent;
		}
		
		public static bool CheckSecondaryRegion( string name )
		{
			foreach( RegionsEngineRegion region in RegionsEngine.NelderimRegions ) 
			{
				if( region.Parent == name )
				{
					return true;
				}
			}
			return false;
		}
				
		public static string StyleText( string text )
		{
			return text.Replace( "_" , " " );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			switch ( info.ButtonID )
			{
				case 0: // Closed
				{
					return;
				}
				case 1: // Previous
				{
					if ( m_Page > 0 )
						from.SendGump( new RegionsGump( from, m_Parent , m_Page - 1 ) );

					break;
				}
				case 2: // Next
				{
					if ( (m_Page + 1) * EntryCount < m_Regions_List.Count )
						from.SendGump( new RegionsGump( from, m_Parent, m_Page + 1 ) );

					break;
				}
				case 3: // Return to Parent
				{
					if ( CheckOverridingRegion( m_Parent ) )
						from.SendGump( new RegionsGump( from, FindOverridingRegion( m_Parent ) , 0  ) );

					break;
				}
				case 4: // rumor for Global ( Default )
				{
					try
					{
						from.SendGump( new RumorsEditGump( from , RegionsEngine.GetRegion( m_Parent ) , PageName.List ) );
					}
					catch( Exception e )
					{
						Console.WriteLine( e.ToString() );
					}

					break;
				}
				default:
				{
						
						if( info.ButtonID - 5 < EntryCount )
						{
							int index = (m_Page * EntryCount) + (info.ButtonID - 5 );
							
							if( index >= 0 && index < m_Regions_List.Count )
							{
								RegionsEngineRegion region = (RegionsEngineRegion)m_Regions_List[index];
								from.SendGump( new RumorsEditGump( from , region , PageName.List ) );
							}
						}
						else if ( info.ButtonID - 6 >=  2 * EntryCount )
						{
							int index = (m_Page * EntryCount) + (info.ButtonID - 6 - 2 * EntryCount);
							
							if ( index >= 0 && index < m_Regions_List.Count )
							{
								RegionsEngineRegion region = (RegionsEngineRegion)m_Regions_List[index];
								if( CheckSecondaryRegion( region.Name ) )
								{
									from.SendGump( new RegionsGump( from, region.Name , 0 ) );
								}
							}
						}
					break;
				}
			}
		}
	}
}
