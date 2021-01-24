using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadHollowReedScroll : CSpellScroll
	{
		[Constructable]
		public UndeadHollowReedScroll() : this( 1 )
		{
		}

		[Constructable]
		public UndeadHollowReedScroll( int amount ) : base( typeof( UndeadHollowReedSpell ), 0xE39, amount )
		{
			Name = "Hedonizm";
			Hue = 38;
		}

		public UndeadHollowReedScroll( Serial serial ) : base( serial )
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
