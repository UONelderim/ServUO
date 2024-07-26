namespace Server.Nelderim
{
	public class East : Faction
	{
		public East(int index) : base(index)
		{
		}
		
		public override string Name => "Frakcja2";

		public override Faction[] Enemies => new[] { West };
	}
}
