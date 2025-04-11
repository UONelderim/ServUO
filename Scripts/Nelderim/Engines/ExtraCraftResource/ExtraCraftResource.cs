#region References

using Server;
using Server.Items;

#endregion

namespace Nelderim
{
	public interface IResource2
	{
		CraftResource Resource2 { get; set; }
	}
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
			writer.Write( (int)0 );
			writer.Write((int)Resource2);
		}

		public override void Deserialize(GenericReader reader)
		{
			var version = reader.ReadInt();
			Resource2 = (CraftResource)reader.ReadInt();
		}
	}
}
