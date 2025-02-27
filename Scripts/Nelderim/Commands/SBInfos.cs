#region References

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Server.Engines.Craft;
using Server.Items;
using Server.Mobiles;

#endregion

namespace Server.Commands
{
	public class SBInfosCommand
	{
		public static void Initialize()
		{
			CommandSystem.Register("SBInfos", AccessLevel.Administrator, SBInfos_OnCommand);
			CommandSystem.Register("CraftInfos", AccessLevel.Administrator, CraftInfos_OnCommand);
		}

		[Usage("SBInfos")]
		[Description("Wyswietla informacje o cenach NPC.")]
		public static void SBInfos_OnCommand(CommandEventArgs e)
		{
			Mobile m = e.Mobile;
			SBInfosAnalyzer analyzer = new SBInfosAnalyzer(m);
		}

		[Usage("CraftInfos")]
		[Description("Wyswietla informacje o kosztach produkcji i zyskach sprzedazy przedmiotow.")]
		public static void CraftInfos_OnCommand(CommandEventArgs e)
		{
			List<CraftSystem> systems = [];
			systems.Add(DefAlchemy.CraftSystem);
			systems.Add(DefBlacksmithy.CraftSystem);
			systems.Add(DefBowFletching.CraftSystem);
			systems.Add(DefCarpentry.CraftSystem);
			systems.Add(DefCartography.CraftSystem);
			systems.Add(DefCooking.CraftSystem);
			systems.Add(DefGlassblowing.CraftSystem);
			systems.Add(DefInscription.CraftSystem);
			systems.Add(DefMasonry.CraftSystem);
			systems.Add(DefTailoring.CraftSystem);
			systems.Add(DefTinkering.CraftSystem);

			string LogsDirectory = "Logs/SBInfos";
			DateTime now = DateTime.Now;
			string fileName = String.Format("craft {0}-{1}-{2} {3}-{4}-{5}", now.Year, now.Month, now.Day, now.Hour,
				now.Minute, now.Second);
			string filePath = Path.Combine(Core.BaseDirectory, LogsDirectory + "/" + fileName + ".csv");

			if (!Directory.Exists(LogsDirectory))
				Directory.CreateDirectory(LogsDirectory);

			FileLogger logger = new FileLogger(filePath, true);

			foreach (CraftSystem sys in systems)
			{
				Dictionary<Type, SBCraftDeal> typeUnitPrice = [];
				Dictionary<Type, int> typeAmount = [];

				foreach (CraftItem item in sys.CraftItems)
				{
					CraftRes res;
					if (item.UseSubRes2 && item.Resources.Count > 1)
						res = item.Resources.GetAt(1);
					else
						res = item.Resources.GetAt(0);

					Type product = item.ItemType;
					Type resType = res.ItemType;

					int resourceAmount = res.Amount;

					if (typeAmount.ContainsKey(resType) && typeAmount[resType] != resourceAmount)
					{
						// produkt mozna pozyskac na kilka sposobow
						// rozniacych sie iloscia potrzebnego materialu

						// TODO: log albo implementacja szacowania obu sposobow
						break;
					}

					double maxUnitPrice = 0.0;
					int maxPrice = 0;
					SBInfo bestSB = null;

					// sposrod wszystkich SB znajdz najlepsza cene za biezacy produkt
					foreach (SBInfo sb in SBInfosAnalyzer.SBInfos)
					{
						GenericSellInfo sellInfo = sb.SellInfo as GenericSellInfo;
						if (sellInfo != null && sellInfo.IsInList(product))
						{
							int price = sellInfo.Table[product];
							double unitPrice = price / (double)resourceAmount;

							if (unitPrice > maxUnitPrice)
							{
								maxPrice = price;
								maxUnitPrice = unitPrice;
								bestSB = sb;
							}
						}
					}

					// jesli produkt mozna sprzedac
					if (maxPrice > 0)
					{
						if (!typeUnitPrice.ContainsKey(resType) ||
						    maxUnitPrice > (typeUnitPrice[resType]).UnitPrice)
							typeUnitPrice[resType] = new SBCraftDeal(sys, bestSB, product, maxUnitPrice);
					}
				}


				// Posortowane w danym rzemiosle.

				foreach (var en in typeUnitPrice)
				{
					Type resType = en.Key;
					Type product = (en.Value).Product;
					double unitPrice = (en.Value).UnitPrice;
					SBInfo sb = (en.Value).SBInfo;

					if (resType != typeof(IronIngot) && resType != typeof(Log) && resType != typeof(Leather) &&
					    resType != typeof(Cloth))
						continue;

					logger.WriteLine("\t{0}\t{1}\t{2}\t{3}\t{4}", resType, unitPrice, product, sys, sb);
				}

				logger.WriteLine("-----------");
			}

			e.Mobile.SendMessage(0x15, "CraftInfos - Log zapisany w {0}", LogsDirectory);
		}
	}

