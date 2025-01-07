
namespace Server.Items
{
	[FlipableAttribute( 11016, 11017 )]
	public class WidowMorphChest : BaseArmor
	{
		public override int BasePhysicalResistance => 8;
		public override int BaseFireResistance => 5;
		public override int BaseColdResistance => 5;
		public override int BasePoisonResistance => 7;
		public override int BaseEnergyResistance => 5;
		public override int InitMinHits => 50;
		public override int InitMaxHits => 65;
		public override int StrReq => 20;
		public override ArmorMaterialType MaterialType => ArmorMaterialType.Leather;
		public override SetItem SetID => SetItem.WidowMorph;
		public override int Pieces => 7;

		[Constructable]
		public WidowMorphChest() : base( 11016 )
		{
			Name = "Tunika Wdowy";
			Weight = 10.0;
			Attributes.RegenHits = 1;
			Attributes.AttackChance = 5;
			
			WidowMorphSet.Attributes(this);
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );
			list.Add( "*nadaje forme wdowy*" );
		}

		public override void OnAdded(IEntity parent)
		{
			base.OnAdded(parent);
			WidowMorphSet.CheckMorph(this, parent);
		}

		public override void OnRemoved(IEntity parent)
		{
			base.OnRemoved(parent);
			WidowMorphSet.CheckMorph(this, parent);
		}

		public WidowMorphChest( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
