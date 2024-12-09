namespace Server.Items
{
	public class SinbadsSword : Cutlass
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		
		[Constructable]
		public SinbadsSword()
		{
			Hue = 343;
			Name = "Miecz Oceanow Nelderim";
			Attributes.BonusDex = 10;
			SkillBonuses.SetValues( 0, SkillName.Cartography, 5 );
			SkillBonuses.SetValues( 1, SkillName.Fishing, 5 );
			SkillBonuses.SetValues( 2, SkillName.Lockpicking, 5 );
			Attributes.AttackChance = 10;
		}

		public SinbadsSword( Serial serial ) : base( serial )
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
	}
}
