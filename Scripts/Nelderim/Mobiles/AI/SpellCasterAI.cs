using System;
using Server.Items;
using Server.Spells.Third;
using Server.Spells.Sixth;
using Server.Spells.Fifth;
using Server.Spells.Seventh;
using Server.Spells.First;
using Server.Spells.Fourth;
using Server.Spells;
using System.Collections.Generic;
using Server.Spells.Necromancy;
using Server.Targeting;
using Server.Spells.Second;

namespace Server.Mobiles
{
    public class SpellCasterAI : BaseAI
    {
        protected DateTime m_NextCastTime;
        protected Combo m_SpellCombo;
        private DateTime m_NextHealTime;

        public SpellCasterAI( BaseCreature m ) : base( m )
        {
        }

        // 23.06.2012 :: zombie
        public virtual int GetMaxCircle()
        {
            double magery = m_Mobile.Skills[SkillName.Magery].Value;

            // circles 1-7, at least 75% success chance for each circle
            //int[] minSkillReq = new int[] { 12, 26, 40, 54, 68, 82, 96 };

            // Prog minimalnej szansy na rzucenie czaru nie moze byc wyzszy niz 64%, gdyz w takim
            // wypadku trenujacy pet nigdy nie przeskoczy progu (bedzie rzucal tylko niskie zaklecia).
            // Wynika to z faktu, ze kregi czarow roznia sie o 20%, a ich progi wynosza +/- 20%
            int[] minSkillReq = new int[] { 0, 14, 28, 42, 57, 71, 85 };

            for ( int i = minSkillReq.Length; i > 0; i-- )
            {
                if ( magery >= minSkillReq[i - 1] )
                    return i;
            }

            return 1;
        }

        protected virtual Spell CheckCastHealingSpell()
        {
            // If I'm poisoned, always attempt to cure.
            if ( m_Mobile.Poisoned )
                return GetCureSpell();

            // Summoned creatures never heal themselves.
            if ( m_Mobile.Summoned )
                return null;

            if ( m_Mobile.Controlled )
            {
                if ( DateTime.Now < m_NextHealTime )
                    return null;
            }

            // 40% na heal gdy ma wiecej niz 40% hp
            if ( m_Mobile.Hits > m_Mobile.HitsMax * 0.4 && Utility.RandomDouble() < 0.6 )
                return null;

            //if ( Utility.Random( 0, 4 + (m_Mobile.Hits == 0 ? m_Mobile.HitsMax : (m_Mobile.HitsMax / m_Mobile.Hits)) ) < 3 )
            //return null;

            Spell spell = null;

            if ( !MortalStrike.IsWounded( m_Mobile ) )
            {
                if ( m_Mobile.Hits < (m_Mobile.HitsMax - 50) && GetMaxCircle() >= 4 )
                {
                    spell = new GreaterHealSpell( m_Mobile, null );

                    if ( spell == null )
                        spell = new HealSpell( m_Mobile, null );
                }
                else if ( m_Mobile.Hits < (m_Mobile.HitsMax - 10) )
                    spell = new HealSpell( m_Mobile, null );
            }

            double delay;

            //if ( m_Mobile.Int >= 500 )
            delay = Utility.RandomMinMax( 1, 3 );
            //else
            //delay = Math.Sqrt( 600 - m_Mobile.Int );

            m_NextHealTime = DateTime.Now + TimeSpan.FromSeconds( delay );

            return spell;
        }

        public virtual bool CanParalyze
        {
            get
            {
                Mobile c = m_Mobile.Combatant as Mobile;
                if ( c == null )
                    return false;

                double secs = (m_Mobile.Skills[SkillName.EvalInt].Value / 10) - (c.Skills[SkillName.MagicResist].Value / 10);

                if ( !c.Player )
                    secs *= 3;

                return secs >= 3 && GetMaxCircle() >= 5 && m_Mobile.Mana >= 15;
            }
        }

        public virtual bool CanMeditate
        {
            get
            {
                return !m_Mobile.Controlled && m_Mobile.Skills[SkillName.Meditation].Value >= 70 && m_Mobile.Mana < m_Mobile.ManaMax * 0.2;
            }
        }

