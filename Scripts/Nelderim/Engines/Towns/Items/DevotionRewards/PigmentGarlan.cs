
namespace Server.Items
{
    public class PigmentGarlan : BasePigment
    {
        [Constructable]
        public PigmentGarlan()
            : this(5)
        {
        
        }
        [Constructable]
        public PigmentGarlan(int uses)
            : base(PigmentTarget.Cloth, uses, 2690)
        {
            Name = "Pigment barw Garlan";
        }

        public PigmentGarlan(Serial serial) : base(serial)
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
