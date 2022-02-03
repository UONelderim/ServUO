using System.Collections.Generic;
using Nelderim;

namespace Server.Items
{
	public partial class TrophyAddon
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public string AnimalName
		{
			get { return TaxidermyKitExt.Get(this).AnimalName; }
			set
			{
				TaxidermyKitExt.Get(this).AnimalName = value;
				InvalidateProperties();
			}
		}
	}

	public partial class TrophyDeed
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public string AnimalName
		{
			get { return TaxidermyKitExt.Get(this).AnimalName; }
			set
			{
				TaxidermyKitExt.Get(this).AnimalName = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int HueVal
		{
			get { return TaxidermyKitExt.Get(this).HueVal; }
			set { TaxidermyKitExt.Get(this).HueVal = value; }
		}
	}

	class TaxidermyKitExt : NExtension<TaxidermyKitExtInfo>
	{
		public static string ModuleName = "TaxidermyKitExt";

		public static void Initialize()
		{
			EventSink.WorldSave += new WorldSaveEventHandler(Save);
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
			foreach (KeyValuePair<Serial, TaxidermyKitExtInfo> kvp in m_ExtensionInfo)
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

	class TaxidermyKitExtInfo : NExtensionInfo
	{
		private string m_AnimalName;
		private int m_HueVal;

		public string AnimalName { get { return m_AnimalName; } set { m_AnimalName = value; } }
		public int HueVal { get { return m_HueVal; } set { m_HueVal = value; } }

		public override void Deserialize(GenericReader reader)
		{
			m_AnimalName = reader.ReadString();
			m_HueVal = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write(m_AnimalName);
			writer.Write(m_HueVal);
		}
	}
}
