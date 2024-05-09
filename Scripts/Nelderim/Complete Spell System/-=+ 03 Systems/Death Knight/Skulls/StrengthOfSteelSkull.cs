using Server.Spells.DeathKnight;

namespace Server.Items
{
	public class StrengthOfSteelSkull : DeathKnightSkull
	{
		[Constructable]
		public StrengthOfSteelSkull() : base( typeof(StrengthOfSteelSpell) )
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add( 1070722, "Lady Nolens");
			list.Add( 1049644, "Wytrzymalosc Stali");
		}

		public StrengthOfSteelSkull( Serial serial ) : base( serial )
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
