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
		
		public interface INecroPet { }

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
		
		protected void FinishSummon(BaseCreature bc, Mobile from)
		{
			bc.Allured = true;
			bc.ControlMaster = from;

			// Mark it as a necro-trainable pet
			if (bc is INecroPet == false)
				bc.GetType().GetInterfaces(); // ensure interface is applied by your creature class

			ScaleCreature(bc, from.Skills[SkillName.Necromancy].Value);
			bc.MoveToWorld(from.Location, from.Map);
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
