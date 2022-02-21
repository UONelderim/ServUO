#region References

using System;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class XmlQuestNPCSzemranyTyp : TalkingBaseCreature
	{
		[Constructable]
		public XmlQuestNPCSzemranyTyp() : this(-1)
		{
		}

		private DateTime m_Spoken;

		public override void OnMovement(Mobile m, Point3D oldLocation)
		{
			if (m.Alive && m is PlayerMobile)
			{
				PlayerMobile pm = (PlayerMobile)m;

				int range = 2;

				if (Utility.RandomDouble() < 0.20)
				{
					if (range >= 0 && InRange(m, range) && !InRange(oldLocation, range) &&
					    DateTime.Now >= m_Spoken + TimeSpan.FromSeconds(10))
					{
						if (Race == Race.NTamael)
						{
							switch (Utility.Random(16))
							{
								case 0:
									Emote("*Rozgląda się uważnie w koło*");
									break;
								case 1:
									Emote("*Podrzuca broń w dłoni*");
									break;
								case 2:
									Emote("*Chowa nerwowo za pas drobny pakunek*");
									break;
								case 3:
									Say("I co Ci do tego...");
									break;
								case 4:
									Say("Nie ma Cie... Ale już! *Odgonił ręką*");
									break;
								case 5:
									Say("Zjeżdżaj stąd...");
									break;
								case 6:
									Emote("*Szura powoli nogą po ziemi rozglądając się w koło*");
									break;
								case 7:
									Say("Jak sie nie ma co sie pragnie... to sie kradnie co popadnie...");
									Emote("*Szepcze coś pod nosem*");
									break;
								case 8:
									Say("Zajebać Ci?");
									break;
								case 9:
									Say("Tyle tej straży... na to maja pieniądze...");
									Emote("*Wzdycha ciężko*");
									break;
								case 10:
									Say("Won mi stąd...");
									break;
								case 11:
									Say("Pilnuj sie... bywa tu całkiem...Niebezpiecznie");
									Emote("*Uśmiecha się szyderczo*");
									break;
								case 12:
									Say("Nic nie widziałeś... rozumiesz?");
									Emote("*Spluwa na ziemie*");
									break;
								case 13:
									Say("Oni tak zawsze mówią, a później nic nie robią.");
									break;
								case 14:
									Say("Jarle precz, knypki precz, w dłonie miecz, wrogów siecz!");
									break;
								case 15:
									Say("Same ścierwa się tu panoszą... Tasandora dla Tamaeli!");
									break;
							}
						}
						else if (Race == Race.NJarling)
						{
							switch (Utility.Random(14))
							{
								case 0:
									Emote("*Rozgląda się uważnie w koło*");
									break;
								case 1:
									Say("Nudzi Ci sie do kurwy?");
									break;
								case 2:
									Say("Zjeżdżaj...");
									break;
								case 3:
									Say("Jak nie kupujesz towaru to spierdalaj...");
									break;
								case 4:
									Emote("*Rozgląda się nerwowo*");
									break;
								case 5:
									Emote("*Gwiżdże cicho pod nosem*");
									break;
								case 6:
									Say("Parszywe miasto, śmierdzi gównem.");
									break;
								case 7:
									Say("Czy my sie aby nie znamy...? Ty sukinsynu...");
									break;
								case 8:
									Say("To twoja ostatnia szansa, żeby odejść stąd o własnych siłach...");
									break;
								case 9:
									Emote("*Powolnym i spokojnym ruchem chowa coś za pazuchę*");
									break;
								case 10:
									Say("Pachnie tu gównem... nie to co na Północy...");
									break;
								case 11:
									Say("Masz jakiś problem?!");
									break;
								case 12:
									Say("Znikaj stąd zanim obiję Ci ryj...");
									break;
								case 13:
									Say("Pilnuj swojego nosa... Dobrze radzę.");
									break;
							}
						}
						else if (Race == Race.NKrasnolud)
						{
							switch (Utility.Random(15))
							{
								case 0:
									Emote("*Rozgląda się uważnie w koło*");
									break;
								case 1:
									Say("Rozpierdole Ci łeb jak czaszkę smoka!");
									break;
								case 2:
									Emote("*Zerka na swoją broń*");
									Say("Krasnoludzka robota to nie jest, ale i tak rozjebie Ci tym łeb");
									break;
								case 3:
									Say("Nie chce Cie tu widzieć... Won!");
									break;
								case 4:
									Say("Spierdalaj...");
									Emote("*Splunął pod nogi*");
									break;
								case 5:
									Say("Jak leziesz kurwa!");
									break;
								case 6:
									Say("Krasnal?! Który to powiedział?! Stólić pyski bo pozabijam!");
									break;
								case 7:
									Say(
										"Pan stworzył Krasnoludy z ognia i ziemi, patrząc na Ciebie budulcem było gówno...");
									break;
								case 8:
									Say("Nie handluję z partaczami, wynoś się...");
									break;
								case 9:
									Emote("*Nuci po cichu melodię*");
									break;
								case 10:
									Say("Przywalę Ci w mordę!");
									break;
								case 11:
									Say("A niech mnie, myślałem, że nie żyjesz... Szkoda...");
									break;
								case 12:
									Say("Zasadzić Ci kopa?!");
									break;
								case 13:
									Say("Prosisz się o połamaną czaszkę!");
									break;
								case 14:
									Say("Sam się kurwa pchasz na nóż, spierdalaj...");
									break;
							}
						}

						m_Spoken = DateTime.Now;
					}
				}
			}
		}

		[Constructable]
		public XmlQuestNPCSzemranyTyp(int gender) : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.8, 3.0)
		{
			SetStr(100, 300);
			SetDex(100, 300);
			SetInt(100, 300);

			Fame = 500;
			Karma = -1000;

			CanHearGhosts = false;

			SpeechHue = Utility.RandomDyedHue();

			Hue = Race.RandomSkinHue();

			switch (gender)
			{
				case -1:
					this.Female = Utility.RandomBool();
					break;
				case 0:
					this.Female = false;
					break;
				case 1:
					this.Female = true;
					break;
			}

			if (this.Female)
			{
				this.Body = 0x191;
				this.Name = NameList.RandomName("female");
				Title = "- Szemrana Dziewucha";
				Item hat = null;
				switch (Utility.Random(6)) //4 hats, one empty, for no hat
				{
					case 0:
						hat = new FeatheredHat(GetRandomHue());
						break;
					case 1:
						hat = new Bonnet(GetRandomHue());
						break;
					case 2:
						hat = new Cap(GetRandomHue());
						break;
					case 3:
						hat = new Bandana(GetRandomHue());
						break;
					case 4:
						hat = new ClothNinjaHood(GetRandomHue());
						break;
					case 5:
						hat = new SkullCap(GetRandomHue());
						break;
				}

				AddItem(hat);
				Item pants = null;
				switch (Utility.Random(3))
				{
					case 0:
						pants = new ShortPants(GetRandomHue());
						break;
					case 1:
						pants = new LongPants(GetRandomHue());
						break;
					case 2:
						pants = new Skirt(GetRandomHue());
						break;
				}

				AddItem(pants);
				Item shirt = null;
				switch (Utility.Random(6))
				{
					case 0:
						shirt = new Doublet(GetRandomHue());
						break;
					case 1:
						shirt = new Surcoat(GetRandomHue());
						break;
					case 2:
						shirt = new Robe(GetRandomHue());
						break;
					case 3:
						shirt = new FancyShirt(GetRandomHue());
						break;
					case 4:
						shirt = new Shirt(GetRandomHue());
						break;
					case 5:
						shirt = new HoodedShroudOfShadows(GetRandomHue());
						break;
				}

				AddItem(shirt);
			}
			else
			{
				this.Body = 0x190;
				this.Name = NameList.RandomName("male");
				Title = "- Szemrany Typ";
				Item hat = null;
				switch (Utility.Random(6))
				{
					case 0:
						hat = new FloppyHat(GetRandomHue());
						break;
					case 1:
						hat = new WideBrimHat();
						break;
					case 2:
						hat = new TallStrawHat(GetRandomHue());
						break;
					case 3:
						hat = new StrawHat(GetRandomHue());
						break;
					case 4:
						hat = new TricorneHat(GetRandomHue());
						break;
					case 5:
						hat = new SkullCap(GetRandomHue());
						break;
				}

				AddItem(hat);
				Item pants = null;
				switch (Utility.Random(2))
				{
					case 0:
						pants = new ShortPants(GetRandomHue());
						break;
					case 1:
						pants = new LongPants(GetRandomHue());
						break;
				}

				AddItem(pants);
				Item shirt = null;
				switch (Utility.Random(7))
				{
					case 0:
						shirt = new Doublet(GetRandomHue());
						break;
					case 1:
						shirt = new Surcoat(GetRandomHue());
						break;
					case 2:
						shirt = new Tunic(GetRandomHue());
						break;
					case 3:
						shirt = new FancyShirt(GetRandomHue());
						break;
					case 4:
						shirt = new Shirt(GetRandomHue());
						break;
					case 5:
						shirt = new Robe(GetRandomHue());
						break;
					case 6:
						shirt = new HoodedShroudOfShadows(GetRandomHue());
						break;
				}

				AddItem(shirt);

				if (Utility.RandomBool())
				{
					AddItem(new Cloak(GetRandomHue()));
				}
			}

			Item hand = null;
			switch (Utility.Random(4))
			{
				case 0:
					hand = new Dagger();
					break;
				case 1:
					hand = new Club();
					break;
				case 2:
					hand = new ButcherKnife();
					break;
				case 3:
					hand = new AssassinSpike();
					break;
			}

			AddItem(hand);

			Item feet = null;
			switch (Utility.Random(3))
			{
				case 0:
					feet = new Boots(Utility.RandomNeutralHue());
					break;
				case 1:
					feet = new Shoes(Utility.RandomNeutralHue());
					break;
				case 2:
					feet = new Sandals(Utility.RandomNeutralHue());
					break;
			}

			AddItem(feet);
			Container pack = new Backpack();

			pack.Movable = false;

			AddItem(pack);
		}

		public XmlQuestNPCSzemranyTyp(Serial serial) : base(serial)
		{
		}


		private static int GetRandomHue()
		{
			switch (Utility.Random(6))
			{
				default:
				case 0: return 0;
				case 1: return Utility.RandomBlueHue();
				case 2: return Utility.RandomGreenHue();
				case 3: return Utility.RandomRedHue();
				case 4: return Utility.RandomYellowHue();
				case 5: return Utility.RandomNeutralHue();
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

			int version = reader.ReadInt();
		}
	}
}
