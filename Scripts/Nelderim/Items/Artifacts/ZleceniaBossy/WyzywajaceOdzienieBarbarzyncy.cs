using Server.Mobiles;

namespace Server.Items
{
    public class WyzywajaceOdzienieBarbarzyncy : StuddedBustierArms
    {
        private const double LethalPoisonChance = 0.1;

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

        public override int BaseColdResistance => 7;
        public override int BaseEnergyResistance => 7;
        public override int BasePhysicalResistance => -15;
        public override int BasePoisonResistance => 17;
        public override int BaseFireResistance => 18;

        [Constructable]
        public WyzywajaceOdzienieBarbarzyncy()
        {
            Hue = 2071;
            Name = "Wyzywajace Odzienie Barbarzyncy";
            Attributes.WeaponDamage = 5;
            SkillBonuses.SetValues(0, SkillName.Camping, 10.0);
            SkillBonuses.SetValues(0, SkillName.MagicResist, 10.0);
        }
        
        public override void AddNameProperties(ObjectPropertyList list)
        {
	        base.AddNameProperties(list);
	        list.Add("*do woreczka z badndażami przyczepinego do tuniki, przyklejony jest tez gruczol wydzielajacy trucizne*");
        }

        public override bool OnEquip(Mobile from)
        {
            if (base.OnEquip(from))
            {
                if (from is PlayerMobile player && Utility.RandomDouble() <= LethalPoisonChance)
                {
                    player.ApplyPoison(player, Poison.Lethal);
                }

                return true;
            }

            return false;
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
    }
}
