using Nelderim;

namespace Server.Nelderim
{
	public class FactionExt() : NExtension<FactionInfo>("Faction")
	{
		public static new void Initialize()
		{
			Register(new FactionExt());
		}
	}
	public class FactionInfo : NExtensionInfo
	{
		public Faction Faction { get; set; } = Faction.None;

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 ); //version
			writer.Write(Faction);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();
			Faction = reader.ReadFaction();
		}
	}
}
