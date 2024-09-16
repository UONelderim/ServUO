using System;
using Server.Engines.BulkOrders;
using Server.Items;
using Server.Targeting;

namespace Server.Engines.Craft
{
	public static class RecycleBook
	{
		public static void Do(Mobile from, CraftSystem craftSystem, ITool tool)
		{
			from.Target = new InternalTarget(craftSystem, tool);
			from.SendLocalizedMessage(1044273); // Target an item to recycle.
		}

		private class InternalTarget : Target
		{
			private readonly CraftSystem m_CraftSystem;
			private readonly ITool m_Tool;

			public InternalTarget(CraftSystem craftSystem, ITool tool)
				: base(2, false, TargetFlags.None)
			{
				m_CraftSystem = craftSystem;
				m_Tool = tool;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				SmeltResult result = SmeltResult.Invalid;

				if (targeted is Item item && IsRecyclable(item))
				{
					result = Rewrite(from, item);
				}

				var message = result switch
				{
					SmeltResult.Invalid => "Nie mozesz tego pociac",
					SmeltResult.NoSkill => "Nie potrafisz tego zrobic",
					SmeltResult.Success => "Pociales przedmiot odzyskujac troche zwojow"
				};

				from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, message));
			}
			
			private bool IsRecyclable(Item targeted)
			{
				var itemType = targeted.GetType();
				return itemType == typeof(Spellbook) || itemType == typeof(Runebook) || itemType == typeof(BulkOrderBook);
			}

			private SmeltResult Rewrite(Mobile from, Item item)
			{
				try
				{
					var scroll = GetResourceItem(item, out var result);
					if (scroll != null)
					{
						item.Delete();
						from.AddToBackpack(scroll);
						from.PlaySound(0x248);
					}

					return result;
				}
				catch (Exception e)
				{
					Diagnostics.ExceptionLogging.LogException(e);
				}

				return SmeltResult.Invalid;
			}

			private Item GetResourceItem(Item item, out SmeltResult result)
			{
				int resourceProductionAmount = GetResourceAmountForProduct(item.GetType());
				if (resourceProductionAmount < 2)
				{
					result = SmeltResult.Invalid;
					return null;
				}

				result = SmeltResult.Success;
				var amount = Math.Max(1, resourceProductionAmount / 2);
				return new BlankScroll(amount);
			}

			private int GetResourceAmountForProduct(Type productType)
			{
				CraftItem craftItem = m_CraftSystem.CraftItems.SearchFor(productType);

				if (craftItem == null)
					return 0;

				CraftRes craftResource = craftItem.Resources.GetAt(0);

				if (craftResource.ItemType != typeof(BlankScroll))
					return 0;

				return craftResource.Amount;
			}
		}
	}
}
