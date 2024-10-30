namespace Server.Items
{
	public enum MilkType
	{
		None,
		Sheep,
		Goat,
		Cow
	}

	public static class MilkTypeExtensions
	{
		public static string GetPropertyString(this MilkType milkType)
		{
			return milkType switch
			{
				MilkType.Sheep => "Owcze Mleko",
				MilkType.Goat => "Kozie Mleko",
				MilkType.Cow => "Krowie Mleko",
				_ => "Puste"
			};
		}
	}
}
