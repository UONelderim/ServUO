using System;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public class ZombieCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill
		{
			get { return 20.0; }
		}
		
		private static Type[] _RequiredBodyParts = new Type[]
		{
			typeof(RottingTorso),
			typeof(RottingLegs)
		};

		public override Type[] RequiredBodyParts
		{
			get { return _RequiredBodyParts; }
		}

		public override Type SummonType
		{
			get { return typeof(Zombie); }
		}
		
		public override string DefaultName
		{
			get { return "kryształ zombie"; }
		}

		[Constructable]
		public ZombieCrystal()
		{
			Hue = 2553;
		}

		public ZombieCrystal( Serial serial ) : base( serial )
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