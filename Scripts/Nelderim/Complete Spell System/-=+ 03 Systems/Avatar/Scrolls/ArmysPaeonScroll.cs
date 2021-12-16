using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarArmysPaeonScroll : CSpellScroll
	{
		[Constructable]
		public AvatarArmysPaeonScroll() : this( 1 )
		{
		}

		[Constructable]
		public AvatarArmysPaeonScroll( int amount ) : base( typeof( AvatarArmysPaeonSpell ), 0xE39, amount )
		{
			Name = "Witalność Armii";
			Hue = 1174;
		}

		public AvatarArmysPaeonScroll( Serial serial ) : base( serial )
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
