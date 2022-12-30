using System;

namespace Server.Engines.Craft
{
	public partial class CraftSystem
	{
		public void AddByproduct(int index, Type type, int amount)
		{
			CraftItem craftItem = CraftItems.GetAt(index);
			craftItem.AddByproduct(type, amount);
		}
	}
}
