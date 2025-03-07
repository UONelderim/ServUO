using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;

namespace Server.Nelderim
{
	public class NelderimRegionCommands
	{
		public static void Initialize()
		{
			TargetCommands.Register(new NRSMakeMobile());
		}
	}

	public class NRSMakeMobile : BaseCommand
	{
		public NRSMakeMobile()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllMobiles;
			Commands = ["NRSMakeMobile"];
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "NRSMakeMobile";
			Description = "Reinicjalizuje moba na podstawie regionu";
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			Mobile from = e.Mobile;
			Mobile m = (Mobile)obj;

			if (m.Region == null)
			{
				from.SendMessage("Wskazany moba nie jest w żadnym regionie");
				return;
			}

			var nRegion = NelderimRegionSystem.GetRegion(m.Region);

			if (m is BaseNelderimGuard guard)
			{
				nRegion.MakeGuard(guard);
			}
			else
			{
				nRegion.MakeMobile(m);
			}
			from.SendMessage("Reinicjalizacja zakonczona");
		}
	}
}
