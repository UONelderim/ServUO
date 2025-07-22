using System;
using Server.Mobiles;

namespace Server.Items
{
	public class AncientLichCrystal : BaseNecroCraftCrystal
	{
		public override double RequiredNecroSkill => 100.0;

		private static Type[] _requiredBodyParts = {
			typeof( WrappedMageTorso ),
			typeof( WrappedLegs ),
			typeof( Phylacery ),
			typeof( Brain ),
		};

		public override Type[] RequiredBodyParts => _requiredBodyParts;
		
		public interface INecroPet { }

		public override Type SummonType => typeof(AncientLich);

		public override string DefaultName => "kryształ starożytnego licza";

		[Constructable]
		public AncientLichCrystal()
		{
			Hue = 2903;
		}

		public AncientLichCrystal( Serial serial ) : base( serial )
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
