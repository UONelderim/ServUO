using Server.Items;
using Server.Nelderim;
using System;

namespace Server.Mobiles
{
    [CorpseName("zwloki lewiatana")]
    public class Leviathan : BaseCreature
    {
        private Mobile m_Fisher;
        private DateTime m_NextWaterBall;
        
        [Constructable]
        public Leviathan()
            : this(null)
        {
        }

        [Constructable]
        public Leviathan(Mobile fisher)
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            m_Fisher = fisher;
            m_NextWaterBall = DateTime.UtcNow;

            // May not be OSI accurate; mostly copied from krakens
            Name = "lewiatan";
            Body = 77;
            BaseSoundID = 353;

            Hue = 0x481;

            SetStr(1000);
            SetDex(501, 520);
            SetInt(501, 515);

            SetHits(1500);

            SetDamage(25, 33);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Cold, 30);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 45, 55);
            SetResistance(ResistanceType.Cold, 45, 55);
            SetResistance(ResistanceType.Poison, 35, 45);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.EvalInt, 97.6, 107.5);
            SetSkill(SkillName.Magery, 97.6, 107.5);
            SetSkill(SkillName.MagicResist, 97.6, 107.5);
            SetSkill(SkillName.Meditation, 97.6, 107.5);
            SetSkill(SkillName.Tactics, 97.6, 107.5);
            SetSkill(SkillName.Wrestling, 97.6, 107.5);

            Fame = 22500;
            Karma = -22500;

            CanSwim = true;
            CantWalk = true;

            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

        public Leviathan(Serial serial)
            : base(serial)
        {
        }

        public Mobile Fisher
        {
            get { return m_Fisher; }
            set { m_Fisher = value; }
        }

        public override int DefaultHitsRegen
        {
            get
            {
                int regen = base.DefaultHitsRegen;

                return IsParagon ? regen : regen += 40;
            }
        }

        public override int DefaultStamRegen
        {
            get
            {
                int regen = base.DefaultStamRegen;

                return IsParagon ? regen : regen += 40;
            }
        }

        public override int DefaultManaRegen
        {
            get
            {
                int regen = base.DefaultManaRegen;

                return IsParagon ? regen : regen += 40;
            }
        }

        public override double TreasureMapChance => 0.25;

        public override int TreasureMapLevel => 5;

        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            if (DateTime.UtcNow >= m_NextWaterBall)
            {
                double damage = combatant.Hits * 0.3;

                if (damage < 10.0)
                    damage = 10.0;
                else if (damage > 40.0)
                    damage = 40.0;

                DoHarmful(combatant);
                MovingParticles(combatant, 0x36D4, 5, 0, false, false, 195, 0, 9502, 3006, 0, 0, 0);
                AOS.Damage(combatant, this, (int)damage, 100, 0, 0, 0, 0);

                if (combatant is PlayerMobile && combatant.Mount != null)
                {
                    (combatant as PlayerMobile).SetMountBlock(BlockMountType.DismountRecovery, TimeSpan.FromSeconds(10), true);
                }

                m_NextWaterBall = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            }
        }

        public static void GiveArtifactTo(Mobile m)
        {
            // Use the correct method from ArtifactHelper to get a random Fishing artifact
            Item item = ArtifactHelper.GetRandomArtifact(ArtGroup.Fishing);

            if (item == null)
                return;

            // Updated message to reflect Fishing theme
            if (m.AddToBackpack(item))
                m.SendMessage("Za pokonanie potężnego lewiatana, otrzymujesz starożytny artefakt z krakena.");
            else
                m.SendMessage("Ponieważ twój plecak jest pełny, nagroda za pokonanie legendarnego lewiatana została umieszczona u twoich stóp.");
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 5);
            AddLoot(LootPack.LootItem<Rope>(2));
            AddLoot(LootPack.LootItem<MessageInABottle>());
            // Added additional thematic loot
            AddLoot(LootPack.LootItem<Spyglass>(15.0));
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

            m_NextWaterBall = DateTime.UtcNow;
        }

        public override void OnKilledBy(Mobile mob)
        {
            base.OnKilledBy(mob);

            if (Paragon.CheckArtifactChance(mob, this))
            {
                GiveArtifactTo(mob);

                if (mob == m_Fisher)
                    m_Fisher = null;
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (m_Fisher != null && 25 > Utility.Random(100))
                GiveArtifactTo(m_Fisher);

            m_Fisher = null;
        }
    }
}
