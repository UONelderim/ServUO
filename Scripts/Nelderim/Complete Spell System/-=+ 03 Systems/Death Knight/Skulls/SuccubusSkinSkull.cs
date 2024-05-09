using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class SuccubusSkinSkull : DeathKnightSkull
	{
		[Constructable]
		public SuccubusSkinSkull() : base( typeof(SuccubusSkinSpell) )
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Sir Luren");
			list.Add( 1049644, "Skora Sukkuba");
		}

		public SuccubusSkinSkull( Serial serial ) : base( serial )
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
