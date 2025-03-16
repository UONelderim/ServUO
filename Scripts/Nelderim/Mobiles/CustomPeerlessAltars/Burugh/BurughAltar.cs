using Server.Mobiles;
using System;

namespace Server.Items
{
    public class BurughAltar : PeerlessAltar
    {
        public override int KeyCount => 3;
        public override MasterKey MasterKey => new BurughKey();

        public override Type[] Keys => new Type[]
        {
	        typeof( OdnogaStaregoGazera ), typeof( PowiekaGazera ), typeof( KielBiesa ) 
};

        public override BasePeerless Boss => new NBurugh();

        [Constructable]
        public BurughAltar() : base(0x207C)
        {
	        
            BossLocation = new Point3D(5800, 1762, 0);
            TeleportDest = new Point3D(5797, 1783, 12);
            ExitDest = new Point3D(719, 1423, 20);

            Name = "Oltarz Burugha";
            Hue = 2207;
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
            new Rectangle2D(5800, 1762, 19, 20),
        };

        public BurughAltar(Serial serial) : base(serial)
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
