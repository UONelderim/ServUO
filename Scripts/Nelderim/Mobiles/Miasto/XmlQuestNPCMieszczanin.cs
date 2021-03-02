using System;
using Server.Items;

namespace Server.Mobiles
{
	public class XmlQuestNPCMieszczanin : TalkingBaseCreature
	{
		[Constructable]
		public XmlQuestNPCMieszczanin() : this(-1)
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
							switch (Utility.Random(32))
							{
								case 0:
									Say("Dobre czasy nastały.");
									break;
								case 1:
									Say("On i tak nie był tego wart.");
									break;
								case 2:
									Say("Psia krew...");
									break;
								case 3:
									Say("I co Ci do tego...");
									break;
								case 4:
									Say("Miłego dnia!");
									break;
								case 5:
									Say("Muszę kupic nowe meble...");
									break;
								case 6:
									Say("Ulice wreszcie czyste, jak zimna żytnia!");
									break;
								case 7:
									Say("Chwała wiekiej Tasandorze!");
									break;
								case 8:
									Say("Podatki, podatki, więcej podatków. A płaca ta sama.");
									break;
								case 9:
									Say("Ta straż ciągle tu tylko węszy.");
									break;
								case 10:
									Say("Cholera... Znów braknie mi na opłaty...");
									break;
								case 11:
									Say("Na Bogów... Co to?!");
									break;
								case 12:
									Say("Najważniejsze, że nic złego się nie stało.");
									break;
								case 13:
									Say("Teraz to dopiero będzie!");
									Emote("*uśmiecha się delikatnie*");
									break;
								case 14:
									Say("Już będzie tylko lepiej...");
									break;
								case 15:
									Say(
										"Sąsiad cholerny nowego konia sobie kupił... Ciekawe skąd ma na to pieniądze...");
									break;
								case 16:
									Say("Tasandora dla Tamaeli!");
									break;
								case 17:
									Say("Cholipka...");
									Emote("*Rozgląda się powoli wzdychając*");
									break;
								case 18:
									Say("Przynieś, podaj, zanieś a co ja kurwa jestem...");
									Emote("*Zagryza zęby w złości*");
									break;
								case 19:
									Say("Tych Krasnali to powinni wypierdzielić na zbity pysk... Ochlejmordy cholerne");
									break;
								case 20:
									Say("Ten generał to wspaniały cżłowiek, przywódzca narodu.");
									break;
								case 21:
									Emote("*Śmieje sie rubasznie*");
									break;
								case 22:
									Emote("*Śpiewa pieśniczkę rozglądając sie w koło*");
									break;
								case 23:
									Emote("*Nuci pod nosem*");
									break;
								case 24:
									Emote("*Gwiżdże machając do kogoś*");
									break;
								case 25:
									Say("No lepiej nie będzie... Jakoś trzeba żyć...");
									break;
								case 26:
									Say("Pięknie się żyje... lepiej być nie może.");
									break;
								case 27:
									Say("Uważaj jak chodzisz!");
									Emote("*Wygraża sie pięścią*");
									break;
								case 28:
									Say("A kim Ty kurwa jesteś że dajesz mi rade co..?");
									break;
								case 29:
									Say("Cóż za brak klasy i obycia...");
									Emote("*Prycha*");
									break;
								case 30:
									Say("Odejdź... Nie mam teraz czasu");
									break;
								case 31:
									Say("Krasnoludy jeszcze da sie przeżyć, ale te jarlińskie ścierwa...");
									Emote("*Rzuca kamienień w złości*");
									break;
								case 32:
									Say("Generał to może chuj... ale patriota");
									break;
								case 33:
									Say("Niech wypieprzają do siebie... a nie smrodzą nasze piękne miasto...");
									break;
							}
						}
						else if (Race == Race.NJarling)
						{
							switch (Utility.Random(31))
							{
								case 0:
									Emote("*Zerka w swe kieszenie*");
									Say("No i tu też zaczynają wiać... tylko, że pustki");
									break;
								case 1:
									Emote("*Podrzuca mały sztylecik w dłoni*");
									break;
								case 2:
									Say("Mam nadzieję, że jutro będzie lepsza pogoda.");
									break;
								case 3:
									Emote("*Patrzy podejrzliwie*");
									break;
								case 4:
									Say("Parszywe szczury...");
									break;
								case 5:
									Emote("*Mamrocze pod nosem*");
									Say("...a ja i tak wiem, że to wina Krasnoludów...");
									break;
								case 6:
									Emote("*Wrzeszczy*");
									Say("Rządamy niższych podatków!");
									break;
								case 7:
									Say(
										"Śnieg, mróz i srogie zimy... dziadek mi o nich opowiadał. Geriador usłany był płatkami śniegu.");
									break;
								case 8:
									Emote("*Parska*");
									Say("...i to niby ma nam pomóc? Jak?!");
									break;
								case 9:
									Say("Tamaele? A chuj im w dupę...");
									break;
								case 10:
									Emote("*Spluwa*");
									break;
								case 11:
									Emote("*Donośnie beka*");
									break;
								case 12:
									Emote("*Uśmiecha się*");
									Say("To będzie dobry dzień.");
									break;
								case 13:
									Emote("*Płacze*");
									break;
								case 14:
									Say("Oby tylko straż się nie interesowała...");
									break;
								case 15:
									Say("A niech to... więcej nie gram z nimi w kości.");
									break;
								case 16:
									Emote("*Spogląda w niebo*");
									Say("O Bogowie... Dajcie mi siłę, by nie zabić tego Tamaelskiego smroda...");
									break;
								case 17:
									Say(
										"Zjadłoby się zupę w karczmie... wypiłoby się z kamratami... ale nie ma z kim... ");
									break;
								case 18:
									Say("Jak nazywa się kompot z pasem? ....kompas... he he");
									Emote("*Śmieje się rubasznie*");
									break;
								case 19:
									Emote("*Drapie się po głowie*");
									break;
								case 20:
									Emote("*Sięga po sakiewkę na ziemi*");
									Say("A to, teraz...należy już do mnie.");
									break;
								case 21:
									Say("Ty Psi synu...");
									break;
								case 22:
									Emote("*Przeciąga się*");
									break;
								case 23:
									Say("A skąd ja mam niby to wiedzieć?");
									break;
								case 24:
									Say("Żaden Jarling nie powinien się tu zapuszczać. Tu jest zbyt gorąco.");
									break;
								case 25:
									Say("Śmierć Krasnoludom, śmierć Tamaelom");
									break;
								case 26:
									Emote("*Podrzuca złotą monetę*");
									break;
								case 27:
									Emote("*Wybucha śmiechem*");
									break;
								case 28:
									Say(
										"...i wtedy ja mu mówię 'Lepiej się cofnij'... i wiesz co? Cofnął się... Głupi Krasnal...");
									break;
								case 29:
									Emote("*Wydmuchuje nos*");
									break;
								case 30:
									Say(
										"...zioła... zaklęcia ...i.. tłuczek... miecza nie ma na tej liście? Jak ja mam niby walczyć?!");
									break;
							}
						}
						else if (Race == Race.NKrasnolud)
						{
							switch (Utility.Random(32))
							{
								case 0:
									Say("Kolejny dzień bez smoków...a to już coś");
									Emote("*Uśmiecha sie radnośnie*");
									break;
								case 1:
									Say("Napiłoby sie piwa z kamratami...");
									break;
								case 2:
									Emote("*Spogląda na niebo*");
									Say("Karwasz barabasz...");
									break;
								case 3:
									Say("Wina, wina, wina dajcie! A jak umre pochowajcie!");
									Emote("*Śpiewa tańcząc i bawiąc się przy tym*");
									break;
								case 4:
									Say("Panie, ześlij mnie jeno buteleczke spirytusu");
									Emote("*Składa ręce ku niebiosom*");
									break;
								case 5:
									Say("Jak leziesz kurwisynu!");
									break;
								case 6:
									Say("Krasnal?! Który  to powiedział?! Cholerne wielkoludy!");
									Emote("*Drze się na całą okolicę*");
									break;
								case 7:
									Say("Hmm... Pojedzone, tera to by sie coś zjadło");
									Emote("*Klepie się po brzuchu*");
									break;
								case 8:
									Emote("*Obraca w palcach złotą monetę*");
									break;
								case 9:
									Emote("*Kopie w kamienie*");
									break;
								case 10:
									Say("Smoki?! Gdzie smoki?!");
									Emote("*Rozgląda się nerwowo*");
									break;
								case 11:
									Say("Jak sie żyje stary pryku heee?");
									Emote("*Śmieje się głośno*");
									break;
								case 12:
									Say("Dej mi spokój, nie mam teraz czasu...");
									break;
								case 13:
									Say("Dam słowo, że jeszcze wczoraj był pełen");
									Emote("*Potrząsa pustym miechem*");
									break;
								case 14:
									Say("Nie zadzieraj... Nie wiesz z kim tańczysz");
									break;
								case 15:
									Say("Zadumane dupki, pieprzeni Tamaelowie...");
									break;
								case 16:
									Say("Mnie to sie w sumie tutej podoba... Tanie piwo i dziewoje");
									Emote("*Śmieje się rubasznie*");
									break;
								case 17:
									Say("Żeby życie miało smaczek... raz wódeczka, raz koniaczek");
									Emote("*Otwiera butle przychlając do dna*");
									break;
								case 18:
									Say("Czym by tu się zająć... Eh stara bieda");
									break;
								case 19:
									Say("Nie chce mi sie gadać... Idź już sobie");
									break;
								case 20:
									Say(
										"Ci strażnicy to by mogli zacząć łapać kogo trzeba, a nie biednych mieszkańców męczą");
									break;
								case 21:
									Say(
										"Póki Krasnoludy nie zaczną rządzić tym wypizdowiem to będzie tu ciągle te same gówno...");
									break;
								case 22:
									Say("A cholera by to wzięła... Kurza twarz...");
									Emote("*Klnie siarczyście*");
									break;
								case 23:
									Say("A idź do diabła pieprzony tamelski kurwisynu...");
									break;
								case 24:
									Emote("*Klnie pod nosem*");
									break;
								case 25:
									Emote("*Grzebie w sakwie*");
									Say("Ktoś sie nie bał i zajebał...");
									break;
								case 26:
									Say("Dzień jak codzień... Nic ciekawego");
									break;
								case 27:
									Say("Pytasz co słychać?");
									Emote("*Śmieje się gromko*");
									Say("...Stare kurwy nie chcą zdychać");
									break;
								case 28:
									Say("Była tam, przysięgam, bestia wielka jak dąb... no albo sosna");
									break;
								case 29:
									Say("Bez nas... to by te całe rycerzyki z gołymi dupskami biegały!");
									break;
								case 30:
									Emote("*Roztacza wokół zapach górskiego trola*");
									Say("Cholerny kapuśniak... ugh");
									break;
								case 31:
									Say("Problem... Dobre sobie, w czarnej dupie kurwa jesteśmy a nie mamy problem...");
									break;
							}
						}

