using System;
using Server.Mobiles;

namespace Server.Items
{
	public class MummyCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill
		{
			get { return 50.0; }
		}
		
		private static Type[] _requiredBodyParts = new Type[]
		{
			typeof( WrappedTorso ),
			typeof( WrappedLegs )
		};

		public override Type[] RequiredBodyParts
		{
			get { return _requiredBodyParts; }
		}

		public override Type SummonType
		{
			get { return typeof(Mummy); }
		}
		
		public override string DefaultName
		{
			get { return "kryształ mumii"; }
		}

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