using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("zwloki dzikiego wilka z klanu drapaczy")]
    public class ClanSSW : BaseCreature
    {
        [Constructable]
        public ClanSSW()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "dziki wilk z klanu drapaczy";
            Body = 98;
            Hue = 0x2C;
            BaseSoundID = 229;

            SetStr(170);
            SetDex(244);
            SetInt(57);

            SetHits(65);

            SetDamage(8, 10);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 80);

            SetResistance(ResistanceType.Physical, 30, 35);
            SetResistance(ResistanceType.Cold, 40, 45);
            SetResistance(ResistanceType.Poison, 25, 30);
            SetResistance(ResistanceType.Energy, 20, 25);

            SetSkill(SkillName.Swords, 99.0, 100.0);
            SetSkill(SkillName.MagicResist, 41.5, 42.5);
            SetSkill(SkillName.Tactics, 65.1, 70.0);
            SetSkill(SkillName.Wrestling, 42.3, 45.5);

            Fame = 3400;
            Karma = -3400;

            SetWeaponAbility(WeaponAbility.ParalyzingBlow);
        }

        public ClanSSW(Serial serial)
            : base(serial)
        {
        }

        public override int Meat => 1;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override PackInstinct PackInstinct => PackInstinct.Canine;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}