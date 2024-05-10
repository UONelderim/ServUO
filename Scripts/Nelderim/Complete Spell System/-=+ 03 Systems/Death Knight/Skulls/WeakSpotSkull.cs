using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class WeakSpotSkull : DeathKnightSkull
	{
		[Constructable]
		public WeakSpotSkull() : base( typeof(WeakSpotSpell) )
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Soterios Lowca Nekromantow");
			list.Add( 1049644, "Slaby Punkt");
		}

		public WeakSpotSkull( Serial serial ) : base( serial )
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
