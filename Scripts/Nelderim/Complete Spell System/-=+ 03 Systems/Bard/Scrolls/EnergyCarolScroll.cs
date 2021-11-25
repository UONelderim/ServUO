using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardEnergyCarolScroll : CSpellScroll
	{
		[Constructable]
		public BardEnergyCarolScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardEnergyCarolScroll( int amount ) : base( typeof( BardEnergyCarolSpell ), 0x14ED, amount )
		{
			Name = "Pobudzająca Pieśń";
			Hue = 0x96;
		}

		public BardEnergyCarolScroll( Serial serial ) : base( serial )
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
