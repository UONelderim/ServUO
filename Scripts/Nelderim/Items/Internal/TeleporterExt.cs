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

	class TeleporterExt : NExtension<TeleporterExtInfo>
	{
		public static string ModuleName = "Teleporter";

		public static void Initialize()
		{
			EventSink.WorldSave += Save;
			Load(ModuleName);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Save(args, ModuleName);
		}
	}

	class TeleporterExtInfo : NExtensionInfo
	{
		public bool AllowGhosts { get; set; }

		public TeleporterExtInfo()
		{
			AllowGhosts = true;
		}

		public override void Deserialize(GenericReader reader)
		{
			AllowGhosts = reader.ReadBool();
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write(AllowGhosts);
		}
	}
}
