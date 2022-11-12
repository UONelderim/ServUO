using System;
using Server.Mobiles;

namespace Server.Items
{
	public class BoneMagiCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill => 40.0;

		private static Type[] _requiredBodyParts = {
			typeof( SkeletonMageTorso ),
			typeof( SkeletonLegs )
		};

		public override Type[] RequiredBodyParts => _requiredBodyParts;

		public override Type SummonType => typeof(BoneMagi);

		public override string DefaultName => "kryształ kościanego maga";

		[Constructable]
		public BoneMagiCrystal()
		{
			Hue = 1172;
		}

		public BoneMagiCrystal( Serial serial ) : base( serial )
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