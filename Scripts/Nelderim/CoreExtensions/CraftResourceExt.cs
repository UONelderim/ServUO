namespace Server.Items
{
	public partial class CraftResources
	{
		public static int GetNameSuffixNumber(CraftResource res)
		{
			return res switch
			{
				CraftResource.DullCopper => 1053108,
				CraftResource.ShadowIron => 1053107,
				CraftResource.Copper => 1053106,
				CraftResource.Bronze => 1053105,
				CraftResource.Gold => 1053104,
				CraftResource.Agapite => 1053103,
				CraftResource.Verite => 1053102,
				CraftResource.Valorite => 1053101,
				CraftResource.Platinum => 1097280,
				CraftResource.SpinedLeather => 1061118,
				CraftResource.HornedLeather => 1061117,
				CraftResource.BarbedLeather => 1061116,
				CraftResource.RedScales => 1061049,
				CraftResource.YellowScales => 1061050,
				CraftResource.BlackScales => 1061051,
				CraftResource.GreenScales => 1061052,
				CraftResource.WhiteScales => 1061053,
				CraftResource.BlueScales => 1061054,
				CraftResource.OakWood => 1072533,
				CraftResource.AshWood => 1072534,
				CraftResource.YewWood => 1072535,
				CraftResource.Heartwood => 1072536,
				CraftResource.Bloodwood => 1072538,
				CraftResource.Frostwood => 1072539,
				_ => 0,
			};
		}
	}
}
