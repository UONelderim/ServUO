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
	        typeof( PieczecNelderimDragon1 ), typeof( PieczecNelderimDragon2 ), typeof( PieczecNelderimDragon3 ) 
};

        public override string[] _Regions => new[]
        {
	        "Wulkan_LVL1_Entrance",
	        "Wulkan_LVL1_VeryEasy",
	        "Wulkan_LVL2_Entrance",
	        "Wulkan_LVL2_Easy",
	        "Wulkan_LVL2_Easy_2",
	        "Wulkan_LVL2_Entrance2",
	        "Wulkan_LVL2_Medium",
	        "Wulkan_LVL2_Medium_2",
	        "Wulkan_LVL3_Entrance",
	        "Wulkan_LVL3_Difficult",
	        "Wulkan_LVL3_Difficult_2",
	        "Wulkan_LVL3_VeryDifficult",
	        "Wulkan_LVL4_Entrance",
	        "Wulkan_LVL4_VeryDifficult"
        };
        public override double _KeyDropChance => 0.10;
        
        public override BasePeerless Boss => new NelderimDragon();

        [Constructable]
        public ImiennySmokAltar() : base(0x207C)
        {
	        
            BossLocation = new Point3D(5609, 566, -36);
            TeleportDest = new Point3D(5628, 566, -31);
            ExitDest = new Point3D(5638, 564, -17);

            Name = "Oltarz Imiennego Smoka";
            Hue = 1574;
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
