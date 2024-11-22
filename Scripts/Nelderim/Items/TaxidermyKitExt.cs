#region References

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

	class TaxidermyKitExt() : NExtension<TaxidermyKitExtInfo>("TaxidermyKitExt")
	{
		public static void Configure()
		{
			Register(new TaxidermyKitExt());
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