	public class SBCraftDeal
	{
		public double UnitPrice { get; }
		public CraftSystem System { get; }
		public Type Product { get; }
		public SBInfo SBInfo { get; }

		public SBCraftDeal(CraftSystem sys, SBInfo sb, Type itemType, double unitPrice)
		{
			Product = itemType;
			System = sys;
			UnitPrice = unitPrice;
			SBInfo = sb;
		}
	}


	// SellInfo = za ile mozemy sprzedac NPC'owi
	// BuyInfo  = za ile mozemy kupic od NPC
	public class SBInfosAnalyzer
	{
		public static List<SBInfo> SBInfos = new List<SBInfo>();

		public static string LogsDirectory = "Logs/SBInfos";
		private static FileLogger m_Logger;

		private readonly Mobile m_Mobile;
		private readonly Dictionary<Type, SBInfoItem> m_SBInfoItems;

		public SBInfosAnalyzer(Mobile m)
		{
			IEnumerable<Type> types = Assembly.GetAssembly(typeof(SBInfo)).GetTypes().Where(myType =>
				myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(SBInfo)));
			foreach (Type type in types)
			{
				try
				{
					SBInfos.Add((SBInfo)type.GetConstructor(new Type[0]).Invoke(new object[0]));
				}
				catch
				{
				}
			}

			m_Mobile = m;
			m_SBInfoItems = new Dictionary<Type, SBInfoItem>();

			if (!Directory.Exists(LogsDirectory))
				Directory.CreateDirectory(LogsDirectory);

			string filename = Path.Combine(Core.BaseDirectory, LogsDirectory + "/" + FileName + ".log");

			m_Logger = new FileLogger(filename, true);
			m_Logger.NewLine = "---------------------";

			Load();

			CheckCashBugs();

			LogPrices();

			m.SendMessage(0x15, "Log zostal zapisany do {0}", LogsDirectory);
		}

		public string FileName
		{
			get
			{
				DateTime now = DateTime.Now;
				return String.Format("{0}_{1}-{2}-{3} {4}-{5}-{6}", m_Mobile.Account, now.Year, now.Month, now.Day,
					now.Hour, now.Minute, now.Second);
			}
		}

		public void LogPrices()
		{
			m_Logger.WriteLine("ITEMS:");

			foreach (Type type in m_SBInfoItems.Keys)
			{
				m_Logger.WriteLine("");
				m_Logger.WriteLine(m_Logger.NewLine);
				m_Logger.WriteLine(type);
				m_Logger.WriteLine(m_Logger.NewLine);

				m_Logger.WriteLine("Buy info:");
				List<SBInfoRecord> buyInfos = m_SBInfoItems[type].BuyInfos;
				foreach (SBInfoRecord r in buyInfos)
					m_Logger.WriteLine("{0} - {1}", r.Price, r.Npc);

				m_Logger.WriteLine("Sell info:");
				List<SBInfoRecord> sellInfos = m_SBInfoItems[type].SellInfos;
				foreach (SBInfoRecord r in sellInfos)
					m_Logger.WriteLine("{0} - {1}", r.Price, r.Npc);
			}

			m_Logger.WriteLine(m_Logger.NewLine);
		}

