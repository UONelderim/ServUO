#region References

#endregion

namespace Nelderim
{
	public class Labels() : NExtension<LabelsInfo>("Labels")
	{
		public static void Configure()
		{
			Register(new Labels());
		}
	}
}
