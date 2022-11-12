using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "resztki szkieleta" )]
	public class SkeletalFighter : BaseCreature
	{
		private bool m_Stunning;

		public override bool IsScaredOfScaryThings{ get{ return false; } }
		//public override bool IsScaryToPets{ get{ return true; } }

		public override bool IsBondable{ get{ return false; } }

		[Constructable]
		public SkeletalFighter() : this( false, 1.0 )
		{
		}

		[Constructable]
		public SkeletalFighter( bool summoned, double scalar ) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "szkielet";
			Body = Utility.RandomList( 50, 56 );
			BaseSoundID = 0x48D;
			VirtualArmor = ((int)(10*scalar));

			SetStr( (int)(56*scalar), (int)(80*scalar) );
			SetDex( (int)(56*scalar), (int)(75*scalar) );
			SetInt( (int)(16*scalar), (int)(40*scalar) );

			SetHits( (int)(34*scalar), (int)(48*scalar) );

			SetDamage( (int)(3*scalar), (int)(7*scalar) );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, (int)(15*scalar), (int)(20*scalar) );
			SetResistance( ResistanceType.Fire, (int)(5*scalar), (int)(10*scalar) );
			SetResistance( ResistanceType.Cold, (int)(25*scalar), (int)(40*scalar) );
			SetResistance( ResistanceType.Poison, (int)(25*scalar), (int)(35*scalar) );
			SetResistance( ResistanceType.Energy, (int)(5*scalar), (int)(15*scalar) );

			if ( summoned )
				SetResistance( ResistanceType.Fire, (int)(5*scalar), (int)(10*scalar) );
			else
				SetResistance( ResistanceType.Fire, (int)(5*scalar), (int)(10*scalar) );

			SetResistance( ResistanceType.Cold, (int)(25*scalar), (int)(40*scalar) );
			SetResistance( ResistanceType.Poison, (int)(25*scalar), (int)(35*scalar) );
			SetResistance( ResistanceType.Energy, (int)(5*scalar), (int)(15*scalar) );

			SetSkill( SkillName.MagicResist, (45.1*scalar), (60.0*scalar) );
			SetSkill( SkillName.Tactics, (45.1*scalar), (60.0*scalar) );
			SetSkill( SkillName.Wrestling, (45.1*scalar), (55.0*scalar) );

			if ( summoned )
			{
				Fame = 10;
				Karma = -10;
			}
			else
			{
				Fame = 450;
				Karma = -450;
			}

			if ( !summoned )
			{
				PackItem( new Bones() );

				if ( 0.1 > Utility.RandomDouble() )
					PackItem( new VileCrystal() );

				if ( 0.15 > Utility.RandomDouble() )
					PackItem( new Skull() );

				if ( 0.2 > Utility.RandomDouble() )
					PackItem( new RibCage() );

				if ( 0.25 > Utility.RandomDouble() )
					PackItem( new Spine() );
			}

			ControlSlots = 2;
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

		public SkeletalFighter( Serial serial ) : base( serial )
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