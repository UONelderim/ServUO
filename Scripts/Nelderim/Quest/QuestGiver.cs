#region References

using System.Collections.Generic;
using Server.Commands;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki nadzorcy")]
	public class QuestGiver : Mobile
	{
		public virtual bool IsInvulnerable { get { return true; } }

		[Constructable]
		public QuestGiver()
		{
			InitStats(31, 41, 51);

			Hue = Race.RandomSkinHue();
			Body = 0x190;
			Blessed = true;

			AddItem(new Robe(Utility.RandomNeutralHue()));
			AddItem(new Boots());
			Utility.AssignRandomHair(this);
			Direction = Direction.South;
			Name = NameList.RandomName("male");
			Title = "- nadzorca";
			CantWalk = true;
		}

		public QuestGiver(Serial serial) : base(serial)
		{
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);
			list.Add(new QuestGiverEntry(from, this));
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

		public class QuestGiverEntry : ContextMenuEntry
		{
			private readonly Mobile m_Mobile;
			private Mobile m_Giver;

			public QuestGiverEntry(Mobile from, Mobile giver) : base(6146, 3)
			{
				m_Mobile = from;
				m_Giver = giver;
			}

			public override void OnClick()
			{
				if (!(m_Mobile is PlayerMobile))
					return;

				PlayerMobile mobile = (PlayerMobile)m_Mobile;

				{
					if (!mobile.HasGump(typeof(QuestGiver_gump)))
					{
						mobile.SendGump(new QuestGiver_gump(mobile));
					}
				}
			}
		}

		private static void GetRandomAOSStats(out int attributeCount, out int min, out int max, int level)
		{
			int rnd = Utility.Random(15);

			if (level == 6)
			{
				attributeCount = Utility.RandomMinMax(2, 6);
				min = 20;
				max = 70;
			}
			else if (level == 5)
			{
				attributeCount = Utility.RandomMinMax(2, 4);
				min = 20;
				max = 50;
			}
			else if (level == 4)
			{
				attributeCount = Utility.RandomMinMax(2, 3);
				min = 20;
				max = 40;
			}
			else if (level == 3)
			{
				attributeCount = Utility.RandomMinMax(1, 3);
				min = 10;
				max = 30;
			}
			else if (level == 2)
			{
				attributeCount = Utility.RandomMinMax(1, 2);
				min = 10;
				max = 30;
			}
			else
			{
				attributeCount = 1;
				min = 10;
				max = 20;
			}
		}

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			Mobile m = from;
			PlayerMobile mobile = m as PlayerMobile;

			if (mobile != null)
			{
				if (dropped is Gold && dropped.Amount == 5)
				{
					mobile.AddToBackpack(new QuestScroll(1));
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false,
						"Kiedy skonczysz, zwroc mi pergamin z misja... by otrzymac swoja nagrode.", mobile.NetState);
					return true;
				}

				if (dropped is Gold && dropped.Amount == 10)
				{
					mobile.AddToBackpack(new QuestScroll(2));
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false,
						"Kiedy skonczysz, zwroc mi pergamin z misja... by otrzymac swoja nagrode.", mobile.NetState);
					return true;
				}

				if (dropped is Gold && dropped.Amount == 15)
				{
					mobile.AddToBackpack(new QuestScroll(3));
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false,
						"Kiedy skonczysz, zwroc mi pergamin z misja... by otrzymac swoja nagrode.", mobile.NetState);
					return true;
				}

				if (dropped is Gold && dropped.Amount == 20)
				{
					mobile.AddToBackpack(new QuestScroll(4));
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false,
						"Kiedy skonczysz, zwroc mi pergamin z misja... by otrzymac swoja nagrode.", mobile.NetState);
					return true;
				}

				if (dropped is Gold && dropped.Amount == 25)
				{
					mobile.AddToBackpack(new QuestScroll(5));
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false,
						"Kiedy skonczysz, zwroc mi pergamin z misja... by otrzymac swoja nagrode.", mobile.NetState);
					return true;
				}

				if (dropped is Gold && dropped.Amount == 30)
				{
					mobile.AddToBackpack(new QuestScroll(6));
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false,
						"Kiedy skonczysz, zwroc mi pergamin z misja... by otrzymac swoja nagrode.", mobile.NetState);
					return true;
				}

				if (dropped is Gold)
				{
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "To nie jest kwota, ktorej szukam.",
						mobile.NetState);
					return false;
				}

				if (dropped is QuestScroll)
				{
					QuestScroll m_Quest = (QuestScroll)dropped;

					if (m_Quest.NNeed > m_Quest.NGot)
					{
						mobile.AddToBackpack(dropped);
						this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Nie ukonczyles tego zadania.",
							mobile.NetState);
						return false;
					}

					string sMessage = "";
					if (m_Quest.NType == 1) { sMessage = "Widze, ze zwyciezyles. Oto twoja nagroda."; }
					else { sMessage = "O prosze, wykonales " + m_Quest.NItemName + "! Oto Twoja nagroda!"; }

					if (Utility.RandomMinMax(1, 4) == 1)
					{
						mobile.AddToBackpack(new Gold(m_Quest.NLevel * Utility.RandomMinMax(125, 200)));
					}
					else
					{
						mobile.AddToBackpack(new Gold(m_Quest.NLevel * Utility.RandomMinMax(75, 150)));

						Item item;

						item = Loot.RandomArmorOrShieldOrWeaponOrJewelry();

						if (item is BaseWeapon)
						{
							BaseWeapon weapon = (BaseWeapon)item;

							int attributeCount;
							int min, max;

							GetRandomAOSStats(out attributeCount, out min, out max, m_Quest.NLevel);

							BaseRunicTool.ApplyAttributesTo(weapon, attributeCount, min, max);

							mobile.AddToBackpack(item);
						}
						else if (item is BaseArmor)
						{
							BaseArmor armor = (BaseArmor)item;

							int attributeCount;
							int min, max;

							GetRandomAOSStats(out attributeCount, out min, out max, m_Quest.NLevel);

							BaseRunicTool.ApplyAttributesTo(armor, attributeCount, min, max);

							mobile.AddToBackpack(item);
						}
						else if (item is BaseHat)
						{
							BaseHat hat = (BaseHat)item;

							int attributeCount;
							int min, max;

							GetRandomAOSStats(out attributeCount, out min, out max, m_Quest.NLevel);

							BaseRunicTool.ApplyAttributesTo(hat, attributeCount, min, max);

							mobile.AddToBackpack(item);
						}
						else if (item is BaseJewel)
						{
							int attributeCount;
							int min, max;

							GetRandomAOSStats(out attributeCount, out min, out max, m_Quest.NLevel);

							BaseRunicTool.ApplyAttributesTo((BaseJewel)item, attributeCount, min, max);

							mobile.AddToBackpack(item);
						}
					}

					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, mobile.NetState);

					dropped.Delete();

					return true;
				}

				mobile.AddToBackpack(dropped);
				this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Tego nie potrzebuje...",
					mobile.NetState);
				return true;
			}

			return false;
		}
	}
}