        public virtual bool CanPoison
        {
            get
            {
                IDamageable c = m_Mobile.Combatant;
                if ( c == null )
                    return false;

                if ( c is BaseCreature )
                {
                    BaseCreature bc = (BaseCreature)c;
                    double val = (m_Mobile.Skills[SkillName.Magery].Value + m_Mobile.Skills[SkillName.Poisoning].Value) / 2;
                    int level = 0;

                    if ( val >= 100 ) level = 3;
                    else if ( val >= 85.1 ) level = 2;
                    else if ( val >= 65.1 ) level = 1;
                    else if ( val >= 0 ) level = 0;

                    if ( bc.PoisonImmune != null )
                        return bc.PoisonImmune.Level < level;
                }

                return true;
            }
        }

        public static bool HasMod( Mobile m, bool positive )
        {
            string[] mods = new string[] { "[Magic] Str Buff", "[Magic] Dex Buff", "[Magic] Int Buff", "[Magic] Str Curse", "[Magic] Dex Curse", "[Magic] Int Curse" };

            for ( int i = 0; i < mods.Length; i++ )
            {
                string s = mods[i];
                StatMod mod = m.GetStatMod( s );

                if ( mod != null && (positive ? mod.Offset > 0 : mod.Offset < 0) )
                    return true;
            }

            return false;
        }

        protected ComboInfo[] m_SpellComboInfos = new ComboInfo[]
            {
                new ComboInfo( 17, 3, new Type[] { typeof( MindBlastSpell ), typeof( ExplosionSpell ), typeof( FireballSpell ), typeof( PoisonSpell ), typeof( EnergyBoltSpell ), typeof( MagicArrowSpell ), typeof( MagicArrowSpell ) } ),
                new ComboInfo( 17, 3, new Type[] { typeof( MagicArrowSpell ), typeof( MindBlastSpell ), typeof( MagicArrowSpell ), typeof( LightningSpell ) } ),
                new ComboInfo( 34, 6, new Type[] { typeof( MagicArrowSpell ), typeof( EnergyBoltSpell ), typeof( MindBlastSpell ) } ),
                new ComboInfo( 17, 3, new Type[] { typeof( FlameStrikeSpell ), typeof( MagicArrowSpell ), typeof( MagicArrowSpell ), typeof( LightningSpell ) } ),
                new ComboInfo( 17, 3, new Type[] { typeof( HarmSpell ), typeof( ExplosionSpell ), typeof( MagicArrowSpell ), typeof( MagicArrowSpell ), typeof( LightningSpell ) } ),
                new ComboInfo( 17, 3, new Type[] { typeof( ParalyzeSpell ), typeof( MagicArrowSpell ), typeof( HarmSpell ), typeof( LightningSpell ) } ),
                new ComboInfo( 33, 7, new Type[] { typeof( FireballSpell ), typeof( MindBlastSpell ), typeof( LightningSpell ) } ),
                new ComboInfo( 50, 7, new Type[] { typeof( ExplosionSpell ), typeof( ParalyzeSpell ), typeof( ExplosionSpell ), typeof( HarmSpell ), typeof( FireballSpell ) } ),
                new ComboInfo( 60, 6, new Type[] { typeof( ExplosionSpell ), typeof( ExplosionSpell ), typeof( EnergyBoltSpell ) } ),
                new ComboInfo( 43, 6, new Type[] { typeof( ExplosionSpell ), typeof( LightningSpell ), typeof( MindBlastSpell ), typeof( FireballSpell ), typeof( FireballSpell ) } ),
                new ComboInfo( 74, 7, new Type[] { typeof( ParalyzeSpell ), typeof( ExplosionSpell ), typeof( FlameStrikeSpell ) } ),
                new ComboInfo( 34, 6, new Type[] { typeof( ExplosionSpell ), typeof( FireballSpell ), typeof( FireballSpell ), typeof( MindBlastSpell ) } ),
                new ComboInfo( 60, 7, new Type[] { typeof( MindBlastSpell ), typeof( CurseSpell ), typeof( EnergyBoltSpell ) } ),
                new ComboInfo( 60, 7, new Type[] { typeof( MagicArrowSpell ), typeof( EnergyBoltSpell ),  typeof( FireballSpell ) } ),
                 new ComboInfo( 60, 7, new Type[] { typeof( MindBlastSpell ), typeof( MagicArrowSpell ), typeof( HarmSpell) } ),
                new ComboInfo( 24, 4, new Type[] { typeof( FireballSpell ), typeof( MagicArrowSpell ), typeof( LightningSpell ) } )
            };

