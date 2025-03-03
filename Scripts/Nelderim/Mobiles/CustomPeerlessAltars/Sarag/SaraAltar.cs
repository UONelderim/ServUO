using Server.Mobiles;
using System;

namespace Server.Items
{
	//It should be SaragAltar :)
    public class SaraAltar : PeerlessAltar
    {
        public override int KeyCount => 3;
        public override MasterKey MasterKey => new SaragKey();

        public override Type[] Keys => new Type[]
        {
	        typeof( KielBiesa ), typeof( PalecLicza ), typeof( PrzekletaKosc ) 
};

        public override BasePeerless Boss => new NSarag();

        [Constructable]
        public SaraAltar() : base(0x207C)
        {
	        
            BossLocation = new Point3D(5765, 1719, 7);
            TeleportDest = new Point3D(5741, 1723, 7);
            ExitDest = new Point3D(5676, 1874, 0);

            Name = "Oltarz Saraga";
            Hue = 1128;
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
            new Rectangle2D(5456, 3792, 19, 20),
        };

        public SaraAltar(Serial serial) : base(serial)
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
