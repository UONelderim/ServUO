namespace Server.Nelderim
{
	public class East : Faction
	{
		public East(int index) : base(index)
		{
		}
		
		public override string Name => "Wschod";

		public override Faction[] Enemies => new[] { West };
	}
}
