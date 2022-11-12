using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "resztki zombie" )]
	public class ZombieMinion : BaseCreature
	{
		private bool m_Stunning;

		public override bool IsScaredOfScaryThings{ get{ return false; } }
		//public override bool IsScaryToPets{ get{ return true; } }

		public override bool IsBondable{ get{ return false; } }

		[Constructable]
		public ZombieMinion() : this( false, 1.0 )
		{
		}

		[Constructable]
		public ZombieMinion( bool summoned, double scalar ) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "zombie";
			Body = 3;
			BaseSoundID = 471;
			VirtualArmor = ((int)(10*scalar));

			SetStr( (int)(46*scalar), (int)(70*scalar) );
			SetDex( (int)(31*scalar), (int)(50*scalar) );
			SetInt( (int)(26*scalar), (int)(40*scalar) );

			SetHits( (int)(28*scalar), (int)(42*scalar) );

			SetDamage( (int)(3*scalar), (int)(7*scalar) );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, (int)(15*scalar), (int)(20*scalar) );
			SetResistance( ResistanceType.Cold, (int)(20*scalar), (int)(30*scalar) );
			SetResistance( ResistanceType.Poison, (int)(5*scalar), (int)(10*scalar) );

			if ( summoned )
				SetResistance( ResistanceType.Cold, (int)(20*scalar), (int)(30*scalar) );
			else
				SetResistance( ResistanceType.Cold, (int)(20*scalar), (int)(30*scalar) );

			SetResistance( ResistanceType.Cold, (int)(20*scalar), (int)(30*scalar) );
			SetResistance( ResistanceType.Poison, (int)(5*scalar), (int)(10*scalar) );

			SetSkill( SkillName.MagicResist, (15.1*scalar), (40.0*scalar) );
			SetSkill( SkillName.Tactics, (35.1*scalar), (50.0*scalar) );
			SetSkill( SkillName.Wrestling, (35.1*scalar), (50.0*scalar) );

			if ( summoned )
			{
				Fame = 60;
				Karma = -60;
			}
			else
			{
				Fame = 600;
				Karma = -600;
			}

			if ( !summoned )
			{
				PackItem( new Bones() );

				if ( 0.1 > Utility.RandomDouble() )
					PackItem( new taintedCrystal() );

				if ( 0.15 > Utility.RandomDouble() )
					PackItem( new Torso() );

				if ( 0.2 > Utility.RandomDouble() )
					PackItem( new RightArm() );

				if ( 0.25 > Utility.RandomDouble() )
					PackItem( new LeftArm() );
			}

			ControlSlots = 1;
		}

		public override bool DeleteOnRelease{ get{ return true; } }

		public override int GetAngerSound()
		{
			return 541;
		}

		public override int GetIdleSound()
		{
			if ( !Controlled )
				return 542;

			return base.GetIdleSound();
		}

		public override int GetDeathSound()
		{
			if ( !Controlled )
				return 545;

			return base.GetDeathSound();
		}

		public override int GetAttackSound()
		{
			return 562;
		}

		public override int GetHurtSound()
		{
			if ( Controlled )
				return 320;

			return base.GetHurtSound();
		}

		public override bool AutoDispel{ get{ return !Controlled; } }
		public override bool BleedImmune{ get{ return true; } }

		/*public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( Controlled || Summoned )
			{
				Mobile master = ( this.ControlMaster );

				if ( master == null )
					master = this.SummonMaster;

				if ( master != null && master.Player && master.Map == this.Map && master.InRange( Location, 20 ) )
				{
					if ( master.Mana >= amount )
					{
						master.Mana -= amount;
					}
					else
					{
						amount -= master.Mana;
						master.Mana = 0;
						master.Damage( amount );
					}
				}
			}

			base.OnDamage( amount, from, willKill );
		}*/

		public override bool BardImmune{ get{ return !Core.AOS || Controlled; } }
		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }

		public ZombieMinion( Serial serial ) : base( serial )
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