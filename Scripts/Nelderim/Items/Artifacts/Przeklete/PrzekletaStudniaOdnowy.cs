using System;
using Server;

namespace Server.Items
{
	public class PrzekletaStudniaOdnowy : GoldRing
	{
        
        public override int InitMinHits { get { return 45; } }
        public override int InitMaxHits { get { return 45; } }

		[Constructable]
		public PrzekletaStudniaOdnowy()
		{
			Name = "Przeklęta Studnia Odnowy";
			Hue = 2700;
			LootType = LootType.Cursed;
			Attributes.RegenMana = 10;
			Attributes.RegenHits = 6;
			Attributes.RegenStam = 6;
			//Server.Engines.XmlSpawner2.XmlAttach.AttachTo(this, new Server.Engines.XmlSpawner2.TemporaryQuestObject("CursedArtifact", 20160));
		}

		public PrzekletaStudniaOdnowy( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

		}
	}
}