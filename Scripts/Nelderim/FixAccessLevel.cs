#region References

using Server.Accounting;
using Server.Misc;

#endregion

namespace Server.Commands
{
	public class FixAccessLevel
	{
		public static void Initialize()
		{
			AccountHandler.LockdownLevel = AccessLevel.Counselor;
		}
	}
}
