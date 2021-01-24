using Server.Mobiles;
namespace Server.Items
{
	public class StandHarpTest : BaseMusicComposedSong 
	{
		private const string Song ="MC1,MCS1,MD1,MDS1,ME1,MF1,MFS1,MG1,MGS1,MA1,MAS1,MB1"
								+ "-,-,-," +
								"MC2,MCS2,MD2,MDS2,ME2,MF2,MFS2,MG2,MGS2,MA2,MAS2,MB2,MC3";
		private const string SongName="Harp Test";
		[Constructable]
		public StandHarpTest() : base( (string)Song, (string)SongName)
		{
			this.Hue = 2117;
		}

		public StandHarpTest( Serial serial ) : base( serial ){
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