#region References

using System.Collections.Generic;
using Nelderim;

#endregion

namespace Server.Items
{
	public partial class Teleporter
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public bool AllowGhosts
		{
			get => TeleporterExt.Get(this).AllowGhosts;
			set => TeleporterExt.Get(this).AllowGhosts = value;
		}
	}

	class TeleporterExt() : NExtension<TeleporterExtInfo>("Teleporter")
	{
		public static void Configure()
		{
			Register(new TeleporterExt());
		}
	}

	class TeleporterExtInfo : NExtensionInfo
	{
		public bool AllowGhosts { get; set; }

		public TeleporterExtInfo()
		{
			AllowGhosts = true;
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 );
			writer.Write(AllowGhosts);
		}

		public override void Deserialize(GenericReader reader)
		{
			var version = reader.ReadInt();
			AllowGhosts = reader.ReadBool();
		}
	}
}
