using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerTrialByFireScroll : CSpellScroll
	{
		[Constructable]
		public RangerTrialByFireScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerTrialByFireScroll( int amount ) : base( typeof( RangerTrialByFireSpell ), 3828, amount )
		{
			Name = "Magiczne ziola";
			Hue = 2001;
		}

		public RangerTrialByFireScroll( Serial serial ) : base( serial )
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
