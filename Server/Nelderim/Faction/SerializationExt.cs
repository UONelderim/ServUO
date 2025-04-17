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

		public override IEntity ReadEntity()
		{
			var serial = ReadSerial();
			if (_Mapping.TryGetValue(serial, out var newSerial))
			{
				Console.WriteLine($"Fixing entity serial {serial}");
				return World.FindEntity(newSerial);
			}
			return World.FindEntity(serial);
		}

		public override Item ReadItem()
		{
			var serial = ReadSerial();
			if (_Mapping.TryGetValue(serial, out var newSerial))
			{
				Console.WriteLine($"Fixing item serial {serial}->{newSerial}");
				var oldItem = World.FindItem(serial);
				Console.WriteLine($"Old: {oldItem.GetType().Name} {oldItem.Location.ToString()}");
				var newItem = World.FindItem(newSerial);
				Console.WriteLine($"New: {newItem.GetType().Name} {newItem.Location.ToString()}");
				return newItem;
			}
			return World.FindItem(serial);
		}

		public override Mobile ReadMobile()
		{
			var serial = ReadSerial();
			if (_Mapping.TryGetValue(serial, out var newSerial))
			{
				Console.WriteLine($"Fixing mobile serial {serial}");
				return World.FindMobile(newSerial);
			}
			return World.FindMobile(serial);
		}
	}
}
