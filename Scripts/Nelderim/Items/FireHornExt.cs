#region References

using System.Collections.Generic;
using Nelderim;

#endregion

namespace Server.Items
{
	public partial class FireHorn
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public int UsesRemaining
		{
			get { return FireHornExt.Get(this).UsesRemaining; }
			set
			{
				FireHornExt.Get(this).UsesRemaining = value;
				InvalidateProperties();
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

	class FireHornExt : NExtension<FireHornExtInfo>
	{
		public static string ModuleName = "FireHorn";

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

	class FireHornExtInfo : NExtensionInfo
	{
		public int UsesRemaining { get; set; }

		public FireHornExtInfo()
		{
			UsesRemaining = Utility.RandomMinMax(FireHorn.InitMinUses, FireHorn.InitMaxUses);
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 ); //version
			writer.Write(UsesRemaining);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = 0;
			if (Fix)
				version = reader.ReadInt(); //version
			UsesRemaining = reader.ReadInt();
		}
	}
}
