namespace Server.Items
{
	public class WidowMorphHelm : BaseArmor
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
		public WidowMorphHelm() : base( 11024 )
		{
			Name = "Helm Wdowy";
			Weight = 5.0;
			Attributes.RegenHits = 1;
			
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
			WidowMorphSet.Apply(this, parent);
		}

		public override void OnRemoved(IEntity parent)
		{
			base.OnRemoved(parent);
			WidowMorphSet.Unapply(parent);
		}

		public WidowMorphHelm( Serial serial ) : base( serial )
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
