#region References

using System;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class XmlQuestNPCPijak : TalkingBaseCreature
	{
		[Constructable]
		public XmlQuestNPCPijak() : this(-1)
		{
		}

		private DateTime m_Spoken;

		public override void OnMovement(Mobile m, Point3D oldLocation)
		{
			if (m.Alive && m is PlayerMobile)
			{
				int range = 2;

				if (Utility.RandomDouble() < 0.20)
				{
					if (range >= 0 && InRange(m, range) && !InRange(oldLocation, range) &&
					    DateTime.Now >= m_Spoken + TimeSpan.FromSeconds(10))
					{
						if (Race == Race.NTamael)
						{
							switch (Utility.Random(19))
							{
								case 0:
									Say("Wina, wina, wiiiina dajcie! A jak umrę pochowajcie!");
									break;
								case 1:
									Say("Wódko ma, wódko ma, wódko ma! Którz bez ciebie sobie w życiu rade da!");
									break;
								case 2:
									Say("I jeszcze jeden... I Jeszcze raz...");
									break;
								case 3:
									Say("Świat się zmienia... Słońce zachodzi, a wódka się kończy...");
									Emote("*Hic*");
									break;
								case 4:
									Say("Mamusia mówiła... Nie pij z krasnalami");
									break;
								case 5:
									Say("Dawno nie było tak słabego piwa...");
									Emote("*Spogląda na butlę w dłoni*");
									Say("Chociaż z drugiej strony...");
									Emote("*Przechyla butlę*");
									break;
								case 6:
									Emote("*Czka*");
									break;
								case 7:
									Say("Dobrodzieju... Nie będę kłamać, zbieram na wino, poratujcie w potrzebie");
									break;
								case 8:
									Say("Czy mam problem z alkoholem?");
									Emote("*Zerka na pustą flaszeczkę*");
									Say("Owszem... Skończył się");
									break;
								case 9:
									Say("No dobra... Chluśniem bo uśniem");
									break;
								case 10:
									Say("Pierdykniem bo odwykniem");
									Emote("*Pogrąża się w upojeniu*");
									break;
								case 11:
									Say("Wyglądasz na światowca! Prawda to, że kurwy z Garlan goliły się tam na dole?");
									break;
								case 12:
									Emote("*Smarka w brudną chustę*");
									break;
								case 13:
									Say("Kiedyś to było...");
									Emote("*Wzdycha ciężko*");
									break;
								case 14:
									Emote("*Poprawia łachmany*");
									break;
								case 15:
									Say("Co mi się tak pić chce? Przecież wczoraj tyle sie piło...");
									break;
								case 16:
									Emote("*Podnosi coś z ziemi*");
									Say("Ah.. jednak nie");
									break;
								case 17:
									Say("Czuję się, jakby mnie ktoś zjadł, a potem wysrał...");
									Emote("*Rozgląda się powoli wzdychając*");
									break;
								case 18:
									Say("Jak nie chlapne sobie winka, to nie pozna mnie rodzinka");
									Emote("*Śmieje sie głośno*");
									break;
							}
						}
						else if (Race == Race.NJarling)
						{
							switch (Utility.Random(18))
							{
								case 0:
									Say("Wódka jak wódka, smakuje jednakowo... czyli wspaniale");
									break;
								case 1:
									Say("Czym sie strułeś tym sie lecz jak to mamusia mówiła");
									Emote("*Otwiera flaszeczkę wódki*");
									break;
								case 2:
									Say("Mianowicie, czas na picie");
									Emote("*Brechta*");
									break;
								case 3:
									Say("Jak sie nie pije to wątroba gnije!");
									Emote("*Rechocze*");
									break;
								case 4:
									Emote("*Zatacza sie*");
									Say("Buja jak na morzu...");
									Emote("*Hic*");
									break;
								case 5:
									Say("Picie to jest życie! A jak!");
									Emote("*Szczerzy sie*");
									break;
								case 6:
									Say("A z chęcią by można taniej gorzałki wypić.");
									break;
								case 7:
									Say("Ugh...moja głowa...");
									Emote("*Wzdycha ciężko*");
									break;
								case 8:
									Say("Pszeciieszzjawcleniepilem...kchanie...noo..wróć...");
									Emote("*Wzdycha ze smutkiem*");
									break;
								case 9:
									Say("Portowa mocna...i od razu człowiek szczęśliwy... he he");
									Emote("*Śmieje się rubasznie*");
									break;
								case 10:
									Say("A niech to... znów zabraknie mi nia gorzałkę...");
									break;
								case 11:
									Say("Nie zapijam... Nie chce się truć");
									break;
								case 12:
									Emote("*Smarka w brudną chustę*");
									break;
								case 13:
									Say("Kiedyś to było...");
									Emote("*Wzdycha ciężko*");
									break;
								case 14:
									Emote("*Poprawia łachmany*");
									break;
								case 15:
									Say(
										"Będziemy pić! To prosta sprawa! Jesteśmy w Tasandorze a tu nie pić nie wypada!");
									Emote("*Wydziera się*");
									break;
								case 16:
									Emote("*Podnosi coś z ziemi*");
									Say("Ha! Będzie na kolejkę!");
									break;
								case 17:
									Say("Kurrrrwa... Zajumali mi sakiewkę");
									Emote("*Rozgląda się powoli wzdychając*");
									break;
							}
						}
						else if (Race == Race.NKrasnolud)
						{
							switch (Utility.Random(18))
							{
								case 0:
									Say("Palę, piję i żyje!");
									break;
								case 1:
									Say("Powiadają, że mój dziad pił prosto z beczki jak z kufla!");
									break;
								case 2:
									Emote("*Przechyla butlę oblewając całą brodę*");
									Say("O żesz kurwa...");
									break;
								case 3:
									Say(
										"Był roz taki tydzień, oko mnie bolało, wypiłech żech se piwko, od razu przestało!");
									Emote("*Klaszcze w rytm melodii*");
									break;
								case 4:
									Say(
										"Jo jest spod Aegis, miołech tam kuzyna, borok nie pił piwa, tera chłopa ni ma!");
									break;
								case 5:
									Say(
										"Jarling nie rozumie nas, gdy pijemy wódkę, Tamael chciałby tak jak my, ale z marnym skutkiem!");
									Emote("*Przyśpiewuje do flaszeczki*");
									break;
								case 6:
									Say("To jest Krasnoludzka brać, lubi dużo pić i spać!");
									break;
								case 7:
									Say("To ma być piwo?! Smakuje jak szczyny trola!");
									Emote("*Spluwa na ziemie*");
									break;
								case 8:
									Say("Smoki?! Pierdolone szkodniki... Zatłukbym ale mam piwo do dopicia...");
									Emote("*Unosi butlę*");
									break;
								case 9:
									Emote("*Wydobywa z siebie przeraźliwe berknięcie*");
									break;
								case 10:
									Say("Karzeł?! Jak Ci upierdole nogi sam będziesz karzeł chuju...");
									Emote("*Przechyla butlę oblewając się trunkiem*");
									break;
								case 11:
									Emote("*Popala tytoń z fajki*");
									break;
								case 12:
									Emote("*Odchrząkuje i spluwa gęstą wydzieliną*");
									break;
								case 13:
									Say("Kiedyś to było...");
									Emote("*Wzdycha ciężko*");
									break;
								case 14:
									Emote("*Poprawia brodę wytrzepując z niej okruchy*");
									break;
								case 15:
									Say("Krasnoludzki browar... tego trzeba w tym zapchlonym mieście");
									break;
								case 16:
									Emote("*Podnosi coś z ziemi*");
									Say("Kurwa... a już myslałem");
									break;
								case 17:
									Say("No i czego sie kurwa gapisz?");
									Emote("*Rozgląda się powoli wzdychając*");
									break;
							}
						}

						m_Spoken = DateTime.Now;
					}
				}
			}
		}

		[Constructable]
		public XmlQuestNPCPijak(int gender) : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.8, 3.0)
		{
			SetStr(100, 300);
			SetDex(100, 300);
			SetInt(100, 300);

			Fame = 100;
			Karma = -500;

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
				Title = "- Pijaczka";
				Item hat = null;
				switch (Utility.Random(5))
				{
					case 0:
						hat = new FeatheredHat(GetRandomHue());
						break;
					case 1:
						hat = new Bandana(GetRandomHue());
						break;
					case 2:
						hat = new SkullCap(GetRandomHue());
						break;
					case 3:
						hat = new Cap(GetRandomHue());
						break;
					case 4:
						hat = null;
						break;
				}

				AddItem(hat);

				Item pants = null;
				switch (Utility.Random(4))
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
					case 3:
						pants = new Kilt(GetRandomHue());
						break;
				}

				AddItem(pants);

				Item shirt = null;
				switch (Utility.Random(3))
				{
					case 0:
						shirt = new Shirt(GetRandomHue());
						break;
					case 1:
						shirt = new FancyShirt(GetRandomHue());
						break;
					case 2:
						shirt = new Robe(GetRandomHue());
						break;
				}

				AddItem(shirt);
			}
			else
			{
				this.Body = 0x190;
				this.Name = NameList.RandomName("male");
				Title = "- Pijak";
				Item hat = null;
				switch (Utility.Random(7))
				{
					case 0:
						hat = new SkullCap(GetRandomHue());
						break;
					case 1:
						hat = new FeatheredHat(GetRandomHue());
						break;
					case 2:
						hat = new Bonnet(GetRandomHue());
						break;
					case 3:
						hat = new Cap(GetRandomHue());
						break;
					case 4:
						hat = new Bandana(GetRandomHue());
						break;
					case 5:
						hat = new FloppyHat(GetRandomHue());
						break;
					case 6:
						hat = null;
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
				switch (Utility.Random(5))
				{
					case 0:
						shirt = new Doublet(GetRandomHue());
						break;
					case 1:
						shirt = new FancyShirt(GetRandomHue());
						break;
					case 2:
						shirt = new Shirt(GetRandomHue());
						break;
					case 3:
						shirt = new Robe(GetRandomHue());
						break;
					case 4:
						shirt = null;
						break;
				}

				AddItem(shirt);
			}

			Item feet = null;
			switch (Utility.Random(3))
			{
				case 0:
					feet = new Sandals(Utility.RandomNeutralHue());
					break;
				case 1:
					feet = new Shoes(Utility.RandomNeutralHue());
					break;
				case 2:
					feet = null;
					break;
			}

			AddItem(feet);
			Container pack = new Backpack();

			pack.Movable = false;

			AddItem(pack);
		}

		public XmlQuestNPCPijak(Serial serial) : base(serial)
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
