using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Server
{
	public sealed class SaveMetrics
	{
		
		private static readonly Meter meter = new("ServUO Save");

		private static readonly Counter<long> numberOfWorldSaves = meter.CreateCounter<long>("Save - Count", null, "Number of world saves.");

		private static readonly Counter<long> itemsPerSecond = meter.CreateCounter<long>("Save - Items", null, "Number of items saved");
		private static readonly Counter<long> mobilesPerSecond = meter.CreateCounter<long>("Save - Mobiles", null, "Number of mobiles saves.");

		private static readonly Counter<long> serializedBytesPerSecond = meter.CreateCounter<long>("Save - Serialized bytes", null, "Amount of world-save bytes serialized per second.");
		private static readonly Counter<long> writtenBytesPerSecond = meter.CreateCounter<long>("Save - Written bytes", null, "Amount of world-save bytes written to disk per second.");

		public SaveMetrics()
		{
			// increment number of world saves
			numberOfWorldSaves.Add(1);
		}

		public void OnItemSaved(int numberOfBytes)
		{
			itemsPerSecond.Add(1);

			serializedBytesPerSecond.Add(numberOfBytes);
		}

		public void OnMobileSaved(int numberOfBytes)
		{
			mobilesPerSecond.Add(1);

			serializedBytesPerSecond.Add(numberOfBytes);
		}

		public void OnGuildSaved(int numberOfBytes)
		{
			serializedBytesPerSecond.Add(numberOfBytes);
		}

		public void OnFileWritten(int numberOfBytes)
		{
			writtenBytesPerSecond.Add(numberOfBytes);
		}
	}
}
