#region References

using Server.Items;
using Nelderim;

#endregion

namespace Server.Items
{
	public partial class EnergyHorn
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public int UsesRemaining
		{
			get
			{
				var ext = EnergyHornExt.Get(this);
				return ext != null ? ext.UsesRemaining : 0;
			}
			set
			{
				var ext = EnergyHornExt.Get(this);
				if (ext != null)
				{
					ext.UsesRemaining = value;
					InvalidateProperties();
				}
			}
		}

		public static int InitMaxUses = 120;
		public static int InitMinUses = 80;

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add(1060584, UsesRemaining.ToString()); // uses remaining: ~1_val~
		}
	}

	class EnergyHornExt : NExtension<EnergyHornExtInfo>
	{
		public static void Configure()
		{
			Register(new EnergyHornExt());
		}

		public EnergyHornExt() : base("EnergyHorn") { }
	}

	class EnergyHornExtInfo : NExtensionInfo
	{
		public int UsesRemaining { get; set; }

		public EnergyHornExtInfo()
		{
			UsesRemaining = Utility.RandomMinMax(EnergyHorn.InitMinUses, EnergyHorn.InitMaxUses);
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write((int)0);
			writer.Write(UsesRemaining);
		}

		public override void Deserialize(GenericReader reader)
		{
			var version = reader.ReadInt();
			UsesRemaining = reader.ReadInt();
		}
	}
}
