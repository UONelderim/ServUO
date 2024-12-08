#region References

using System.Collections.Generic;

#endregion


namespace Server.Mobiles
{
	public class Biedak : NBaseTalkingNPC
	{
		private static readonly Dictionary<Race, List<Action>> _Actions = new Dictionary<Race, List<Action>>
		{
			{
				Race.DefaultRace,
				new List<Action>
				{
					m => m.Say("Daj Panie choć złamanego grosza..."),
					m => m.Say("...i tak to właśnie pozbyłem się mego dobytku..."),
					m => m.Say("Psia krew...znów te szczury wygryzły mi koszulę..."),
					m => m.Say("A z chęcią bym się napił teraz gorzałki..."),
					m => m.Say("A mamusia mówiła... nie handluj z Krasnoludami..."),
					m => m.Say("Eh.. i po co mi żyć w tej biedocie.."),
					m => m.Say("Ja chromolę... gdzie moje pieniądze?! ...a... nie mam ich..."),
					m => m.Say("Ta straż ciągle tu tylko węszy."),
					m => m.Say("A niech to... znów zabraknie mi nia gorzałkę...")
				}
			}
		};

		protected override Dictionary<Race, List<Action>> NpcActions => _Actions;

		[Constructable]
		public Biedak() : base("- Biedak")
		{
		}

		public Biedak(Serial serial) : base(serial)
		{
		}

		public override void OnGenderChanged(bool oldFemale)
		{
			base.OnGenderChanged(oldFemale);
			if (Female)
			{
				Title = "- Biedaczka";
			}
			else
			{
				Title = "- Biedak";
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			var version = reader.ReadInt();
		}
	}
}
