using Server.Nelderim;

namespace Server
{
	public partial class Mobile
	{
		[CommandProperty(AccessLevel.Decorator, AccessLevel.Administrator)]
		public Faction Faction
		{
			get => Faction.Get(this).Faction;
			set
			{
				var oldFaction = Faction;

				var newFaction = value ?? Faction.Default;

				Faction.Get(this).Faction = newFaction;

				Delta(MobileDelta.Race);

				OnFactionChange(oldFaction);
			}
		}

		private void OnFactionChange(Faction oldFaction)
		{
		}
	}
}