        public override bool DoActionFlee()
        {
            return false;
        }

        /*
        public override bool DoActionFlee()
        {
            Mobile c = m_Mobile.Combatant;

            if ( ( m_Mobile.Mana > 20 || m_Mobile.Mana == m_Mobile.ManaMax ) && m_Mobile.Hits > ( m_Mobile.HitsMax / 2 ) )
            {
                m_Mobile.DebugSay( "I am stronger now, my guard is up" );
                Action = ActionType.Guard;
            }
            else if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
            {
                if ( m_Mobile.Debug )
                    m_Mobile.DebugSay( "I am scared of {0}", m_Mobile.FocusMob.Name );

                RunFrom( m_Mobile.FocusMob );
                m_Mobile.FocusMob = null;

                if ( m_Mobile.Poisoned && Utility.Random( 0, 5 ) == 0 )
                    new CureSpell( m_Mobile, null ).Cast();
            }
            else
            {
                m_Mobile.DebugSay( "Area seems clear, but my guard is up" );

                Action = ActionType.Guard;
                m_Mobile.Warmode = true;
            }

            return true;
        }
        */

        protected virtual Combo GetRandomCombo( BaseCreature caster, int maxCircle )
        {
            int random = Utility.Random( m_SpellComboInfos.Length );
            ComboInfo info = m_SpellComboInfos[random];
            Combo combo = new Combo( caster, info );

            if ( caster.Mana >= combo.Info.ManaReq && maxCircle >= combo.Info.CircleReq )
            {
                caster.DebugSay( String.Format( "Combo: [{0}]", combo.Info.Name ) );

                return combo;
            }

            return null;
        }

        public virtual double ScaleByNecromancy( double v )
        {
            return m_Mobile.Skills[SkillName.Necromancy].Value * v * 0.1;
        }

        public virtual double ScaleByMagery( double v )
        {
            return m_Mobile.Skills[SkillName.Magery].Value * v * 0.01;
        }

        public virtual double HealChance { get { return 0.05; } } // 5% chance to heal at gm necromancy, uses spirit speak healing
        public virtual double TeleportChance { get { return 0.05; } } // 5% chance to teleport at gm magery
        public virtual double DispelChance { get { return 0.10; } } // szansa na podjecie decyzji o dispelowaniu przy GM magii
        public virtual bool CanChannel { get { return m_Mobile.Skills[SkillName.SpiritSpeak].Value > 30.0; } }
        public virtual bool CanCastNecroSpells { get { return m_Mobile.Skills[SkillName.Necromancy].Value > 50.0; } }
        public virtual bool CanCastMagerySpells { get { return m_Mobile.Skills[SkillName.Magery].Value > 30.0; } }
        public virtual double MyNecro { get { return m_Mobile.Skills[SkillName.Magery].Value; } }
        public virtual double MyMagery { get { return m_Mobile.Skills[SkillName.Magery].Value; } }
        public virtual double MySpiritSpeak { get { return m_Mobile.Skills[SkillName.SpiritSpeak].Value; } }

        // 24.06.2012 :: zombie 
        protected Spell GetCureSpell()
        {
            // if ( (int)m_Mobile.Poison.Level >= (int)Poison.Greater.Level && GetMaxCirle() >= 4 )
            // return new ArchCureSpell( m_Mobile, null );
            // else
            return new CureSpell( m_Mobile, null );
        }
        // zombie

        public virtual bool CanCastNecroBias( int bias )
        {
            return (CanCastNecroSpells && Utility.Random( bias ) == 0);
        }

