using System;
using Server;


namespace Server.Items
{
	public abstract class BasePotrawka : Item
	{
		
		public virtual TimeSpan Cooldown { get { return TimeSpan.FromMinutes(60.0); }  }

		public override double DefaultWeight
		{
			get { return 1.0; }
		}

		public BasePotrawka( int hue ) : base( 0x284F )
		{
			Hue = hue;
		}

		public BasePotrawka( Serial serial ) : base( serial )
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

	public class Potrawka : BasePotrawka
	{
		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
			}
			else if ( from.GetStatMod( "[Klops] STR" ) != null )
			{
				from.SendLocalizedMessage( 1062927 ); // You have eaten one of these recently and eating another would provide no benefit.
			}
			else
			{
				from.PlaySound( 0x1EE );
				from.AddStatMod( new StatMod( StatType.Str, "[Klops] STR", 10, TimeSpan.FromMinutes( 5.0 ) ) );

				Consume();
			}
		}

		[Constructable]
		public Potrawka() : base( 0x284F )
		{
		Stackable = true;
			Name = "pożywne klopsiki";
			Hue = 51;
			Label1 = "sprawia, ze stajesz sie silniejszy";
		}

		public Potrawka( Serial serial ) : base( serial )
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
			//if ( Hue == 151 )
			//	Hue = 51;
		}
	}
		public class PysznaPotrawka : BasePotrawka
	{
		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
			}
			else if ( from.GetStatMod( "[Klops] DEX" ) != null )
			{
				from.SendLocalizedMessage( 1062927 ); // You have eaten one of these recently and eating another would provide no benefit.
			}
			else
			{
				from.PlaySound( 0x1EE );
				from.AddStatMod( new StatMod( StatType.Dex, "[Klops] DEX", 10, TimeSpan.FromMinutes( 5.0 ) ) );

				Consume();
			}
		}


		[Constructable]
		public PysznaPotrawka() : base( 0x284F )
		{
		Stackable = true;
			Name = "tłuste klopsiki";
			Hue = 39;
			Label1 = "zwieksza Twoja zrecznosc";
		}

		public PysznaPotrawka( Serial serial ) : base( serial )
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
			//if ( Hue == 151 )
			//	Hue = 51;
		}
	}
	public class PotrawkaBle : BasePotrawka
	{
		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
			}
			else if ( from.GetStatMod( "[Klops] INT" ) != null )
			{
				from.SendLocalizedMessage( 1062927 ); // You have eaten one of these recently and eating another would provide no benefit.
			}
			else
			{
				from.PlaySound( 0x1EE );
				from.AddStatMod( new StatMod( StatType.Int, "[Klops] INT", 10, TimeSpan.FromMinutes( 5.0 ) ) );

				Consume();
			}
		}

		[Constructable]
		public PotrawkaBle() : base( 0x284F )
		{
		Stackable = true;
			Name = "klopsiki z dynią";
			Hue = 11;
			Label1 = "wzmaga prace umyslu";
		}

		public PotrawkaBle( Serial serial ) : base( serial )
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
			//if ( Hue == 151 )
			//	Hue = 51;
		}
	}
	
}