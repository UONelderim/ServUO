#region AuthorHeader
//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//
#endregion AuthorHeader
using System;
using System.Collections;

using Server;
using Server.Gumps;
using Server.Items;
using Xanthos.Utilities;

namespace Arya.Auction
{
	/// <summary>
	/// Summary description for AuctionSearchGump.
	/// </summary>
	public class AuctionSearchGump : Gump
	{
		private ArrayList m_List;
		private bool m_ReturnToAuction;

		public AuctionSearchGump( Mobile m, ArrayList items, bool returnToAuction ) : base( 50, 50 )
		{
			m.CloseGump( typeof( AuctionSearchGump ) );

			m_List = items;
			m_ReturnToAuction = returnToAuction;

			MakeGump();
		}

		private void MakeGump()
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);

			AddImageTiled(49, 34, 402, 347, 3004);
			AddImageTiled(50, 35, 400, 345, 2624);
			AddAlphaRegion(50, 35, 400, 345);

			AddImage(165, 65, 10452);
			AddImage(0, 20, 10400);
			AddImage(0, 320, 10402);
			AddImage(35, 20, 10420);
			AddImage(421, 20, 10410);
			AddImage(410, 20, 10430);
			AddImageTiled(90, 32, 323, 16, 10254);
			AddLabel(185, 45, Misc.kGreenHue, AuctionSystem.ST[ 32 ] );
			AddImage(420, 320, 10412);
			AddImage(0, 170, 10401);
			AddImage(420, 170, 10411);

			// TEXT 0 : Search text
			AddLabel(70, 115, Misc.kLabelHue, AuctionSystem.ST[ 33 ] );
			AddImageTiled(145, 135, 200, 20, 3004);
			AddImageTiled(146, 136, 198, 18, 2624);
			AddAlphaRegion(146, 136, 198, 18);
			AddTextEntry(146, 135, 198, 20, Misc.kRedHue, 0, @"");

			AddLabel(70, 160, Misc.kLabelHue, AuctionSystem.ST[ 34 ] );

			AddCheck(260, 221, 2510, 2511, false, 1);
			AddLabel(280, 220, Misc.kLabelHue, AuctionSystem.ST[ 35 ] );

			if ( Core.AOS )
			{
				AddCheck(260, 261, 2510, 2511, false, 9);
				AddLabel(280, 260, Misc.kLabelHue, AuctionSystem.ST[ 36 ] );

				AddCheck(260, 241, 2510, 2511, false, 4);
				AddLabel(280, 240, Misc.kLabelHue, AuctionSystem.ST[ 37 ] );
			}

			AddCheck(260, 201, 2510, 2511, false, 3);
			AddLabel(280, 200, Misc.kLabelHue, AuctionSystem.ST[ 38 ] );

			AddCheck(260, 181, 2510, 2511, false, 5);
			AddLabel(280, 180, Misc.kLabelHue, AuctionSystem.ST[ 39 ] );

			AddCheck(90, 181, 2510, 2511, false, 6);
			AddLabel(110, 180, Misc.kLabelHue, AuctionSystem.ST[ 40 ] );

			AddCheck(90, 201, 2510, 2511, false, 7);
			AddLabel(110, 200, Misc.kLabelHue, AuctionSystem.ST[ 41 ] );

			AddCheck(90, 221, 2510, 2511, false, 8);
			AddLabel(110, 220, Misc.kLabelHue, AuctionSystem.ST[ 42 ] );

			AddCheck(90, 241, 2510, 2511, false, 2);
			AddLabel(110, 240, Misc.kLabelHue, AuctionSystem.ST[ 43 ] );

			AddCheck(90, 261, 2510, 2511, false, 12);
			AddLabel(110, 260, Misc.kLabelHue, AuctionSystem.ST[ 44 ] );

			if ( Core.AOS )
			{
				AddCheck(90, 280, 2510, 2511, false, 11);
				AddLabel(110, 279, Misc.kLabelHue, AuctionSystem.ST[ 45 ] );

				AddCheck(260, 280, 2510, 2511, false, 10);
				AddLabel(280, 279, Misc.kLabelHue, AuctionSystem.ST[ 46 ] );
			}

