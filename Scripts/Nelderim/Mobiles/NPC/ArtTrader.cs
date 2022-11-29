using System;
using System.Collections.Generic;


namespace Server.Mobiles
{
	public class ArtTrader : BaseVendor
	{
		private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos => m_SBInfos;

		public override Type Currency => typeof(ArtefaktowyPyl);

		[Constructable]
		public ArtTrader() : base( "- Handlarz Artefaktów" )
		{
	
		}

		public override void InitSBInfo()
		{
			SBInfos.Add( new SBArtTrader() );
		}

		public override VendorShoeType ShoeType => Utility.RandomBool() ? VendorShoeType.Shoes : VendorShoeType.Sandals;

		public override void InitOutfit()
		{
			base.InitOutfit();

			AddItem( new Items.Robe( Utility.RandomPinkHue() ) );
		}
		

		public ArtTrader( Serial serial ) : base( serial )
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
