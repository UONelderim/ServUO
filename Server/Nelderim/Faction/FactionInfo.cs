using Server;

namespace Nelderim.Factions
{
	public class FactionInfo : NExtensionInfo
	{
		public Faction Faction { get; set; }

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 ); //version
			writer.Write(Faction);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = 0;
			Faction = reader.ReadFaction();
		}
	}
}
