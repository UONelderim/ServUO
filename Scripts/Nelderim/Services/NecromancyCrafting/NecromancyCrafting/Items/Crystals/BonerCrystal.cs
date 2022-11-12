using System;
using Server.Mobiles;

namespace Server.Items
{
	public class BonerCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill
		{
			get { return 120.0; }
		}

		private static Type[] _requiredBodyParts = new Type[]
		{
			typeof( SkeletonTorso ),
			typeof( SkeletonLegs ),
			typeof( Phylacery),			
			typeof( Brain ),
		};

		public override Type[] RequiredBodyParts
		{
			get { return _requiredBodyParts; }
		}

		public override Type SummonType
		{
			get { return typeof(Boner); }
		}
		
		public override string DefaultName
		{
			get { return "kryształ kościeja"; }
		}

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