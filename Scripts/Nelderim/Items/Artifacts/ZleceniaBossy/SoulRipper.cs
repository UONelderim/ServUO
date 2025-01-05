using Server.Mobiles;


namespace Server.Items
{
    public class SoulRipper : Katana
    {
        [Constructable]
        public SoulRipper()
        {
            Hue = 1152;
            Name = "Wysysacz Dusz";
            Weight = 5.0;
            Slayer = SlayerName.Silver;
            WeaponAttributes.HitLeechMana = 50;
            Attributes.WeaponSpeed = 20;
            WeaponAttributes.HitHarm = 30;
            Attributes.WeaponDamage = 50;
            AosElementDamages.Energy = 100;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1049644, "Z kazdym Twym uderzeniem, Twa dusza slabnie, jesli nie jestes morderca");
        }

        public SoulRipper(Serial serial) : base(serial)
        {
        }

        public override int InitMinHits => 50;

        public override int InitMaxHits => 50;


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

        public void OnHit(Mobile attacker, Mobile defender, double damageBonus)
        {
            base.OnHit(attacker, defender, damageBonus);

            PlayerMobile playerMobile = attacker as PlayerMobile;
            if (playerMobile != null)
            {
                if (attacker.Kills < 5)
                {
                    playerMobile.Damage(8);
                    attacker.FixedParticles(0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot);
                    attacker.PlaySound(0x208);
                    playerMobile.SendMessage("Twoja dusza cierpi czarny pomiocie!");
                }
            }

        }
    }
}
