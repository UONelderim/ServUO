using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("zwloki lorda juka")]
    public class JukaLord : BaseCreature
    {
        public override double HealChance => 1.0;

        [Constructable]
        public JukaLord()
            : base(AIType.AI_Archer, FightMode.Closest, 10, 3, 0.2, 0.4)
        {
            Name = "juka lord";
            Body = 766;

            SetStr(401, 500);
            SetDex(81, 100);
            SetInt(151, 200);

            SetHits(241, 300);

            SetDamage(10, 12);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 45, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 20, 25);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 90.1, 100.0);
            SetSkill(SkillName.Archery, 95.1, 100.0);
            SetSkill(SkillName.Healing, 80.1, 100.0);
            SetSkill(SkillName.MagicResist, 120.1, 130.0);
            SetSkill(SkillName.Swords, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 95.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 15000;
            Karma = -15000;

            SetWearable(new JukaBow(), dropChance: 1);
        }

        public JukaLord(Serial serial)
            : base(serial)
        {
        }

        public override bool AlwaysMurderer => true;
        public override bool CanRummageCorpses => true;
        public override int Meat => 1;
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.LootItemCallback(AddLootContainer));
        }

        private Item AddLootContainer(IEntity e)
        {
            var pack = new Backpack();

            pack.DropItem(new Arrow(Utility.RandomMinMax(25, 35)));
            pack.DropItem(new Arrow(Utility.RandomMinMax(25, 35)));
            pack.DropItem(new Bandage(Utility.RandomMinMax(5, 15)));
            pack.DropItem(new Bandage(Utility.RandomMinMax(5, 15)));
            pack.DropItem(Loot.RandomGem());
            pack.DropItem(new ArcaneGem());
            AddLoot(LootPack.LootItem<PalecKoscianegoSmoka>(30.0));

            return pack;
        }
        
        private static readonly string[] _ToSay =
        [
	        "{0}!!  Musisz sie bardziej postarac!",
	        "{0}!!  Przygotuj sie na porazke!",
	        "{0}!!  Me armie Cie zmiazdza!",
	        "{0}!!  Zaplacisz za to!"
        ];

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (from != null && !willKill && amount > 5 && from.Player && 5 > Utility.Random(100))
            {
                Say(true, String.Format(_ToSay[Utility.Random(_ToSay.Length)], from.Race.GetName(Cases.Wolacz)));
            }

            base.OnDamage(amount, from, willKill);
        }

        public override int GetIdleSound()
        {
            return 0x262;
        }

        public override int GetAngerSound()
        {
            return 0x263;
        }

        public override int GetHurtSound()
        {
            return 0x1D0;
        }

        public override int GetDeathSound()
        {
            return 0x28D;
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
