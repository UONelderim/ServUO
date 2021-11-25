using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardFoeRequiemScroll : CSpellScroll
	{
		[Constructable]
		public BardFoeRequiemScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardFoeRequiemScroll( int amount ) : base( typeof( BardFoeRequiemSpell ), 0x14ED, amount )
		{
			Name = "Soniczny Podmuch";
			Hue = 0x96;
		}

		public BardFoeRequiemScroll( Serial serial ) : base( serial )
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
