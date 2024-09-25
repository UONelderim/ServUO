using System;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public class ZombieCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill => 20.0;

		private static Type[] _RequiredBodyParts = {
			typeof(RottingTorso),
			typeof(RottingLegs)
		};

		public override Type[] RequiredBodyParts => _RequiredBodyParts;

		public override Type SummonType => typeof(Zombie);

		public override string DefaultName => "kryształ zombie";

		[Constructable]
		public ZombieCrystal()
		{
			Hue = 2280;
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