			// BUTTON 1 : Search
			AddButton(255, 350, 4005, 4006, 1, GumpButtonType.Reply, 0);
			AddLabel(295, 350, Misc.kLabelHue, AuctionSystem.ST[ 16 ] );

			// BUTTON 0 : Cancel
			AddButton(85, 350, 4017, 4018, 0, GumpButtonType.Reply, 0);
			AddLabel(125, 350, Misc.kLabelHue, AuctionSystem.ST[ 47 ] );
			
			// CHECK 0: Search withing existing results
			AddCheck(80, 310, 9721, 9724, false, 0);
			AddLabel(115, 312, Misc.kLabelHue, AuctionSystem.ST[ 48 ] );
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if ( ! AuctionSystem.Running )
			{
				sender.Mobile.SendMessage( AuctionConfig.MessageHue, AuctionSystem.ST[ 15 ] );
				return;
			}

			if ( info.ButtonID == 0 )
			{
				// Cancel
				sender.Mobile.SendGump( new AuctionListing( sender.Mobile, m_List, true, m_ReturnToAuction ) );
				return;
			}

			bool searchExisting = false;
			bool artifacts = false;
			bool commodity = false;
			ArrayList types = new ArrayList();
			string text = null;

			if ( info.TextEntries[ 0 ].Text != null && info.TextEntries[ 0 ].Text.Length > 0 )
			{
				text = info.TextEntries[ 0 ].Text;
			}

			foreach( int check in info.Switches )
			{
				switch ( check )
				{
					case 0 : searchExisting = true;
						break;

					case 1: types.Add( typeof( MapItem ) );
						break;

					case 2: types.Add( typeof( BaseReagent ) );
						break;

					case 3: commodity = true;
						break;

					case 4:
						types.Add( typeof( StatCapScroll ) );
						types.Add( typeof( PowerScroll ) );
						break;

					case 5: types.Add( typeof( BaseJewel ) );
						break;

					case 6: types.Add( typeof( BaseWeapon ) );
						break;

					case 7: types.Add( typeof( BaseArmor ) );
						break;

					case 8: types.Add( typeof( BaseShield ) );
						break;

					case 9: artifacts = true;
						break;

					case 10: types.Add( typeof( Server.Engines.BulkOrders.SmallBOD ) );
						break;

					case 11: types.Add( typeof( Server.Engines.BulkOrders.LargeBOD ) );
						break;

					case 12:
						types.Add( typeof( BasePotion ) );
						types.Add( typeof( PotionKeg ) );
						break;
				}
			}

			ArrayList source = null;

			if ( searchExisting )
			{
				source = new ArrayList( m_List );
			}
			else
			{
				source = new ArrayList( AuctionSystem.Auctions );
			}

			ArrayList typeSearch = null;
			ArrayList commoditySearch = null;
			ArrayList artifactsSearch = null;

			if ( types.Count > 0 )
			{
				typeSearch = AuctionSearch.ForTypes( source, types );
			}

			if ( commodity )
			{
				commoditySearch = AuctionSearch.ForCommodities( source );
			}

			if ( artifacts )
			{
				artifactsSearch = AuctionSearch.ForArtifacts( source );
			}

			ArrayList results = new ArrayList();

			if ( typeSearch == null && artifactsSearch == null && commoditySearch == null )
			{
				results.AddRange( source );
			}
			else
			{
				if ( typeSearch != null )
					results.AddRange( typeSearch );

				if ( commoditySearch != null )
					results = AuctionSearch.Merge( results, commoditySearch );

				if ( artifactsSearch != null )
					results = AuctionSearch.Merge( results, artifactsSearch );
			}

			// Perform search
			if ( text != null )
			{
				results = AuctionSearch.SearchForText( results, text );
			}
			
			sender.Mobile.SendGump( new AuctionListing( sender.Mobile, results, true, m_ReturnToAuction ) );
		}
	}
}