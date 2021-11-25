using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericHammerOfFaithScroll : CSpellScroll
	{
		[Constructable]
		public ClericHammerOfFaithScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericHammerOfFaithScroll( int amount ) : base( typeof( ClericHammerOfFaithSpell ), 0xE39, amount )
		{
			Name = "Topór Wiary";
			Hue = 0x1F0;
		}

		public ClericHammerOfFaithScroll( Serial serial ) : base( serial )
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
