namespace Server.Nelderim
{
	public class KompaniaHandlowa : Faction
	{
		public KompaniaHandlowa(int index) : base(index)
		{
		}
		
		public override string Name => "Kompania Handlowa";

		public override Faction[] Enemies => new Faction[] { };
	}
}
