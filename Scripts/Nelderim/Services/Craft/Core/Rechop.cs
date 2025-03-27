using System;
using Server.Items;
using Server.Targeting;

namespace Server.Engines.Craft
{
	public class Rechop
	{
		public static void Do(Mobile from, CraftSystem craftSystem, ITool tool)
		{
			int num = craftSystem.CanCraft(from, tool, null);

			if (num > 0 && num != 1044267)
			{
				from.SendGump(new CraftGump(from, craftSystem, tool, num));
			}
			else
			{
				from.Target = new InternalTarget(craftSystem, tool);
				from.SendLocalizedMessage(1044273); // Target an item to recycle.
			}
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
				int num = m_CraftSystem.CanCraft(from, m_Tool, null);

				if (num > 0)
				{
					from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, num));
				}
				else
				{
					SmeltResult result = SmeltResult.Invalid;
					bool isStoreBought = false;
					object message;

					if (targeted is IQuality iq)
					{
						isStoreBought = !iq.PlayerConstructed;
					}

					if (targeted is Item item)
					{
						result = Apply(from, item);
					}

					switch (result)
					{
						default:
						case SmeltResult.Invalid:
							message = "Nie mozesz odzyskac z tego drewna";
							break; 
						case SmeltResult.NoSkill:
							message = "Nie masz pojęcia jak pracować z tym drewnem";
							break; 
						case SmeltResult.Success:
							message = isStoreBought ? 500418 : 1044270;
							break; 
					}

					from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, message));
				}
			}

			private SmeltResult Apply(Mobile from, Item item)
			{
				if (item is not IResource ir)
				{
					return SmeltResult.Invalid;
				}
				try
				{
					var resource = ir.Resource;
					if (CraftResources.GetType(resource) != CraftResourceType.Wood)
						return SmeltResult.Invalid;

					CraftResourceInfo info = CraftResources.GetInfo(resource);

					if (info == null || info.ResourceTypes.Length == 0)
						return SmeltResult.Invalid;

					CraftItem craftItem = m_CraftSystem.CraftItems.SearchFor(item.GetType());

					if (craftItem == null || craftItem.Resources.Count == 0)
						return SmeltResult.Invalid;

					CraftRes craftResource = craftItem.Resources.GetAt(0);

					if (craftResource.Amount < 2)
						return SmeltResult.Invalid; // Not enough metal to resmelt

					Type resourceType = info.ResourceTypes[0];
					
					var subResource = m_CraftSystem.CraftSubRes.SearchFor(resourceType);

					double difficulty = subResource.RequiredSkill;

					double skill = Math.Max(from.Skills[m_CraftSystem.MainSkill].Value,
						from.Skills[SkillName.Lumberjacking].Value);

					if (difficulty > skill)
						return SmeltResult.NoSkill;

					Item resultResource = (Item)Activator.CreateInstance(resourceType);

					if (item is IQuality iq && iq.PlayerConstructed)
						resultResource.Amount = (int)(craftResource.Amount * .66);
					else
						resultResource.Amount = 1;

					item.Delete();
					from.AddToBackpack(resultResource);

					from.PlaySound(0x13E);
					return SmeltResult.Success;
				}
				catch (Exception e)
				{
					Diagnostics.ExceptionLogging.LogException(e);
				}

				return SmeltResult.Invalid;
			}
		}
	}
}
