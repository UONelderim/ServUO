using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class ShieldOfHateSkull : DeathKnightSkull
	{
		[Constructable]
		public ShieldOfHateSkull() : base( typeof(ShieldOfHateSpell) )
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Lorda Tasandora");
			list.Add( 1049644, "Tarcza Nienawisci");
		}

		public ShieldOfHateSkull( Serial serial ) : base( serial )
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
