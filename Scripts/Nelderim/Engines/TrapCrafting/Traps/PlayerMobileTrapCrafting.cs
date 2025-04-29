using Server.Items;

namespace Server.Mobiles
{
	public partial class PlayerMobile
	{
		// Traps
		[CommandProperty(AccessLevel.GameMaster)]
		public int TrapsActive => BaseTinkerTrap.ActiveTraps(this);
	}
}
