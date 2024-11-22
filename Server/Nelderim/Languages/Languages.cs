namespace Nelderim
{
	public class Languages() : NExtension<LanguagesInfo>("Languages")
	{
		public static void Configure()
		{
			Register(new Languages());
		}
	}
}
