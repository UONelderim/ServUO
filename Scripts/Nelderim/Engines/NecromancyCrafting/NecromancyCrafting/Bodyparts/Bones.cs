using System;
using Server.Network;

namespace Server.Items
{
	public class Bones : Item
	{
		public override string DefaultName
		{
			get { return "bones"; }
		}

		[Constructable]
		public Bones() : base( 0x1B1A )
		{
			Weight = 1.0;
		}

		public Bones( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( this.GetWorldLocation(), 3 ))
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			else
				from.SendAsciiMessage( "The bones of a skeleton." );
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