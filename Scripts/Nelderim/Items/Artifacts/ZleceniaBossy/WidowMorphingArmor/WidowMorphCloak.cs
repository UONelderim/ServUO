namespace Server.Items
{  
	public class WidowMorphCloak : BaseCloak
	{
		public override SetItem SetID => SetItem.WidowMorph;
		public override int Pieces => 7;

		[Constructable]
		public WidowMorphCloak() : base( 0x2B04 )
		{
			Weight = 4.0;
			Name = "Plaszcz Wdowy";
			
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
		
		public WidowMorphCloak( Serial serial ) : base( serial )
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
