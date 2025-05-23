using Server;
using Server.Commands;
using Server.Network;
using Server.Targeting;

namespace Nelderim
{
	public class NamesCommands
	{
		public static void Initialize()
		{
			CommandSystem.Register("nazwij", AccessLevel.Player, NazwijCommand);
		}
		
		
		[Usage("nazwij <imie>")]
		[Description("Przyznaje dla postaci wybrane imie")]
		public static void NazwijCommand(CommandEventArgs e)
		{
			string newName = e.ArgString.Trim();

			if (e.Length > 0)
				e.Mobile.Target = new NazwijTarget(newName);
			else
				e.Mobile.SendMessage("Format: nazwij {Imie}");
		}

		public class NazwijTarget : Target
		{
			private readonly string _newName;

			public NazwijTarget(string newName) : base(-1, false, TargetFlags.None)
			{
				_newName = newName;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (from != targeted && targeted is Mobile m )
				{
					m.NameFor[from] = _newName;
					from.SendMessage("Nazwales ta postac " + _newName);
				}
				else 
				{
					from.SendMessage("Nie mozesz tego nazwac");
				}
			}
		}
	}
}
