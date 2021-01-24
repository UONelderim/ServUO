using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("zwloki olbrzymiego drzewca")]
    public class WoodTreefellow : BaseCreature
    {

        [Constructable]
        public WoodTreefellow()
            : this(5)
        {
        }

        [Constructable]
        public WoodTreefellow(int woodAmount)
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "olbrzymi drzewiec";
            Body = 301;
            Hue = CraftResources.GetHue(CraftResource.RegularWood);

            SetStr(226, 255);
            SetDex(126, 145);
            SetInt(71, 92);

            SetHits(136, 153);

            SetDamage(9, 16);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 50.1, 95.0);
            SetSkill(SkillName.Tactics, 65.1, 100.0);
            SetSkill(SkillName.Wrestling, 65.1, 100.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 20;

            PackItem(new Log(woodAmount));

            SetWeaponAbility( WeaponAbility.Dismount );
        }

        public override double WeaponAbilityChance => 0.2;

        public override int GetIdleSound()
        {
            return 443;
        }

        public override int GetDeathSound()
        {
            return 31;
        }

        public override int GetAttackSound()
        {
            return 672;
        }

        public override bool BleedImmune { get { return true; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public WoodTreefellow(Serial serial)
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

            if (BaseSoundID == 442)
                BaseSoundID = -1;
        }
    }
}
