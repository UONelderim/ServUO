using System.Collections;
using Server.Gumps;
using Server.Items;
using Server.Multis;

namespace Server.Engines.BulkOrders
{
	public class HuntingBulkOrderBook : Item, ISecurable, IUsesRemaining
	{
		private ArrayList m_Entries;
		private BOBFilter m_Filter;
		private string m_BookName;
		private SecureLevel m_Level;
		private int m_UsesRemaining;

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get{ return m_UsesRemaining; }
			set{ m_UsesRemaining = value; }
		}

		public bool ShowUsesRemaining{ get{ return false;} set{} }

		[CommandProperty( AccessLevel.GameMaster )]
		public string BookName
		{
			get{ return m_BookName; }
			set{ m_BookName = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SecureLevel Level
		{
			get{ return m_Level; }
			set{ m_Level = value; }
		}

		public ArrayList Entries
		{
			get{ return m_Entries; }
		}

		public BOBFilter Filter
		{
			get{ return m_Filter; }
		}

		[Constructable]
		public HuntingBulkOrderBook() : base( 0x2259 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			
			Name = "Ksiega zlecen Mysliwego";
			Hue = 2702;

			m_Entries = new ArrayList();
			m_Filter = new BOBFilter();

			m_Level = SecureLevel.CoOwners;

			m_UsesRemaining = 1500;
		}

		public HuntingBulkOrderBook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) m_UsesRemaining );

			writer.Write( (int) m_Level );

			writer.Write( m_BookName );

			m_Filter.Serialize( writer );

			writer.WriteEncodedInt( (int) m_Entries.Count );

			for ( int i = 0; i < m_Entries.Count; ++i )
			{
				object obj = m_Entries[i];

				if ( obj is BOBLargeEntry )
				{
					writer.WriteEncodedInt( 0 );
					((BOBLargeEntry)obj).Serialize( writer );
				}
				else if ( obj is BOBSmallEntry )
				{
					writer.WriteEncodedInt( 1 );
					((BOBSmallEntry)obj).Serialize( writer );
				}
				else
				{
					writer.WriteEncodedInt( -1 );
				}
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_UsesRemaining = (int)reader.ReadInt();
					m_Level = (SecureLevel)reader.ReadInt();
					goto case 0;
				}
				case 0:
				{
					m_BookName = reader.ReadString();

					m_Filter = new BOBFilter( reader );

					int count = reader.ReadEncodedInt();

					m_Entries = new ArrayList( count );

					for ( int i = 0; i < count; ++i )
					{
						int v = reader.ReadEncodedInt();

						switch ( v )
						{
							case 0: m_Entries.Add( new BOBLargeEntry( reader ) ); break;
							case 1: m_Entries.Add( new BOBSmallEntry( reader ) ); break;
						}
					}

					break;
				}
			}

			var newBook = new BulkOrderBook();
			newBook.Entries.AddRange(m_Entries);
			newBook.Name = m_BookName;
			ReplaceWith(newBook);
		}
	}
}
