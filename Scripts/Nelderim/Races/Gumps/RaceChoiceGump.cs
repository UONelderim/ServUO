#region References

using Nelderim;
using Server.Network;

#endregion

namespace Server.Gumps
{
	public class RaceChoiceGump : Gump
	{
		protected Mobile m_From;
		protected Race m_Race;

		protected int m_SkinHue;
		protected int m_HairHue;
		protected int m_HairItemID;
		protected int m_FacialHairItemID;

		public enum ButtonID
		{
			Cancel,
			Apply
		}

		public string Color(string text)
		{
			return Color(text, 0xFFFFFF);
		}

		public string Color(string text, int color)
		{
			return $"<BASEFONT COLOR=#{color:X6}>{text}</BASEFONT>";
		}

		public string Center(string text)
		{
			return $"<CENTER>{text}</CENTER>";
		}

		public int GetHairGumpId(int hairId)
		{
			var offset = m_From.Female ? 10000 : 0;

			return hairId switch
			{
				Hair.Human.Short => 50700,
				Hair.Human.Long => 50701,
				Hair.Human.PonyTail => 50702,
				Hair.Human.Mohawk => 50703,
				Hair.Human.Pageboy => 50710,
				Hair.Human.Buns => 50712,
				Hair.Human.Afro => 50900,
				Hair.Human.Receeding => 50901,
				Hair.Human.PigTails => 50902,
				Hair.Human.Krisna => 50715,
				Hair.Elf.MidLong => 50916,
				Hair.Elf.LongFeather => 50917,
				Hair.Elf.Short => 50918,
				Hair.Elf.Mullet => 50919,
				Hair.Elf.Flower => 50890,
				Hair.Elf.Long => 50891,
				Hair.Elf.Knob => 50892,
				Hair.Elf.Braided => 50893,
				Hair.Elf.Bun => 50894,
				Hair.Elf.Spiked => 50895,
				_ => 0
			} + offset;
		}

		public int GetBeardGumpId(int facialHairId) =>
			facialHairId switch
			{
				Beard.Human.Long => 50801,
				Beard.Human.Short => 50802,
				Beard.Human.Goatee => 50800,
				Beard.Human.Mustache => 50808,
				Beard.Human.MidShort => 50904,
				Beard.Human.MidLong => 50905,
				Beard.Human.Vandyke => 50906,
				_ => 0
			};

		public virtual bool AllowAppearanceChange()
		{
			return true;
		}

		public virtual void DrawBackground()
		{
			AddBackground(0, 0, 605, 570, 9200);
			AddImageTiled(5, 5, 595, 560, 9274);
			AddLabel(20, 10, 930, $"WYBOR RASY NELDERIM - {m_Race.GetName().ToUpper()}");

			AddHtml(390, 257, 203, 25,
				Color(Center("RASA " + m_Race.GetPluralName(Cases.Dopelniacz).ToUpper()), 0x333333), true, false);
			AddHtmlLocalized(388, 282, 205, 255, m_Race.DescNumber, true, true); // opis

			AddButton(457, 540, 0xef, 0xf0, (int)ButtonID.Apply, GumpButtonType.Reply, 0);
			AddButton(527, 540, 242, 243, (int)ButtonID.Cancel, GumpButtonType.Reply, 0);
		}

		public void DrawPaperdoll()
		{
			AddImageTiled(376, 5, 217, 251, 1800); // paperdoll

			// Body
			if (m_From.Female)
				AddImage(400, 20, 0xd, m_SkinHue - 1);
			else
				AddImage(400, 20, 0xc, m_SkinHue - 1);

			// Hair
			if (m_HairItemID != Hair.Human.Bald)
				AddImage(400, 18, GetHairGumpId(m_HairItemID), m_HairHue - 1);
			// FacialHair
			if (m_FacialHairItemID != Beard.Human.Clean)
				AddImage(400, 20, GetBeardGumpId(m_FacialHairItemID), m_HairHue - 1);
		}

