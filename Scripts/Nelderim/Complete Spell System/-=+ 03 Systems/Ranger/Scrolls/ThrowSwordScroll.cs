using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerThrowSwordScroll : CSpellScroll
	{
		[Constructable]
		public RangerThrowSwordScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerThrowSwordScroll( int amount ) : base( typeof( RangerThrowSwordSpell ), 3828, amount )
		{
			Name = "Celność łowcy";
			Hue = 2001;
		}

		public RangerThrowSwordScroll( Serial serial ) : base( serial )
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
