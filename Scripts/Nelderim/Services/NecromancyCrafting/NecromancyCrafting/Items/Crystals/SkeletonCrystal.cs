using System;
using Server.Mobiles;

namespace Server.Items
{
	public class SkeletonCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill => 20.0;

		private static Type[] _requiredBodyParts = {
			typeof( SkeletonTorso ),
			typeof( SkeletonLegs )
		};

		public override Type[] RequiredBodyParts => _requiredBodyParts;

		public interface INecroPet { }
		public override Type SummonType => typeof(Skeleton);

		public override string DefaultName => "kryształ szkieleta";

		[Constructable]
		public SkeletonCrystal()
		{
			Hue = 1293;
		}

		public SkeletonCrystal( Serial serial ) : base( serial )
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
