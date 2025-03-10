namespace Server.Items
{
	public class PeerlessExitMoongate : Moongate
	{
		private PeerlessAltar _Altar;
		
		public PeerlessExitMoongate(PeerlessAltar altar)
			: base(altar.ExitDest, altar.Map)
		{
			_Altar = altar;
			Name = "Portal do wyjścia";
			Hue = altar.ExitMoongateHue;
			Dispellable = false;
			ItemID = 0x1FD4;
			MoveToWorld(altar.TeleportDest, altar.Map);
		}
		
		public override void OnGateUsed(Mobile m)
		{
			base.OnGateUsed(m);
			_Altar.Exit(m);
		}
	}
}
