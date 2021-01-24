using System;
using Server;

namespace Server.Items
{
	public class PrzekletaSamaritanRobe : Robe
	{
        
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

		public override int BasePhysicalResistance{ get{ return 15; } }
		public override bool CanFortify{ get{ return false; } }

		[Constructable]
		public PrzekletaSamaritanRobe()
		{
			Hue = 2700;
			Name = "Przeklęta Szata Samarytanina";
			LootType = LootType.Cursed;
			//Server.Engines.XmlSpawner2.XmlAttach.AttachTo(this, new Server.Engines.XmlSpawner2.TemporaryQuestObject("CursedArtifact", 20160));
		}

		public PrzekletaSamaritanRobe( Serial serial ) : base( serial )
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
