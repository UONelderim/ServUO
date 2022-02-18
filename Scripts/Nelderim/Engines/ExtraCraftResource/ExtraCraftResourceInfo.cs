#region References

using Server;
using Server.Items;

#endregion

namespace Nelderim.ExtraCraftResource
{
	class ExtraCraftResourceInfo : NExtensionInfo
	{
		public CraftResource Resource2 { get; set; }

		public override void Deserialize(GenericReader reader)
		{
			Resource2 = (CraftResource)reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write((int)Resource2);
		}
	}
}
