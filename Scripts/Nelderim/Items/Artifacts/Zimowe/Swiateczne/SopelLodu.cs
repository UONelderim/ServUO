using System;
using Server;

namespace Server.Items
{
	
	public class SopelLodu : PlateArms
	{
        
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 25; } }
		public override int BaseColdResistance { get { return 20; } }
		public override int BasePoisonResistance { get { return 5; } }	
		public override int BaseEnergyResistance { get { return -10; } }

		public int AosStrReq { get { return 65; } }
		public int OldStrReq { get { return 15; } }

		public int ArmorBase { get { return 13; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }
		public override CraftResource DefaultResource { get { return CraftResource.Iron; } }

		public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

		[Constructable]
		public SopelLodu()
		{
			Hue = 1152;
            Name = "Sopel Lodu";
            Attributes.WeaponSpeed = 10;
            Attributes.BonusStam = 10;
            Attributes.ReflectPhysical = 20;
            Attributes.WeaponDamage = 10;
			Weight = 10.0;
			SkillBonuses.SetValues( 0, SkillName.Tactics, 5.0 );
		}

		public SopelLodu( Serial serial ) : base( serial )
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
