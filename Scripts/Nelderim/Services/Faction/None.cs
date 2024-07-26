namespace Server.Nelderim
{
	public class None : Faction
	{
		public None(int index) : base(index)
		{
		}
		
		public override string Name => "";

		public override Faction[] Enemies => new Faction[] { };
	}
}
