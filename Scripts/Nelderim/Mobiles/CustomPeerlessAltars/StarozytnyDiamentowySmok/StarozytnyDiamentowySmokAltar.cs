using Server.Mobiles;
using System;

namespace Server.Items
{
    public class StarozytnyDiamentowySmokAltar : PeerlessAltar
    {
        public override int KeyCount => 3;
        public override MasterKey MasterKey => new StarozytnyDiamentowySmokKey();

        public override Type[] Keys => new Type[]
        {
	        typeof( PalecStarozytnegoDiamentowegoSmoka ), typeof( PazurStarozytnegoDiamentowegoSmoka ), typeof( ZabStarozytnegoDiamentowegoSmoka ) 
};

        public override BasePeerless Boss => new StarozytnyDiamentowySmok();

        [Constructable]
        public StarozytnyDiamentowySmokAltar() : base(0x207C)
        {
	        
            BossLocation = new Point3D(5316, 2222, 25);
            TeleportDest = new Point3D(5316, 2237, 25);
            ExitDest = new Point3D(5317, 2256, 10);

            Name = "Oltarz Starozytnego Diamentowego Smoka";
            Hue = 1153;
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
            new Rectangle2D(5303, 2211, 19, 20),
        };

        public StarozytnyDiamentowySmokAltar(Serial serial) : base(serial)
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
