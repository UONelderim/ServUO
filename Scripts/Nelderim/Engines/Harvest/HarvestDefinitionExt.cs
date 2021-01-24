using Server.Nelderim;
using System;
using System.Collections.Generic;

namespace Server.Engines.Harvest
{
    public partial class HarvestDefinition
    {

        private Type m_RegionType;
        private Dictionary<string, HarvestVein[]> m_RegionVeinCache = new Dictionary<string, HarvestVein[]>();
        public Type RegionType { get { return m_RegionType; } set { m_RegionType = value; } }

        public virtual void GetRegionVeins( out HarvestVein[] veins, Map map, int x, int y )
        {
            // domsyslna lista surowcow wystepujacych na mapie:
            veins = null;

            if ( m_RegionType == null )
            {
                return;
            }

            Point3D p = new Point3D( x, y, 4 );
            Region here = Region.Find( p, map );
            if ( here == null )
            {
                return;
            }

            Region harvestReg = here.GetRegion( m_RegionType );
            if ( harvestReg != null && harvestReg.Name != null )
            {
                if ( m_RegionVeinCache.ContainsKey( harvestReg.Name ) )
                {
                    // use cached veins for this region
                    veins = m_RegionVeinCache[harvestReg.Name];
                    return;
                }
                else
                {
                    List<double> factors;
                    RegionsEngine.GetResourceVeins( harvestReg.Name, out factors );
                    if ( factors != null && factors.Count > 0 )
                    {
                        veins = VeinsFromRegionFactors( factors );
                    }

                    // caching veins for this region
                    m_RegionVeinCache.Add( harvestReg.Name, veins );
                }
            }
        }

        public virtual HarvestVein[] VeinsFromRegionFactors( List<double> factors )
        {
            if ( factors.Count == 0 )
            {
                return null;
            }

            HarvestVein[] veins = new HarvestVein[factors.Count];

            veins[0] = new HarvestVein( factors[0], 0.0, Resources[0], null );
            for ( int i = 1; i < factors.Count; i++ )
            {
                if ( Resources.Length - 1 < i )
                    break;
                veins[i] = new HarvestVein( factors[i], 0.0, Resources[i], null /*m_Resources[i-1]*/ );
            }

            return veins;
        }
    }
}
