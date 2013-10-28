using System;

namespace Server.Mobiles
{
    [CorpseName("a rotworm corpse")]
    public class RotWorm : BaseCreature
    {
        [Constructable]
        public RotWorm()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a rotworm";
            this.Body = 732;

            this.SetStr(244);
            this.SetDex(80);
            this.SetInt(17);

            this.SetHits(215);

            this.SetDamage(1, 5);

            this.SetDamageType(ResistanceType.Physical, 100);
            //SetDamageType( ResistanceType.Poison, 40 );

            this.SetResistance(ResistanceType.Physical, 37);
            this.SetResistance(ResistanceType.Fire, 30);
            this.SetResistance(ResistanceType.Cold, 35);
            this.SetResistance(ResistanceType.Poison, 73);
            this.SetResistance(ResistanceType.Energy, 26);

            this.SetSkill(SkillName.MagicResist, 25.0);
            this.SetSkill(SkillName.Tactics, 25.0);
            this.SetSkill(SkillName.Wrestling, 50.0);
        }

        public RotWorm(Serial serial)
            : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Meager);
        }

        public override int GetIdleSound()
        {
            return 1503;
        }

        public override int GetAngerSound()
        {
            return 1500;
        }

        public override int GetHurtSound()
        {
            return 1502;
        }

        public override int GetDeathSound()
        {
            return 1501;
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