namespace Server.Items
{
	public class Bones : Item
	{
		public override string DefaultName
		{
			get { return "kości"; }
		}

		[Constructable]
		public Bones() : base( 0x1B1A )
		{
			Weight = 1.0;
		}

		public Bones( Serial serial ) : base( serial )
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