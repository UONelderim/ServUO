/*
 * Created by SharpDevelop.
 * User: Shazzy
 * Date: 11/30/2005
 * Time: 7:27 PM
 *  You can change the message on the candle on lines 22 thru 27
 * ChristmasCandle2008
 */
using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x2373, 0x236E )]
	public class ChristmasCandle2010 : Item, IDyable
	{
		public static string GetRandomTitle()
		{
			string[] titles = new string[]
				{
					/*  1 */ "Wesolych Swiat od Maupishona",
					/* 2 */ "Wesolych Swiat od Kaczego",
					/* 3 */ "Wesolych Swiat od Juriego",
					/* 4 */ "Wesolych Swiat od Halranda*",
					/* 5 */ "Wesolych Swiat od Levego",
					/* 6 */ "Wesolych Swiat od Surmiego",
					/* 7 */ "Wesolych Swiat od NBladesa",

				};

			if ( titles.Length > 0 )
				return titles[Utility.Random( titles.Length )];

			return null;
		}

		private string m_Title;

		[CommandProperty( AccessLevel.GameMaster )]
		public string Title
		{
			get{ return m_Title; }
			set{ m_Title = value; InvalidateProperties(); }
		}

		[Constructable]
		public ChristmasCandle2010() : this( Utility.RandomDyedHue(), GetRandomTitle() )
		{
		}

		[Constructable]
		public ChristmasCandle2010( int hue ) : this( hue, GetRandomTitle() )
		{
		}

		[Constructable]
		public ChristmasCandle2010( string title ) : this( Utility.RandomBirdHue(), title )
		{
		}

		[Constructable]
		public ChristmasCandle2010( int hue, string title ) : base( 0x236E )
		{
		        
			Weight = 3.0;
			Hue = hue;
			LootType = LootType.Blessed;
            Light = LightType.Circle300;
			m_Title = title;
		}

        public override int LabelNumber{ get{ return 1070875; } } 

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Title != null )
				list.Add( m_Title ); 
		}

		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;

			Hue = sender.DyedHue;

			return true;
		}

		public ChristmasCandle2010( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (string) m_Title );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_Title = reader.ReadString();
					break;
				}
			}
		}
	}
}
