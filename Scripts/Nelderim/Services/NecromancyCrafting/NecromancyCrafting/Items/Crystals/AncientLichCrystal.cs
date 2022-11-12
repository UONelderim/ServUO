using System;
using Server.Mobiles;

namespace Server.Items
{
	public class AncientLichCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill
		{
			get { return 100.0; }
		}
		
		private static Type[] _requiredBodyParts = new Type[]
		{
			typeof( WrappedMageTorso ),
			typeof( WrappedLegs ),
			typeof( Phylacery ),
			typeof( Brain ),
		};

		public override Type[] RequiredBodyParts
		{
			get { return _requiredBodyParts; }
		}

		public override Type SummonType
		{
			get { return typeof(AncientLich); }
		}
		
		public override string DefaultName
		{
			get { return "kryształ starożytnego licza"; }
		}

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