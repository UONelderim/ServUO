using System;
using Nelderim.Configuration;
using Server.Engines.Craft;
using Server.Network;

namespace Server.Items
{
	public partial class Spellbook : IScissorable
	{
		public virtual bool Scissor(Mobile from, Scissors scissors)
		{
			if (Deleted || !from.CanSee(this))
				return false;
			
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack.
				return false;
			}

			CraftSystem system = DefInscription.CraftSystem;

			CraftItem item = system.CraftItems.SearchFor(GetType());

			if (item != null && item.Resources.Count == 1 && item.Resources.GetAt(0).Amount >= 2)
			{
				var resource = item.Resources.GetAt(0);
				var resourceItem = (Item)Activator.CreateInstance(resource.ItemType);
				base.ScissorHelper(from, resourceItem, resource.Amount / 2);
				return true;
			}

			from.SendLocalizedMessage(502440); // Scissors can not be used on that to produce anything.
			return false;
		}

		public void OnSequenceFinished()
		{
			if (m_MaxHitPoints == 0)
				return;
            
			double baseChance = NConfig.Durability.Enabled ? NConfig.Durability.SpellBookLossChance : 0.25;
			double chance = NegativeAttributes.Antique > 0 ? baseChance * 2 : baseChance;

			if (chance >= Utility.RandomDouble()) // 25% chance to lower durability
			{
				if (m_HitPoints >= 1)
				{
					HitPoints--;
				}
				else if (m_MaxHitPoints > 0)
				{
					MaxHitPoints--;

					if (Parent is Mobile)
						((Mobile)Parent).LocalOverheadMessage(MessageType.Regular, 0x3B2, 1061121); // Your equipment is severely damaged.

					if (m_MaxHitPoints == 0)
					{
						Delete();
					}
				}
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			if (m_AosAttributes.IsEmpty && m_AosSkillBonuses.IsEmpty)
			{
				LootType = LootType.Blessed;
			}
			else
			{
				LootType = LootType.Regular;
			}
		}
	}
}
