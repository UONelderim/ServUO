using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class MordercaMorrlok : BaseCreature
	{
		public override double DifficultyScalar{ get{ return 1.15; } }
	    private Timer m_SoundTimer;
	    private bool m_HasTeleportedAway;
              
		[Constructable]
		public MordercaMorrlok() : base( AIType.AI_Melee, FightMode.Closest, 11, 1, 0.2, 0.4 )
		{

			Title = "- skrytobojca";
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new Skirt( Utility.RandomNeutralHue() ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}


			SetStr( 196, 215 );
			SetDex( 105, 180 );
			SetInt( 51, 65 );
			SetHits( 200, 300 );
			SetDamage( 13, 20 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );


			SetSkill( SkillName.Fencing, 90.0, 102.5 );
			SetSkill( SkillName.Poisoning, 60.0, 82.5 );
			SetSkill( SkillName.MagicResist, 87.5, 100.0 );
			SetSkill( SkillName.Swords, 60.0, 82.5 );
			SetSkill( SkillName.Tactics, 80.0, 102.5 );
			SetSkill( SkillName.Anatomy, 80.0, 102.3 );
            SetSkill( SkillName.Hiding, 200.0 );
            SetSkill( SkillName.Stealth, 200.0 );

			Fame = 5000;
			Karma = -5000;

                	
			StuddedChest chest = new StuddedChest ();
			chest.Hue = 1109;
			chest.Movable = false;
			EquipItem ( chest );

			StuddedGorget Gorget = new StuddedGorget ();
			Gorget.Hue = 1109;
			Gorget.Movable = false;
			EquipItem( Gorget );

			StuddedGloves Gloves = new StuddedGloves ();
			Gloves.Hue = 1109;
			Gloves.Movable = false;
			EquipItem( Gloves );

			StuddedLegs legs = new StuddedLegs ();
			legs.Hue = 1109;
			legs.Movable = false;
			EquipItem ( legs );

			StuddedArms arms = new StuddedArms ();
			arms.Hue = 1109;
			arms.Movable = false;
			EquipItem ( arms );

			Cloak Cloa = new Cloak();
				Cloa.Hue = 1109;
				EquipItem ( Cloa );
			Boots Boot = new Boots ();
				Boot.Hue = 1109;
				EquipItem ( Boot );

			
			EquipItem( new Kryss() );

            SetWeaponAbility( WeaponAbility.MortalStrike );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 1 );

		}

        public override void OnCombatantChange()
	    {
		    base.OnCombatantChange();

		    if ( Hidden && Combatant != null )
			    Combatant = null;
	    }

        public virtual void SendTrackingSound()
	    {
		   if ( Hidden )
		   {
			   Effects.PlaySound( this.Location, this.Map, 0x12C );
			   Combatant = null;
		   }
		   else
		   {
			   Frozen = false;

			   if ( m_SoundTimer != null )
				m_SoundTimer.Stop();

			   m_SoundTimer = null;
		   }
	    }

        public override void OnThink()
	    {
                 
            if ( !m_HasTeleportedAway && Hits < (HitsMax / 4) )
		    {
			    Map map = this.Map;

			    if ( map != null )
			    {
				    for ( int i = 0; i < 10; ++i )
				    {
					    int x = X + (Utility.RandomMinMax( 5, 10 ) * (Utility.RandomBool() ? 1 : -1));
					    int y = Y + (Utility.RandomMinMax( 5, 10 ) * (Utility.RandomBool() ? 1 : -1));
					    int z = Z;

					    if ( !map.CanFit( x, y, z, 16, false, false ) )
					    continue;

					    Point3D from = this.Location;
					    Point3D to = new Point3D( x, y, z );

					    this.Location = to;
					    this.ProcessDelta();
					    this.Hidden = true;
					    this.Combatant = null;

					    Effects.SendLocationParticles( EffectItem.Create( from, map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
					    Effects.SendLocationParticles( EffectItem.Create(   to, map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );

					    Effects.PlaySound( to, map, 0x1FE );

					    m_HasTeleportedAway = true;
					    m_SoundTimer = Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 2.5 ), new TimerCallback( SendTrackingSound ) );

				            this.UseSkill( SkillName.Stealth );
                                            AIObject.Action = ActionType.Flee;

 					    break;
				    }
			    }
		    }
          	base.OnThink();
	    }


        public int GetRange( PlayerMobile pm )
	    {
  			return 4;
        }

        public override void OnMovement( Mobile m, Point3D oldLocation )
	    {
		    if ( m.Alive && m is PlayerMobile )
            {
		    PlayerMobile pm = (PlayerMobile)m;
  		    int range = GetRange( pm );
                           
                if ( range >= 0 && InRange( m, range ) && !InRange( oldLocation, range ) && this.Hits == this.HitsMax && this.Hidden == true && IsEnemy( m ) )
                {
                        this.Frozen = false;
                        this.Hidden = false;
                        this.Combatant = m;
                }
            }
	    }
	     
		public override bool AlwaysMurderer{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override bool ShowFameTitle{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }

		public override Poison HitPoison{ get{ return Poison.Greater; } }
		public override double HitPoisonChance{ get{ return 0.65; } }
		public MordercaMorrlok( Serial serial ) : base( serial )
		{
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
