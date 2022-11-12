using System;
using Server.Mobiles;

namespace Server.Items
{
	public class MummyCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill => 50.0;

		private static Type[] _requiredBodyParts = {
			typeof( WrappedTorso ),
			typeof( WrappedLegs )
		};

		public override Type[] RequiredBodyParts => _requiredBodyParts;

		public override Type SummonType => typeof(Mummy);

		public override string DefaultName => "kryształ mumii";

		[Constructable]
		public MummyCrystal()
		{
			Hue = 1161;
		}

		public MummyCrystal( Serial serial ) : base( serial )
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