using Server.Mobiles;
namespace Server.Items
{
	public class LuteTest : BaseMusicComposedSong 
	{
		private const string Song ="LC1,LCS1,LD1,LDS1,LE1,LF1,LFS1,LG1,LGS1,LA1,LAS1,LB1," +
								   "-,-,-," +
								   "LC2,LCS2,LD2,LDS2,LE2,LF2,LFS2,LG2,LGS2,LA2,LAS2,LB2,LC3";
		private const string SongName="Lute Test";
		[Constructable]
		public LuteTest() : base( (string)Song, (string)SongName)
		{
			this.Hue = 2117;
		}

		public LuteTest( Serial serial ) : base( serial ){
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