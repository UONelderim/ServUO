using System;
using Server.Mobiles;

namespace Server.Items
{
	public class BoneKnightCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill => 30.0;

		private static Type[] _requiredBodyParts = {
			typeof( SkeletonTorso ),
			typeof( SkeletonLegs )	
		};

		public override Type[] RequiredBodyParts => _requiredBodyParts;

		public override Type SummonType => typeof(BoneKnight);

		public override string DefaultName => "kryształ kościanego rycerza";

		[Constructable]
		public BoneKnightCrystal()
		{
			Hue = 2874;
		}

		public BoneKnightCrystal( Serial serial ) : base( serial )
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
