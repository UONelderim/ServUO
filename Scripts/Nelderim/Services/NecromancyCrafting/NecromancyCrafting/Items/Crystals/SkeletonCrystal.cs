using System;
using Server.Mobiles;

namespace Server.Items
{
	public class SkeletonCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill
		{
			get { return 20.0; }
		}
		
		private static Type[] _requiredBodyParts = new Type[]
		{
			typeof( SkeletonTorso ),
			typeof( SkeletonLegs )
		};

		public override Type[] RequiredBodyParts
		{
			get { return _requiredBodyParts; }
		}

		public override Type SummonType
		{
			get { return typeof(Skeleton); }
		}
		
		public override string DefaultName
		{
			get { return "kryształ szkieleta"; }
		}

		[Constructable]
		public SkeletonCrystal()
		{
			Hue = 1965;
		}

		public SkeletonCrystal( Serial serial ) : base( serial )
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