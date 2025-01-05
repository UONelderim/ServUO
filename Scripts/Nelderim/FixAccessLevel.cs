#region References

using Server.Accounting;

#endregion

namespace Server.Commands
{
	public class FixAccessLevel
	{
		public static void Initialize()
		{
			IAccount iacc = Accounts.GetAccount("owner");
			// iacc.SetPassword("1234");
		}
	}
}
