using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadLureStoneScroll : CSpellScroll
	{
		[Constructable]
		public UndeadLureStoneScroll() : this( 1 )
		{
		}

		[Constructable]
		public UndeadLureStoneScroll( int amount ) : base( typeof( UndeadLureStoneSpell ), 0xE39, amount )
		{
			Name = "Gnijące Zwłoki";
			Hue = 38;
		}

		public UndeadLureStoneScroll( Serial serial ) : base( serial )
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
