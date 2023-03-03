using System;
using Server.Engines.Craft;

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
	}
}
