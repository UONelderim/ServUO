using System;
using Server;
using Server.Spells;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells.Eighth;


namespace Server.Items
{
	public class StaffofSnakes : BaseMagicObject
	{
		public override int InitMinHits{ get{ return 60; } }
		public override int InitMaxHits{ get{ return 60; } }


		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ConcussionBlow; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ForceOfNature; } }



		public override int AosStrengthReq{ get{ return 20; } }
		public override int AosMinDamage{ get{ return 15; } }
		public override int AosMaxDamage{ get{ return 17; } }
		public override int AosSpeed{ get{ return 33; } }



		public int OldStrengthReq{ get{ return 20; } }
		public int OldMinDamage{ get{ return 10; } }
		public int OldMaxDamage{ get{ return 30; } }
		public int OldSpeed{ get{ return 33; } }


		[Constructable]
		public StaffofSnakes() : base( MagicObjectEffect.Charges, 80, 90 )
		{
			Hue = 0x304;
			Weight = 5.0;
			ItemID = 0x13F8;
			Name = "Kij Przywolywacza Demonow";
			AosElementDamages.Poison = 100;
			Attributes.SpellChanneling = 1;
			Attributes.WeaponDamage = 35;
			Attributes.WeaponSpeed = 20;
			Slayer = SlayerName.Fey;
			Layer = Layer.TwoHanded;
			WeaponAttributes.HitPoisonArea = 50;
			WeaponAttributes.HitLeechMana = 50;
		}


        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1049644, "Przyzywa Stworzenia" );
		}


		public StaffofSnakes( Serial serial ) : base( serial )
		{
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}


		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
	    }


		public override void OnMagicObjectUse( Mobile from )
		{
			Cast( new SummonDaemonSpell( from, this ) );
		}
	}
}
