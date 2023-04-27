using System;
using Server;


namespace Server.Items
{
	public abstract class BaseWywarSily : Item
	{

		public virtual TimeSpan Cooldown { get { return TimeSpan.FromMinutes(60.0); }  }

		public override double DefaultWeight
		{
			get { return 1.0; }
		}

		public BaseWywarSily( int hue ) : base( 3854 )
		{
			Hue = hue;
		}

		public BaseWywarSily( Serial serial ) : base( serial )
		{
		}
		

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
			}
			else if ( from.GetStatMod( "[Wywar] STR" ) != null )
			{
				from.SendLocalizedMessage( 1062927 ); // You have eaten one of these recently and eating another would provide no benefit.
			}
			else
			{
				from.PlaySound( 0x1EE );
				from.AddStatMod( new StatMod( StatType.Str, "[Wywar] STR", 10, TimeSpan.FromMinutes( 5.0 ) ) );

				Consume();
			}
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

	public class WywarSily : BaseWywarSily
	{


		[Constructable]
		public WywarSily() : base( 3854 )
		{
		    Stackable = true;
			Name = "Wywar Sily";
			Hue = 2377;
		}

		public WywarSily( Serial serial ) : base( serial )
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