#region References

using Server;
using Server.Items;

#endregion

namespace Nelderim
{
	class ExtraCraftResource() : NExtension<ExtraCraftResourceInfo>("ExtraCraftResource")
	{
		public static void Configure()
		{
			Register(new ExtraCraftResource());
		}
	}
	
	class ExtraCraftResourceInfo : NExtensionInfo
	{
		public CraftResource Resource2 { get; set; }

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 ); //version
			writer.Write((int)Resource2);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = 0;
			if (Fix)
				version = reader.ReadInt(); //version
			Resource2 = (CraftResource)reader.ReadInt();
		}
	}
}
