namespace Nelderim.Factions
{
	public class West : Faction
	{
		public West(int index) : base(index)
		{
		}
		
		public override string Name => "Frakcja2";

		public override Faction[] Enemies => new[] { East };
	}
}
