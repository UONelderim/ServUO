namespace Nelderim.Factions
{
	public class West : Faction
	{
		public West(int index) : base(index)
		{
		}
		
		public override string Name => "Frakcja1";

		public override Faction[] Enemies => new[] { East };
	}
}
