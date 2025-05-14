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
		
		private static string GetName(int powerLevel)
		{
			return powerLevel switch
			{
				105 => "Wspanialy Zwoj (105 umiejetnosci)",
				110 => "Idealny Zwoj (110 umiejetnosci)",
				115 => "Mityczny Zwoj (115 umiejetnosci)",
				120 => "Legendarny Zwoj (120 umiejetnosci)",
				_ => "Invalid item"
			};
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
			private static readonly List<SkillName> _AllowedSkills =
				PowerScroll.Skills.Where(s => !Utility.CraftSkills.Contains(s)).ToList();

			private readonly int _PowerLevel;
			private readonly IEntity _DisplayEntity;
			
			public PowerScrollBuyInfo(int powerLevel, int amount) : base (GetBuyPrice(powerLevel), amount, 0x14F0, 0x481)
			{
				_PowerLevel = powerLevel;
				CreateCallback = InitializePS;
				
				_DisplayEntity = new Item
				{
					ItemID = 0x14F0,
					Hue = 0x481,
					Name = GetName(_PowerLevel)
				};
			}

			private void InitializePS(PowerScroll powerScroll, GenericBuyInfo buyInfo)
			{
				powerScroll.Value = _PowerLevel;
				powerScroll.Skill = Utility.RandomList(_AllowedSkills);
			}
			
			public override IEntity GetDisplayEntity()
			{
				return _DisplayEntity;
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
