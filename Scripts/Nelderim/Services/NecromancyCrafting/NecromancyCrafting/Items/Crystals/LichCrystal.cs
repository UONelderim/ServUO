using System;
using Server.Mobiles;

namespace Server.Items
{
	public class LichCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill
		{
			get { return 70.0; }
		}
		
		private static Type[] _requiredBodyParts = new Type[]
		{
			typeof( WrappedMageTorso  ),
			typeof( WrappedLegs )
		};

		public override Type[] RequiredBodyParts
		{
			get { return _requiredBodyParts; }
		}

		public override Type SummonType
		{
			get { return typeof(Lich); }
		}
		
		public override string DefaultName
		{
			get { return "kryształ licza"; }
		}

		[Constructable]
		public LichCrystal()
		{
			Hue = 1266;
		}

		public LichCrystal( Serial serial ) : base( serial )
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