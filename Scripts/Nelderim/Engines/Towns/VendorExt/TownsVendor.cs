namespace Nelderim.Towns
{
	class TownsVendor() : NExtension<TownsVendorInfo>("TownsVendor")
	{
		public static new void Initialize()
		{
			Register(new TownsVendor());
		}
	}
}
