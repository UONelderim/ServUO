using Nelderim;

namespace Server.Items
{
	public partial class BankBox
	{
		public override void UpdateTotal(Item sender, TotalType type, int delta)
		{
			base.UpdateTotal(sender, type, delta);
			if (type == TotalType.Gold && delta != 0)
			{
				BankLog.Log(Owner, delta, "gold");
			}
		}
	}
}
