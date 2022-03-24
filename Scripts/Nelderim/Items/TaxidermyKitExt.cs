#region References

using System.Collections.Generic;
using Nelderim;

#endregion

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
			EventSink.WorldSave += Save;
			Load(ModuleName);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Save(args, ModuleName);
		}
	}

	class TaxidermyKitExtInfo : NExtensionInfo
	{
		public string AnimalName { get; set; }

		public int HueVal { get; set; }

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 ); //version
			writer.Write(AnimalName);
			writer.Write(HueVal);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = 0;
			if (Fix)
				version = reader.ReadInt(); //version
			AnimalName = reader.ReadString();
			HueVal = reader.ReadInt();
		}
	}
}
