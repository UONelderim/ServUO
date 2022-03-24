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
			Save(args, ModuleName);
		}
	}

	class CorpseExtInfo : NExtensionInfo
	{
		public double CampingCarved { get; set; }

		public CorpseExtInfo()
		{
			CampingCarved = -1.0;
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 ); //version
			writer.Write(CampingCarved);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = 0;
			if (Fix)
				version = reader.ReadInt(); //version
			CampingCarved = reader.ReadDouble();
		}
	}
}