		public void DrawChoices()
		{
			AddHtml(15, 40, 360, 28, Center("Kolor skory"), true, false); // Kolor skory

			int x = 15;
			int y = 80;

			for (int i = 101; i <= m_Race.SkinHues.Length + 100; i++)
			{
				if (x > 370)
				{
					x = 15;
					y = y + 25;
				}

				AddButton(x, y, 0x56f7, 0x56f7, i, GumpButtonType.Reply, 0);
				AddImage(x, y, 0x56f7, m_Race.SkinHues[i - 101] - 1);
				x = x + 20;
			}

			AddHtml(15, y + 25, 360, 28, Center("Kolor wlosow"), true, false); // Kolor wlosow

			y = y + 60;
			x = 15;

			for (int i = 201; i <= m_Race.HairHues.Length + 200; i++)
			{
				if (x > 370)
				{
					x = 15;
					y = y + 25;
				}

				AddButton(x, y, 0x56f7, 0x56f7, i, GumpButtonType.Reply, 0);
				AddImage(x, y, 0x56f7, m_Race.HairHues[i - 201] - 1);
				x = x + 20;
			}

			AddHtml(15, y + 25, 360, 28, Center("Fryzura"), true, false); // Fryzura
			y = y + 60;
			x = 15;

			int[] hairStyles = m_From.Female ? m_Race.HairTable[0] : m_Race.HairTable[1];
			for (int i = 301; i <= (hairStyles.Length + 300); i++)
			{
				if (x > 300)
				{
					x = 15;
					y = y + 50;
				}

				AddButton(x, y, 0xd0, 0xd1, i, GumpButtonType.Reply, 0);

				int hid = hairStyles[i - 301];
				int gumpID = GetHairGumpId(hid);

				if (gumpID == 0)
					AddHtml(x + 30, y, 50, 50, Color("Brak"), false, false);
				else
					AddImage(x - 50, y - 50, gumpID);

				x = x + 80;
			}

			AddHtml(15, y + 35, 360, 28, Center("Zarost"), true, false); // Zarost
			y = y + 70;
			x = 15;

			int[] facialHairStyles = m_From.Female ? m_Race.BeardTable[0] : m_Race.BeardTable[1];
			for (int i = 401; i <= (facialHairStyles.Length + 400); i++)
			{
				if (x > 300)
				{
					x = 15;
					y = y + 50;
				}

				AddButton(x, y, 0xd0, 0xd1, i, GumpButtonType.Reply, 0);

				int fhid = facialHairStyles[i - 401];
				int gumpID = GetBeardGumpId(fhid);

				if (gumpID == 0)
					AddHtml(x + 30, y, 50, 50, Color("Brak"), false, false);
				else
					AddImage(x - 50, y - 50, gumpID);

				x = x + 80;
			}
		}

		public RaceChoiceGump(Mobile from, Race race) : this(from, race, race.SkinHues[0], race.HairHues[0],
			Hair.Human.Bald, Beard.Human.Clean)
		{
		}

		public RaceChoiceGump(Mobile from, Race race, int skinHue, int hairHue, int hairItemID, int facialHairItemID) :
			base(40, 40)
		{
			m_From = from;

			if (!AllowAppearanceChange())
			{
				from.SendLocalizedMessage(501704); // You cannot disguise yourself while incognitoed.
				return;
			}

			m_Race = race;

			m_SkinHue = skinHue;
			m_HairHue = hairHue;
			m_HairItemID = hairItemID;
			m_FacialHairItemID = facialHairItemID;

			from.CloseGump(typeof(RaceChoiceGump)); // TODO: czy zamknie tez gump klasy pochodnej?

			Closable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);

			DrawBackground();

			DrawPaperdoll();

			DrawChoices();
		}

		public static void Initialize()
		{
			//EventSink.Movement += new MovementEventHandler( EventSink_Movement_DGumpClose );
		}

		private static void EventSink_Movement_DGumpClose(MovementEventArgs args)
		{
			Mobile m = args.Mobile;
			m.CloseGump(typeof(RaceChoiceGump));
		}

		public virtual void ApplyAppearance(Race race, int skinHue, int hairHue, int hairID, int facialHairID)
		{
			m_From.Race = race;
			m_From.HairItemID = hairID;
			m_From.HairHue = hairHue;
			m_From.Hue = skinHue;
			if (!m_From.Female)
				m_From.FacialHairItemID = facialHairID;
			m_From.FacialHairHue = m_HairHue;

			m_From.SendMessage(0x20, "Od teraz jestes {0}.", m_Race.GetName(Cases.Narzednik));
		}

		public virtual void ExtendedResponse(RelayInfo info)
		{
			m_From.SendGump(new RaceChoiceGump(m_From, m_Race, m_SkinHue, m_HairHue, m_HairItemID, m_FacialHairItemID));
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (info.ButtonID == (int)ButtonID.Cancel)
				return;
			if (info.ButtonID == (int)ButtonID.Apply)
			{
				ApplyAppearance(m_Race, m_SkinHue, m_HairHue, m_HairItemID, m_FacialHairItemID);
				return;
			}

			if (info.ButtonID < 200)
				m_SkinHue = m_Race.ClipSkinHue(m_Race.SkinHues[info.ButtonID - 101]);
			else if (info.ButtonID < 300)
				m_HairHue = m_Race.ClipHairHue(m_Race.HairHues[info.ButtonID - 201]);
			else if (info.ButtonID < 400)
			{
				int[] hs = m_From.Female ? m_Race.HairTable[0] : m_Race.HairTable[1];
				m_HairItemID = hs[info.ButtonID - 301];
			}
			else if (info.ButtonID < 500)
			{
				int[] fhs = m_From.Female ? m_Race.BeardTable[0] : m_Race.BeardTable[1];
				m_FacialHairItemID = fhs[info.ButtonID - 401];
			}
			
			m_From.LanguagesKnown.Clear();
			var langs = m_Race.DefaultLanguages;
			foreach (var lang in langs.Keys)
			{
				m_From.LanguagesKnown[lang] = langs[lang];
			}
			m_From.LanguageSpeaking = NLanguage.Powszechny;

			ExtendedResponse(info);
		}
	}
}
