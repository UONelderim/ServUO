using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class StrikeSkull : DeathKnightSkull
	{
		[Constructable]
		public StrikeSkull() : base( typeof(StrikeSpell) )
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Ksiaze Yngvinrill z Podrmoku");
			list.Add( 1049644, "Uderzenie");
		}

		public StrikeSkull( Serial serial ) : base( serial )
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
