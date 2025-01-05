using Server.Spells.Eighth;


namespace Server.Items
{
	public class StaffofSnakes : GnarledStaff
	{
		public override int InitMinHits => 60;
		public override int InitMaxHits => 60;


		[Constructable]
		public StaffofSnakes()
		{
			Weight = 5.0;
			Hue = 0x304;
			Name = "Kij Przywolywacza Demonow";
			
			AosElementDamages.Poison = 100;
			Attributes.SpellChanneling = 1;
			Attributes.WeaponDamage = 35;
			Attributes.WeaponSpeed = 20;
			Slayer = SlayerName.Fey;
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

		//
		// public override void OnMagicObjectUse( Mobile from )
		// {
		// 	Cast( new SummonDaemonSpell( from, this ) );
		// }
	}
}
