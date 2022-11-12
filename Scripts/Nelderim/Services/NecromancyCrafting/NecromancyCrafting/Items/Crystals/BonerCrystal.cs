using System;
using Server.Mobiles;

namespace Server.Items
{
	public class BonerCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill => 120.0;

		private static Type[] _requiredBodyParts = {
			typeof( SkeletonTorso ),
			typeof( SkeletonLegs ),
			typeof( Phylacery),			
			typeof( Brain ),
		};

		public override Type[] RequiredBodyParts => _requiredBodyParts;

		public override Type SummonType => typeof(Boner);

		public override string DefaultName => "kryształ kościeja";

		[Constructable]
		public BonerCrystal()
		{
			Hue = 38;
		}

		public BonerCrystal( Serial serial ) : base( serial )
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