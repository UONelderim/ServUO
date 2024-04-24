using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;

namespace Server.Mobiles
{
	public class SBPowerScrollTrader : SBInfo
	{
		public override IShopSellInfo SellInfo => new InternalSellInfo();
		public override List<IBuyItemInfo> BuyInfo => new InternalBuyInfo();

		private static int BasePrice => 10;
		
		private static int GetBuyPrice(double value)
		{
			return BasePrice * PowerScrollPrice(value) * 2;
		}
		
		private static int PowerScrollPrice(double psValue)
		{
			return psValue switch
			{
				105 => 1,
				110 => 1 * 8,
				115 => 1 * 8 * 6,
				120 => 1 * 8 * 6 * 4,
				_ => 60000
			};
		}

		public class PowerScrollBuyInfo : GenericBuyInfo<PowerScroll>
		{
			private static readonly List<SkillName> allowedSkills =
				PowerScroll.Skills.Where(s => !Utility.CraftSkills.Contains(s)).ToList();

			private int _powerLevel { get; }
			
			public PowerScrollBuyInfo(int powerLevel, int amount) : base (GetBuyPrice(powerLevel), amount, 0x14F0, 0x481)
			{
				_powerLevel = powerLevel;
				CreateCallback = InitializePS;
			}

			private void InitializePS(PowerScroll powerScroll, GenericBuyInfo buyInfo)
			{
				powerScroll.Value = _powerLevel;
				powerScroll.Skill = Utility.RandomList(allowedSkills);
			}
		}
		
		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add( new PowerScrollBuyInfo(105, 50));
				Add( new PowerScrollBuyInfo(110, 40));
				Add( new PowerScrollBuyInfo(115, 30));
				Add( new PowerScrollBuyInfo(120, 20));
			}
		}

		public class InternalSellInfo : IShopSellInfo
		{
			private Dictionary<Type, int> m_Table = new Dictionary<Type, int>();
			private Type[] m_Types;
			
			public InternalSellInfo()
			{
				Add(typeof(PowerScroll), 1);
			}
			
			public void Add( Type type, int price )
			{
				m_Table[type] = price;
				m_Types = null;
			}
			
			public string GetNameFor(IEntity e)
			{
				if (e.Name != null)
					return e.Name;

				if (e is Item item)
					return item.LabelNumber.ToString();

				return e.GetType().Name;
			}

			public int GetSellPriceFor(IEntity item)
			{
				if (item is PowerScroll ps)
				{
					return BasePrice * PowerScrollPrice(ps.Value);
				}

				return 1;
			}

			public int GetSellPriceFor(IEntity item, IVendor vendor)
			{
				return GetSellPriceFor(item);
			}

			public int GetBuyPriceFor(IEntity item)
			{
				return 1;
			}

			public int GetBuyPriceFor(IEntity item, IVendor vendor)
			{
				return GetBuyPriceFor(item);
			}

			public bool IsSellable(IEntity item)
			{
				return m_Table.ContainsKey(item.GetType());
			}

			public Type[] Types
			{
				get
				{
					if (m_Types == null)
					{
						m_Types = new Type[m_Table.Keys.Count];
						m_Table.Keys.CopyTo(m_Types, 0);
					}

					return m_Types;
				}
			}

			public bool IsResellable(IEntity item)
			{
				return false;
			}

			public void OnSold(Mobile seller, IVendor vendor, IEntity item, int amount)
			{
				EventSink.InvokeValidVendorSell(new ValidVendorSellEventArgs(seller, vendor, item, GetSellPriceFor(item, vendor)));
			}
		}
	}
}
