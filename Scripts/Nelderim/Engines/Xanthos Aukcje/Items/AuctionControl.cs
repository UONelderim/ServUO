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

namespace Arya.Auction
{
	/// <summary>
	/// This is the auction control stone. This item should NOT be deleted
	/// </summary>
	public class AuctionControl : Item
	{
		/// <summary>
		/// This item holds all the current auctions
		/// </summary>
		private ArrayList m_Auctions;
		/// <summary>
		/// This lists all auctions whose reserve hasn't been met
		/// </summary>
		private ArrayList m_Pending;
		/// <summary>
		/// Flag used to force the deletion of the system
		/// </summary>
		private bool m_Delete = false;
		/// <summary>
		/// The max number of concurrent auctions for each account
		/// </summary>
		private int m_MaxAuctionsParAccount = 5;
		/// <summary>
		/// The minimum number of days an auction must last
		/// </summary>
		private int m_MinAuctionDays = 1;
		/// <summary>
		/// The max number of days an auction can last
		/// </summary>
		private int m_MaxAuctionDays = 14;

		/// <summary>
		/// Gets or sets the list of current auction entries
		/// </summary>
		public ArrayList Auctions
		{
			get { return m_Auctions; }
			set { m_Auctions = value; }
		}

		/// <summary>
		/// Gets or sets the pending auction entries
		/// </summary>
		public ArrayList Pending
		{
			get { return m_Pending; }
			set { m_Pending = value; }
		}

		[ CommandProperty( AccessLevel.GameMaster, AccessLevel.Administrator ) ]
		/// <summary>
		/// Gets or sets the max number of auctions a single account can have
		/// </summary>
		public int MaxAuctionsParAccount
		{
			get { return m_MaxAuctionsParAccount; }
			set { m_MaxAuctionsParAccount = value; }
		}

		[ CommandProperty( AccessLevel.GameMaster, AccessLevel.Administrator ) ]
		/// <summary>
		/// Gets or sets the minimum days an auction must last
		/// </summary>
		public int MinAuctionDays
		{
			get { return m_MinAuctionDays; }
			set { m_MinAuctionDays = value; }
		}

		[ CommandProperty( AccessLevel.GameMaster, AccessLevel.Administrator ) ]
		/// <summary>
		/// Gets or sets the max number of days an auction can last
		/// </summary>
		public int MaxAuctionDays
		{
			get { return m_MaxAuctionDays; }
			set { m_MaxAuctionDays = value; }
		}

		public AuctionControl() : base( 4484 )
		{
			Name = "Auction System";
			Visible = false;
			Movable = false;
			m_Auctions = new ArrayList();
			m_Pending = new ArrayList();

			AuctionSystem.ControlStone = this;
		}

		public AuctionControl( Serial serial ) : base( serial )
		{
			m_Auctions = new ArrayList();
			m_Pending = new ArrayList();
		}

		#region Serialization

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize (writer);

			writer.Write( 1 ); // Version

			// Version 1 : changes in AuctionItem
			// Version 0
			writer.Write( m_MaxAuctionsParAccount );
			writer.Write( m_MinAuctionDays );
			writer.Write( m_MaxAuctionDays );

			writer.Write( m_Auctions.Count );

			foreach( AuctionItem auction in m_Auctions )
			{
				auction.Serialize( writer );
			}

			writer.Write( m_Pending.Count );

			foreach( AuctionItem auction in m_Pending )
			{
				auction.Serialize( writer );
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize (reader);

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				case 0:
					m_MaxAuctionsParAccount = reader.ReadInt();
					m_MinAuctionDays = reader.ReadInt();
					m_MaxAuctionDays = reader.ReadInt();

					int count = reader.ReadInt();

					for ( int i = 0; i < count; i++ )
					{
						m_Auctions.Add( AuctionItem.Deserialize( reader, version ) );
					}

					count = reader.ReadInt();

					for ( int i = 0; i < count; i++ )
					{
						m_Pending.Add( AuctionItem.Deserialize( reader, version ) );
					}
					break;
			}

			AuctionSystem.ControlStone = this;
		}

		#endregion

		public override void OnDelete()
		{
			// Don't allow users to delete this item unless it's done through the control gump
			if ( !m_Delete )
			{
				AuctionControl newStone = new AuctionControl();
				newStone.m_Auctions.AddRange( this.m_Auctions );
				newStone.MoveToWorld( this.Location, this.Map );
				
				newStone.Items.AddRange( Items );
				Items.Clear();
				foreach( Item item in newStone.Items )
				{
					item.Parent = newStone;
				}

				newStone.PublicOverheadMessage( Server.Network.MessageType.Regular, 0x40, false, AuctionSystem.ST[ 121 ] );
			}

			base.OnDelete ();
		}

		/// <summary>
		/// Deletes the item from the world without triggering the auto-recreation
		/// This function also closes all current auctions
		/// </summary>
		public void ForceDelete()
		{
			m_Delete = true;
			Delete();
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties (list);

			list.Add( AuctionSystem.Running ? 3005117 : 3005118 ); // [Active] - [Inactive]
		}
	}
}