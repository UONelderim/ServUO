using System;

namespace Server.Items.Crops
{


    public class PlainTobaccoSapling : BaseSeedling
    {
        public override Type PlantType => typeof(PlainTobaccoPlant);

        [Constructable]
        public PlainTobaccoSapling(int amount) : base(amount, 0x0CB6)
        {
            Hue = 2129;
            Name = "Szczepka tytoniu pospolitego";
            Stackable = true;
        }

        [Constructable]
        public PlainTobaccoSapling() : this(1)
        {
        }

        public PlainTobaccoSapling(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PlainTobaccoPlant : Plant
    {
        public override Type SeedType => typeof(PlainTobaccoSapling);
        public override Type CropType => typeof(PlainTobaccoCrop);
		protected override int YoungPlantGraphics => 0x0C97;
		protected override int MaturePlantGraphics => 0x0C97;

		[Constructable]
        public PlainTobaccoPlant() : base(0x0C97)
		{
			Hue = 2129;
            Name = "Tyton pospolity";
            Stackable = true;
        }

        public PlainTobaccoPlant(Serial serial) : base(serial)
        {
            //m_plantedTime = DateTime.Now;	// ???
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PlainTobaccoCrop : Crop
    {
        public override Type ReagentType => typeof(PlainTobacco);

        [Constructable]
        public PlainTobaccoCrop(int amount) : base(amount, 0x0C93)
        {
            Hue = 2129;
            Name = "Swieze liscie tytoniu pospolitego";
            Stackable = true;
        }

        [Constructable]
        public PlainTobaccoCrop() : this(1)
        {
        }

        public PlainTobaccoCrop(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }


}