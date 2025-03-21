using System.Threading;
using Nelderim;

namespace Server
{
	public sealed class DualSaveStrategy : StandardSaveStrategy
	{
		public DualSaveStrategy()
		{
		}

		public override string Name => "Dual";

		public override void Save(SaveMetrics metrics, bool permitBackgroundWrite)
		{
			PermitBackgroundWrite = permitBackgroundWrite;

			var saveThread = new Thread(() => SaveItems(metrics))
			{
				Name = "Item Save Subset"
			};

			saveThread.Start();

			SaveMobiles(metrics);
			SaveGuilds(metrics);
			NExtension.SaveAll();

			saveThread.Join();

			if (permitBackgroundWrite && UseSequentialWriters)  //If we're permitted to write in the background, but we don't anyways, then notify.
				World.NotifyDiskWriteComplete();
		}
	}
}
