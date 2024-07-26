namespace Server.Nelderim
{
	public class VoxPopuli : Faction
	{
		public VoxPopuli(int index) : base(index)
		{
		}

		public override string Name => "Vox Populi";

		public override Faction[] Enemies => new Faction[] { };
	}
}