						m_Spoken = DateTime.Now;
					}
				}
			}
		}

		[Constructable]
		public XmlQuestNPCMieszczanin(int gender) : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.8, 3.0)
		{
			SetStr(100, 300);
			SetDex(100, 300);
			SetInt(100, 300);

			Fame = 5000;
			Karma = 3000;

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
				Title = "- Mieszczanka";
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
						hat = new WideBrimHat(GetRandomHue());
						break;
					case 4:
						hat = new FloppyHat(GetRandomHue());
						break;
					case 5:
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
						pants = new ElvenPants(GetRandomHue());
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
						shirt = new Tunic(GetRandomHue());
						break;
					case 2:
						shirt = new FancyDress(GetRandomHue());
						break;
					case 3:
						shirt = new FancyShirt(GetRandomHue());
						break;
					case 4:
						shirt = new ElvenDarkShirt(GetRandomHue());
						break;
					case 5:
						shirt = new ElvenShirt(GetRandomHue());
						break;
				}

				AddItem(shirt);
			}
			else
			{
				this.Body = 0x190;
				this.Name = NameList.RandomName("male");
				Title = "- Mieszczanin";
				Item hat = null;
				switch (Utility.Random(6)) //6 hats, one empty, for no hat
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
						hat = new WideBrimHat(GetRandomHue());
						break;
					case 4:
						hat = new FloppyHat(GetRandomHue());
						break;
					case 5:
						hat = null;
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
						pants = new ElvenPants(GetRandomHue());
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
						shirt = new Tunic(GetRandomHue());
						break;
					case 3:
						shirt = new FancyShirt(GetRandomHue());
						break;
					case 4:
						shirt = new Shirt(GetRandomHue());
						break;
					case 5:
						shirt = new ElvenDarkShirt(GetRandomHue());
						break;
				}

				AddItem(shirt);

				if (Utility.RandomBool())
				{
					AddItem(new Cloak(GetRandomHue()));
				}
			}

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
				case 3:
					feet = new ThighBoots(Utility.RandomNeutralHue());
					break;
			}

			AddItem(feet);
			Container pack = new Backpack();

			pack.Movable = false;

			AddItem(pack);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Poor);
		}

		public XmlQuestNPCMieszczanin(Serial serial) : base(serial)
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

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