		public void CheckCashBugs()
		{
			m_Logger.WriteLine(m_Logger.NewLine);
			m_Logger.WriteLine("CASH BUGS:");
			m_Logger.WriteLine(m_Logger.NewLine);

			foreach (Type type in m_SBInfoItems.Keys)
			{
				SBInfoRecord buy = GetLowestBuyPriceRecord(type);
				SBInfoRecord sell = GetHighestSellPriceRecord(type);

				if (buy == null || sell == null)
					continue;

				if (sell.Price > buy.Price)
				{
					int profit = sell.Price - buy.Price;
					m_Logger.WriteLine("{0} - zysk: {1}", type, profit);
					m_Logger.WriteLine("Kupujesz u {0} za {1}", buy.Npc, buy.Price);
					m_Logger.WriteLine("Sprzedajesz u {0} za {1}", sell.Npc, sell.Price);
					m_Logger.WriteLine(m_Logger.NewLine);
				}
			}
		}

		public SBInfoRecord GetHighestSellPriceRecord(Type type)
		{
			SBInfoRecord record = null;
			SBInfoItem sbInfoItem = m_SBInfoItems[type];

			foreach (SBInfoRecord r in sbInfoItem.SellInfos)
			{
				if (record == null || r.Price > record.Price)
					record = r;
			}

			return record;
		}

		public SBInfoRecord GetLowestBuyPriceRecord(Type type)
		{
			SBInfoRecord record = null;
			SBInfoItem sbInfoItem = m_SBInfoItems[type];

			foreach (SBInfoRecord r in sbInfoItem.BuyInfos)
			{
				if (record == null || r.Price < record.Price)
					record = r;
			}

			return record;
		}

		public void Load()
		{
			int buyCnt = 0, sellCnt = 0;

			foreach (SBInfo info in SBInfos)
			{
				foreach (var buy in info.BuyInfo)
				{
					if (Add(buy.Type, new SBInfoRecord(buy.Type, buy.Price, info), true))
						buyCnt++;
				}

				if (info.SellInfo is GenericSellInfo sell)
				{

					foreach (KeyValuePair<Type, int> pair in sell.Table)
					{
						Type type = pair.Key;
						int price = pair.Value;

						if (Add(type, new SBInfoRecord(type, price, info), false))
							sellCnt++;
					}
				}
				else
				{
					Console.WriteLine($"Unable to process sellinfo for {info.GetType().Name}");
				}
			}

			m_Logger.WriteLine("{0}: {1} items loaded ({2} BuyInfos, {3} SellInfos)", m_Mobile.Name,
				m_SBInfoItems.Count, buyCnt, sellCnt);
		}

		public bool Add(Type type, SBInfoRecord sbInfoRecord, bool buy)
		{
			if (type == null)
				return false;

			if (!m_SBInfoItems.ContainsKey(type))
				m_SBInfoItems.Add(type, new SBInfoItem());

			SBInfoItem sbInfoItem = m_SBInfoItems[type];

			if (buy)
				sbInfoItem.BuyInfos.Add(sbInfoRecord);
			else
				sbInfoItem.SellInfos.Add(sbInfoRecord);

			return true;
		}
	}

	public class SBInfoItem
	{
		public SBInfoItem()
		{
			BuyInfos = new List<SBInfoRecord>();
			SellInfos = new List<SBInfoRecord>();
		}

		public List<SBInfoRecord> BuyInfos { get; set; }

		public List<SBInfoRecord> SellInfos { get; set; }
	}

	public class SBInfoRecord
	{
		public SBInfoRecord(Type type, int price, SBInfo npc)
		{
			Npc = npc;
			Price = price;
		}

		public SBInfo Npc { get; set; }

		public int Price { get; set; }

		public Type Type { get; set; }
	}
}
