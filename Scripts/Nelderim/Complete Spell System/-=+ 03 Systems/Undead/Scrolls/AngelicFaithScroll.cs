using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadAngelicFaithScroll : CSpellScroll
	{
		[Constructable]
		public UndeadAngelicFaithScroll() : this( 1 )
		{
		}

		[Constructable]
		public UndeadAngelicFaithScroll( int amount ) : base( typeof( UndeadAngelicFaithSpell ), 0xE39, amount )
		{
			Name = "Demoniczny Awatar";
			Hue = 38;
		}

		public UndeadAngelicFaithScroll( Serial serial ) : base( serial )
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
