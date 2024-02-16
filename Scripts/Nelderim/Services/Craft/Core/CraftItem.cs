using System;
using System.Collections.Generic;
using Nelderim;

namespace Server.Engines.Craft
{
	public partial class CraftItem
	{
		public class ByproductInfo
		{
			Type m_Type;
			int m_Amount;

			public ByproductInfo(Type type, int amount)
			{
				m_Type = type;
				m_Amount = amount;
			}

			public Type Type => m_Type;
			public int Amount => m_Amount;
		};
		
		public void AddByproduct(Type type, int amount)
		{
			Byproducts ??= new List<ByproductInfo>();

			Byproducts.Add(new ByproductInfo(type, amount));
		}

		public List<ByproductInfo> Byproducts { get; private set; }
		
		private void CreateByproducts(Mobile from)
		{
			if (Byproducts == null)
				return;

			foreach (var bpi in Byproducts)
			{
				Item item = Activator.CreateInstance(bpi.Type) as Item;
				if (item == null)
					continue;

				if (item.Stackable)
					item.Amount = bpi.Amount;
				else
					for (var i = 0; i < bpi.Amount - 1; ++i)
					{
						Item remainingItem = Activator.CreateInstance(bpi.Type) as Item;
						if (remainingItem != null)
							from.AddToBackpack(remainingItem);
					}

				LabelsConfig.AddCreationMark(item, from);
				if (from.IsStaff())
				{
					LabelsConfig.AddTamperingMark(item, from);
				}

				from.AddToBackpack(item);
			}
		}
	}
}
