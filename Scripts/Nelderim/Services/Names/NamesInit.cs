#region References

using Server;
using Server.Commands;


#endregion

namespace Nelderim
{
	public class NamesInit
	{
		public static string ModuleName = "Names";
		
		public static void Initialize()
		{
			EventSink.WorldSave += Save;
			Names.Load(ModuleName);
		}
		
		public static void Save(WorldSaveEventArgs args)
		{
			Names.Save(args, ModuleName);
		}
	}
}
