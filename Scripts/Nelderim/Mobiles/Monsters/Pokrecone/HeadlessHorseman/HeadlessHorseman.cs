using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("zwloki Ichabod Crane")]
    public class HeadlessHorseman : BaseCreature
    {
        public override bool ShowFameTitle => false;

        public override int GetIdleSound()
        {
            return 0x482;
        }

        public override int GetDeathSound()
        {
            return 0x485;
        }

        public override int GetAttackSound()
        {
            return 0x47F;
        }

        public override int GetHurtSound()
        {
            return Hits > 1000 ? 0x480 :
                Hits > 500 ? 0x484 :
                Hits > 100 ? 0x44A :
                0x486;
        }

        public override int GetAngerSound()
        {
            return 0x481;
        }

        [Constructable]
        public HeadlessHorseman() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Ichabod Crane";
            Body = 185;
            BaseSoundID = 0x47F;
            Hue = 0x4001;

            SetStr(430, 500);
            SetDex(90, 120);
            SetInt(700, 800);

            SetHits(7000, 9000);
            SetMana(500);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Fire, 25);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 40, 65);

            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Swords, 120.0);

            PlateChest chest = new PlateChest();
            chest.Hue = 1345;
            chest.Movable = false;
            AddItem(chest);

            PlateArms arms = new PlateArms();
            arms.Hue = 1345;
            arms.Movable = false;
            AddItem(arms);

            PlateGloves gloves = new PlateGloves();
            gloves.Hue = 1345;
            gloves.Movable = false;
            AddItem(gloves);

            PlateGorget gorget = new PlateGorget();
            gorget.Hue = 1345;
            gorget.Movable = false;
            AddItem(gorget);

            PlateLegs legs = new PlateLegs();
            legs.Hue = 1345;
            legs.Movable = false;
            AddItem(legs);

            Cloak cloak = new Cloak();
            cloak.Hue = 1175;
            cloak.Movable = false;
            AddItem(cloak);

            HessianSword weapon = new HessianSword();
            weapon.Movable = false;
            AddItem(weapon);

            Lantern lantern = new Lantern();
            lantern.Movable = false;
            lantern.Ignite();
            AddItem(lantern);

            ThighBoots boots = new ThighBoots();
            boots.Movable = false;
            AddItem(boots);

            AddItem(new Gold(500, 1000));

            new Nightmare().Rider = this;
        }

        public override bool BardImmune => true;
        public override bool AutoDispel => false;
        public override Poison PoisonImmune => Poison.Lethal;
        public override bool AlwaysMurderer => true;

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (from != null && from != this)
            {
                AOS.Damage(from, this, Utility.RandomMinMax(5, 15), 50, 0, 50, 0, 0);

                MovingEffect(from, 0xC6B, 10, 0, false, false, 0, 0);
                PlaySound(0x491);

                if (0.05 > Utility.RandomDouble())
                    Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => ThrowPumpkin_Callback(from));
            }
        }

        public virtual void ThrowPumpkin_Callback(Mobile from)
        {
            Map map = from.Map;

            if (map == null)
                return;

            int count = Utility.RandomMinMax(1, 2);

            for (int i = 0; i < count; ++i)
            {
                int x = from.X + Utility.RandomMinMax(-1, 1);
                int y = from.Y + Utility.RandomMinMax(-1, 1);
                int z = from.Z;

                if (!map.CanFit(x, y, z, 16, false, true))
                {
                    z = map.GetAverageZ(x, y);

                    if (z == from.Z || !map.CanFit(x, y, z, 16, false, true))
                        continue;
                }

                PumpkinBomb pumpkin = new PumpkinBomb();
                pumpkin.MoveToWorld(new Point3D(x, y, z), map);
            }
        }

        public override bool OnBeforeDeath()
        {
            switch (Utility.Random(20))
            {
                case 0:
                    PackItem(new PumpkinLantern());
                    break;
            }

            PackItem(new Gold(1000));

            Hue = 0;

            return base.OnBeforeDeath();
        }

        public HeadlessHorseman(Serial serial) : base(serial)
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
