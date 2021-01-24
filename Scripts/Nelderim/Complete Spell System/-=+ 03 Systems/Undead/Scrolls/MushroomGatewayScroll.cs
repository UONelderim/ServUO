using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadMushroomGatewayScroll : CSpellScroll
	{
		[Constructable]
		public UndeadMushroomGatewayScroll() : this( 1 )
		{
		}

		[Constructable]
		public UndeadMushroomGatewayScroll( int amount ) : base( typeof( UndeadMushroomGatewaySpell ), 0xE39, amount )
		{
			Name = "Limbo";
			Hue = 38;
		}

		public UndeadMushroomGatewayScroll( Serial serial ) : base( serial )
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
