using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class SoulReaperSkull : DeathKnightSkull
	{
		[Constructable]
		public SoulReaperSkull() : base( typeof(SoulReaperSpell) )
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Lord Thyrian Zlotobrody");
			list.Add( 1049644, "Zniwiarz Dusz");
		}

		public SoulReaperSkull( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
