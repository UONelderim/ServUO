using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class LucifersBoltSkull : DeathKnightSkull
	{
		[Constructable]
		public LucifersBoltSkull() : base( typeof(LucifersBoltSpell) )
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Halrand Wulfrost z Garlan");
			list.Add( 1049644, "Promien Smierci");
		}

		public LucifersBoltSkull( Serial serial ) : base( serial )
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
