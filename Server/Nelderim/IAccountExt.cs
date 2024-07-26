using Server.Nelderim;

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
