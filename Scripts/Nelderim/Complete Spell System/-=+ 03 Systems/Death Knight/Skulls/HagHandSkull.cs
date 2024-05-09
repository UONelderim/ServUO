using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class HagHandSkull : DeathKnightSkull
	{
		[Constructable]
		public HagHandSkull() : base( typeof(HagHandSpell) )
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Sir Maeril z Tasandory");
			list.Add( 1049644, "Reka Wiedzmy");
		}

		public HagHandSkull( Serial serial ) : base( serial )
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
