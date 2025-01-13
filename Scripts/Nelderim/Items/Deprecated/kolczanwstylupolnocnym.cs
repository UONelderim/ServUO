
namespace Server.Items
{
	public class kolczanwstylupolnocnym : BaseQuiver
	{
		public override int DefaultMaxWeight => 80;
		public override bool CanFortify => false;

		public kolczanwstylupolnocnym() : base(0x2B02)
		{
			WeightReduction = 40;
			LowerAmmoCost = 15;
			Capacity = 800;
			Name = "kołczan w stylu północnym";
			Hue = 1150;
			
		}

		public kolczanwstylupolnocnym( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
