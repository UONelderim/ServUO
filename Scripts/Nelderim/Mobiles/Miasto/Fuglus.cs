#region References

using System.Collections.Generic;

#endregion

namespace Server.Mobiles
{
	public class Fuglus : NBaseTalkingNPC
	{
		private static readonly Dictionary<Race, List<Action>> _Actions = new Dictionary<Race, List<Action>>
		{
			{
				Race.DefaultRace, new List<Action>
				{
					m =>
					{
						m.Say("O ja biedny, nieszczęśliwy!");
						m.Emote("*Szlocha...*");
					},
					m => m.Say("Znowu mnie wyruchali bez mydła!"),
					m =>
					{
						m.Say("Płaczę bo lubię!");
						m.Emote("*Szlocha...*");
					},
					m => m.Say("Dlaczego wojownik musi mieć pod górkę?!"),
					m =>
					{
						m.Say("Nie umiem, nie umiem");
						m.Emote("*Szlocha...*");
					},
					m => m.Say("Jak się wkurwię wpadnę w furię!"),
					m => m.Say("Do czego by się tu dzisiaj przypierdolić..."),
					m => m.Say("O co ci kurwa chodzi co?!"),
					m => m.Say("Czemu mnie drażnisz?"),
					m => m.Say("Wszyscy przeciwko mnie?! Dlaczego!!!"),
					m => m.Say("Do czego by się tu dzisiaj przypierdolić..."),
					m => m.Say("Dobra nie mam siły do ciebie."),
					m => m.Say("Chciałem być kowalem, a zostałem furiatem!"),
					m => m.Say(
						"Czysty wojownik nie ma szans w tym świecie... dlatego postanowiłem, że przestaje sie myć!"),
					m => m.Say("Od poł godziny probuje ustalić co ci nie pasuje..."),
					m => m.Say("Nie pyskuj gnoju!"),
					m => m.Say(
						"Czy słyszałeś już, że krasnoludy dużo pierdzą po grochówce? Chcesz bym ci o tym opowiedział?"),
					m =>
					{
						m.Say("To wszystko wina świata!");
						m.Emote("*Szlocha...*");
					},
				}
			}
		};

		protected override Dictionary<Race, List<Action>> NpcActions => _Actions;

		[Constructable]
		public Fuglus() : base("- Furiat")
		{
		}

		public override void OnGenderChanged(bool oldFemale)
		{
			base.OnGenderChanged(oldFemale);
			if (Female)
			{
				Name = "Fugiella";
				Title = "- Furiatka";
			}
			else
			{
				Name = "Fuglus";
				Title = "- Furiat";
			}
		}

		public Fuglus(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
