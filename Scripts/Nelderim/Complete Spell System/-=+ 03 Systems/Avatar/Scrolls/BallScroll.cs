using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarBallScroll : CSpellScroll
	{
		[Constructable]
		public AvatarBallScroll() : this( 1 )
		{
		}

		[Constructable]
		public AvatarBallScroll( int amount ) : base( typeof( AvatarBallSpell ), 0xE39, amount )
		{
			Name = "Kula Sniegu";
			Hue = 1174;
		}

		public AvatarBallScroll( Serial serial ) : base( serial )
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