        public virtual Spell GetRandomCurseSpell()
        {
            int necro = (int)Math.Min( 5, (m_Mobile.Skills[SkillName.Necromancy].Value - 20) / 10 );
            int mage = (int)Math.Min( 5, m_Mobile.Skills[SkillName.Magery].Value / 10 );

            if ( necro >= 0 && Utility.Random( 3 ) < 2 )
            {
                switch ( Utility.Random( necro ) )
                {
                    case 0: return new CorpseSkinSpell( m_Mobile, null );
                    case 1: return new EvilOmenSpell( m_Mobile, null );
                    case 2: return new BloodOathSpell( m_Mobile, null );
                    case 3: return new MindRotSpell( m_Mobile, null );
                    default: return new EvilOmenSpell( m_Mobile, null );
                }
            }
            else
            {
                switch ( Utility.Random( mage ) )
                {
                    case 0: return new ClumsySpell( m_Mobile, null );
                    case 1: return new FeeblemindSpell( m_Mobile, null );
                    case 2: return new WeakenSpell( m_Mobile, null );
                    default: return new CurseSpell( m_Mobile, null );
                }
            }
        }

        public virtual Spell GetRandomManaDrainSpell()
        {
            if ( Utility.RandomBool() )
            {
                if ( m_Mobile.Skills[SkillName.Magery].Value >= 80.0 )
                    return new ManaVampireSpell( m_Mobile, null );
            }

            return new ManaDrainSpell( m_Mobile, null );
        }

        public virtual bool CheckDispel()
        {
            return (GetMaxCircle() >= 6 && ScaleByMagery( DispelChance ) > Utility.RandomDouble());
        }

        public virtual Spell DoDispel( Mobile toDispel )
        {
            // Warunek szansy na dispel przeniesiony do funkcji wybierajacej czar.
            // Dzieki temu mobek bedzie normalnie walczyl w obecnosci przywolancow.
            //if (GetMaxCircle() >= 6 && ScaleByMagery(DispelChance) > Utility.RandomDouble())
            if ( GetMaxCircle() >= 6 )
                return new DispelSpell( m_Mobile, null );

            return ChooseSpell( toDispel );
        }

        public virtual Spell ChooseSpell( Mobile c )
        {
            return null;
        }

        public virtual Spell GetRandomNecroCurseSpell()
        {
            switch ( Utility.Random( 4 ) )
            {
                default:
                case 0: m_Mobile.DebugSay( "Blood Oath" ); return new BloodOathSpell( m_Mobile, null );
                case 1: m_Mobile.DebugSay( "Corpse Skin" ); return new CorpseSkinSpell( m_Mobile, null );
                case 2: m_Mobile.DebugSay( "Evil Omen" ); return new EvilOmenSpell( m_Mobile, null );
                case 3: m_Mobile.DebugSay( "Mind Rot" ); return new MindRotSpell( m_Mobile, null );
            }
        }

        // 24.06.2012 :: zombie
        public virtual Spell GetRandomMageryCurseSpell()
        {
            if ( Utility.RandomBool() )
            {
                if ( GetMaxCircle() >= 4 )
                    return new CurseSpell( m_Mobile, null );
            }

            switch ( Utility.Random( 3 ) )
            {
                default:
                case 0: return new WeakenSpell( m_Mobile, null );
                case 1: return new ClumsySpell( m_Mobile, null );
                case 2: return new FeeblemindSpell( m_Mobile, null );
            }
        }
        // zombie

        public virtual Spell GetRandomDamageSpell()
        {
            if ( !CanCastMagerySpells && !CanCastNecroSpells )
                return null;
            else if ( !CanCastMagerySpells )
                return GetRandomNecroDamageSpell();
            else if ( !CanCastNecroSpells )
                return GetRandomMageryDamageSpell();
            else
            {
                double necro = m_Mobile.Skills[SkillName.Necromancy].Value;
                double magery = m_Mobile.Skills[SkillName.Magery].Value;
                double factor = necro / (necro + magery);

                if ( Utility.RandomDouble() > factor )
                    return GetRandomMageryDamageSpell();
                else
                    return GetRandomNecroDamageSpell();
            }
        }

