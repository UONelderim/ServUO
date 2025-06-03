using Server.Mobiles;
using System;

namespace Server.Items
{
    public class GorogonAltar : PeerlessAltar
    {
        public override int KeyCount => 3;
        public override MasterKey MasterKey => new GorogonKey();

		public override Type[] Keys => new Type[]
		{
			typeof( PieczecGorogon1 ), typeof( PieczecGorogon2 ), typeof( PieczecGorogon3 ) 
		};

		public override string[] _Regions => new[]
		{
			"Gorogon_VeryEasy",
			"Gorogon_Easy",
			"Gorogon_Medium",
			"Gorogon_Difficult",
			"Gorogon_VeryDifficult"
		};

		public override double _KeyDropChance => 0.10;

        public override BasePeerless Boss => new NGorogon();

        [Constructable]
        public GorogonAltar() : base(0x207C)
        {
	        
            BossLocation = new Point3D(5849, 71, 0);
            TeleportDest = new Point3D(5862, 75, 0);
            ExitDest = new Point3D(5838, 106, 1);

            Name = "Oltarz Gorogona";
            Hue = 38;
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
            new Rectangle2D(5830, 55, 19, 20),
        };

        public GorogonAltar(Serial serial) : base(serial)
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
