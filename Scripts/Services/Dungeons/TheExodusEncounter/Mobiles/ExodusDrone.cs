using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drone's corpse")]
    public class ExodusDrone : BaseCreature
    {
        [Constructable]
        public ExodusDrone()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Exodus Drone";
            Body = 0x2F4;
            Hue = 0xA92;
            SetStr(554, 650);
            SetDex(77, 85);
            SetInt(64, 85);

            SetHits(332, 389);
            SetDamage(13, 19);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 25, 35);
            SetResistance(ResistanceType.Poison, 25, 34);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 83.8, 95.4);
            SetSkill(SkillName.Tactics, 82.9, 94.9);
            SetSkill(SkillName.Wrestling, 84.7, 95.9);

            Fame = 18000;
            Karma = -18000;

            FieldActive = CanUseField;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.LootItem<PowerCrystal>(false, true));
            AddLoot(LootPack.LootItem<ArcaneGem>(false, true));
            AddLoot(LootPack.LootItem<ClockworkAssembly>(false, true));
        }

        public ExodusDrone(Serial serial)
            : base(serial)
        {
        }

        public bool FieldActive { get; private set; }

        public bool CanUseField => Hits >= HitsMax * 9 / 10; // TODO: an OSI bug prevents to verify this

        public override bool IsScaredOfScaryThings => false;

        public override bool IsScaryToPets => true;

        public override bool BardImmune => false;

        public override Poison PoisonImmune => Poison.Lethal;

        public override int GetIdleSound() { return 0x218; }
        public override int GetAngerSound() { return 0x26C; }
        public override int GetDeathSound() { return 0x211; }
        public override int GetAttackSound() { return 0x232; }
        public override int GetHurtSound() { return 0x140; }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (FieldActive)
                damage = 0; // no melee damage when the field is up
        }

        public override void AlterSpellDamageFrom(Mobile from, ref int damage)
        {
            if (!FieldActive)
                damage = 0; // no spell damage when the field is down
        }

        public override void OnDamagedBySpell(Mobile from)
        {
            if (from != null && from.Alive && 0.4 > Utility.RandomDouble())
            {
                SendEBolt(from);
            }

            if (!FieldActive)
            {
                // should there be an effect when spells nullifying is on?
                FixedParticles(0, 10, 0, 0x2522, EffectLayer.Waist);
            }
            else if (FieldActive && !CanUseField)
            {
                FieldActive = false;

                // TODO: message and effect when field turns down; cannot be verified on OSI due to a bug
                FixedParticles(0x3735, 1, 30, 0x251F, EffectLayer.Waist);
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (FieldActive)
            {
                FixedParticles(0x376A, 20, 10, 0x2530, EffectLayer.Waist);

                PlaySound(0x2F4);

                attacker.SendAsciiMessage("Twoja bron nie moze przebic magicznej bariery tego stworzenia");
            }

            if (attacker != null && attacker.Alive && attacker.Weapon is BaseRanged && 0.4 > Utility.RandomDouble())
            {
                SendEBolt(attacker);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // TODO: an OSI bug prevents to verify if the field can regenerate or not
            if (!FieldActive && !IsHurt())
                FieldActive = true;
        }

        public override bool Move(Direction d)
        {
            bool move = base.Move(d);

            if (move && FieldActive && Combatant != null)
                FixedParticles(0, 10, 0, 0x2530, EffectLayer.Waist);

            return move;
        }

        public void SendEBolt(Mobile to)
        {
            MovingParticles(to, 0x379F, 7, 0, false, true, 0xBE3, 0xFCB, 0x211);
            to.PlaySound(0x229);
            DoHarmful(to);
            AOS.Damage(to, this, 50, 0, 0, 0, 0, 100);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();

            FieldActive = CanUseField;
        }
    }
}
