using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Rogue
{
	public class RogueShieldOfEarthScroll : CSpellScroll
	{
		[Constructable]
		public RogueShieldOfEarthScroll() : this( 1 )
		{
		}

		[Constructable]
		public RogueShieldOfEarthScroll( int amount ) : base( typeof( RogueShieldOfEarthSpell ), 0xE39, amount )
		{
			Name = "Klody pod nogi";
			Hue = 0x20;
		}

		public RogueShieldOfEarthScroll( Serial serial ) : base( serial )
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
