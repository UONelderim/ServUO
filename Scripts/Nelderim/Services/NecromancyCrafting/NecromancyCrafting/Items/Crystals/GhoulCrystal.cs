using System;
using Server.Mobiles;

namespace Server.Items
{
	public class GhoulCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill
		{
			get { return 80.0; }
		}
		
		private static Type[] _requiredBodyParts = new Type[]
		{
			typeof(ToxicTorso),
			typeof(RottingLegs)
		};

		public override Type[] RequiredBodyParts
		{
			get { return _requiredBodyParts; }
		}

		public override Type SummonType
		{
			get { return typeof(Ghoul); }
		}
		
		public override string DefaultName
		{
			get { return "kryształ ghoula"; }
		}

		[Constructable]
		public GhoulCrystal()
		{
			Hue = 1167;
		}

		public GhoulCrystal( Serial serial ) : base( serial )
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