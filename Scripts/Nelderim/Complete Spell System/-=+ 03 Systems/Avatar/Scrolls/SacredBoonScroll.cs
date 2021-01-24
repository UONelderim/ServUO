using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarSacredBoonScroll : CSpellScroll
	{
		[Constructable]
		public AvatarSacredBoonScroll() : this( 1 )
		{
		}

		[Constructable]
		public AvatarSacredBoonScroll( int amount ) : base( typeof( AvatarSacredBoonSpell ), 0xE39, amount )
		{
			Name = "Święty znak";
			Hue = 1174;
		}

		public AvatarSacredBoonScroll( Serial serial ) : base( serial )
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
