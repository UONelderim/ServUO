#region References
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
#endregion

namespace Server.Diagnostics
{
	public abstract class BaseProfile
	{
		public static void WriteAll<T>(TextWriter op, IEnumerable<T> profiles) where T : BaseProfile
		{
			var list = new List<T>(profiles);

			list.Sort((a, b) => -a.TotalTime.CompareTo(b.TotalTime));

			foreach (var prof in list)
			{
				prof.WriteTo(op);
				op.WriteLine();
			}
		}

		private TimeSpan _totalTime;
		private TimeSpan _peakTime;

		private readonly Stopwatch _stopwatch;

		public string Name { get; }

		public long Count { get; private set; }

		public TimeSpan AverageTime => TimeSpan.FromTicks(_totalTime.Ticks / Math.Max(1, Count));

		public TimeSpan PeakTime => _peakTime;

		public TimeSpan TotalTime => _totalTime;

		protected BaseProfile(string name)
		{
			Name = name;

			_stopwatch = new Stopwatch();
		}

		public virtual void Start()
		{
			if (_stopwatch.IsRunning)
			{
				_stopwatch.Reset();
			}

			_stopwatch.Start();
		}

		public virtual void Finish()
		{
			var elapsed = _stopwatch.Elapsed;

			_totalTime += elapsed;

			if (elapsed > _peakTime)
			{
				_peakTime = elapsed;
			}

			Count++;

			_stopwatch.Reset();
		}

		public virtual void WriteTo(TextWriter op)
		{
			op.Write(
				"{0,-100} {1,12:N0} {2,12:F5} {3,-12:F5} {4,12:F5}",
				Name,
				Count,
				AverageTime.TotalSeconds,
				PeakTime.TotalSeconds,
				TotalTime.TotalSeconds);
		}
	}
}