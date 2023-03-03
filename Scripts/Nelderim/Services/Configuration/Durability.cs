// **********
// ServUO - Durability.cs
// **********

using static Server.Config;

namespace Nelderim.Config
{
	public class Durability
	{
		internal Durability() { }

		public bool Enabled => Get("NelderimDurability.Enabled", true);
		public double BaseArmorLossChance => Get("NelderimDurability.BaseArmorLossChance", 0.01);
		public double BaseClothingLossChance => Get("NelderimDurability.BaseClothingLossChance", 0.01);
		public double BaseJewelLossChance => Get("NelderimDurability.BaseJewelLossChance", 0.01);
		public double BaseQuiverLossChance => Get("NelderimDurability.BaseQuiverLossChance", 0.01);
		public double BaseTalismanLossChance => Get("NelderimDurability.BaseTalismanLossChance", 0.01);
		public double BaseWeaponLossChance => Get("NelderimDurability.BaseWeaponLossChance", 0.01);
		public double SpellBookLossChance => Get("NelderimDurability.SpellBookLossChance", 0.01);
	}
}
