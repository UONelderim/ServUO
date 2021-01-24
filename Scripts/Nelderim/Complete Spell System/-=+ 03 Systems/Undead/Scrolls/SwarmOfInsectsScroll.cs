using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadSwarmOfInsectsScroll : CSpellScroll
	{
		[Constructable]
		public UndeadSwarmOfInsectsScroll() : this( 1 )
		{
		}

		[Constructable]
		public UndeadSwarmOfInsectsScroll( int amount ) : base( typeof( UndeadSwarmOfInsectsSpell ), 0xE39 )
		{
			Name = "Chmara Insektów";
			Hue = 38;
		}

		public UndeadSwarmOfInsectsScroll( Serial serial ) : base( serial )
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
