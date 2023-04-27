using System;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseMagicFish : Item
	{
		public override double DefaultWeight
		{
			get { return 1.0; }
		}

		public BaseMagicFish( int hue ) : base( 0xDD6 )
		{
			Hue = hue;
		}

		public BaseMagicFish( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class PrizedFish : BaseMagicFish
	{
		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
			}
			else if ( from.GetStatMod( "[Ser] INT" ) != null )
			{
				from.SendLocalizedMessage( 1062927 ); // You have eaten one of these recently and eating another would provide no benefit.
			}
			else
			{
				from.PlaySound( 0x1EE );
				from.AddStatMod( new StatMod( StatType.Int, "[Ser] INT", 5, TimeSpan.FromMinutes( 5.0 ) ) );

				Consume();
			}
		}

		public override int LabelNumber{ get{ return 1041073; } } // prized fish

		[Constructable]
		public PrizedFish() : base( 51 )
		{
		}

		public PrizedFish( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Hue == 151 )
				Hue = 51;
		}
	}

	public class WondrousFish : BaseMagicFish
	{
		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
			}
			else if ( from.GetStatMod( "[Ser] DEX" ) != null )
			{
				from.SendLocalizedMessage( 1062927 ); // You have eaten one of these recently and eating another would provide no benefit.
			}
			else
			{
				from.PlaySound( 0x1EE );
				from.AddStatMod( new StatMod( StatType.Dex, "[Ser] DEX", 5, TimeSpan.FromMinutes( 5.0 ) ) );

				Consume();
			}
		}

		public override int LabelNumber{ get{ return 1041074; } } // wondrous fish

		[Constructable]
		public WondrousFish() : base( 86 )
		{
		}

		public WondrousFish( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Hue == 286 )
				Hue = 86;
		}
	}

	public class TrulyRareFish : BaseMagicFish
	{
		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
			}
			else if ( from.GetStatMod( "[Ser] STR" ) != null )
			{
				from.SendLocalizedMessage( 1062927 ); // You have eaten one of these recently and eating another would provide no benefit.
			}
			else
			{
				from.PlaySound( 0x1EE );
				from.AddStatMod( new StatMod( StatType.Str, "[Ser] STR", 5, TimeSpan.FromMinutes( 5.0 ) ) );

				Consume();
			}
		}

		public override int LabelNumber{ get{ return 1041075; } } // truly rare fish

		[Constructable]
		public TrulyRareFish() : base( 76 )
		{
		}

		public TrulyRareFish( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Hue == 376 )
				Hue = 76;
		}
	}

	public class PeculiarFish : BaseMagicFish
	{
		public override int LabelNumber{ get{ return 1041076; } } // highly peculiar fish

		[Constructable]
		public PeculiarFish() : base( 66 )
		{
		}

		public PeculiarFish( Serial serial ) : base( serial )
		{
		}
		

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Hue == 266 )
				Hue = 66;
		}
	}
}