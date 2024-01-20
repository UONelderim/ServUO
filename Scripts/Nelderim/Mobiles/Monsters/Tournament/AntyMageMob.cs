
using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName( "resztki dziwnego twora wragny" )]
    public class AntyMageMob : BaseCreature
    {
        public override double DifficultyScalar{ get{ return 1.15; } }
        public override bool BleedImmune { get { return true; } }
        public override double SwitchTargetChance { get { return 0.5; } }
        public override double AttackMasterChance { get { return 0.5; } }

        public override void  AddWeaponAbilities()
        {
            WeaponAbilities.Add( WeaponAbility.BleedAttack, 0.4 );
        }
        
        private static bool OverrideRules = true;

        private static double m_AbilityChance = 0.45;

        private static int m_ReturnTime = 240;

        private static int m_MinTime    = 10;
        private static int m_MaxTime    = 20;

        private DateTime m_NextAbilityTime;

        private DateTime m_NextReturnTime;

        private ArrayList m_Minions;
        
        [Constructable]
        public AntyMageMob() : base( AIType.AI_Boss, FightMode.Strongest, 12, 1, 0.2, 0.4 )
        {
            Name = "dziwny twor Wragny";
            Body = 78;
            BaseSoundID = 412;
			Hue = 1152;

            SetStr( 324, 458 );
            SetDex( 144, 172 );
            SetInt( 1449, 1568 );

            SetHits( 1120, 1190 );

            SetDamage( 25, 37 );

            SetDamageType( ResistanceType.Physical, 20 );
            SetDamageType( ResistanceType.Cold, 40 );
            SetDamageType( ResistanceType.Energy, 40 );

            SetResistance( ResistanceType.Physical, 60, 70 );
            SetResistance( ResistanceType.Fire, 25, 35 );
            SetResistance( ResistanceType.Cold, 50, 70 );
            SetResistance( ResistanceType.Poison, 50, 70 );
            SetResistance( ResistanceType.Energy, 25, 35 );

            SetSkill( SkillName.EvalInt, 120.1, 140.0 );
            SetSkill( SkillName.Magery, 120.1, 140.0 );
            SetSkill( SkillName.Necromancy, 120.1, 140.0 );
            SetSkill( SkillName.SpiritSpeak, 120.1, 140.0 );
            SetSkill( SkillName.Meditation, 100.1, 120.0 );
            SetSkill( SkillName.Poisoning, 100.1, 120.0 );
            SetSkill( SkillName.MagicResist, 200.1, 240.0 );
            SetSkill( SkillName.Tactics, 90.1, 110.0 );
            SetSkill( SkillName.Wrestling, 75.1, 110.0 );

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 60;

            PackItem( new GnarledStaff() );
            PackNecroReg( 150, 200 );

            m_Minions = new ArrayList();
			
        }

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{			
            if( !IsBonded && !corpse.Carved && !IsChampionSpawn )
            {
				if ( Utility.RandomDouble() < 0.50 )
					corpse.DropItem( new Pumice() );		
            }

			base.OnCarve(from, corpse, with);
		}
		
		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			reflect = true; // Every spell is reflected back to the caster
		}

        public override int GetIdleSound()
        {
            return 0x19D;
        }

        public override int GetAngerSound()
        {
            return 0x175;
        }

        public override int GetDeathSound()
        {
            return 0x108;
        }

        public override int GetAttackSound()
        {
            return 0xE2;
        }

        public override int GetHurtSound()
        {
            return 0x28B;
        }

        public override void GenerateLoot()
        {
            AddLoot( LootPack.FilthyRich, 3 );
            AddLoot( LootPack.UltraRich );
            AddLoot( LootPack.HighScrolls, 2 );
        }

        public override bool Unprovokable{ get{ return false; } }
        public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
        public override int TreasureMapLevel{ get{ return 5; } }

        public override bool CanPaperdollBeOpenedBy( Mobile from )
        {
            return false;
        }

        private int CountAliveMinions()
        {
            int alive = 0;

            foreach( Mobile m in m_Minions )
            {
                if ( m.Alive && !m.Deleted ) alive++;
            }

            return alive;
        }

        private void SpawnMinions()
        {
            if ( CountAliveMinions() != 0 ) return;

            m_Minions.Clear();

            Map map = this.Map;

            if ( map == null ) return;

            int type = Utility.Random( 2 );

            ShowMorphEffect();

            switch( type )
            {
                default:
                case 0: BodyMod = Utility.RandomList( 50, 56 ); break;
                case 1: BodyMod = 3; break;
            }

            int minions = Utility.RandomMinMax( 5, 8 );

            for ( int i = 0; i < minions; ++i )
            {
                BaseCreature minion;

                switch ( type )
                {
                default:
                case 0: minion = new Skeleton(); break;
                case 1: minion = new Zombie(); break;
                }

                minion.Team = this.Team;

                bool validLocation = false;

                Point3D loc = this.Location;

                for ( int j = 0; !validLocation && j < 5; ++j )
                {
                    int x = X + Utility.Random( 8 ) - 4;
                    int y = Y + Utility.Random( 8 ) - 4;
                    int z = map.GetAverageZ( x, y );

                    if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
                        loc = new Point3D( x, y, Z );
                    else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
                        loc = new Point3D( x, y, z );
                }

                minion.MoveToWorld( loc, map );
                minion.Combatant = Combatant;

                m_Minions.Add( minion );
            }

            m_NextReturnTime = DateTime.Now + TimeSpan.FromSeconds( m_ReturnTime );
        }

        private void PoisonAttack()
        {
            Combatant.FixedParticles( 0x374A, 10, 15, 5021, EffectLayer.Waist );
            Combatant.PlaySound( 0x474 );

            Combatant.ApplyPoison( this, Poison.GetPoison( 4 ) );
        }

        public override void OnThink()
        {
            if ( BodyMod != 0 )
            {
                if ( CountAliveMinions() == 0 || DateTime.Now > m_NextReturnTime )
                {
                    m_Minions.Clear();

                    ShowMorphEffect();

                    BodyMod = 0;
                }
            }

            if ( !OverrideRules || Combatant == null )
            {
                base.OnThink();

                return;
            }

            if ( DateTime.Now >= m_NextAbilityTime )
            {
                if ( m_AbilityChance > Utility.RandomDouble() )
                {
                    if ( Utility.RandomBool() )
                        PoisonAttack();
                    else
                        SpawnMinions();
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
            }

            base.OnThink();
        }
        
        public void ShowMorphEffect()
        {
            Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 5042 );    

            Effects.PlaySound( this, this.Map, 0x201 );
        }

        public AntyMageMob( Serial serial ) : base( serial )
        {
            m_Minions = new ArrayList();
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 0 );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    }
}
