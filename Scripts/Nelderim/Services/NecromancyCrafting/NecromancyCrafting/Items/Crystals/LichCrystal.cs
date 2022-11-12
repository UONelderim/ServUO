using System;
using Server.Mobiles;

namespace Server.Items
{
	public class LichCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill => 70.0;

		private static Type[] _requiredBodyParts = {
			typeof( WrappedMageTorso  ),
			typeof( WrappedLegs )
		};

		public override Type[] RequiredBodyParts => _requiredBodyParts;

		public override Type SummonType => typeof(Lich);

		public override string DefaultName => "kryształ licza";

		[Constructable]
		public LichCrystal()
		{
			Hue = 1266;
		}

		public LichCrystal( Serial serial ) : base( serial )
		{
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