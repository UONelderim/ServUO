using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarRestorationScroll : CSpellScroll
	{
		[Constructable]
		public AvatarRestorationScroll() : this( 1 )
		{
		}

		[Constructable]
		public AvatarRestorationScroll( int amount ) : base( typeof( AvatarRestorationSpell ), 0xE39, amount )
		{
			Name = "Odrodzenie";
			Hue = 1174;
		}

		public AvatarRestorationScroll( Serial serial ) : base( serial )
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
