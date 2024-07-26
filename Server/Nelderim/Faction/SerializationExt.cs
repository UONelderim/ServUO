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
}
