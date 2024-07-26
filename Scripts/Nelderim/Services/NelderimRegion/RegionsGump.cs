using System;
using System.Collections.Generic;
using Server.Network;
using Server.Gumps;
using Server.Commands;
using static Server.Gumps.PropsConfig;

namespace Server.Nelderim
{
	public class RegionsGump : Gump
	{
		public static void Initialize()
		{
            CommandSystem.Register( "Regions", AccessLevel.Counselor, e => e.Mobile.SendGump(new RegionsGump(e.Mobile)) );
		}
		
		public static int PrevWidth = 24;
		public static int NextWidth = 24;

		private static bool PrevLabel = false, NextLabel = false;

		private static int PrevLabelOffsetX = PrevWidth + 1;
		private static int PrevLabelOffsetY = 0;

		private static int NextLabelOffsetX = -29;
		private static int NextLabelOffsetY = 0;

		private static int EntryWidth = 180;
		private static int EntryCount = 15;
		
		private static int ReturnWidth = 24;
		private static int ReturnButtonID1 = 2223;
		private static int ReturnButtonID2 = 2223;
		
		private static int NextRegWidth = 24;
		private static int NextRegButtonID1 = 2224;
		private static int NextRegButtonID2 = 2224;
		
		private static int TotalWidth = OffsetSize + EntryWidth + OffsetSize + SetWidth  + OffsetSize + NextWidth + OffsetSize + PrevWidth + OffsetSize;

		private static int BackWidth = BorderSize + TotalWidth + BorderSize;

		private string _currentRegion;
		private int m_Page;
		private List<NelderimRegion> _RegionList;
		
		public RegionsGump( Mobile owner, string region = "Default" , int page = 0) : base( GumpOffsetX, GumpOffsetY )
		{
			owner.CloseGump( typeof( RegionsGump ) );
			_currentRegion = region;
			m_Page = page;
			_RegionList = NelderimRegionSystem.NelderimRegions[region].Regions;
			
			int count = _RegionList.Count - page * EntryCount;

			if ( count < 0 )
				count = 0;
			else if ( count > EntryCount )
				count = EntryCount;

			int totalHeight = OffsetSize + EntryHeight + OffsetSize + (EntryHeight + OffsetSize) * (count + 1);

			AddPage( 0 );

			AddBackground( 0, 0, BackWidth, BorderSize + totalHeight + BorderSize, BackGumpID );
			AddImageTiled( BorderSize, BorderSize, TotalWidth, totalHeight, OffsetGumpID );

			int x = BorderSize + OffsetSize;
			int y = BorderSize + OffsetSize;

			int emptyWidth = TotalWidth - PrevWidth - NextWidth - OffsetSize * 4;

			AddImageTiled( x, y, emptyWidth, EntryHeight, EntryGumpID );

			AddLabel( x + TextOffsetX, y, TextHue,
				$"Page {page + 1} of {(_RegionList.Count + EntryCount - 1) / EntryCount} ({_RegionList.Count})");

			x += emptyWidth + OffsetSize;

			AddImageTiled( x, y, PrevWidth, EntryHeight, HeaderGumpID );

			if ( page > 0 )
			{
				AddButton( x + PrevOffsetX, y + PrevOffsetY, PrevButtonID1, PrevButtonID2, 1, GumpButtonType.Reply, 0 );

				if ( PrevLabel )
					AddLabel( x + PrevLabelOffsetX, y + PrevLabelOffsetY, TextHue, "Previous" );
			}
			
			x += PrevWidth + OffsetSize;

			AddImageTiled( x, y, NextWidth, EntryHeight, HeaderGumpID );

			if ( (page + 1) * EntryCount < _RegionList.Count )
			{
				AddButton( x + NextOffsetX, y + NextOffsetY, NextButtonID1, NextButtonID2, 2, GumpButtonType.Reply, 1 );

				if ( NextLabel )
					AddLabel( x + NextLabelOffsetX, y + NextLabelOffsetY, TextHue, "Next" );
			}

			x = BorderSize + OffsetSize;
			y += EntryHeight + OffsetSize;

			AddImageTiled( x, y, SetWidth, EntryHeight, SetGumpID );
				
			if( !IsDefaultRegion(region) )
				AddButton( x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, 4, GumpButtonType.Reply, 0 );
				
			x += SetWidth + OffsetSize;	
			
				
			AddImageTiled( x, y, EntryWidth , EntryHeight, SetGumpID );
			AddLabelCropped( x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, 0x58,  region );
			
			x += EntryWidth + OffsetSize;

				AddImageTiled( x, y, ReturnWidth, EntryHeight, SetGumpID );
					
				if( !IsDefaultRegion( region ) )							
					AddButton( x + SetOffsetX, y + SetOffsetY, ReturnButtonID1, ReturnButtonID2, 3, GumpButtonType.Reply, 0 );

				x += ReturnWidth + OffsetSize;
				
				AddImageTiled( x, y, NextRegWidth, EntryHeight, SetGumpID );
			
			for ( int i = 0, index = page * EntryCount; i < EntryCount && index < _RegionList.Count; ++i, ++index )
			{
				x = BorderSize + OffsetSize;
				y += EntryHeight + OffsetSize;
				
				NelderimRegion reg = _RegionList[index];
				AddImageTiled( x, y, SetWidth, EntryHeight, SetGumpID );
					
				AddButton( x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, i + 5, GumpButtonType.Reply, 0 );
				x += SetWidth + OffsetSize;
				
				AddImageTiled( x, y, EntryWidth, EntryHeight, EntryGumpID );
				AddLabelCropped( x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, 0x58,  StyleText( reg.Name ) );

				x += EntryWidth + OffsetSize;
				
				AddImageTiled( x, y, ReturnWidth, EntryHeight, SetGumpID );
	
				x += ReturnWidth + OffsetSize;
				
				AddImageTiled( x, y, NextRegWidth, EntryHeight, SetGumpID );
				if( HasSubRegions( reg.Name ) )
					AddButton( x + SetOffsetX, y + SetOffsetY, NextRegButtonID1, NextRegButtonID2, i + 6 + EntryCount * 2, GumpButtonType.Reply, 0 );

					
			}
		}
		public static bool IsDefaultRegion( string regionName )
		{
			return regionName == "Default";
		}
		
