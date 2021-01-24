using System;
using Server.Items;

namespace Server.Items
{
	public class PrzekleteSongWovenMantle : LeafArms
	{

        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance{ get{ return 14; } }
		public override int BaseColdResistance{ get{ return 14; } }
		public override int BaseEnergyResistance{ get{ return 16; } }

		[Constructable]
		public PrzekleteSongWovenMantle()
		{
			Hue = 2700;
Name = "Przeklęte Naramienniki Barda";
			SkillBonuses.SetValues( 0, SkillName.Musicianship, 10.0 );
			Attributes.Luck = 300;
			Attributes.DefendChance = 15;
			LootType = LootType.Cursed;
			//Server.Engines.XmlSpawner2.XmlAttach.AttachTo(this, new Server.Engines.XmlSpawner2.TemporaryQuestObject("CursedArtifact", 20160));
		}

		public PrzekleteSongWovenMantle( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}