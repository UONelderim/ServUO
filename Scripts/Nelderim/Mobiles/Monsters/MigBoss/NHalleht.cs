#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class NHalleht : BaseCreature
	{
		[Constructable]
		public NHalleht() : base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			InitStats(100, 125, 25);
			Title = " - Wieszcz Przeznaczenia";

			Body = 0x190;

			Hue = 0x0;
			Blessed = true;
			Name = "Halleht";

			Robe chest = new Robe();
			chest.Hue = 0x1;
			AddItem(chest);

			QuarterStaff gloves = new QuarterStaff();
			gloves.Hue = 0x482;
			AddItem(gloves);


			Sandals buty = new Sandals();
			buty.Hue = 0x482;
			AddItem(buty);

			Skills[SkillName.Anatomy].Base = 120.0;
			Skills[SkillName.Tactics].Base = 120.0;
			Skills[SkillName.Archery].Base = 120.0;
			Skills[SkillName.MagicResist].Base = 120.0;
			Skills[SkillName.DetectHidden].Base = 100.0;
		}

		public NHalleht(Serial serial) : base(serial)
		{
		}

		public override void OnSpeech(SpeechEventArgs e)
		{
			base.OnSpeech(e);

			Mobile from = e.Mobile;

			if (from.InRange(this, 2))
			{
				if (e.Speech.ToLower().IndexOf("zadan") >= 0 || e.Speech.ToLower().IndexOf("praca") >= 0)
				{
					string message1 =
						"Strzege przejscia do tego, ktory jest najpotezniejszy. Jesli twa dusza jest gotowa na spotkanie z przeznaczniem przynies mi serca trzech ksiazat demonow!";
					this.Say(message1);
					string message2 = "Wpierw przynies mi serce Dzahhara Ksiecia Cierpienia!";
					this.Say(message2);
					string message3 =
						"Nastepnie zas serce Katrilla Pana Mordu.. . .. by dopelnic przeznaczenia jako ostatnie przynies mi serce Delotha Ksiecia Ciemnosci";
					this.Say(message3);
				}
			}
		}

		public int serce1;
		public int serce2;
		public int serce3;

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (dropped is NSerceDzahhara)
			{
				dropped.Delete();
				Say(true,
					" Pierwsza pieczec zostala otwarta, do otwarcia nastepnej potrzebuje serce Ksiecia Mordu - Katrilla! ");
				serce1 = 1;
			}

			if (dropped is NSerceKatrilla && serce1 < 1)
			{
				Say(true, "Glupcze najpierw potrzebuje serce Dzahhara!");
			}

			if (dropped is NSerceKatrilla && serce1 > 0)
			{
				dropped.Delete();
				Say(true,
					" Druga pieczec zostala otwarta, do otwarcia ostateniej potrzebuje serce Ksiecia Ciemnosci – Delotha ");
				serce2 = 1;
			}

			if (dropped is NSerceDelotha && serce1 < 1)
			{
				Say(true, "Glupcze najpierw potrzebuje serce Dzahhara!");
			}


			if (dropped is NSerceDelotha && serce2 < 1 && serce1 > 0)
			{
				Say(true, "Glupcze, teraz potrzebuje serce Katrilla!");
			}

			if (dropped is NSerceDelotha && serce2 > 0)
			{
				dropped.Delete();

				serce3 = 1;
				if (serce1 > 0 && serce2 > 0 && serce3 > 0)
				{
					Point3D loc = new Point3D(5141, 670, 40);
					Map map = Map.Felucca;
					wrotademonow portal = new wrotademonow();
					portal.MoveToWorld(loc, map);

					Say(true,
						" Ostatnia pieczec zostala otwarta, droga do najpotezniejszego - Wladcy Demonow, stoi otworem. Spieszcie sie gdyz zaraz sie zamknie! ");

					serce1 = 0;
					serce2 = 0;
					serce3 = 0;
				}
			}

			return base.OnDragDrop(from, dropped);
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
