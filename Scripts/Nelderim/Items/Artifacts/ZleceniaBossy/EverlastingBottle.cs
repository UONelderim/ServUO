using System;


namespace Server.Items
{
	public class EverlastingBottle : Item
	{
		private DateTime timer;

		public override double DefaultWeight => 1.0;


		[Constructable]
		public EverlastingBottle() : base( 3854 )
		{
			Hue = 0x849;
			Name = "Wieczna Butelka";
			timer = DateTime.UtcNow;
		}


		public override void OnDoubleClick( Mobile from )
		{
			if (DateTime.UtcNow > timer && from.Thirst < 20)
			{
				timer = DateTime.Now + TimeSpan.FromHours(4);
				from.Thirst = 20;
				from.SendMessage( "You take a healthy drink from the bottle...and it magically begins to refill." );
				from.PlaySound( Utility.RandomList( 0x30, 0x2D6 ) );
			}
			else if (from.Thirst > 20)
				from.SendMessage("You are not thirsty!");
			else
				from.SendMessage("The bottle isn't full yet.");
		}


		public EverlastingBottle( Serial serial ) : base( serial )
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
			ItemID = 3854;
			Hue = 0x849;
		}
	}
}