using System;
using System.Linq;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
	public class StableInfo
	{
		public static void Initialize()
		{
			CommandSystem.Register("StableInfo", AccessLevel.GameMaster, ShowStable_Command);
		}
		
		private static void ShowStable_Command (CommandEventArgs e)
		{
			if (e.Arguments.Length >= 1)
			{
				try
				{
					var serial = new Serial(Convert.ToInt32(e.Arguments[0], 16));
					ShowStable_OnTarget(e.Mobile, World.FindMobile(serial));
				}
				catch
				{
					e.Mobile.SendMessage("Invalid serial");
				}
			}
			else
			{
				e.Mobile.SendMessage("Target mobile:");
				e.Mobile.Target = new StableInfoTarget();
			}
		}

		private static void ShowStable_OnTarget(Mobile from, Mobile m)
		{
			if (m is BaseCreature)
			{
				ShowStableMaster(from, (BaseCreature)m);
			}	
			else if (m is PlayerMobile)
			{
				ListStabledCreatures(from, (PlayerMobile)m);
			}
			else
			{
				from.SendMessage("You cannot target that.");
			}
		}

		private static void ListStabledCreatures(Mobile from, PlayerMobile pm)
		{
			foreach (var mobile in pm.Stabled)
			{
				from.SendMessage($"{mobile.GetType().Name} {mobile.Serial} {mobile.Name}");
			}
		}

		private static void ShowStableMaster(Mobile from, BaseCreature bc)
		{
			var owner = World.Mobiles.Values.FirstOrDefault(m => m.Stabled.Contains(bc));
			from.SendMessage(owner != null ? $"{owner.Serial} {owner.Name}" : "Not found");
		}
		
		private class StableInfoTarget : Target
		{
			public StableInfoTarget() : base(15, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object target)
			{
				if (target is Mobile)
				{
					ShowStable_OnTarget(from, (Mobile)target);
				}
				else
				{
					from.SendMessage("You have to target a mobile");
				}
			}
		}
	}
}

