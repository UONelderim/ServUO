namespace Server.Mobiles
{
	public partial class BaseCreature
	{
		private bool _CollectedByHunter;

		public bool CollectedByHunter
		{
			get => _CollectedByHunter;
			set => _CollectedByHunter = value;
		}
	}
}
