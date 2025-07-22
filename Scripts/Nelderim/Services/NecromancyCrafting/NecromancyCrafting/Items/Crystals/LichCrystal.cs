using System;
using Server.Mobiles;

namespace Server.Items
{
	public class LichCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill => 70.0;

		private static Type[] _requiredBodyParts = {
			typeof( WrappedMageTorso  ),
			typeof( WrappedLegs )
		};

		public override Type[] RequiredBodyParts => _requiredBodyParts;

		public override Type SummonType => typeof(Lich);
		
		public interface INecroPet { }

		public override string DefaultName => "kryształ licza";

		[Constructable]
		public LichCrystal()
		{
			Hue = 1266;
		}

		public LichCrystal( Serial serial ) : base( serial )
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
