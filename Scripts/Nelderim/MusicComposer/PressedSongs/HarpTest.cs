using Server.Mobiles;
namespace Server.Items
{
	public class HarpTest : BaseMusicComposedSong 
	{
		private const string Song ="HC1,HCS1,HD1,HDS1,HE1,HF1,HFS1,HG1,HGS1,HA1,HAS1,HB1"
								+ "-,-,-," +
								"HC2,HCS2,HD2,HDS2,HE2,HF2,HFS2,HG2,HGS2,HA2,HAS2,HB2,HC3";
		private const string SongName="Harp Test";
		[Constructable]
		public HarpTest() : base( (string)Song, (string)SongName)
		{
			this.Hue = 2117;
		}

		public HarpTest( Serial serial ) : base( serial ){
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