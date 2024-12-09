namespace Server.Items
{
	public class ArcaneArms : LeatherArms
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		
		[Constructable]
		public ArcaneArms()
		{
			Name = "Naramienniki Arkanisty";
			Hue = 1366;
			Attributes.DefendChance = 10; 
			Attributes.LowerManaCost = 10;
			Attributes.SpellDamage = 10;
		}

		public ArcaneArms( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

		}
	}
}
