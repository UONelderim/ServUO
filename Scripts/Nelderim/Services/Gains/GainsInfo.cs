#region References

using System;
using Server;

#endregion

namespace Nelderim.Gains
{
	class GainsInfo : NExtensionInfo
	{
		private double _GainFactor = 1.0;
		private Timer _GainBoostTimer;
		private DateTime _GainBoostEndTime;

		public double GainFactor
		{
			get => _GainFactor;
			set => _GainFactor = Math.Max(Math.Round(value, 1), 1.0);
		}

		public DateTime GainBoostEndTime
		{
			get => _GainBoostEndTime;
			set
			{
				_GainBoostTimer?.Stop();
				
				_GainBoostEndTime = value;
				if(GainBoostEndTime > DateTime.Now)
					_GainBoostTimer = Timer.DelayCall(GainBoostEndTime - DateTime.Now, () => GainFactor = 1.0);
			}
		}

		public bool ActivateGainBoost(double gainFactor, TimeSpan duration)
		{
			if (GainBoostEndTime > DateTime.Now) return false;

			GainFactor = gainFactor;
			GainBoostEndTime = DateTime.Now + duration;
			return true;
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 ); //version
			writer.Write(GainFactor);
			writer.Write(GainBoostEndTime - DateTime.Now);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt(); //version
			
			GainFactor = reader.ReadDouble();
			GainBoostEndTime = DateTime.Now + reader.ReadTimeSpan();
		}
	}
}