        public virtual Spell GetRandomNecroDamageSpell()
        {
            double skill = m_Mobile.Skills[SkillName.Necromancy].Value + 20.0;

            if ( skill < 70 )
            {
                switch ( Utility.Random( 4 ) )
                {
                    case 0:
                    case 1: return new PainSpikeSpell( m_Mobile, null ); // 40
                    case 2: return new EvilOmenSpell( m_Mobile, null ); // 40
                    default: return new BloodOathSpell( m_Mobile, null ); // 40
                }
            }
            else if ( skill < 85 )
            {
                switch ( Utility.Random( 6 ) )
                {
                    case 0: return new PainSpikeSpell( m_Mobile, null ); // 40
                    case 1: return new EvilOmenSpell( m_Mobile, null ); // 40
                    case 2: return new BloodOathSpell( m_Mobile, null ); // 40
                    default: return new PoisonStrikeSpell( m_Mobile, null ); // 70
                }
            }
            else if ( skill < 100 )
            {
                switch ( Utility.Random( 7 ) )
                {
                    case 0: return new PainSpikeSpell( m_Mobile, null ); // 40
                    case 1: return new EvilOmenSpell( m_Mobile, null ); // 40
                    case 2: return new BloodOathSpell( m_Mobile, null ); // 40
                    case 3:
                    case 4: return new PoisonStrikeSpell( m_Mobile, null ); // 70
                    default: return new StrangleSpell( m_Mobile, null ); // 85
                }
            }
            else
            {
                switch ( Utility.Random( 8 ) )
                {
                    case 0: return new PainSpikeSpell( m_Mobile, null ); // 40
                    case 1: return new EvilOmenSpell( m_Mobile, null ); // 40
                    case 2: return new BloodOathSpell( m_Mobile, null ); // 40
                    case 3: return new MindRotSpell( m_Mobile, null );
                    case 4: return new PoisonStrikeSpell( m_Mobile, null ); // 70
                    case 5:
                    case 6: return new StrangleSpell( m_Mobile, null ); // 85
                    default: return new VengefulSpiritSpell( m_Mobile, null ); // 100
                }
            }
        }

        public virtual Spell GetRandomMageryDamageSpell()
        {
            int maxCirle = GetMaxCircle();

            if ( m_Mobile.Combatant != null && maxCirle >= 3 && Utility.RandomDouble() > 0.2 && m_Mobile.GetDistanceToSqrt( m_Mobile.Combatant ) <= 2 )
                return new HarmSpell( m_Mobile, null );

            switch ( Utility.Random( maxCirle * 2 ) )
            {
                case 0:
                case 1: return new MagicArrowSpell( m_Mobile, null ); // 1
                case 2: return new MagicArrowSpell( m_Mobile, null );
                case 3: return new FireballSpell( m_Mobile, null ); // 2 
                case 4: return new HarmSpell( m_Mobile, null );
                case 5: return new FireballSpell( m_Mobile, null );  // 3
                case 6:
                case 7: return new LightningSpell( m_Mobile, null ); // 4
                case 8:
                case 9: return new MindBlastSpell( m_Mobile, null ); // 5
                case 10: return new EnergyBoltSpell( m_Mobile, null );
                case 11: return new ExplosionSpell( m_Mobile, null ); // 6
                default: return new FlameStrikeSpell( m_Mobile, null ); // 7
            }
        }


