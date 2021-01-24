using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadNaturesPassageScroll : CSpellScroll
	{
		[Constructable]
		public UndeadNaturesPassageScroll() : this( 1 )
		{
		}

		[Constructable]
		public UndeadNaturesPassageScroll( int amount ) : base( typeof( UndeadNaturesPassageSpell ), 0xE39, amount )
		{
			Name = "Ścieżka Śmierci";
			Hue = 38;
		}

		public UndeadNaturesPassageScroll( Serial serial ) : base( serial )
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
