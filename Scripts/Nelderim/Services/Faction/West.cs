namespace Server.Nelderim
{
	public class West : Faction
	{
		public West(int index) : base(index)
		{
		}
		
		public override string Name => "Zachod";

		public override Faction[] Enemies => new[] { East };
	}
}
