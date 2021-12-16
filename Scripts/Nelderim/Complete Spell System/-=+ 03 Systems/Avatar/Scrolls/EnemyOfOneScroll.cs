using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarEnemyOfOneScroll : CSpellScroll
	{
		[Constructable]
		public AvatarEnemyOfOneScroll() : this( 1 )
		{
		}

		[Constructable]
        public AvatarEnemyOfOneScroll(int amount) : base( typeof( AvatarEnemyOfOneSpell ), 0xE39, amount )
		{
            Name = "Naznaczony";
			Hue = 1174;
		}

        public AvatarEnemyOfOneScroll(Serial serial) : base(serial)
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