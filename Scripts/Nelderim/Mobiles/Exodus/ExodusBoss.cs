using Nelderim;
using Server.Items;
using System;

namespace Server.Mobiles
{
	[CorpseName( "Zwloki pancernego mechanicznego straznika" )]
	public class ExodusBoss : BasePeerless
	{
		private bool m_FieldActive;
		public bool FieldActive => m_FieldActive;

		public bool CanUseField => (Hits - 1) % 2500 > 2240;

		public override bool IsScaredOfScaryThings => false;
		public override bool IsScaryToPets => true;

		[Constructable]
		public ExodusBoss() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{
			Name = "Pancerny mechaniczny straznik";
			Body = 0x2F5;
			Hue = 2391;

			SetStr( 851, 950 );
			SetDex( 71, 80 );
			SetInt( 61, 90 );

			SetHits( 25000 );

			SetDamage( 16, 22 );

			SetResistance( ResistanceType.Physical, 60, 70 );
			SetResistance( ResistanceType.Fire, 50 );
			SetResistance( ResistanceType.Cold, 50);
			SetResistance( ResistanceType.Poison, 50 );
			SetResistance( ResistanceType.Energy, 50 );

			SetSkill( SkillName.MagicResist, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 101.1, 106.0 );

			Fame = 21000;
			Karma = -21000;
			VirtualArmor = 65;

			switch( Utility.Random( 3 ) )
			{
				case 0: PackItem( new PowerCrystal() ); break;
				case 1: PackItem( new ArcaneGem() ); break;
				case 2: PackItem( new ClockworkAssembly() ); break;
			}

            if (Utility.RandomDouble() < .10)
                PackItem(new IronWire());

			PackItem(new PowerGeneratorKey());

			m_FieldActive = CanUseField;
		}

		public override void GenerateLoot()
		{
			AddLoot(NelderimLoot.MysticScrolls);
		}
		
		public override bool AutoDispel => true;
		public override Poison PoisonImmune => Poison.Lethal;

		public override int GetIdleSound()
		{
			return 0x218;
		}

		public override int GetAngerSound()
		{
			return 0x26C;
		}

		public override int GetDeathSound()
		{
			return 0x211;
		}

		public override int GetAttackSound()
		{
			return 0x232;
		}

		public override int GetHurtSound()
		{
			return 0x140;
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( m_FieldActive )
				damage = 0; // no melee damage when the field is up
		}

		public override void AlterSpellDamageFrom( Mobile from, ref int damage )
		{
			if ( !m_FieldActive )
				damage = 0; // no spell damage when the field is down
		}

		public override void OnDamagedBySpell( Mobile from )
		{
			if( from != null && from.Alive && 0.4 > Utility.RandomDouble() )
			{
				SendEBolt( from );
			}

			if ( !m_FieldActive )
			{
				// should there be an effect when spells nullifying is on?
				this.FixedParticles( 0, 10, 0, 0x2522, EffectLayer.Waist );

				from.SendAsciiMessage("Twoje czary nie szkodza stworzeniu gdy nie oslania sie magiczna bariera");
			}
			else if ( m_FieldActive && !CanUseField )
			{
				m_FieldActive = false;

				// TODO: message and effect when field turns down; cannot be verified on OSI due to a bug
				this.FixedParticles( 0x3735, 1, 30, 0x251F, EffectLayer.Waist );
			}
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( m_FieldActive )
			{
				this.FixedParticles( 0x376A, 20, 10, 0x2530, EffectLayer.Waist );

				PlaySound( 0x2F4 );

				attacker.SendAsciiMessage("Twoja bron nie moze przebic magicznej bariery tego stworzenia");
			}
			else if( !m_FieldActive && CanUseField )
			{
				m_FieldActive = true;
			}

			if( attacker != null && attacker.Alive && attacker.Weapon is BaseRanged && 0.4 > Utility.RandomDouble() )
			{
				SendEBolt( attacker );
			}
		}

		public override void OnThink()
		{
			base.OnThink();

			// TODO: an OSI bug prevents to verify if the field can regenerate or not
			if ( !m_FieldActive && !IsHurt() )
				m_FieldActive = true;
		}

		public override bool Move( Direction d )
		{
			bool move = base.Move( d );

			if ( move && m_FieldActive && this.Combatant != null )
				this.FixedParticles( 0, 10, 0, 0x2530, EffectLayer.Waist );

			return move;
		}

		public void SendEBolt( Mobile to )
		{
			this.MovingParticles( to, 0x379F, 7, 0, false, true, 0xBE3, 0xFCB, 0x211 );
			to.PlaySound( 0x229 );
			this.DoHarmful( to );
			AOS.Damage( to, this, 50, 0, 0, 0, 0, 100 );
		}

		public override void OnDeath(Container c)
		{
			base.OnDeath(c); 
			
			Point3D moongateLocation = new Point3D(5497, 211, -42);
			Map targetMap = Map.Felucca; 
			
			Point3D destinationLocation = new Point3D(5511, 211, -32);
			Map destinationMap = Map.Felucca;

			Moongate portal = new Moongate(destinationLocation, destinationMap)
			{
				Name = "Portal do wyjścia",
				Hue = 2108, 
				Dispellable = false,
				ItemID = 0x1FD4,
			};
			
			portal.MoveToWorld(moongateLocation, targetMap);
			
			Timer.DelayCall(TimeSpan.FromMinutes(10), () =>
			{
				if (portal != null && !portal.Deleted)
					portal.Delete();
			});
		}

		
		public ExodusBoss( Serial serial ) : base( serial )
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

			m_FieldActive = CanUseField;
		}
	}
}
