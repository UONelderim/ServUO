using Server.Mobiles;
using System;

namespace Server.Items
{
    public class Shimmeringltar : PeerlessAltar
    {
	    public override int KeyCount => 3;
	    public override MasterKey MasterKey => new PrismOfLightKey();

        public override Type[] Keys => new Type[]
        {
	        typeof(JaggedCrystals), typeof(BrokenCrystals), typeof(PiecesOfCrystal),
	        typeof(CrushedCrystals), typeof(ScatteredCrystals), typeof(ShatteredCrystals)
};

        public override BasePeerless Boss => new ShimmeringEffusion();

        [Constructable]
        public Shimmeringltar() : base(0x207C)
        {
	        
	        BossLocation = new Point3D(6708, 138, -1);
	        TeleportDest = new Point3D(6707, 120, -1);
	        ExitDest = new Point3D(3384, 1955, 0);

            Name = "Oltarz Lsniacego Wysieku";
            Hue = 1152;
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
	        new Rectangle2D(6692, 116, 45, 35),
        };

        public Shimmeringltar(Serial serial) : base(serial)
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
