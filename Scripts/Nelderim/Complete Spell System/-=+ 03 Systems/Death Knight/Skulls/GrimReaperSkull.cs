using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class GrimReaperSkull : DeathKnightSkull
	{
		[Constructable]
		public GrimReaperSkull() : base( typeof(GrimReaperSpell) )
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Ksiaze Myrhal z Thila");
			list.Add( 1049644, "Ponury Zniwiarz");
		}

		public GrimReaperSkull( Serial serial ) : base( serial )
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
