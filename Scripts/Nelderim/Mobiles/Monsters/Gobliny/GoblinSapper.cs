using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "zwloki goblina sapera" )]
	public class GoblinSapper : BaseCreature
	{
		[Constructable]
		public GoblinSapper() : base( AIType.AI_Melee, FightMode.Strongest, 12, 1, 0.2, 0.4 )
		{
			Name = "goblin saper";
			Body = 17;
			BaseSoundID = 0x45A;
			Hue = 2130;

			SetStr( 96, 120 );
			SetDex( 81, 105 );
			SetInt( 36, 60 );

			SetHits( 88, 92 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 50.1, 75.0 );
			SetSkill( SkillName.Tactics, 55.1, 80.0 );
			SetSkill( SkillName.Wrestling, 1000, 1000 );

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 28;

		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public void Explode()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 5 ) )
			{
				if ( !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned ) )
					list.Add( m );
				else if ( m.Player && m.AccessLevel == AccessLevel.Player )
					list.Add( m );
			}

			foreach ( Mobile m in list )
			{
				DoHarmful( m );

				m.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				m.PlaySound( 0x207 );

				m.SendMessage( "Otrzymujesz obrazenia od eksplodujacego goblina!" );

				int toDamage = Utility.RandomMinMax( 10, 30 );
				m.Damage( toDamage, this );
				
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );
			this.Kill();
			this.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
			this.PlaySound( 0x207 );
			Explode();
		}

		public override bool OnBeforeDeath()
		{
			Explode();
			this.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
			this.PlaySound( 0x207 );
			return base.OnBeforeDeath();
		}

        public override double AttackMasterChance { get { return 0.25; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 1; } }
		public override int Meat{ get{ return 1; } }


		public GoblinSapper( Serial serial ) : base( serial )
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
