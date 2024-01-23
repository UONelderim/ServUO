using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("resztki poslanca smierci")]
    public class Lucifer : BaseCreature
    {
        [Constructable]
        public Lucifer()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Posłaniec Śmierci";
            Body = 40;
            BaseSoundID = 357;

            SetStr(9860, 11850);
            SetDex(1770, 2550);
            SetInt(1510, 2500);

            SetHits(39200, 51100);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 100.1, 200.0);
            SetSkill(SkillName.EvalInt, 100.1, 200.0);
            SetSkill(SkillName.Magery, 100.5, 200.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            PackItem(new Longsword());
        }

        public Lucifer(Serial serial)
            : base(serial)
        {
        }

        public override bool CanRummageCorpses => true;
        public override Poison HitPoison => Poison.Lethal;
        public override Poison PoisonImmune => Poison.Lethal;
        public override double HitPoisonChance => 0.8;
        public override int TreasureMapLevel => 5;
        public override int Meat => 1;

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
