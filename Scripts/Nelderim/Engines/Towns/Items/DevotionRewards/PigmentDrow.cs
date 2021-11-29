
namespace Server.Items
{
    public class PigmentDrow : BasePigment
    {
        [Constructable]
        public PigmentDrow()
            : this(5)
        {
        
        }
        [Constructable]
        public PigmentDrow(int uses)
            : base(PigmentTarget.Cloth, uses, 2882)
        {
            Name = "Pigment barw Podmroku";
        }

        public PigmentDrow(Serial serial) : base(serial)
        {
        }
        
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
