namespace Server.Items
{
	public class ToxicTorso : Item
	{
		public override string DefaultName
		{
			get { return "toksyczny tułów"; }
		}

		[Constructable]
		public ToxicTorso() : base( 0x1CDE )
		{
			Weight = 1.0;
			Stackable = true;
		}

		public ToxicTorso( Serial serial ) : base( serial )
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