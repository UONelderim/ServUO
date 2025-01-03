using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
    public class WyzywajaceOdzienieBarbarzyncy : StuddedBustierArms
    {
        private const double LethalPoisonChance = 0.1;

        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        public override int BaseColdResistance { get { return 7; } }
        public override int BaseEnergyResistance { get { return 7; } }
        public override int BasePhysicalResistance { get { return -15; } }
        public override int BasePoisonResistance { get { return 17; } }
        public override int BaseFireResistance { get { return 18; } }

        [Constructable]
        public WyzywajaceOdzienieBarbarzyncy()
        {
            Hue = 2071;
            Name = "Wyzywajace Odzienie Barbarzyncy";
            Attributes.WeaponDamage = 5;
            SkillBonuses.SetValues(0, SkillName.Camping, 10.0);
            SkillBonuses.SetValues(0, SkillName.MagicResist, 10.0);
            Label1 = "*do woreczka z badndażami przyczepinego do tuniki, przyklejony jest tez gruczol wydzielajacy trucizne*";
        }

        public WyzywajaceOdzienieBarbarzyncy(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override bool OnEquip(Mobile from)
        {
            if (base.OnEquip(from))
            {
                if (from is PlayerMobile player && Utility.RandomDouble() <= LethalPoisonChance)
                {
                    // Apply Lethal poison to the wearer
                    player.ApplyPoison(player, Poison.Lethal);
                }

                return true;
            }

            return false;
        }


        public void OnRemoved(IEntity parent)
        {
            base.OnRemoved(parent);

            if (parent is Mobile mobile)
            {
                // Remove the Lethal poison effect
                mobile.CurePoison(mobile);
            }
        }
    }
}
