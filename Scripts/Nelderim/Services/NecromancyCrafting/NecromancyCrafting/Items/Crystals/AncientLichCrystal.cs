using System;
using Server.Mobiles;

namespace Server.Items
{
	public class AncientLichCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill => 100.0;

		private static Type[] _requiredBodyParts = {
			typeof( WrappedMageTorso ),
			typeof( WrappedLegs ),
			typeof( Phylacery ),
			typeof( Brain ),
		};

		public override Type[] RequiredBodyParts => _requiredBodyParts;

		public override Type SummonType => typeof(AncientLich);

		public override string DefaultName => "kryształ starożytnego licza";

		[Constructable]
		public AncientLichCrystal()
		{
			Hue = 2903;
		}

		public AncientLichCrystal( Serial serial ) : base( serial )
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