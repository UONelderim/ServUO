
namespace Server.Items
{
	public class kolczanwstyluzachodnim : BaseQuiver
	{
		public override int DefaultMaxWeight => 100;
		public override bool CanFortify => false;

		public kolczanwstyluzachodnim() : base(0x2B02)
		{
			WeightReduction = 50;
			LowerAmmoCost = 20;
			Capacity = 1000;
			Name = "kołczan w stylu zachodnim";
			Hue = 2697;
			
		}

		public kolczanwstyluzachodnim( Serial serial ) : base( serial )
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
