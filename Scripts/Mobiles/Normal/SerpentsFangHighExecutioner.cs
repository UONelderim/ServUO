using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("zwloki kata")]
    public class SerpentsFangHighExecutioner : SerpentsFangAssassin
    {
        [Constructable]
        public SerpentsFangHighExecutioner()
            : base()
        {
            Name = "Kat";
            Title = "- Przynależy do Vox Populi";
            SetStr(545, 560);
            SetDex(160, 175);
            SetInt(160, 175);

            SetHits(800);
            SetStam(190, 205);

            SetDamage(15, 20);

            Fame = 25000;
            Karma = -25000;
        }

        public SerpentsFangHighExecutioner(Serial serial)
            : base(serial)
        {
        }

        public override bool AlwaysMurderer => true;
        public override bool ShowFameTitle => false;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 6);
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from != null)
                from.Damage(damage / 2, from);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new SerpentFangKey());

            if (Utility.RandomDouble() < 0.5)
                c.DropItem(new SerpentFangSectBadge());
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