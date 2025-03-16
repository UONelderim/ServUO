#region References

using Nelderim;

#endregion

namespace Server.Items
{
	//REMOVE ME
	class FireHornExt() : NExtension<FireHornExtInfo>("FireHorn")
	{
		public static void Configure()
		{
			Register(new FireHornExt());
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
			writer.Write( (int)0 );
			writer.Write(UsesRemaining);
		}

		public override void Deserialize(GenericReader reader)
		{
			var version = reader.ReadInt();
			UsesRemaining = reader.ReadInt();
		}
	}
}
