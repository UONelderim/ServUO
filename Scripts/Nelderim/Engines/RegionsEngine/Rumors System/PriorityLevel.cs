// 05.06.26 :: LogoS
// 05.07.01 :: troyan :: dodanie wartosci do PriorityLevel
// 05.12.13 :: troyan :: rozwiniecie

namespace Server.Nelderim
{
	public enum PriorityLevel
	{
		None = 0,
		VeryLow = 1,
		Low = 2,
		Medium = 4,
		High = 8,
		VeryHigh = 16
	}

	public enum RumorType
	{
		Guard,
		Vendor,
		BuyandSell
	}

	public enum NewsType
	{
		MOTD = 0,
		News = 1,
		Rumor = 2,
		All = 10
	}
}
