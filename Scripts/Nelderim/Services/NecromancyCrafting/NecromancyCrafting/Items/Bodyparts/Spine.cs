namespace Server.Items
{
	public class Spine : Item
	{
		public override string DefaultName
		{
			get { return "kręgosłup"; }
		}

		[Constructable]
		public Spine() : base( 0x1B1B )
		{
			Weight = 1.0;
			Stackable = true;
		}

		public Spine( Serial serial ) : base( serial )
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