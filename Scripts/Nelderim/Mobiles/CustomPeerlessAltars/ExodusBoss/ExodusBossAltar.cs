using Server.Mobiles;
using System;

namespace Server.Items
{
    public class ExodusBossAltar : PeerlessAltar
    {
        public override int KeyCount => 3;
        public override MasterKey MasterKey => new ExodusBossKey();

        public override Type[] Keys => new Type[]
        {
	        typeof( PieczecExoddusBoss1 ), typeof( PieczecExoddusBoss2 ), typeof( PieczecExoddusBoss3 ) 
};  

        public override string[] _Regions => new[]
        {
	        "MechanicznaKrypta_VeryEasy",
	        "MechanicznaKrypta_Easy",
	        "MechanicznaKrypta_Medium",
	        "MechanicznaKrypta_Difficult",
	        "MechanicznaKrypta_VeryDifficult"
        };
        public override double _KeyDropChance => 0.10;

        public override BasePeerless Boss => new ExodusBoss();

        [Constructable]
        public ExodusBossAltar() : base(0x207C)
        {
	        
            BossLocation = new Point3D(5492, 216, -44);
            TeleportDest = new Point3D(5504, 210, -42);
            ExitDest = new Point3D(5510, 211, -32);

            Name = "Oltarz Pancernego Straznika";
            Hue = 2210;
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
            new Rectangle2D(5800, 1762, 19, 20),
        };

        public ExodusBossAltar(Serial serial) : base(serial)
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
