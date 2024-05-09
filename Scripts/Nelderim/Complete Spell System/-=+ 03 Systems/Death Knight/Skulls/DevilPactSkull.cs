using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class DevilPactSkull : DeathKnightSkull
	{
		[Constructable]
		public DevilPactSkull() : base( typeof(DevilPactSpell) )
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Fron z Talas");
			list.Add( 1049644, "Pakt Ze Smiercia");
		}

		public DevilPactSkull( Serial serial ) : base( serial )
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
