#region References

using System;
using Server;

#endregion

namespace Nelderim.Gains
{
	class GainsInfo : NExtensionInfo
	{
		private double _GainFactor = 1.0;

		public double GainFactor
		{
			get => _GainFactor;
			set => _GainFactor = Math.Min(Math.Round(value, 1), 1.0);
		}

		public double StrGain { get; set; }

		public double DexGain { get; set; }

		public double IntGain { get; set; }

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)1 ); //version
			writer.Write(StrGain);
			writer.Write(DexGain);
			writer.Write(IntGain);
			
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = 0;
			if (Fix)
				version = reader.ReadInt(); //version
			switch (version)
			{
				case 1:
				case 0:
					if(version == 0)
						/*LastPowerHour =*/ reader.ReadDateTime();
					StrGain = reader.ReadDouble();
					DexGain = reader.ReadDouble();
					IntGain = reader.ReadDouble();
					break;
			}
		}
	}
}
