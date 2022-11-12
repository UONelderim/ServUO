using System;
using Server.Mobiles;

namespace Server.Items
{
	public class BoneMagiCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill
		{
			get { return 40.0; }
		}
		
		private static Type[] _requiredBodyParts = new Type[]
		{
			typeof( SkeletonMageTorso ),
			typeof( SkeletonLegs )
		};

		public override Type[] RequiredBodyParts
		{
			get { return _requiredBodyParts; }
		}

		public override Type SummonType
		{
			get { return typeof(BoneMagi); }
		}
		
		public override string DefaultName
		{
			get { return "kryształ kościanego maga"; }
		}

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