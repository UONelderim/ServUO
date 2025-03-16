using Server.Mobiles;
using System;

namespace Server.Items
{
    public class KoscianegoSmokAltar : PeerlessAltar
    {
        public override int KeyCount => 3;
        public override MasterKey MasterKey => new KoscianegoSmokKey();

        public override Type[] Keys => new Type[]
        {
	        typeof( ZabKoscianegoSmoka ), typeof( PazurKoscianegoSmoka ), typeof( PalecKoscianegoSmoka ) 
};

        public override BasePeerless Boss => new NSkeletalDragon();

        [Constructable]
        public KoscianegoSmokAltar() : base(0x207C)
        {
	        
            BossLocation = new Point3D(6043, 279, 8);
            TeleportDest = new Point3D(6044, 262, 9);
            ExitDest = new Point3D(6040, 234, 8);

            Name = "Oltarz Koscinego Smoka";
            Hue = 1060;
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
            new Rectangle2D(6027, 262, 19, 20),
        };

        public KoscianegoSmokAltar(Serial serial) : base(serial)
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