        public Mobile FindDispelTarget( bool activeOnly )
        {
            if ( m_Mobile.Deleted || m_Mobile.Int < 95 || CanDispel( m_Mobile ) || m_Mobile.AutoDispel )
                return null;

            if ( activeOnly )
            {
                List<AggressorInfo> aggressed = m_Mobile.Aggressed;
                List<AggressorInfo> aggressors = m_Mobile.Aggressors;

                Mobile active = null;
                double activePrio = 0.0;

                Mobile comb = m_Mobile.Combatant as Mobile;

                if ( comb != null && !comb.Deleted && comb.Alive && !comb.IsDeadBondedPet && m_Mobile.InRange( comb, 12 ) && CanDispel( comb ) )
                {
                    active = comb;
                    activePrio = m_Mobile.GetDistanceToSqrt( comb );

                    if ( activePrio <= 2 )
                        return active;
                }

                for ( int i = 0; i < aggressed.Count; ++i )
                {
                    AggressorInfo info = aggressed[i];
                    Mobile m = info.Defender;

                    if ( m != comb && m.Combatant == m_Mobile && m_Mobile.InRange( m, 12 ) && CanDispel( m ) )
                    {
                        double prio = m_Mobile.GetDistanceToSqrt( m );

                        if ( active == null || prio < activePrio )
                        {
                            active = m;
                            activePrio = prio;

                            if ( activePrio <= 2 )
                                return active;
                        }
                    }
                }

                for ( int i = 0; i < aggressors.Count; ++i )
                {
                    AggressorInfo info = aggressors[i];
                    Mobile m = info.Attacker;

                    if ( m != comb && m.Combatant == m_Mobile && m_Mobile.InRange( m, 12 ) && CanDispel( m ) )
                    {
                        double prio = m_Mobile.GetDistanceToSqrt( m );

                        if ( active == null || prio < activePrio )
                        {
                            active = m;
                            activePrio = prio;

                            if ( activePrio <= 2 )
                                return active;
                        }
                    }
                }

                return active;
            }
            else
            {
                Map map = m_Mobile.Map;

                if ( map != null )
                {
                    Mobile active = null, inactive = null;
                    double actPrio = 0.0, inactPrio = 0.0;

                    Mobile comb = m_Mobile.Combatant as Mobile;

                    if ( comb != null && !comb.Deleted && comb.Alive && !comb.IsDeadBondedPet && CanDispel( comb ) )
                    {
                        active = inactive = comb;
                        actPrio = inactPrio = m_Mobile.GetDistanceToSqrt( comb );
                    }

                    foreach ( Mobile m in m_Mobile.GetMobilesInRange( 12 ) )
                    {
                        if ( m != m_Mobile && CanDispel( m ) )
                        {
                            double prio = m_Mobile.GetDistanceToSqrt( m );

                            if ( !activeOnly && (inactive == null || prio < inactPrio) )
                            {
                                inactive = m;
                                inactPrio = prio;
                            }

                            if ( (m_Mobile.Combatant == m || m.Combatant == m_Mobile) && (active == null || prio < actPrio) )
                            {
                                active = m;
                                actPrio = prio;
                            }
                        }
                    }

                    return active != null ? active : inactive;
                }
            }

            return null;
        }

        public override bool Think()
        {
            if ( m_Mobile.Deleted )
                return false;

            Target targ = m_Mobile.Target;

            if ( targ != null )
            {
                ProcessTarget();

                return true;
            }
            else
            {
                return base.Think();
            }
        }

        public void OnFailedMove()
        {
            if ( !m_Mobile.DisallowAllMoves && (ScaleByMagery( TeleportChance ) > Utility.RandomDouble()) )
            {
                if ( m_Mobile.Target != null )
                    m_Mobile.Target.Cancel( m_Mobile, TargetCancelType.Canceled );

                new TeleportSpell( m_Mobile, null ).Cast();

                m_Mobile.DebugSay( "I am stuck, I'm going to try teleporting away" );
            }
            else if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
            {
                if ( m_Mobile.Debug )
                    m_Mobile.DebugSay( "My move is blocked, so I am going to attack {0}", m_Mobile.FocusMob.Name );

                m_Mobile.Combatant = m_Mobile.FocusMob;
                Action = ActionType.Combat;
            }
            else
            {
                m_Mobile.DebugSay( "I am stuck" );
            }
        }

        public virtual void Run( Direction d )
        {
            if ( (m_Mobile.Spell != null && m_Mobile.Spell.IsCasting) || m_Mobile.Paralyzed || m_Mobile.Frozen || m_Mobile.DisallowAllMoves )
                return;

            m_Mobile.Direction = d | Direction.Running;

            if ( !DoMove( m_Mobile.Direction, true ) )
                OnFailedMove();
        }

