using System.Linq;
using Server.Items;

namespace Server.Commands
{
	public class FixShieldResistances
	{
		
		public static void Initialize()
		{
			CommandSystem.Register("FixShieldResistances", AccessLevel.Administrator, OnCommand);
		}

		private static void OnCommand(CommandEventArgs e)
		{
			var fixedn = 0;
			var items = World.Items.Values.OfType<BaseShield>();
			foreach (var shield in items.ToArray())
			{
				if(shield.Resource != shield.DefaultResource && shield.PhysicalBonus == 0 && shield.FireBonus == 0 && shield.ColdBonus == 0 && shield.PoisonBonus == 0 && shield.EnergyBonus == 0)
				{
					var oldRes = shield.Resource;
					shield.Resource = CraftResource.None;
					shield.Resource = oldRes;
					fixedn++;
					e.Mobile.SendMessage(shield.Serial.ToString());
				}

			}
			e.Mobile.SendMessage($"Fixed {fixedn} shields");
		}
	}
}
