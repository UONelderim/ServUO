using Server.Mobiles;
using System;

namespace Server.Items
{
    public class ImiennySmokAltar : PeerlessAltar
    {
        public override int KeyCount => 3;
        public override MasterKey MasterKey => new ImiennySmokKey();

        public override Type[] Keys => new Type[]
        {
	        typeof( PalecImiennegoSmoka ), typeof( PazurImiennegoSmoka ), typeof( ZabImiennegoSmoka ) 
};

        public override BasePeerless Boss => new NelderimDragon();

        [Constructable]
        public ImiennySmokAltar() : base(0x207C)
        {
	        
            BossLocation = new Point3D(5615, 563, -36);
            TeleportDest = new Point3D(5688, 563, 4);
            ExitDest = new Point3D(534, 1169, -3);

            Name = "Oltarz Imiennego Smoka";
            Hue = 22;
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
            new Rectangle2D(5604, 514, 25, 40),
        };

        public ImiennySmokAltar(Serial serial) : base(serial)
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
