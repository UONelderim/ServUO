#region References

using Server;

#endregion

namespace Nelderim
{
	class LabelsInit
	{
		public static string ModuleName = "Labels";

		public static void Initialize()
		{
			EventSink.WorldSave += Save;
			Labels.Load(ModuleName);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Labels.Cleanup();
			Labels.Save(args, ModuleName);
		}
	}
}
