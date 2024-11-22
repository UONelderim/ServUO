using System.Collections.Generic;
using Server;

namespace Nelderim
{
	public class Names() : NExtension<NamesInfo>("Names")
	{
		public static void Configure()
		{
			Register(new Names());
		}
	}
	
	public class NamesInfo : NExtensionInfo
	{
		public Dictionary<Mobile, string> Names = new();

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 ); //version
			writer.Write(Names.Keys.Count);
			foreach (var mobile in Names.Keys)
			{
				writer.Write(mobile);
				writer.Write(Names[mobile]);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();
			int namesCount = reader.ReadInt();
			Names = new Dictionary<Mobile, string>(namesCount);
			for (var i = 0; i < namesCount; i++)
			{
				var mobile = reader.ReadMobile();
				var name = reader.ReadString();
				if(mobile != null && name != null )
					Names[mobile] = name;
			}
		}
	}
}