        public virtual void RunTo( Mobile m )
        {
            if ( m.Paralyzed || m.Frozen )
            {
                if ( m_Mobile.InRange( m, 1 ) )
                    RunFrom( m );
                else if ( !m_Mobile.InRange( m, m_Mobile.RangeFight > 2 ? m_Mobile.RangeFight : 2 ) && !MoveTo( m, true, 1 ) )
                    OnFailedMove();
            }
            else
            {
                if ( !m_Mobile.InRange( m, m_Mobile.RangeFight ) )
                {
                    if ( !MoveTo( m, true, 1 ) )
                        OnFailedMove();
                }
                else if ( m_Mobile.InRange( m, m_Mobile.RangeFight - 1 ) )
                {
                    RunFrom( m );
                }
            }
        }

        public virtual void RunFrom( Mobile m )
        {
            Run( (m_Mobile.GetDirectionTo( m ) - 4) & Direction.Mask );
        }

        public bool CanDispel( Mobile m )
        {
            return (m is BaseCreature && ((BaseCreature)m).Summoned && m_Mobile.CanBeHarmful( m, false ) && !((BaseCreature)m).IsAnimatedDead);
        }

        private static int[] m_Offsets = new int[]
            {
                -1, -1,
                -1,  0,
                -1,  1,
                 0, -1,
                 0,  1,
                 1, -1,
                 1,  0,
                 1,  1,

                -2, -2,
                -2, -1,
                -2,  0,
                -2,  1,
                -2,  2,
                -1, -2,
                -1,  2,
                 0, -2,
                 0,  2,
                 1, -2,
                 1,  2,
                 2, -2,
                 2, -1,
                 2,  0,
                 2,  1,
                 2,  2
            };

        protected virtual bool ProcessTarget()
        {
            Target targ = m_Mobile.Target;

            if ( targ == null )
                return false;

            bool isDispel = (targ is DispelSpell.InternalTarget);
            bool isParalyze = (targ is ParalyzeSpell.InternalTarget);
            bool isTeleport = (targ is TeleportSpell.InternalTarget);
            bool isInvisible = (targ is InvisibilitySpell.InternalTarget);
            bool teleportAway = false;

            if ( m_Mobile.Hits <= m_Mobile.HitsMax * 0.3 )
                teleportAway = true;

            Mobile toTarget;

            if ( isInvisible )
            {
                toTarget = m_Mobile;
            }
            else if ( isDispel )
            {
                toTarget = FindDispelTarget( false );

                if ( toTarget != null && m_Mobile.InRange( toTarget, 10 ) )
                    RunFrom( toTarget );
            }
            else if ( isParalyze || isTeleport )
            {
                toTarget = FindDispelTarget( true );

                if ( toTarget == null )
                {
                    toTarget = m_Mobile.Combatant as Mobile;

                    if ( toTarget != null )
                        RunTo( toTarget );
                }
                else if ( m_Mobile.InRange( toTarget, 10 ) )
                {
                    RunFrom( toTarget );
                    teleportAway = true;
                }
                else
                {
                    teleportAway = true;
                }
            }
            else
            {
                toTarget = m_Mobile.Combatant as Mobile;

                if ( toTarget != null )
                    RunTo( toTarget );
            }

            if ( (targ.Flags & TargetFlags.Harmful) != 0 && toTarget != null )
            {
                if ( (targ.Range == -1 || m_Mobile.InRange( toTarget, targ.Range )) && m_Mobile.CanSee( toTarget ) && m_Mobile.InLOS( toTarget ) )
                {
                    targ.Invoke( m_Mobile, toTarget );
                }
                else if ( isDispel )
                {
                    targ.Cancel( m_Mobile, TargetCancelType.Canceled );
                }
            }
            else if ( (targ.Flags & TargetFlags.Beneficial) != 0 )
            {
                targ.Invoke( m_Mobile, m_Mobile );
            }
            else if ( isTeleport && toTarget != null )
            {
                Map map = m_Mobile.Map;

                if ( map == null )
                {
                    targ.Cancel( m_Mobile, TargetCancelType.Canceled );
                    return true;
                }

                int px, py;

                if ( teleportAway )
                {
                    int rx = m_Mobile.X - toTarget.X;
                    int ry = m_Mobile.Y - toTarget.Y;

                    double d = m_Mobile.GetDistanceToSqrt( toTarget );

                    px = toTarget.X + (int)(rx * (10 / d));
                    py = toTarget.Y + (int)(ry * (10 / d));
                }
                else
                {
                    px = toTarget.X;
                    py = toTarget.Y;
                }

                for ( int i = 0; i < m_Offsets.Length; i += 2 )
                {
                    int x = m_Offsets[i], y = m_Offsets[i + 1];

                    Point3D p = new Point3D( px + x, py + y, 0 );

                    LandTarget lt = new LandTarget( p, map );

                    if ( (targ.Range == -1 || m_Mobile.InRange( p, targ.Range )) && m_Mobile.InLOS( lt ) && map.CanSpawnMobile( px + x, py + y, lt.Z ) && !SpellHelper.CheckMulti( p, map ) )
                    {
                        targ.Invoke( m_Mobile, lt );
                        return true;
                    }
                }

                int teleRange = targ.Range;

                if ( teleRange < 0 )
                    teleRange = 12;

                for ( int i = 0; i < 10; ++i )
                {
                    Point3D randomPoint = new Point3D( m_Mobile.X - teleRange + Utility.Random( teleRange * 2 + 1 ), m_Mobile.Y - teleRange + Utility.Random( teleRange * 2 + 1 ), 0 );

                    LandTarget lt = new LandTarget( randomPoint, map );

                    if ( m_Mobile.InLOS( lt ) && map.CanSpawnMobile( lt.X, lt.Y, lt.Z ) && !SpellHelper.CheckMulti( randomPoint, map ) )
                    {
                        targ.Invoke( m_Mobile, new LandTarget( randomPoint, map ) );
                        return true;
                    }
                }

                targ.Cancel( m_Mobile, TargetCancelType.Canceled );
            }
            else
            {
                targ.Cancel( m_Mobile, TargetCancelType.Canceled );
            }

            return true;
        }
    }


