using System;
using Server.Network;

namespace Server.Items
{
	public class SkelMageBod : Item
	{
		public override string DefaultName
		{
			get { return "Tułów szkieleta maga"; }
		}

		[Constructable]
		public SkelMageBod() : base( 0x1D91 )
		{
			Weight = 1.0;
		}

		public SkelMageBod( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( this.GetWorldLocation(), 3 ))
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			else
				from.SendAsciiMessage( "The skeletal reamains of some one." );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}