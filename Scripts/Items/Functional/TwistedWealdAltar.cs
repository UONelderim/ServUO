using Server.Mobiles;
using System;

namespace Server.Items
{
    public class TwistedWealdAltar : PeerlessAltar
    {
        public override int KeyCount => 3;
        public override MasterKey MasterKey => new TwistedWealdKey();

        public override Type[] Keys => new Type[]
{
            typeof( BlightedCotton ), typeof( GnawsFang ), typeof( IrksBrain ),
            typeof( LissithsSilk ), typeof( SabrixsEye ), typeof( ThornyBriar )
};

        public override BasePeerless Boss => new DreadHorn();

        [Constructable]
        public TwistedWealdAltar() : base(0x207C)
        {
            BossLocation = new Point3D(5302, 1844, 10);
            TeleportDest = new Point3D(5291, 1844, 12);
            ExitDest = new Point3D(5256, 1831, 17);
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
            new Rectangle2D(5283, 1834, 33, 38),
        };

        public TwistedWealdAltar(Serial serial) : base(serial)
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
