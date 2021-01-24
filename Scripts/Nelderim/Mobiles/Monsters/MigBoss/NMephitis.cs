using System;
using System.Collections;
using Server.Items;
using Server.Engines.CannedEvil;

namespace Server.Mobiles
{
    public class NMephitis : BaseChampion
    {
        public override ChampionSkullType SkullType { get { return ChampionSkullType.Venom; } }

        public override Type[] UniqueList { get { return new Type[] { typeof( Calm ) }; } }
        public override Type[] SharedList { get { return new Type[] { typeof( OblivionsNeedle ), typeof( ANecromancerShroud ), typeof( EmbroideredOakLeafCloak ), typeof( TheMostKnowledgePerson ) }; } }
        public override Type[] DecorativeList { get { return new Type[] { typeof( Web ), typeof( MonsterStatuette ) }; } }
        public override MonsterStatuetteType[] StatueTypes { get { return new MonsterStatuetteType[] { MonsterStatuetteType.Spider }; } }

        private const double ChanceToThrowWeb = 0.25; // 25% chance for throwing web

        public static Hashtable m_Table = new Hashtable();

        public static bool UnderWebEffect( Mobile m )
        {
            return m_Table.Contains( m );
        }

        [Constructable]
        public NMephitis() : base( AIType.AI_Melee )
        {
            Body = 173;
            Name = "Mephitis";

            BaseSoundID = 0x183;

            SetStr( 505, 1000 );
            SetDex( 102, 300 );
            SetInt( 402, 600 );

            SetHits( 3000 );
            SetStam( 105, 600 );

            SetDamage( 21, 33 );

            SetDamageType( ResistanceType.Physical, 50 );
            SetDamageType( ResistanceType.Poison, 50 );

            SetResistance( ResistanceType.Physical, 75, 80 );
            SetResistance( ResistanceType.Fire, 60, 70 );
            SetResistance( ResistanceType.Cold, 60, 70 );
            SetResistance( ResistanceType.Poison, 100 );
            SetResistance( ResistanceType.Energy, 60, 70 );

            SetSkill( SkillName.MagicResist, 70.7, 140.0 );
            SetSkill( SkillName.Tactics, 97.6, 100.0 );
            SetSkill( SkillName.Wrestling, 97.6, 100.0 );

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80;
            //PSDropCount = 3;
            //TotalGoldDrop = 25000;
        }

        public override void GenerateLoot()
        {
            AddLoot( LootPack.UltraRich, 3 );
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }

        public override void OnGotMeleeAttack( Mobile attacker )
        {
            base.OnGotMeleeAttack( attacker );

            Mobile person = attacker;

            if ( attacker is BaseCreature )
            {
                if ( ((BaseCreature)attacker).Summoned )
                {
                    person = ((BaseCreature)attacker).SummonMaster;
                }
            }

            if ( person == null )
                return;

            if ( ChanceToThrowWeb >= Utility.RandomDouble() && (person is PlayerMobile) && !UnderWebEffect( person ) )
            {
                Direction = GetDirectionTo( person );
                MovingEffect( person, 0xF7E, 10, 1, true, false, 0x496, 0 );

                MephitisCocoon mCocoon = new MephitisCocoon( (PlayerMobile)person );
                m_Table[person] = true;

                mCocoon.MoveToWorld( person.Location, person.Map );
                mCocoon.Movable = false;
            }
        }

        public NMephitis( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
        //protected override PowerScroll CreateRandomPowerScroll()
        //{
        //    int level;
        //    double random = Utility.RandomDouble();

        //    if ( 0.05 >= random )
        //        level = 20; // 5%
        //    else if ( 0.20 >= random )
        //        level = 15; // 15%
        //    else if ( 0.50 >= random )
        //        level = 10; // 30%
        //    else
        //        level = 5; // 50%

        //    return PowerScroll.CreateRandomNoCraft( level, level );
        //}
        public class MephitisCocoon : Item
        {
            DelayTimer m_Timer;

            public MephitisCocoon( PlayerMobile target ) : base( 0x10da )
            {
                Weight = 1.0;
                int nCocoonID = (int)(3 * Utility.RandomDouble());
                ItemID = 4314 + nCocoonID; // is this correct itemid?

                int oldBodyValue = target.BodyMod;

                target.Frozen = true;
                //target.BodyMod = target.Female ? 0x191 : 0x190;
                target.BodyMod = 0x0;
                target.Hidden = true;

                target.SendLocalizedMessage( 1042555 ); // You become entangled in the spider web.

                m_Timer = new DelayTimer( this, target, oldBodyValue );
                m_Timer.Start();
            }

            public MephitisCocoon( Serial serial ) : base( serial )
            {
            }

            public override void Serialize( GenericWriter writer )
            {
                base.Serialize( writer );

                writer.Write( (int)0 ); // version
            }

            public override void Deserialize( GenericReader reader )
            {
                base.Deserialize( reader );

                int version = reader.ReadInt();
            }
        }

        private class DelayTimer : Timer
        {
            private PlayerMobile m_Target;
            private MephitisCocoon m_Cocoon;
            private int m_OldBodyValue;

            private int m_Ticks;

            public DelayTimer( MephitisCocoon mCocoon, PlayerMobile target, int bodyvalue ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
            {
                m_Target = target;
                m_Cocoon = mCocoon;
                m_OldBodyValue = bodyvalue;

                m_Ticks = 0;
            }

            protected override void OnTick()
            {
                m_Ticks++;

                if ( !m_Target.Alive )
                {
                    FreeMobile( true );
                    return;
                }

                if ( m_Ticks != 6 )
                    return;

                FreeMobile( true );
            }

            public void FreeMobile( bool Recycle )
            {
                if ( !m_Target.Deleted )
                {
                    m_Target.Frozen = false;
                    m_Target.SendLocalizedMessage( 1042532 ); // You free yourself from the web!

                    NMephitis.m_Table.Remove( m_Target );

                    if ( m_Target.Alive )
                    {
                        m_Target.BodyMod = m_OldBodyValue;
                        m_Target.Hidden = false;
                    }
                }

                if ( Recycle )
                    m_Cocoon.Delete();

                this.Stop();
            }
        }

    }
}
