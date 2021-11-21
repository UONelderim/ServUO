using System;
using Server;


namespace Server.Items
{
	public abstract class BaseWywarInteligencji : Item
	{
		public virtual int Bonus{ get{ return 0; } }
		public virtual StatType Type{ get{ return StatType.Int; } }
		public virtual TimeSpan Duration{ get{ return TimeSpan.FromMinutes( 60.0 ); } }
		public virtual TimeSpan Cooldown { get { return TimeSpan.FromMinutes(60.0); }  }

		public override double DefaultWeight
		{
			get { return 1.0; }
		}

		public BaseWywarInteligencji ( int hue ) : base( 3854 )
		{
			Hue = hue;
		}

		public BaseWywarInteligencji ( Serial serial ) : base( serial )
		{
		}

		public virtual bool Apply( Mobile from )
		{
			bool applied = Spells.SpellHelper.AddStatOffset( from, Type, Bonus, TimeSpan.FromMinutes( 60.0 ) );
			

			if ( !applied )
				from.SendLocalizedMessage( 502173 ); // You are already under a similar effect.

			return applied;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( Apply( from ) )
			{
				from.FixedEffect( 0x375A, 10, 15 );
				from.PlaySound( 0x1E7 );
				Delete();
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

	public class WywarInteligencji  : BaseWywarInteligencji 
	{
		public override int Bonus{ get{ return 10; } }
		public override StatType Type{ get{ return StatType.Int; } }


		[Constructable]
		public WywarInteligencji() : base( 3854 )
		{
		    Stackable = true;
			Name = "Wywar Inteligencji";
			Hue = 1274;
		}

		public WywarInteligencji( Serial serial ) : base( serial )
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