using System;
using Server.ACC.CSS;

namespace Server.Items
{
	public abstract class DeathKnightSkull : CSpellScroll
	{
		protected DeathKnightSkull(Type type) : base( type, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Czaszka Rycerza Smierci";
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull( Serial serial ) : base( serial )
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
}
