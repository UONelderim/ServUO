#region References

using Server;

#endregion

namespace Nelderim
{
	class CharacterSheet : NExtension<CharacterSheetInfo>
	{
		public static string ModuleName = "CharacterSheet";

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
