#region References

using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

#endregion

// COPYRIGHT BY ROMANTHEBRAIN
namespace Server
{
	public class SkillNewCharPickGump : Gump
	{
		private readonly int switches = 3;
		private readonly SkillBallNewChar m_SkillBall;
		private readonly double val = 50;

		private class SkillIdName
		{
			public readonly SkillName skill;
			public readonly string description;

			public SkillIdName(SkillName sn, string desc)
			{
				skill = sn;
				description = desc;
			}
		}

		private static readonly SkillIdName[] SkillNames =
		{
			new SkillIdName(SkillName.Alchemy, "Alchemia"), new SkillIdName(SkillName.Anatomy, "Anatomia"),
			new SkillIdName(SkillName.AnimalLore, "Wiedza o zwierzetach"),
			new SkillIdName(SkillName.ItemID, "Identyfikacja"), new SkillIdName(SkillName.ArmsLore, "Wiedza o broni"),
			new SkillIdName(SkillName.Parry, "Parowanie"), new SkillIdName(SkillName.Begging, "Zebranie"), // ?
			new SkillIdName(SkillName.Blacksmith, "Kowalstwo"),
			new SkillIdName(SkillName.Fletching, "Wyrabianie lukow"),
			new SkillIdName(SkillName.Peacemaking, "Uspokajanie"), new SkillIdName(SkillName.Camping, "Obozowanie"),
			new SkillIdName(SkillName.Carpentry, "Stolarstwo"), new SkillIdName(SkillName.Cartography, "Kartografia"),
			new SkillIdName(SkillName.Cooking, "Gotowanie"), new SkillIdName(SkillName.DetectHidden, "Wykrywanie"),
			new SkillIdName(SkillName.Discordance, "Dezorientacja"), new SkillIdName(SkillName.EvalInt, "Madrosc"),
			new SkillIdName(SkillName.Healing, "Leczenie"), new SkillIdName(SkillName.Fishing, "Rybactwo"),
			new SkillIdName(SkillName.Herbalism, "Zielarstwo"), new SkillIdName(SkillName.Herding, "Pasterstwo"),
			new SkillIdName(SkillName.Hiding, "Ukrywanie"), new SkillIdName(SkillName.Provocation, "Prowokacja"),
			new SkillIdName(SkillName.Inscribe, "Inskrypcja"), new SkillIdName(SkillName.Lockpicking, "Wlamywanie"),
			new SkillIdName(SkillName.Magery, "Magia"), new SkillIdName(SkillName.MagicResist, "Odpornosc na magie"),
			new SkillIdName(SkillName.Tactics, "Taktyka"), new SkillIdName(SkillName.Snooping, "Zagladanie"),
			new SkillIdName(SkillName.Musicianship, "Kunszt muzyczny"),
			new SkillIdName(SkillName.Poisoning, "Zatruwanie"), new SkillIdName(SkillName.Archery, "Lucznictwo"),
			new SkillIdName(SkillName.SpiritSpeak, "Rozmowa z duchami"),
			new SkillIdName(SkillName.Stealing, "Okradanie"), new SkillIdName(SkillName.Tailoring, "Krawiectwo"),
			new SkillIdName(SkillName.AnimalTaming, "Oswajanie"),
			new SkillIdName(SkillName.TasteID, "Ocena smaku"), // ?
			new SkillIdName(SkillName.Tinkering, "Majsterkowanie"), new SkillIdName(SkillName.Tracking, "Tropienie"),
			new SkillIdName(SkillName.Veterinary, "Weterynaria"), new SkillIdName(SkillName.Swords, "Walka mieczami"),
			new SkillIdName(SkillName.Macing, "Walka obuchami"), new SkillIdName(SkillName.Fencing, "Walka szpadami"),
			new SkillIdName(SkillName.Wrestling, "Walka piesciami"),
			new SkillIdName(SkillName.Lumberjacking, "Drwalstwo"), new SkillIdName(SkillName.Mining, "Gornictwo"),
			new SkillIdName(SkillName.Meditation, "Medytacja"),
			//new SkillIdName(SkillName.Stealth,	"Skradanie"),  // disabled by default
			//new SkillIdName(SkillName.RemoveTrap,	"Usuwanie pulapek"),    // disabled by default
			new SkillIdName(SkillName.Necromancy, "Nekromancja"),
			//new SkillIdName(SkillName.Focus,	"Koncentracja"),
			new SkillIdName(SkillName.Chivalry, "Rycerstwo"), new SkillIdName(SkillName.Bushido, "Tradycja wojenna"),
			new SkillIdName(SkillName.Ninjitsu, "Skrytobojstwo"),
			//new SkillIdName(SkillName.Spellweaving,	"Druidyzm"),       // disabled by default
		};

