using System;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Targeting;

namespace Server.Spells.SkillMasteries
{
    public enum TrainingType
    {
        Empowerment,
        Berserk,
        ConsumeDamage
    }

    public class CombatTrainingSpell : SkillMasterySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Combat Training", "",
                -1,
                9002
            );

        public override double RequiredSkill { get { return 90; } }
        public override double UpKeep { get { return 12; } }
        public override int RequiredMana { get { return 30; } }
        public override bool PartyEffects { get { return false; } }
        public override SkillName CastSkill { get { return SkillName.AnimalTaming; } }

        public TrainingType SpellType { get; set; }

        private int _DamageTaken;
        private bool _Expired;

        public CombatTrainingSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            CombatTrainingSpell spell = GetSpell(Caster, typeof(CombatTrainingSpell)) as CombatTrainingSpell;

            if (spell != null)
            {
                spell.Expire();
                return false;
            }

            if (Caster is PlayerMobile && ((PlayerMobile)Caster).AllFollowers == null || ((PlayerMobile)Caster).AllFollowers.Count == 0)
            {
                Caster.SendLocalizedMessage(1156112); // This ability requires you to have pets.
                return false;
            }

            return base.CheckCast();
        }

        public override void SendCastEffect()
        {
            base.SendCastEffect();

            Caster.PrivateOverheadMessage(MessageType.Regular, 0x35, false, "You ready your pet for combat, increasing its battle effectiveness!", Caster.NetState);
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void OnSelected(TrainingType type, Mobile target)
        {
            SpellType = type;
            Target = target;

            BeginTimer();
            Server.Timer.DelayCall(TimeSpan.FromSeconds(8), CheckDamage);

            Target.FixedParticles(0x373A, 10, 80, 5018, 0, 0, EffectLayer.Waist);

            BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.CombatTraining, 1155933, 1156107, String.Format("{0}\t{1}\t{2}", SpellType.ToString(), Target.Name, ((int)ScaleUpkeep()).ToString())));
            //You train ~2_NAME~ to use ~1_SKILLNAME~.<br>Mana Upkeep: ~3_COST~

            FinishSequence();
        }

        public override void EndEffects()
        {
            BuffInfo.RemoveBuff(Caster, BuffIcon.CombatTraining);
            Caster.SendSound(0x1ED);

            _Expired = true;
        }

        protected override void DoEffects()
        {
            Caster.FixedParticles(0x376A, 10, 30, 5052, 1261, 0, EffectLayer.LeftFoot, 0);
            Caster.DisruptiveAction();
        }

        public override bool OnTick()
        {
            if (Target == null)
            {
                Expire();
                return false;
            }

            return base.OnTick();
        }

        public override double DamageModifier(Mobile victim)
        {
            if (Target == null)
                return 0.0;

            double dam = (double)_DamageTaken / (double)Target.HitsMax;

            if (dam > 1.0) dam = 1.0;

            return dam; 
        }

        private void CheckDamage()
        {
            if (_Expired)
                return;

            _DamageTaken = 0;

            Server.Timer.DelayCall(TimeSpan.FromSeconds(8), CheckDamage);
        }

        public static void CheckDamage(Mobile attacker, Mobile defender, ref int damage)
        {
            if (defender is BaseCreature && (((BaseCreature)defender).Controlled || ((BaseCreature)defender).Summoned))
            {
                Mobile master = ((BaseCreature)defender).GetMaster();

                if (master != null)
                {
                    CombatTrainingSpell spell = GetSpell(master, typeof(CombatTrainingSpell)) as CombatTrainingSpell;

                    if (spell != null)
                    {
                        switch (spell.SpellType)
                        {
                            case TrainingType.Empowerment:
                                break;
                            case TrainingType.Berserk:
                                damage = damage - (int)((double)damage * spell.DamageModifier(defender));
                                defender.FixedParticles(0x376A, 10, 30, 5052, 1261, 7, EffectLayer.LeftFoot, 0);
                                break;
                            case TrainingType.ConsumeDamage:
                                break;
                        }

                        if(spell._DamageTaken == 0)
                            defender.FixedEffect(0x3779, 10, 90, 1743, 0);

                        spell._DamageTaken = damage;
                    }
                }
            }
            else if (attacker is BaseCreature && (((BaseCreature)attacker).Controlled || ((BaseCreature)attacker).Summoned))
            {
                Mobile master = ((BaseCreature)attacker).GetMaster();

                if (master != null)
                {
                    CombatTrainingSpell spell = GetSpell(master, typeof(CombatTrainingSpell)) as CombatTrainingSpell;

                    if (spell != null)
                    {
                        switch (spell.SpellType)
                        {
                            case TrainingType.Empowerment:
                                damage = damage + (int)((double)damage * spell.DamageModifier(defender));
                                attacker.FixedParticles(0x376A, 10, 30, 5052, 1261, 7, EffectLayer.LeftFoot, 0);
                                break;
                            case TrainingType.Berserk:
                                break;
                            case TrainingType.ConsumeDamage:
                                break;
                        }
                    }
                }
            }
        }

        public static void OnCreatureHit(Mobile attacker, Mobile defender, ref int damage)
        {
            if (attacker is BaseCreature && (((BaseCreature)attacker).Controlled || ((BaseCreature)attacker).Summoned))
            {
                Mobile master = ((BaseCreature)attacker).GetMaster();

                if (master != null)
                {
                    CombatTrainingSpell spell = GetSpell(master, typeof(CombatTrainingSpell)) as CombatTrainingSpell;

                    if (spell != null)
                    {
                        switch (spell.SpellType)
                        {
                            case TrainingType.Empowerment:
                                break;
                            case TrainingType.Berserk:
                                damage = damage + (int)((double)damage * spell.DamageModifier(defender));
                                attacker.FixedParticles(0x376A, 10, 30, 5052, 1261, 7, EffectLayer.LeftFoot, 0);
                                break;
                            case TrainingType.ConsumeDamage:
                                break;
                        }
                    }
                }
            }
        }

        public static int RegenBonus(Mobile m)
        {
            if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned))
            {
                Mobile master = ((BaseCreature)m).GetMaster();

                if (master != null)
                {
                    CombatTrainingSpell spell = GetSpell(master, typeof(CombatTrainingSpell)) as CombatTrainingSpell;

                    if (spell != null)
                    {
                        return (int)(30.0 * spell.DamageModifier(m));
                    }
                }
            }

            return 0;
        }

        public static int GetHitChanceBonus(Mobile m)
        {
            if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned))
            {
                Mobile master = ((BaseCreature)m).GetMaster();

                if (master != null)
                {
                    CombatTrainingSpell spell = GetSpell(master, typeof(CombatTrainingSpell)) as CombatTrainingSpell;

                    if (spell != null)
                    {
                        return (int)(45 * spell.DamageModifier(m));
                    }
                }
            }

            return 0;
        }

        public class InternalTarget : Target
        {
            public CombatTrainingSpell Spell { get; set; }

            public InternalTarget(CombatTrainingSpell spell)
                : base(8, false, TargetFlags.None)
            {
                Spell = spell;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature && ((BaseCreature)targeted).GetMaster() == from && from.Spell == Spell)
                {
                    Spell.Caster.FixedEffect(0x3779, 10, 20, 1270, 0);
                    Spell.Caster.SendSound(0x64E);

                    int taming = (int)from.Skills[SkillName.AnimalTaming].Value;
                    int lore = (int)from.Skills[SkillName.AnimalLore].Value;

                    from.CheckTargetSkill(SkillName.AnimalTaming, (BaseCreature)targeted, taming - 25, taming + 25);
                    from.CheckTargetSkill(SkillName.AnimalLore, (BaseCreature)targeted, lore - 25, lore + 25);

                    from.CloseGump(typeof(ChooseTrainingGump));
                    from.SendGump(new ChooseTrainingGump(from, (BaseCreature)targeted, Spell));
                }
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                from.SendLocalizedMessage(1156110); // Your ability was canceled.
                Spell.FinishSequence();
            }
        }
    }

    public class ChooseTrainingGump : Gump
    {
        public CombatTrainingSpell Spell { get; private set; }
        public Mobile Caster { get; private set; }
        public BaseCreature Target { get; private set; }

        public const int Hue = 0x07FF;

        public ChooseTrainingGump(Mobile caster, BaseCreature target, CombatTrainingSpell spell) : base(100, 100)
        {
            Spell = spell;
            Caster = caster;
            Target = target;

            AddBackground(0, 0, 260, 187, 3600);
            AddAlphaRegion(10, 10, 240, 167);

            AddImageTiled(220, 15, 30, 162, 10464);

            AddHtmlLocalized(20, 20, 150, 16, 1156113, Hue, false, false); // Select Training

            AddButton(20, 40, 9762, 9763, 1, GumpButtonType.Reply, 0);
            AddHtmlLocalized(43, 40, 150, 16, 1156109, Hue, false, false); // Empowerment

            AddButton(20, 60, 9762, 9763, 2, GumpButtonType.Reply, 0);
            AddHtmlLocalized(43, 60, 150, 16, 1153271, Hue, false, false); // Berserk

            AddButton(20, 80, 9762, 9763, 3, GumpButtonType.Reply, 0);
            AddHtmlLocalized(43, 80, 150, 16, 1156108, Hue, false, false); // Consume Damage
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info.ButtonID == 0)
            {
                Spell.FinishSequence();
                state.Mobile.SendLocalizedMessage(1156110); // Your ability was canceled. 
                return;
            }

            Spell.OnSelected((TrainingType)info.ButtonID - 1, Target);
        }
    }
}
