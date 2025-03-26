using System;
using System.Collections.Generic;
using System.Linq;
using Server.Mobiles;

namespace Server.Items
{
	public abstract partial class PeerlessAltar
	{
		public static List<PeerlessAltar> Altars = [];
		
		public virtual string[] _Regions => [];
		public virtual double _KeyDropChance => 0.01;

		public static void AddLoot(BaseCreature bc)
		{
			if (bc?.Region?.Name == null)
				return;
			
			foreach (var altar in Altars)
			{
				if (altar._Regions.Any(r => bc.Region.Name.Contains(r)))
				{
					if (Utility.RandomDouble() < altar._KeyDropChance)
					{
						var type = Utility.RandomList(altar.Keys);
						bc.AddLoot(LootPack.LootItemCallback(_ => Activator.CreateInstance(type) as Item));
					}
					break;
				}
			}
		}
	}
}