		public SkillNewCharPickGump(SkillBallNewChar ball) : base(0, 0)
		{
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = true;
			m_SkillBall = ball;
			this.AddPage(0);
			this.AddBackground(39, 33, 600, 530, 9200);
			this.AddLabel(67, 41, 1153, @"Wybierz umiejetnosci dla postaci");
			this.AddButton(80, 530, 2071, 2072, (int)Buttons.Close, GumpButtonType.Reply, 0);
			this.AddBackground(52, 60, 570, 460, 9350);
			this.AddPage(1);
			this.AddButton(540, 530, 2311, 2312, (int)Buttons.FinishButton, GumpButtonType.Reply, 0);

			int rows = 18;
			int index = 0;
			for (int i = 0; i < rows; ++i)
			{
				if (index >= SkillNames.Length)
					break;
				this.AddCheck(55, 65 + 25 * i, 210, 211, false, index);
				this.AddLabel(80, 65 + 25 * i, 0, SkillNames[index].description);
				index++;
			}

			for (int i = 0; i < rows; ++i)
			{
				if (index >= SkillNames.Length)
					break;
				this.AddCheck(240, 65 + 25 * i, 210, 211, false, index);
				this.AddLabel(265, 65 + 25 * i, 0, SkillNames[index].description);
				index++;
			}

			for (int i = 0; i < rows; ++i)
			{
				if (index >= SkillNames.Length)
					break;
				this.AddCheck(425, 65 + 25 * i, 210, 211, false, index);
				this.AddLabel(450, 65 + 25 * i, 0, SkillNames[index].description);
				index++;
			}
		}

		public enum Buttons
		{
			Close,
			FinishButton,
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile m = state.Mobile;

			switch (info.ButtonID)
			{
				case 0: { break; }
				case 1:
				{
					if (info.Switches.Length < switches)
					{
						m.SendGump(new SkillNewCharPickGump(m_SkillBall));
						m.SendMessage(0, "Musisz wybrac jeszcze {0} umiejetnosci.", switches - info.Switches.Length);
						break;
					}

					if (info.Switches.Length > switches)
					{
						m.SendGump(new SkillNewCharPickGump(m_SkillBall));
						m.SendMessage(0, "Musisz odznaczyc {0} umiejetnosci. Maksymalna ilosc wynosi {1}.",
							info.Switches.Length - switches, switches);
						break;
					}

					Skills skills = m.Skills;

					for (int i = 0; i < SkillNames.Length; ++i)
					{
						SkillName sn = SkillNames[i].skill;
						if (!info.IsSwitched(i))
						{
							// reset other skills
							skills[sn].Base = 0;
						}
						else
						{
							// set chosen skills
							skills[sn].Base = val;
							// add skill items to backpack
							CharacterCreation.AddSkillItems(sn, m);
						}
					}

					m_SkillBall.Delete();
					break;
				}
			}
		}
	}

	public class SkillBallNewChar : Item
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile BondedCharacter { set; get; }

		public SkillBallNewChar(PlayerMobile player) : base(0xE73)
		{
			BondedCharacter = player;
			Weight = 1.0;
			Hue = 105;
			Name = "a three 50 limited skill ball";
			Movable = false;
		}

		[Constructable]
		public SkillBallNewChar() : base(0xE73)
		{
			Weight = 1.0;
			Hue = 105;
			Name = "a three 50 limited skill ball";
			Movable = false;
		}

		public override void OnDoubleClick(Mobile m)
		{
			if (BondedCharacter == null)
			{
				m.SendMessage("Ten przedmiot nie jest powiazany z rzadna postacia.");
			}
			else if (BondedCharacter != m)
			{
				m.SendMessage("Ten przedmiot nie jest powiazany z inna postacia.");
			}
			else if (m.Backpack != null && m.Backpack.GetAmount(typeof(SkillBallNewChar)) > 0)
			{
				m.SendMessage("Wybierz trzy umiejetnosci dla swojej postaci.");
				m.CloseGump(typeof(SkillNewCharPickGump));
				m.SendGump(new SkillNewCharPickGump(this));
			}
			else
				m.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
		}

		public SkillBallNewChar(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(1); // version 

			writer.Write(BondedCharacter);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			BondedCharacter = reader.ReadMobile();
		}
	}
}
