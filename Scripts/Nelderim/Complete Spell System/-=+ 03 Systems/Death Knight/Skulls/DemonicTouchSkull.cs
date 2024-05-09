using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class DemonicTouchSkull : DeathKnightSkull
	{
		[Constructable]
		public DemonicTouchSkull() : base( typeof(DemonicTouchSpell) )
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Lord Monduiz Dephaar");
			list.Add( 1049644, "Dotyk Demona");
		}
		
		public DemonicTouchSkull( Serial serial ) : base( serial )
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
