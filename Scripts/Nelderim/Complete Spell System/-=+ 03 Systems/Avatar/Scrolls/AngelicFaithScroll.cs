using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarAngelicFaithScroll : CSpellScroll
	{
		[Constructable]
		public AvatarAngelicFaithScroll() : this( 1 )
		{
		}

		[Constructable]
		public AvatarAngelicFaithScroll( int amount ) : base( typeof( AvatarAngelicFaithSpell ), 0xE39, amount )
		{
			Name = "Awatar Pradawnego Mnicha";
			Hue = 1174;
		}

		public AvatarAngelicFaithScroll( Serial serial ) : base( serial )
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
