using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class BanishSkull : DeathKnightSkull
	{
		[Constructable]
		public BanishSkull() : base( typeof(BanishSpell))
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Swiety Kargoth");
			list.Add( 1049644, "Wygnanie");
		}

		public BanishSkull( Serial serial ) : base( serial )
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
