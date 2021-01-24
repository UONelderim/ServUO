using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadGraspingRootsScroll : CSpellScroll
	{
		[Constructable]
		public UndeadGraspingRootsScroll() : this( 1 )
		{
		}

		[Constructable]
		public UndeadGraspingRootsScroll( int amount ) : base( typeof( UndeadGraspingRootsSpell ), 0xE39, amount )
		{
			Name = "Uchwyt Zza Grobu";
			Hue = 38;
		}

		public UndeadGraspingRootsScroll( Serial serial ) : base( serial )
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
