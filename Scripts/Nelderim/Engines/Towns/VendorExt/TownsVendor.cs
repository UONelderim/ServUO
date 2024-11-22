namespace Nelderim.Towns
{
	class TownsVendor() : NExtension<TownsVendorInfo>("TownsVendor")
	{
		public static void Configure()
		{
			Register(new TownsVendor());
		}
	}
}
