#region References

using System;
using System.Collections.Generic;
using Server.Items;
using Server.Nelderim;

#endregion

namespace Server.Engines.Harvest
{
	public partial class HarvestDefinition
	{
		private readonly Dictionary<string, HarvestVein[]> m_RegionVeinCache = new Dictionary<string, HarvestVein[]>();
		public Type RegionType { get; set; }

		public virtual void GetRegionVeins(out HarvestVein[] veins, Map map, int x, int y)
		{
			// domsyslna lista surowcow wystepujacych na mapie:
			veins = null;

			if (RegionType == null)
			{
				return;
			}

			Point3D p = new Point3D(x, y, 4);
			Region here = Region.Find(p, map);
			if (here == null)
			{
				return;
			}

			Region harvestReg = here.GetRegion(RegionType);
			if ( harvestReg?.Name != null )
			{
				if (m_RegionVeinCache.TryGetValue(harvestReg.Name, out var regionVeins))
				{
					veins = regionVeins;
				}
				else
				{
					var factors = NelderimRegionSystem.GetRegion(harvestReg.Name).ResourceVeins();
					if (factors != null && factors.Count > 0)
					{
						veins = VeinsFromRegionFactors(factors);
					}

					// caching veins for this region
					m_RegionVeinCache.Add(harvestReg.Name, veins);
				}
			}
		}

		public virtual HarvestVein[] VeinsFromRegionFactors( Dictionary<CraftResource, double> factors )
        {
            if ( factors.Count == 0 )
            {
                return null;
            }
            
            HarvestVein[] veins = new HarvestVein[Resources.Length];
            
            for (var i = 0; i < Resources.Length; i++)
            {
	            var craftResource = CraftResources.GetFromType(Resources[i].Types[0]);
	            if (factors.TryGetValue(craftResource, out var factor))
	            {
		            veins[i] = new HarvestVein(factor, 0.0, Resources[i], i == 0 ? null : Resources[0]);
	            }
            }
            return veins;
        }
	}
}
