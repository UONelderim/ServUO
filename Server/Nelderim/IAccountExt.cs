using Nelderim.Factions;

namespace Server.Accounting
{
	public interface IFactionAccount
	{
		public Faction Faction { get; set; } 
	}
	
	public partial interface IAccount : IFactionAccount
	{
		
	}
}