		public static string GetParentName( string regionName )
		{
			return NelderimRegionSystem.NelderimRegions[regionName].Parent?.Name ?? "Default";
		}
		
		public static bool HasSubRegions( string regionName )
		{
			return NelderimRegionSystem.NelderimRegions[regionName].Regions?.Count > 0;
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
						from.SendGump( new RegionsGump( from, _currentRegion , m_Page - 1 ) );

					break;
				}
				case 2: // Next
				{
					if ( (m_Page + 1) * EntryCount < _RegionList.Count )
						from.SendGump( new RegionsGump( from, _currentRegion, m_Page + 1 ) );

					break;
				}
				case 3: // Return to Parent
				{
					if ( !IsDefaultRegion( _currentRegion ) )
						from.SendGump( new RegionsGump( from, GetParentName( _currentRegion ) , 0  ) );

					break;
				}
				case 4: // rumor for Global ( Default )
				{
					try
					{
						from.SendGump( new RumorsEditGump( from , NelderimRegionSystem.GetRegion( _currentRegion ) , PageName.List ) );
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
							
							if( index >= 0 && index < _RegionList.Count )
							{
								NelderimRegion region = _RegionList[index];
								from.SendGump( new RumorsEditGump( from , region , PageName.List ) );
							}
						}
						else if ( info.ButtonID - 6 >=  2 * EntryCount )
						{
							int index = (m_Page * EntryCount) + (info.ButtonID - 6 - 2 * EntryCount);
							
							if ( index >= 0 && index < _RegionList.Count )
							{
								NelderimRegion region = _RegionList[index];
								if( HasSubRegions( region.Name ) )
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
