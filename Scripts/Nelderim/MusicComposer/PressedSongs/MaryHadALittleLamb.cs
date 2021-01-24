using Server.Mobiles;
namespace Server.Items
{
	public class MaryHadALittleLamb : BaseMusicComposedSong 
	{
		private const string Song ="HE1,HD1,HC1,HD1,HE1,HE1,HE1,-,HD1,HD1,HD1,-,HE1,HE1,HE1,-," +
					"HE1,HD1,HC1,HD1,HE1,HE1,HE1,-,HD1,HD1,HE1,HD1,HC1";
		private const string SongName="Mary Had A Little Lamb";
		[Constructable]
		public MaryHadALittleLamb() : base( (string)Song, (string)SongName)
		{
			this.Hue = 2117;
		}

		public MaryHadALittleLamb( Serial serial ) : base( serial ){
			this.Hue = 2117;
		}
		
		public override void Serialize( GenericWriter writer ){
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader ){
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}