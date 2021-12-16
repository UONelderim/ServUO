using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarCurseRemovalScroll : CSpellScroll
	{
		[Constructable]
		public AvatarCurseRemovalScroll() : this( 1 )
		{
		}

		[Constructable]
		public AvatarCurseRemovalScroll( int amount ) : base( typeof( AvatarCurseRemovalSpell ), 0xE39, amount )
		{
			Name = "Reka Mnicha";
			Hue = 1174;
		}

		public AvatarCurseRemovalScroll( Serial serial ) : base( serial )
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
