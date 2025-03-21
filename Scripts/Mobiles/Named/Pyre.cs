using System;
using Nelderim;
using Server.Engines.CannedEvil;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("zwloki starego feniksa")]
    public class Pyre : BaseChampion
    {
	    public override ChampionSkullType SkullType => ChampionSkullType.None;
	    public override Type[] UniqueList => [];
	    public override Type[] SharedList => [];
	    public override Type[] DecorativeList => Type.EmptyTypes;
	    public override MonsterStatuetteType[] StatueTypes => [];
        [Constructable]
        public Pyre(): base(AIType.AI_Mage, FightMode.Aggressor)
        {
            Name = "stary feniks";
            Hue = 1560;
            BodyValue = 0x340;

            FightMode = FightMode.Closest;

            SetStr(605, 611);
            SetDex(391, 519);
            SetInt(669, 818);

            SetHits(1783, 1939);

            SetDamage(30);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 65);
            SetResistance(ResistanceType.Fire, 72, 75);
            SetResistance(ResistanceType.Poison, 36, 41);
            SetResistance(ResistanceType.Energy, 50, 51);

            SetSkill(SkillName.Wrestling, 121.9, 130.6);
            SetSkill(SkillName.Tactics, 114.4, 117.4);
            SetSkill(SkillName.MagicResist, 147.7, 153.0);
            SetSkill(SkillName.Poisoning, 122.8, 124.0);
            SetSkill(SkillName.Magery, 121.8, 127.8);
            SetSkill(SkillName.EvalInt, 103.6, 117.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);

            Fame = 21000;
            Karma = -21000;
			
			Tamable = false;

            SetWeaponAbility(WeaponAbility.BleedAttack);
            SetWeaponAbility(WeaponAbility.ParalyzingBlow);
        }

        public override bool GivesMLMinorArtifact => false;

        public Pyre(Serial serial)
            : base(serial)
        {
        }
        public override bool CanBeParagon => false;
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Paragon.ChestChance > Utility.RandomDouble())
                c.DropItem(new ParagonChest(Name, 5));
        }

        public override void GenerateLoot()
        {
            AddLoot(NelderimLoot.AvatarScrolls);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
            {
                Body = 0x5;
            }
        }
    }
}
