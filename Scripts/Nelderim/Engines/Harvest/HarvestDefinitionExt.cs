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
				if (m_RegionVeinCache.TryGetValue(harvestReg.Name, out var cachedRegionVeins))
				{
					veins = cachedRegionVeins;
				}
				else
				{
					var factors = NelderimRegionSystem.GetRegion(harvestReg.Name).ResourceVeins();
					var regionVeins = VeinsFromRegionFactors(factors);
					if (regionVeins != null && regionVeins.Length > 0)
						veins = regionVeins;

					// caching veins for this region
					m_RegionVeinCache.Add(harvestReg.Name, veins);
				}
			}
		}

		public virtual HarvestVein[] VeinsFromRegionFactors( Dictionary<CraftResource, double> factors )
        {
	        if (factors == null || factors.Count == 0)
	        {
		        return null;
	        }

	        var veins = new List<HarvestVein>();
	        for (var i = 0; i < Resources.Length; i++)
	        {
		        var craftResource = CraftResources.GetFromType(Resources[i].Types[0]);
		        if (factors.TryGetValue(craftResource, out var factor))
		        {
			        veins.Add(new HarvestVein(factor, 0.0, Resources[i], i == 0 ? null : Resources[0]));
		        }
	        }
	        return veins.ToArray();
        }
	}
}
