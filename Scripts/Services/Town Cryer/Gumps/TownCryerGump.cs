using System;
using System.Collections.Generic;
using System.Linq;

using Server.Engines.CityLoyalty;
using Server.Engines.Quests;
using Server.Guilds;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Services.TownCryer
{
	public class TownCryerGump : BaseTownCryerGump
	{
		public enum GumpCategory
		{
			News = 1,
			EventModerator,
			City,
			Guild
		}

		private City _City;

		public GumpCategory Category { get; set; }
		public int Page { get; set; }
		public int Pages { get; set; }

		public City City
		{
			get => _City;
			set
			{
				var city = _City;

				if (city != value)
				{
					_City = value;
					SetDefaultCity();
				}
			}
		}

		public static Dictionary<PlayerMobile, City> LastCity { get; private set; }

		public TownCryerGump(PlayerMobile pm, TownCrier tc, int page = 0, GumpCategory Cartegory = GumpCategory.News)
			: base(pm, tc)
		{
			Page = page;
			Category = GumpCategory.News;

			SetDefaultCity();
		}

		public override void AddGumpLayout()
		{
			base.AddGumpLayout();

			switch (Category)
			{
				case GumpCategory.News:
					BuildNewsPage();
					break;
				case GumpCategory.EventModerator:
					BuildEMPage();
					break;
				case GumpCategory.City:
					BuildCityPage();
					break;
				case GumpCategory.Guild:
					BuildGuildPage();
					break;
			}

			// AddButton(275, 598, Category == GumpCategory.News ? 0x5F6 : 0x5F5, 0x5F6, 1, GumpButtonType.Reply, 0);
			// AddButton(355, 598, Category == GumpCategory.EventModerator ? 0x5F4 : 0x5F3, 0x5F4, 2, GumpButtonType.Reply, 0);
			// AddButton(435, 598, Category == GumpCategory.City ? 0x5F8 : 0x5F7, 0x5F8, 3, GumpButtonType.Reply, 0);
			// AddButton(515, 598, Category == GumpCategory.Guild ? 0x5F2 : 0x5F1, 0x5F2, 4, GumpButtonType.Reply, 0);
		}

		private void BuildNewsPage()
		{
			var perPage = 20;

			var y = 170;
			var start = Page * perPage;

			Pages = (int)Math.Ceiling(TownCryerSystem.NewsEntries.Count / (double)perPage);

			for (var i = start; i < TownCryerSystem.NewsEntries.Count && i < perPage; i++)
			{
				var entry = TownCryerSystem.NewsEntries[i];

				AddButton(50, y, 0x5FB, 0x5FC, 100 + i, GumpButtonType.Reply, 0);
				var doneQuest = entry.QuestType != null && QuestHelper.CheckDoneOnce(User, entry.QuestType, Cryer, false);

				if (entry.Title.Number > 0)
				{
					AddHtmlLocalized(87, y, 700, 20, entry.Title.Number, doneQuest ? C32216(0x696969) : 0, false, false);
				}
				else
				{
					AddLabelCropped(87, y, 700, 20, doneQuest ? 0x3B2 : 0, entry.Title);
				}

				y += 23;
			}

			if (TownCryerSystem.NewsEntries.Count > perPage)
			{
				AddButton(350, 570, 0x605, 0x606, 5, GumpButtonType.Reply, 0);
				AddButton(380, 570, 0x609, 0x60A, 6, GumpButtonType.Reply, 0);
				AddButton(430, 570, 0x607, 0x608, 7, GumpButtonType.Reply, 0);
				AddButton(455, 570, 0x603, 0x604, 8, GumpButtonType.Reply, 0);

				AddHtml(395, 570, 35, 20, Center($"{Page + 1}/{Pages + 1}"), false, false);
			}
		}

		private void BuildEMPage()
		{
			var perPage = 15;
			AddPage(1);
			var y = 170;

			for (var i = 0; i < TownCryerSystem.ModeratorEntries.Count && i < perPage; i++)
			{
				var entry = TownCryerSystem.ModeratorEntries[i];

				AddButton(50, y, 0x5FB, 0x5FC, 200 + i, GumpButtonType.Reply, 0);
				AddLabelCropped(87, y, 631, 20, 0, entry.Title);

				if (User.AccessLevel > AccessLevel.Player) // Couselors+ can moderate events
				{
					AddButton(735, y, 0x5FD, 0x5FE, 2000 + i, GumpButtonType.Reply, 0);
					AddButton(760, y, 0x5FF, 0x600, 2500 + i, GumpButtonType.Reply, 0);
				}

				y += 23;
			}

			AddButton(320, 525, 0x627, 0x628, 9, GumpButtonType.Reply, 0);
		}

		private void BuildCityPage()
		{
			AddButton(233, 150, City == City.Twierdza ? 0x5E5 : 0x5E4, City == City.Twierdza ? 0x5E5 : 0x5E4, 10, GumpButtonType.Reply, 0);
			AddTooltip(CityLoyaltySystem.GetCityLocalization(City.Twierdza));

			AddButton(280, 150, City == City.Tirassa ? 0x5E7 : 0x5E6, City == City.Tirassa ? 0x5E7 : 0x5E6, 11, GumpButtonType.Reply, 0);
			AddTooltip(CityLoyaltySystem.GetCityLocalization(City.Tirassa));

			AddButton(327, 150, City == City.Orod ? 0x5E5 : 0x5E4, City == City.Orod ? 0x5E5 : 0x5E4, 12, GumpButtonType.Reply, 0);
			AddTooltip(CityLoyaltySystem.GetCityLocalization(City.Orod));

			AddButton(374, 150, City == City.Tasandora ? 0x5E3 : 0x5E2, City == City.Tasandora ? 0x5E3 : 0x5E2, 13, GumpButtonType.Reply, 0);
			AddTooltip(CityLoyaltySystem.GetCityLocalization(City.Tasandora));

			AddButton(418, 150, City == City.ArtTrader ? 0x5DD : 0x5DC, City == City.ArtTrader ? 0x5DD : 0x5DC, 14, GumpButtonType.Reply, 0);
			AddTooltip(CityLoyaltySystem.GetCityLocalization(City.ArtTrader));

			AddButton(463, 150, City == City.Garlan ? 0x5DF : 0x5DE, City == City.Garlan ? 0x5DF : 0x5DE, 15, GumpButtonType.Reply, 0);
			AddTooltip(CityLoyaltySystem.GetCityLocalization(City.Garlan));

			AddButton(509, 150, City == City.Lotharn ? 0x5E1 : 0x5E0, City == City.Lotharn ? 0x5E1 : 0x5E0, 16, GumpButtonType.Reply, 0);
			AddTooltip(CityLoyaltySystem.GetCityLocalization(City.Lotharn));

			AddButton(555, 150, City == City.Celendir ? 0x5ED : 0x5ED, City == City.Celendir ? 0x5ED : 0x5EC, 17, GumpButtonType.Reply, 0);
			AddTooltip(CityLoyaltySystem.GetCityLocalization(City.Celendir));

			AddButton(601, 150, City == City.LDelmah ? 0x5E9 : 0x5E8, City == City.LDelmah ? 0x5E9 : 0x5E8, 18, GumpButtonType.Reply, 0);
			AddTooltip(CityLoyaltySystem.GetCityLocalization(City.LDelmah));

			AddHtmlLocalized(0, 260, 854, 20, CenterLoc, $"#{TownCryerSystem.GetCityLoc(City)}", 0, false, false); // The Latest News from the City of ~1_CITY~

			var y = 300;

			for (var i = 0; i < TownCryerSystem.CityEntries.Count && i < TownCryerSystem.MaxPerCityGovernorEntries; i++)
			{
				var entry = TownCryerSystem.CityEntries[i];

				if (entry.City != City)
				{
					continue;
				}

				AddButton(50, y, 0x5FB, 0x5FC, 300 + i, GumpButtonType.Reply, 0);
				AddLabelCropped(87, y, 700, 20, 0, entry.Title);

				var city = CityLoyaltySystem.GetCitizenship(User, false);

				if ((city != null && city.Governor == User) || User.AccessLevel >= AccessLevel.GameMaster) // Only Governors
				{
					AddButton(735, y, 0x5FD, 0x5FE, 3000 + i, GumpButtonType.Reply, 0);
					AddButton(760, y, 0x5FF, 0x600, 3500 + i, GumpButtonType.Reply, 0);
				}

				y += 23;
			}

			AddImage(230, 460, 0x5F0);
		}

		private void BuildGuildPage()
		{
			var list = TownCryerSystem.GuildEntries.OrderBy(e => e.EventTime).ToList();

			var perPage = 20;
			var y = 170;

			var start = Page * perPage;
			var guild = User.Guild as Guild;

			Pages = (int)Math.Ceiling(list.Count / (double)perPage);

			for (var i = start; i < list.Count && i < perPage; i++)
			{
				var entry = TownCryerSystem.GuildEntries[i];

				AddButton(50, y, 0x5FB, 0x5FC, 400 + TownCryerSystem.GuildEntries.IndexOf(entry), GumpButtonType.Reply, 0);

				AddLabelCropped(87, y, 700, 20, 0, entry.FullTitle);

				if ((guild == entry.Guild && User.GuildRank != null && User.GuildRank.Rank >= 3) || User.AccessLevel >= AccessLevel.GameMaster) // Only warlords+
				{
					var index = TownCryerSystem.GuildEntries.IndexOf(entry);
					AddButton(735, y, 0x5FD, 0x5FE, 4000 + index, GumpButtonType.Reply, 0);
					AddButton(760, y, 0x5FF, 0x600, 4500 + index, GumpButtonType.Reply, 0);
				}

				y += 23;
			}

			AddButton(350, 570, 0x605, 0x606, 5, GumpButtonType.Reply, 0);
			AddButton(380, 570, 0x609, 0x60A, 6, GumpButtonType.Reply, 0);
			AddButton(430, 570, 0x607, 0x608, 7, GumpButtonType.Reply, 0);
			AddButton(455, 570, 0x603, 0x604, 8, GumpButtonType.Reply, 0);

			AddHtml(395, 570, 35, 20, Center($"{Page + 1}/{Pages + 1}"), false, false);
		}

		private void SetDefaultCity()
		{
			if (LastCity == null || !LastCity.ContainsKey(User))
			{
				LastCity = new Dictionary<PlayerMobile, City>();

				var system = CityLoyaltySystem.GetCitizenship(User, false);

				if (system != null)
				{
					LastCity[User] = system.City;
				}
			}
			else
			{
				LastCity[User] = _City;
			}
		}

		public override void OnResponse(RelayInfo info)
		{
			var id = info.ButtonID;

			switch (id)
			{
				case 0: break;
				case 1:
				case 2:
				case 3:
				case 4:
				{
					Category = (GumpCategory)id;
					Refresh();
					break;
				}
				case 5: // <<
				{
					Page = 0;
					Refresh();
					break;
				}
				case 6: // <
				{
					Page = Math.Max(0, Page - 1);
					Refresh();
					break;
				}
				case 7: // >
				{
					Page = Math.Min(Pages, Page + 1);
					Refresh();
					break;
				}
				case 8: // >>
				{
					Page = Pages;
					Refresh();
					break;
				}
				case 9: // Learn More - EM Page
				{
					User.LaunchBrowser(TownCryerSystem.EMEventsPage);
					Refresh();
					break;
				}
				case 10:
				{
					City = City.Twierdza;
					Refresh();
					break;
				}
				case 11:
				{
					City = City.Tirassa;
					Refresh();
					break;
				}
				case 12:
				{
					City = City.Orod;
					Refresh();
					break;
				}
				case 13:
				{
					City = City.Tasandora;
					Refresh();
					break;
				}
				case 14:
				{
					City = City.ArtTrader;
					Refresh();
					break;
				}
				case 15:
				{
					City = City.Garlan;
					Refresh();
					break;
				}
				case 16:
				{
					City = City.Lotharn;
					Refresh();
					break;
				}
				case 17:
				{
					City = City.Celendir;
					Refresh();
					break;
				}
				case 18:
				{
					City = City.LDelmah;
					Refresh();
					break;
				}
				default:
				{
					if (id < 200)
					{
						id -= 100;

						if (id >= 0 && id < TownCryerSystem.NewsEntries.Count)
						{
							_ = SendGump(new TownCryerNewsGump(User, Cryer, TownCryerSystem.NewsEntries[id]));
						}
					}
					else if (id < 300)
					{
						id -= 200;

						if (id >= 0 && id < TownCryerSystem.ModeratorEntries.Count)
						{
							_ = SendGump(new TownCryerEventModeratorGump(User, Cryer, TownCryerSystem.ModeratorEntries[id]));
						}
					}
					else if (id < 400)
					{
						id -= 300;

						if (id >= 0 && id < TownCryerSystem.CityEntries.Count)
						{
							_ = SendGump(new TownCryerCityGump(User, Cryer, TownCryerSystem.CityEntries[id]));
						}
					}
					else if (id < 600)
					{
						id -= 400;

						if (id >= 0 && id < TownCryerSystem.GuildEntries.Count)
						{
							_ = SendGump(new TownCryerGuildGump(User, Cryer, TownCryerSystem.GuildEntries[id]));
						}
					}
					else if (id < 3000)
					{
						if (id < 2500)
						{
							id -= 2000;

							if (id >= 0 && id < TownCryerSystem.ModeratorEntries.Count)
							{
								_ = SendGump(new CreateEMEntryGump(User, Cryer, TownCryerSystem.ModeratorEntries[id]));
							}
						}
						else
						{
							id -= 2500;

							if (id >= 0 && id < TownCryerSystem.ModeratorEntries.Count)
							{
								TownCryerSystem.ModeratorEntries.RemoveAt(id);
							}

							Refresh();
						}
					}
					else if (id < 4000)
					{
						var city = CityLoyaltySystem.GetCitizenship(User, false);

						if ((city != null && city.Governor == User) || User.AccessLevel >= AccessLevel.GameMaster) // Only Governors
						{
							if (id < 3500)
							{
								id -= 3000;

								if (id >= 0 && id < TownCryerSystem.CityEntries.Count)
								{
									_ = SendGump(new CreateCityEntryGump(User, Cryer, City, TownCryerSystem.CityEntries[id]));
								}
							}
							else
							{
								id -= 3500;

								if (id >= 0 && id < TownCryerSystem.CityEntries.Count)
								{
									TownCryerSystem.CityEntries.RemoveAt(id);
								}
							}
						}

						Refresh();
					}
					else if (id < 5000)
					{
						if (id < 4500)
						{
							id -= 4000;

							if (id >= 0 && id < TownCryerSystem.GuildEntries.Count)
							{
								_ = SendGump(new CreateGuildEntryGump(User, Cryer, TownCryerSystem.GuildEntries[id]));
							}
						}
						else
						{
							id -= 4500;

							if (id >= 0 && id < TownCryerSystem.GuildEntries.Count)
							{
								TownCryerSystem.GuildEntries.RemoveAt(id);
							}

							Refresh();
						}
					}
				}

				break;
			}
		}
	}
}
