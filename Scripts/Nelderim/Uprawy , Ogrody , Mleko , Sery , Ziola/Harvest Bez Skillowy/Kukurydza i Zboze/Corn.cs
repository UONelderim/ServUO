using System;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 0xC80, 0xC7F, 0xC81, 0xC82 )]
	public class Corn : Food
	{
		[Constructable]
		public Corn() : this( 1 )
		{
		}

		[Constructable]
		public Corn( int amount ) : base( amount, 0xC80 )
		{
			this.Weight = 1.0;
			this.FillFactor = 2;
			this.Name = "kukurydza";
		}

		public Corn( Serial serial ) : base( serial )
		{
		}

		//public override Item Dupe( int amount )
		//{
		//	return base.Dupe( new Corn(), amount );
		//}

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