    #region Combo
    public class Combo
    {
        private BaseCreature m_Caster;
        private ComboInfo m_Info;
        private List<Spell> m_Spells;
        private int m_Position;

        public Combo( BaseCreature caster, ComboInfo info )
        {
            m_Caster = caster;
            m_Spells = new List<Spell>();
            m_Position = 0;
            m_Info = new ComboInfo( info.SpellTypes );

            string[] spellNames = new string[info.SpellTypes.Length];

            for ( int i = 0; i < info.SpellTypes.Length; i++ )
            {
                Type t = (Type)info.SpellTypes[i];

                if ( !t.IsSubclassOf( typeof( Spell ) ) )
                    continue;

                Spell spell = Activator.CreateInstance( t, new object[] { caster, null } ) as Spell;
                m_Info.ManaReq += spell.GetMana();

                if ( spell is MagerySpell )
                {
                    int circle = (int)((MagerySpell)spell).Circle;

                    if ( circle > m_Info.CircleReq )
                        m_Info.CircleReq = circle;
                }

                spellNames[i] = spell.Name;
                m_Spells.Add( spell );
            }

            m_Info.Name = String.Join( ", ", spellNames );
        }

        public ComboInfo Info
        {
            get { return m_Info; }
        }

        public bool Finished
        {
            get { return m_Position > m_Spells.Count - 1; }
        }

        public BaseCreature Caster
        {
            get { return m_Caster; }
        }

        public void MoveNext()
        {
            m_Position++;
        }

        public Spell GetSpell()
        {
            return m_Spells[m_Position];
        }
    }

    public class ComboInfo
    {
        private string m_Name;
        private Type[] m_SpellTypes;
        private int m_ManaReq;
        private int m_CircleReq;

        public ComboInfo( Type[] spellTypes ) : this( 0, 1, spellTypes )
        {
        }

        public ComboInfo( int mana, int circle, Type[] spellTypes )
        {
            m_SpellTypes = spellTypes;
            m_ManaReq = mana;
            m_CircleReq = circle;
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public Type[] SpellTypes
        {
            get { return m_SpellTypes; }
        }

        public int CircleReq
        {
            get { return m_CircleReq; }
            set { m_CircleReq = value; }
        }

        public int ManaReq
        {
            get { return m_ManaReq; }
            set { m_ManaReq = value; }
        }
    }

    #endregion
}
