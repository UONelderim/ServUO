using Server.Mobiles;
using System;

namespace Server.Items
{
    public class ParoxysmusAltar2 : PeerlessAltar
    {
        public override int KeyCount => 4;
        public override MasterKey MasterKey => new ParoxysmusKey2();

        public override Type[] Keys => new Type[]
        {
	        typeof( CoagulatedLegs ), typeof( PartiallyDigestedTorso ),
	        typeof( GelatanousSkull ), typeof( SpleenOfThePutrefier )
};

        public override BasePeerless Boss => new ChiefParoxysmus();

        [Constructable]
        public ParoxysmusAltar2() : base(0x207A)
        {
	        
	        BossLocation = new Point3D(6950, 241, 0);
	        TeleportDest = new Point3D(6932, 246, 0);
	        ExitDest = new Point3D(6928, 242, 0);

            Name = "Oltarz Oblesnego Kucharza";
            Hue = 0x465;
        }

        public override Rectangle2D[] BossBounds => m_Bounds;

        private readonly Rectangle2D[] m_Bounds = new Rectangle2D[]
        {
	        new Rectangle2D(147, 1345, 35, 48),
        };

        public ParoxysmusAltar2(Serial serial) : base(serial)
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
