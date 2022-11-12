namespace Server.Items
{
	public class Jawbone : Item
	{
		public override string DefaultName
		{
			get { return "szczęka"; }
		}

		[Constructable]
		public Jawbone() : base( 0x1B13 )
		{
			Weight = 1.0;
			Stackable = true;
		}

		public Jawbone( Serial serial ) : base( serial )
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