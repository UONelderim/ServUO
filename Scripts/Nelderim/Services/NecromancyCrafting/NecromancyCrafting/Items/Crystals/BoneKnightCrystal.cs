using System;
using Server.Mobiles;

namespace Server.Items
{
	public class BoneKnightCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill
		{
			get { return 30.0; }
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
			get { return typeof(BoneKnight); }
		}
		
		public override string DefaultName
		{
			get { return "kryształ kościanego rycerza"; }
		}

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