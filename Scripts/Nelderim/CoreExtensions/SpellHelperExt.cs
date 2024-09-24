using Server.Regions;

namespace Server.Spells
{
	public partial class SpellHelper
	{
		private static bool[,] _NRules = {
			/*				    InDungeonRegion */
			/* Recall From */ { false },
			/* Recall To */   { false },
			/* Gate From */   { false },
			/* Gate To */     { false },
			/* Mark In */     { false },
			/* Tele From */   {  true },
			/* Tele To */     {  true }
		};
        
		private static TravelValidator[] _NValidators = {
			InDungeonRegion
		};

		private static bool InDungeonRegion(Map map, Point3D loc)
		{
			foreach (var region in map.Regions.Values)
			{
				if (region is DungeonRegion && region.Contains(loc))
					return true;
			}
			return false;
		}
	}
}
