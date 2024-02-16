using Server.Engines.Harvest;

namespace Server.Items
{
    [Flipable(0xE86, 0xE85)]
    public class DrunkMinersPickaxe : BaseAxe
    {
        [Constructable]
        public DrunkMinersPickaxe()
            : base(0xE86)
        {
            Weight = 11.0;
            UsesRemaining = 50;
			Name = "Kilof Pijanego Gornika";
            ShowUsesRemaining = true;
        }

        public DrunkMinersPickaxe(Serial serial)
            : base(serial)
        {
        }

        public override HarvestSystem HarvestSystem => Mining.System;
        public override int StrengthReq => 50;
        public override int MinDamage => 12;
        public override int MaxDamage => 16;
        public override float Speed => 3.00f;
        public override int InitMinHits => 31;
        public override int InitMaxHits => 60;
        public override WeaponAnimation DefAnimation => WeaponAnimation.Slash1H;
		
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
