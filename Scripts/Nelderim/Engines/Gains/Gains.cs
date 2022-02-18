#region References

using Server;

#endregion

namespace Nelderim.Gains
{
	class Gains : NExtension<GainsInfo>
	{
		public static string ModuleName = "Gains";

		public static void Initialize()
		{
			EventSink.WorldSave += Save;
			Load(ModuleName);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Save(args, ModuleName);
		}
	}
}
