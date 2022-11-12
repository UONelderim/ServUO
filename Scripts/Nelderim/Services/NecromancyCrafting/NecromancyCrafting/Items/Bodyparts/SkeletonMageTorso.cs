namespace Server.Items
{
	public class SkeletonMageTorso : Item
	{
		public override string DefaultName
		{
			get { return "tułów szkieleta maga"; }
		}

		[Constructable]
		public SkeletonMageTorso() : base( 0x1D91 )
		{
			Weight = 1.0;
			Stackable = true;
		}

		public SkeletonMageTorso( Serial serial ) : base( serial )
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