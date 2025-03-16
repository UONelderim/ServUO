using Server.Mobiles;
using System;

namespace Server.Items
{
    public class StarozytnyOgnistySmokAltar : PeerlessAltar
    {
        public override int KeyCount => 3;
        public override MasterKey MasterKey => new StarozytnyOgnistySmokKey();

        public override Type[] Keys => new Type[]
        {
	        typeof( PalecStarozytnegoOgnistegoSmoka ), typeof( PazurStarozytnegoOgnistegoSmoka ), typeof( ZabStarozytnegoOgnistegoSmoka ) 
};

        public override BasePeerless Boss => new NStarozytnySmok();

        [Constructable]
        public StarozytnyOgnistySmokAltar() : base(0x207C)
        {
	        
            BossLocation = new Point3D(5482, 1975, 5);
            TeleportDest = new Point3D(5483, 2024, 10);
            ExitDest = new Point3D(5557, 1942, 5);

            Name = "Oltarz Starozytnego Ognistego Smoka";
            Hue = 1070;
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
            new Rectangle2D(5464, 1956, 19, 20),
        };

        public StarozytnyOgnistySmokAltar(Serial serial) : base(serial)
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
