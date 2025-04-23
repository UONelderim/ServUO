using System;
using System.Collections.Generic;
using System.IO;
using Server.Nelderim;

namespace Server
{
	public partial class GenericReader
	{
		public abstract Faction ReadFaction();
	}
	
	public partial class GenericWriter
	{
		public abstract void Write(Faction value);
	}
	
	public partial class BinaryFileReader
	{
		public override Faction ReadFaction()
		{
			return Faction.Factions[ReadByte()];
		}
	}
	
	public partial class BinaryFileWriter
	{
		public override void Write(Faction value)
        {
        	if (value != null)
        	{
        		Write((byte)value.Index);
        	}
        	else
        	{
        		Write((byte)0xFF);
        	}
        }
	}

	public partial class AsyncWriter
	{
		public override void Write(Faction value)
		{
			if (value != null)
			{
				Write((byte)value.Index);
			}
			else
			{
				Write((byte)0xFF);
			}
		}
	}

	public class SerialFixingBinaryFileReader(BinaryReader br, Dictionary<Serial, Serial> mapping)
		: BinaryFileReader(br)
	{
		private Dictionary<Serial, Serial> _Mapping = mapping;

		public override Serial ReadSerial()
		{
			var serial =  base.ReadSerial();
			if (_Mapping.TryGetValue(serial, out var newSerial))
			{
				Console.WriteLine($"Fixing serial {serial} -> {newSerial}");
				return newSerial;
			}
			return serial;
		}
	}
}
