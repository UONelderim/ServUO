namespace Server.Items
{
	public class Skull : Item
	{
		public override string DefaultName
		{
			get { return "czaszka"; }
		}

		[Constructable]
		public Skull() : base( 0x1AE4 )
		{
			Weight = 1.0;
			Stackable = true;
		}

		public Skull( Serial serial ) : base( serial )
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