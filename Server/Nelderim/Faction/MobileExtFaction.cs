using Server.Nelderim;

namespace Server
{
	public partial class Mobile
	{
		[CommandProperty(AccessLevel.Decorator, AccessLevel.Administrator)]
		public Faction Faction
		{
			get => FactionExt.Get(this).Faction;
			set
			{
				var oldFaction = Faction;

				var newFaction = value ?? Faction.Default;

				FactionExt.Get(this).Faction = newFaction;

				Delta(MobileDelta.Race);

				OnFactionChange(oldFaction);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public Faction AccountFaction //I don't know how to access it directly through account.faction ingame
		{
			get => Account?.Faction;
			set => Account.Faction = value;
		}

		private void OnFactionChange(Faction oldFaction)
		{
		}
	}
}
