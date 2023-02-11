#region References

using System;
using System.Collections.Generic;
using System.Linq;
using Server.Engines.Craft;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class CraftSB : SBInfo
	{
		public static SBInfo Weaponsmith = new CraftSB(DefBlacksmithy.CraftSystem,
			new List<Type> { typeof(BaseWeapon), typeof(BaseShield)},
			new List<Type> { typeof(DragonBardingDeed),  typeof(Shuriken) }
		);

		public static SBInfo Armorer = new CraftSB(DefBlacksmithy.CraftSystem,
			new List<Type> { typeof(BaseArmor) },
			new List<Type> { typeof(DragonBardingDeed), typeof(BaseShield) }
		);

		public static SBInfo Tinekerer = new CraftSB(DefTinkering.CraftSystem,
			new List<Type>(),
			new List<Type>()
		);

		public static SBInfo Carpenter = new CraftSB(DefCarpentry.CraftSystem,
			new List<Type>(),
			new List<Type> { typeof(BaseInstrument) }
		);

		public static SBInfo Fletcher = new CraftSB(DefBowFletching.CraftSystem,
			new List<Type> { typeof(BaseWeapon) },
			new List<Type>()
		);

		public static SBInfo LeatherWorker = new CraftSB(DefTailoring.CraftSystem,
			new List<Type>(),
			new List<Type> { typeof(BaseClothing) }
		);

		public static SBInfo Tailor = new CraftSB(DefTailoring.CraftSystem,
			new List<Type>(),
			new List<Type> { typeof(BaseArmor) }
		);

		private GenericSellInfo m_SellInfo;

		private readonly CraftSystem m_CraftSystem;
		private readonly List<Type> m_Include;
		private readonly List<Type> m_Exclude;
		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo { get; } = new List<IBuyItemInfo>();

		private CraftSB(CraftSystem system, List<Type> include, List<Type> exclude)
		{
			m_CraftSystem = system;
			m_Include = include;
			m_Exclude = exclude;
			SellInit();
		}

		private int CalculatePrice(CraftItem item)
		{
			CraftRes res;

			if (item.UseSubRes2 && item.Resources.Count > 1)
				res = item.Resources.GetAt(1);
			else
				res = item.Resources.GetAt(0);

			int amount = res.Amount;
			double unitPrice = 1.0;

			if (res.ItemType == typeof(Log))
			{
				if (m_CraftSystem == DefCarpentry.CraftSystem) // SeedBox: 50 klod
					unitPrice = 2.50;
				else if (m_CraftSystem == DefBowFletching.CraftSystem) // crossbow: 7 klod
					unitPrice = 3.50;
				else if (m_CraftSystem == DefTinkering.CraftSystem) // ClockFrame: 6 klod
					unitPrice = 1.50; // majster ma zarabiac metalem :P
				else
					unitPrice = 2.00;
			}
			else if (res.ItemType == typeof(IronIngot))
			{
				if (m_CraftSystem == DefTinkering.CraftSystem) // kula do boli: 10 sztab
					unitPrice = 3.33;
				else if (m_CraftSystem == DefBlacksmithy.CraftSystem) // 20 lub 750*95% (smocza zbroja)
					unitPrice = 3.33;
				else
					unitPrice = 2.00;
			}
			else if (res.ItemType == typeof(Leather))
			{
				if (m_CraftSystem == DefTailoring.CraftSystem) // StuddedXxxx: 14 skor
					unitPrice = 4.75;
				else if (m_CraftSystem == DefBowFletching.CraftSystem) // BowstringLeather: masowo
					unitPrice = 2.00;
				else
					unitPrice = 2.00;
			}
			else if (res.ItemType == typeof(Cloth))
			{
				unitPrice = 1.50;
			}

			return (int)(amount * unitPrice);
		}

		private void SellInit()
		{
			m_SellInfo = new GenericSellInfo();

			if (m_Include.Count > 0)
			{
				foreach (var includeType in m_Include)
				{
					if (m_Exclude.Any(exclude => includeType.IsSubclassOf(exclude) || includeType == exclude))
						continue;

					var item = m_CraftSystem.CraftItems.SearchForSubclass(includeType);
					if (item != null)
					{
						m_SellInfo.Add(item.ItemType, CalculatePrice(item));
					}
				}
			}
			else
			{
				foreach (CraftItem item in m_CraftSystem.CraftItems)
				{
					if (m_Exclude.Any(exclude => item.ItemType.IsSubclassOf(exclude) || item.ItemType == exclude))
						continue;

					m_SellInfo.Add(item.ItemType, CalculatePrice(item));
				}
			}
		}
	}
}