namespace Server.Gumps
{
	public class QuestGiver_gump : Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register("QuestGiver_gump", AccessLevel.GameMaster,
				QuestGiver_gump_OnCommand);
		}

		private static void QuestGiver_gump_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendGump(new QuestGiver_gump(e.Mobile));
		}

		public QuestGiver_gump(Mobile owner) : base(50, 50)
		{
			AddPage(0);
			AddImageTiled(54, 33, 369, 400, 2624);
			AddAlphaRegion(54, 33, 369, 400);
			AddImageTiled(416, 39, 44, 389, 203);

			AddImage(97, 49, 9005);
			AddImageTiled(58, 39, 29, 390, 10460);
			AddImageTiled(412, 37, 31, 389, 10460);
			AddLabel(140, 60, 0x34, "Nadzorca");

			AddHtml(107, 140, 300, 230, " < BODY > " +
			                            //////////////////////  xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx  THIS IS THE LENGTH OF THE TEXT ALLOWED //
			                            "<BASEFONT COLOR=YELLOW>Witaj odważny poszukiwaczu przygód. jestem miejscowym nadzorca.<BR>" +
			                            "<BASEFONT COLOR=YELLOW>Jesli cokolwiek musi być<BR>" +
			                            "<BASEFONT COLOR=YELLOW>zrobione w tym miescie... to ja musze tego dopilnowac.<BR>" +
			                            "<BASEFONT COLOR=YELLOW>Chociaz nie powinienem zatrudniac obywateli,<BR>" +
			                            "<BASEFONT COLOR=YELLOW>to wygladasz na takiego, ktory poradzi sobie z kazdym zagrozeniem.<BR>" +
			                            "<BASEFONT COLOR=YELLOW>Oczywiście moglbym wpasc<BR>" +
			                            "<BASEFONT COLOR=YELLOW>w wielkie klopoty jesli dowiedza sie, ze ja<BR>" +
			                            "<BASEFONT COLOR=YELLOW>pozwolilem jakiejs sprawie wysliznac sie, jako, ze zloto<BR>" +
			                            "<BASEFONT COLOR=YELLOW>jest chodliwym towarem, ze tak to ujme, i zwykle mieszczanstwo chce<BR>" +
			                            "<BASEFONT COLOR=YELLOW>zdobyc bogactwa wylacznie dla siebie.<BR>" +
			                            "<BASEFONT COLOR=YELLOW><BR>" +
			                            "<BASEFONT COLOR=YELLOW>Wiesz co, jesli dasz mi kilka zlotych monet<BR>" +
			                            "<BASEFONT COLOR=YELLOW>bede mniej uwazac na to co mowie...<BR>" +
			                            "<BASEFONT COLOR=YELLOW>Im wiecej zlota...<BR>" +
			                            "<BASEFONT COLOR=YELLOW>...tym bardziej nieuwazny bede<BR>" +
			                            "<BASEFONT COLOR=YELLOW>w tym co mowie.<BR>" +
			                            "<BASEFONT COLOR=YELLOW><BR>" +
			                            "<BASEFONT COLOR=YELLOW>5 centarow - Zadanie 1 poziomu<BR>" +
			                            "<BASEFONT COLOR=YELLOW>10 centarow - Zadanie 2 poziomu<BR>" +
			                            "<BASEFONT COLOR=YELLOW>15 centarow - Zadanie 3 poziomu<BR>" +
			                            "<BASEFONT COLOR=YELLOW>20 centarow - Zadanie 4 poziomu<BR>" +
			                            "<BASEFONT COLOR=YELLOW>25 centarow - Zadanie 5 poziomu<BR>" +
			                            "<BASEFONT COLOR=YELLOW>30 centarow - Zadanie 6 poziomu<BR>" +
			                            "<BASEFONT COLOR=YELLOW><BR>" +
			                            "<BASEFONT COLOR=YELLOW>Jako nagrode otrzymasz troche zlota<BR>" +
			                            "<BASEFONT COLOR=YELLOW>lub magiczny przedmiot.<BR>" +
			                            "</BODY>", false, true);

			AddImage(430, 9, 10441);
			AddImageTiled(40, 38, 17, 391, 9263);
			AddImage(6, 25, 10421);
			AddImage(34, 12, 10420);
			AddImageTiled(94, 25, 342, 15, 10304);
			AddImageTiled(40, 427, 415, 16, 10304);
			AddImage(-10, 314, 10402);
			AddImage(56, 150, 10411);
			AddImage(155, 120, 2103);
			AddImage(136, 84, 96);
			AddButton(225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;
			switch (info.ButtonID)
			{
				case 0: { break; }
			}
		}
	}
}
