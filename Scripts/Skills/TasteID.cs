// Updated TasteID.cs
using System;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;

namespace Server.SkillHandlers
{
	/// <summary>
	/// Handler umiejętności TasteID (Guślarstwo), pozwalającej na zdalne aktywowanie VoodooPin i VoodooDoll.
	/// </summary>
	public class TasteID
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.TasteID].Callback = OnUse;
		}

		public static TimeSpan OnUse(Mobile from)
		{
			if (from.Skills[SkillName.TasteID].Value < 50.0)
			{
				from.SendMessage("Potrzebujesz co najmniej 50.0 w Guślarstwie, aby użyć tej umiejętności.");
				return TimeSpan.FromSeconds(1.0);
			}

			from.SendMessage("Wskaż voodoo szpilkę lub laleczkę, na której chcesz użyć Guślarstwa.");
			from.Target = new TasteIDTarget();
			return TimeSpan.FromSeconds(1.0);
		}

		private class TasteIDTarget : Target
		{
			public TasteIDTarget() : base(2, false, TargetFlags.None)
			{
				AllowNonlocal = false;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is VoodooPin pin && pin.Deleted == false)
				{
					pin.OnDoubleClick(from);
				}
				else if (targeted is VoodooDoll doll && doll.Deleted == false)
				{
					doll.OnDoubleClick(from);
				}
				else
				{
					from.SendMessage("To nie jest szpilka ani laleczka.");
				}
			}

			protected override void OnTargetOutOfRange(Mobile from, object targeted)
			{
				from.SendMessage("Jesteś za daleko, aby użyć umiejętności Guślarstwa.");
			}
		}
	}
}
