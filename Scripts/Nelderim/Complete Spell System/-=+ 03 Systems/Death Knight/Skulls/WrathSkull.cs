using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class WrathSkull : DeathKnightSkull
	{
		[Constructable]
		public WrathSkull() : base( typeof(WrathSpell) )
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Lord Khayven");
			list.Add( 1049644, "Gniew");
		}

		public WrathSkull( Serial serial ) : base( serial )
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
