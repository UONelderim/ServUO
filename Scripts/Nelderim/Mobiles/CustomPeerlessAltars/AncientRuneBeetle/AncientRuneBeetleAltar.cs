using Server.Mobiles;
using System;

namespace Server.Items
{
    public class AncientRuneBeetleAltar : PeerlessAltar
    {
        public override int KeyCount => 3;
        public override MasterKey MasterKey => new AncientRuneBeetleKey();

        public override Type[] Keys => new Type[]
        {
	        typeof( PieczecAncientRuneBeetle1 ), typeof( PieczecAncientRuneBeetle2 ), typeof( PieczecAncientRuneBeetle3 ) 
};
        public override string[] _Regions => new[]
        {
	        "LoenTorech_LVL1_Entrance",
	        "LoenTorech_LVL1_VeryEasy",
	        "LoenTorech_LVL1_Easy",
	        "LoenTorech_LVL1_Medium",
	        "LoenTorech_LVL1_Difficult",
	        "LoenTorech_LVL1_VeryDifficult",
	        "LoenTorech_LVL2_Entrance",
	        "LoenTorech_LVL2_VeryEasy",
	        "LoenTorech_LVL2_Easy",
	        "LoenTorech_LVL2_Medium",
	        "LoenTorech_LVL2_Difficult",
	        "LoenTorech_LVL2_VeryDifficult",
	        "LoenTorech_LVL2_Boss"
        };

        public override double _KeyDropChance => 0.10;
        public override BasePeerless Boss => new AncientRuneBeetle();

        [Constructable]
        public AncientRuneBeetleAltar() : base(0x207C)
        {
	        
            BossLocation = new Point3D(5469, 1876, 0);
            TeleportDest = new Point3D(5435, 1873, 0);
            ExitDest = new Point3D(5407, 1872, 0);

            Name = "Oltarz Starozytnego Runicznego Zuka";
            Hue = 2127;
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
            new Rectangle2D(5468, 1841, 19, 20),
        };

        public AncientRuneBeetleAltar(Serial serial) : base(serial)
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
