#region References

using System.Collections.Generic;
using Nelderim;

#endregion

namespace Server.Items
{
	public partial class Corpse
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public double CampingCarved
		{
			get { return CorpseExt.Get(this).CampingCarved; }
			set { CorpseExt.Get(this).CampingCarved = value; }
		}
	}

	class CorpseExt : NExtension<CorpseExtInfo>
	{
		public static string ModuleName = "Corpse";

		public static void Initialize()
		{
			EventSink.WorldSave += Save;
			Load(ModuleName);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Cleanup();
			Save(args, ModuleName);
		}

		private static void Cleanup()
		{
			List<Serial> toRemove = new List<Serial>();
			foreach (KeyValuePair<Serial, CorpseExtInfo> kvp in m_ExtensionInfo)
			{
				if (World.FindItem(kvp.Key) == null)
					toRemove.Add(kvp.Key);
			}

			foreach (Serial serial in toRemove)
			{
				m_ExtensionInfo.TryRemove(serial, out _);
			}
		}
	}

	class CorpseExtInfo : NExtensionInfo
	{
		public double CampingCarved { get; set; }

		public CorpseExtInfo()
		{
			CampingCarved = -1.0;
		}

		public override void Deserialize(GenericReader reader)
		{
			CampingCarved = reader.ReadDouble();
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write(CampingCarved);
		}
	}
}
