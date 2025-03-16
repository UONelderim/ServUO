using Server.Mobiles;
using System;

namespace Server.Items
{
    public class StarozytnyLodowySmokAltar : PeerlessAltar
    {
        public override int KeyCount => 3;
        public override MasterKey MasterKey => new StarozytnyLodowySmokKey();

        public override Type[] Keys => new Type[]
        {
	        typeof( PalecStarozytnegoLodowegoSmoka ), typeof( PazurStarozytnegoLodowegoSmoka ), typeof( ZabStarozytnegoLodowegoSmoka ) 
};

        public override BasePeerless Boss => new NStarozytnyLodowySmok();

        [Constructable]
        public StarozytnyLodowySmokAltar() : base(0x207C)
        {
	        
            BossLocation = new Point3D(5787, 1904, 5);
            TeleportDest = new Point3D(5786, 1928, 5);
            ExitDest = new Point3D(5776, 1952, 0);

            Name = "Oltarz Starozytnego Lodowego Smoka";
            Hue = 1579;
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
            new Rectangle2D(5766, 1889, 19, 20),
        };

        public StarozytnyLodowySmokAltar(Serial serial) : base(serial)
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
