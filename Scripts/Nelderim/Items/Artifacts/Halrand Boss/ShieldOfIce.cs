using System;
using Server;

namespace Server.Items
{
	public class ShieldOfIce : BaseShield
	{
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 15; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }
		

		[Constructable]
		public ShieldOfIce() : base( 0x1B7B )
		{
			Weight = 6.0;
			Name = "Tarcza z Lodowcow Geriadoru";
			Hue = 1151;
			
			Attributes.CastSpeed = 1;
			Attributes.DefendChance = 15;
			ArmorAttributes.SoulCharge = 20;
			
		}

		public ShieldOfIce( Serial serial ) : base(serial)
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );//version
		}
	}
}
