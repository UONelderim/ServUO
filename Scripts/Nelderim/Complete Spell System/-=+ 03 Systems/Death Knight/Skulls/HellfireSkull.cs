using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class HellfireSkull : DeathKnightSkull
	{
		[Constructable]
		public HellfireSkull() : base( typeof(HellfireSpell) )
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Sir Farian z Tasandory");
			list.Add( 1049644, "Ogien Piekielny");
		}

		public HellfireSkull( Serial serial ) : base( serial )
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
