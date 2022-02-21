namespace Server.Mobiles
{
	public interface UltimaLiveQuery
	{
		int QueryMobile(Mobile m, int previousBlock);
	}

	public partial class PlayerMobile
	{
		public static UltimaLiveQuery BlockQuery;
		private int m_PreviousMapBlock = -1;

		[CommandProperty(AccessLevel.GameMaster, true)]
		public int UltimaLiveMajorVersion { get; set; } = 0;

		[CommandProperty(AccessLevel.GameMaster, true)]
		public int UltimaLiveMinorVersion { get; set; } = 0;
	}
}
