using Server.Mobiles;
using System;

namespace Server.Items
{
    public class ZhoarminthAltar : PeerlessAltar
    {
        public override int KeyCount => 3;
        public override MasterKey MasterKey => new ZhoarminthKey();

        public override Type[] Keys => new Type[]
        {
	        typeof( ZabZhoarmintha ), typeof( RogZhoarmintha ), typeof( PalceZhoarmintha ) 
};

        public override BasePeerless Boss => new Zhoaminth();

        [Constructable]
        public ZhoarminthAltar() : base(0x207C)
        {
	        
            BossLocation = new Point3D(5461, 3796, -25);
            TeleportDest = new Point3D(5473, 3823, -25);
            ExitDest = new Point3D(5473, 3830, -25);

            Name = "Oltarz Zhoarmintha";
            Hue = 1161;
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
            new Rectangle2D(5456, 3792, 19, 20),
        };

        public ZhoarminthAltar(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
