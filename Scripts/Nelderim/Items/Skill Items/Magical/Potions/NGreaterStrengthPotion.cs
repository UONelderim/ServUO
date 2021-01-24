using System;
using Server;

namespace Server.Items
{
	public class NGreaterStrengthPotion : BaseStrengthPotion
	{
		public override int StrOffset{ get{ return 20; } }
        // 06.07.2012 :: zombie :: czas dzialania pota wynosi 5 min (wczesniej 2)
		public override TimeSpan Duration{ get{ return TimeSpan.FromMinutes( 20.0 ); } }
        // zombie
		public override int LabelNumber{ get{ return 1071087; } }

		[Constructable]
		public NGreaterStrengthPotion() : base( PotionEffect.NStrengthGreater )
		{
			Name = "potężna mikstura siły";
			Weight = 0.5;
		}

		public NGreaterStrengthPotion( Serial serial ) : base( serial )
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