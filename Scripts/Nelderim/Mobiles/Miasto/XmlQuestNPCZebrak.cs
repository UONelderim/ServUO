#region References

using System;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class XmlQuestNPCZebrak : TalkingBaseCreature
	{
		[Constructable]
		public XmlQuestNPCZebrak() : this(-1)
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
									Say("Złota daj żebrakowi...");
									break;
								case 1:
									Say("O Panocku o licu złotym, zlituj sie");
									break;
								case 2:
									Say("Psia mać...");
									break;
								case 3:
									Say("Choć okrucha chleba... błagam");
									Emote("*Składa ręce błagalnie*");
									break;
								case 4:
									Say("Mam chorą córkę...");
									break;
								case 5:
									Say("Wszystko mi zabrali... Wszystko...");
									Emote("*Szlocha*");
									break;
								case 6:
									Say("Czy Ty przypadkiem ode mnie nie pożyczałeś pieniędzy?");
									break;
								case 7:
									Say("Poratujcie w potrzebie...");
									break;
								case 8:
									Say("Kiedyś to sie żylo...");
									Emote("*Wzdycha ze smutkiem*");
									break;
								case 9:
									Say("Ta straż ciągle tu tylko węszy.");
									break;
								case 10:
									Say("Za co ja dzieciaki wyżywie... Poratujcie błagam");
									break;
								case 11:
									Say("Na chleb jeno...");
									break;
								case 12:
									Emote("*Smarka w brudną chustę*");
									break;
								case 13:
									Say("Kiedyś to było...");
									Emote("Wzdycha ciężko");
									break;
								case 14:
									Emote("*Poprawia łachmany*");
									break;
								case 15:
									Say("Było sie młodym i głupim... Tak wyszło...");
									break;
								case 16:
									Emote("*Podnosi coś z ziemi*");
									Say("Ah.. jednak nie");
									break;
								case 17:
									Say("Cholipka...");
									Emote("*Rozgląda się powoli wzdychając*");
									break;
								case 18:
									Say("Wszystko co miałem dla kraju oddałem...");
									break;
							}
						}
						else if (Race == Race.NJarling)
						{
							switch (Utility.Random(18))
							{
								case 0:
									Say("Złota daj żebrakowi...");
									break;
								case 1:
									Say("O Panocku o licu złotym, zlituj sie");
									break;
								case 2:
									Say("Psia mać...");
									break;
								case 3:
									Say("Choć okrucha chleba... błagam");
									Emote("*Składa ręce błagalnie*");
									break;
								case 4:
									Say("Mam chorą córkę...");
									break;
								case 5:
									Say("Wszystko mi zabrali... Wszystko...");
									Emote("*Szlocha*");
									break;
								case 6:
									Say("Czy Ty przypadkiem ode mnie nie pożyczałeś pieniędzy?");
									break;
								case 7:
									Say("Poratujcie w potrzebie...");
									break;
								case 8:
									Say("Kiedyś to sie żylo...");
									Emote("*Wzdycha ze smutkiem*");
									break;
								case 9:
									Say("Ta straż ciągle tu tylko węszy.");
									break;
								case 10:
									Say("Za co ja dzieciaki wyżywie... Poratujcie błagam");
									break;
								case 11:
									Say("Na chleb jeno...");
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
									Say("Było sie młodym i głupim... Tak wyszło...");
									break;
								case 16:
									Emote("*Podnosi coś z ziemi*");
									Say("Ah.. jednak nie");
									break;
								case 17:
									Say("Cholipka...");
									Emote("*Rozgląda się powoli wzdychając*");
									break;
								case 18:
									Say("Aż sie tu nie chce żyć... parszywe miasto");
									break;
							}
						}
						else if (Race == Race.NKrasnolud)
						{
							switch (Utility.Random(18))
							{
								case 0:
									Say("Złota daj żebrakowi...");
									break;
								case 1:
									Say("O Panocku o licu złotym, zlituj sie");
									break;
								case 2:
									Say("Psia mać...");
									break;
								case 3:
									Say("Choć okrucha chleba... błagam");
									Emote("*Składa ręce błagalnie*");
									break;
								case 4:
									Say("Mam chorą córkę...");
									break;
								case 5:
									Say("Wszystko mi zabrali... Wszystko...");
									Emote("*Szlocha*");
									break;
								case 6:
									Say("Czy Ty przypadkiem ode mnie nie pożyczałeś pieniędzy?");
									break;
								case 7:
									Say("Poratujcie w potrzebie...");
									break;
								case 8:
									Say("Kiedyś to sie żylo...");
									Emote("*Wzdycha ze smutkiem*");
									break;
								case 9:
									Say("Ta straż ciągle tu tylko węszy.");
									break;
								case 10:
									Say("Za co ja dzieciaki wyżywie... Poratujcie błagam");
									break;
								case 11:
									Say("Na chleb jeno...");
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
									Say("Było sie młodym i głupim... Tak wyszło...");
									break;
								case 16:
									Emote("*Podnosi coś z ziemi*");
									Say("Ah.. jednak nie");
									break;
								case 17:
									Say("Cholipka...");
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
		public XmlQuestNPCZebrak(int gender) : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.8, 3.0)
		{
			SetStr(100, 300);
			SetDex(100, 300);
			SetInt(100, 300);

			Fame = 0;
			Karma = 300;

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
				Title = "- Żebraczka";
				Item hat = null;
				switch (Utility.Random(2))
				{
					case 0:
						hat = new SkullCap(GetRandomHue());
						break;
					case 1:
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
				Title = "- Żebrak";
				Item hat = null;
				switch (Utility.Random(2))
				{
					case 0:
						hat = new SkullCap(GetRandomHue());
						break;
					case 1:
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
				switch (Utility.Random(4))
				{
					case 0:
						shirt = new FancyShirt(GetRandomHue());
						break;
					case 1:
						shirt = new Shirt(GetRandomHue());
						break;
					case 2:
						shirt = new Robe(GetRandomHue());
						break;
					case 3:
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

		public XmlQuestNPCZebrak(Serial serial) : base(serial)
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
