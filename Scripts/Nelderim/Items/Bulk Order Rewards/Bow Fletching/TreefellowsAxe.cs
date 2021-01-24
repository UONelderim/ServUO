using System;
using Server.Items;
using Server.Network;
using Server.Engines.Craft;

namespace Server.Items
{
    [FlipableAttribute(0xF43, 0xF44)]
    public class TreefellowsAxe : BaseAxe
    {
        public override int LabelNumber { get { return 3000299; } } // drzewcowa siekiera

        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ArmorIgnore; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

        public override int StrengthReq { get { return 20; } }
        public override int MinDamage { get { return 13; } }
        public override int MaxDamage { get { return 15; } }
        public override float Speed { get { return 2.75f; } }

        public override int InitMinHits { get { return 31; } }
        public override int InitMaxHits { get { return 80; } }
        
		[Constructable]
		public TreefellowsAxe() : this( 10 )
		{
		}

        [Constructable]
        public TreefellowsAxe(int uses) : base(0xF43)
        {
            Weight = 4.0;
            Hue = 0x973;
            UsesRemaining = uses;
            ShowUsesRemaining = true;
        }

        public TreefellowsAxe(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
