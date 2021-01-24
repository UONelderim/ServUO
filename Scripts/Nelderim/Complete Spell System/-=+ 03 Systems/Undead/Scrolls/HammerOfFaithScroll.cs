using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadHammerOfFaithScroll : CSpellScroll
	{
		[Constructable]
		public UndeadHammerOfFaithScroll() : this( 1 )
		{
		}

		[Constructable]
		public UndeadHammerOfFaithScroll( int amount ) : base( typeof( UndeadHammerOfFaithSpell ), 0xE39, amount )
		{
			Name = "Sierp Wiary Smierci";
			Hue = 38;
		}

		public UndeadHammerOfFaithScroll( Serial serial ) : base( serial )
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
