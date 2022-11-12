namespace Server.Items
{
	public class RottingTorso : Item
	{
		public override string DefaultName
		{
			get { return "gnijący tułów"; }
		}

		[Constructable]
		public RottingTorso() : base( 0x1CDE )
		{
			Weight = 1.0;
			Stackable = true;
		}

		public RottingTorso( Serial serial ) : base( serial )
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