using Server.Commands;
using Server.SkillHandlers;
using Server.Targeting;

namespace Server.Nelderim.Commands
{
	public class ImbueWeight
	{
		public static void Initialize()
		{
			CommandSystem.Register("ImbueWeight", AccessLevel.GameMaster, OnCommand);
		}

		public static void OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendMessage("Wskaz przedmiot");
			e.Mobile.Target = new InternalTarget();
		}

		private class InternalTarget : Target
		{
			public InternalTarget() : base(15, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object obj)
			{
				if (obj is not Item item)
				{
					from.SendMessage("Musisz wybrac przedmiot.");
					return;
				}
				from.SendMessage(Imbuing.GetTotalWeight(item, -1, true, false).ToString());
			}
		}
	}
}
