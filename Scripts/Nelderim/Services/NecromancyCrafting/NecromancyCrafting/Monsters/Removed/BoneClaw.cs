using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "resztki koscianego pazura" )]
	public class BoneClaw : BaseCreature
	{
		private bool m_Stunning;

		public override bool IsScaredOfScaryThings{ get{ return false; } }
		//public override bool IsScaryToPets{ get{ return true; } }

		public override bool IsBondable{ get{ return false; } }

		[Constructable]
		public BoneClaw() : this( false, 1.0 )
		{
		}

		[Constructable]
		public BoneClaw( bool summoned, double scalar ) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "kosciany pazur";
			Body = 308;
			BaseSoundID = 0x48D;

			SetStr( (int)(500*scalar), (int)(750*scalar) );
			SetDex( (int)(176*scalar), (int)(195*scalar) );
			SetInt( (int)(136*scalar), (int)(160*scalar) );

			SetHits( (int)(500*scalar), (int)(1000*scalar) );

			SetDamage( (int)(35*scalar), (int)(50*scalar) );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Cold, 50 );

			SetResistance( ResistanceType.Physical, (int)(35*scalar), (int)(75*scalar) );
			SetResistance( ResistanceType.Fire, (int)(20*scalar), (int)(60*scalar) );
			SetResistance( ResistanceType.Cold, (int)(30*scalar), (int)(90*scalar) );
			SetResistance( ResistanceType.Poison, (int)(20*scalar), (int)(100*scalar) );
			SetResistance( ResistanceType.Energy, (int)(30*scalar), (int)(60*scalar) );

			if ( summoned )
				SetResistance( ResistanceType.Fire, (int)(20*scalar), (int)(60*scalar) );
			else
				SetResistance( ResistanceType.Fire, (int)(20*scalar), (int)(60*scalar) );

			SetResistance( ResistanceType.Cold, (int)(30*scalar), (int)(90*scalar) );
			SetResistance( ResistanceType.Poison, (int)(20*scalar), (int)(100*scalar) );
			SetResistance( ResistanceType.Energy, (int)(30*scalar), (int)(60*scalar) );

			SetSkill( SkillName.MagicResist, (45.1*scalar), (70.0*scalar) );
			SetSkill( SkillName.Tactics, (85.1*scalar), (100.0*scalar) );
			SetSkill( SkillName.Wrestling, (85.1*scalar), (95.0*scalar) );

			if ( summoned )
			{
				Fame = 2500;
				Karma = -2500;
			}
			else
			{
				Fame = 10000;
				Karma = -10000;
			}

			if ( !summoned )
			{
				PackItem( new Bones() );

				if ( 0.1 > Utility.RandomDouble() )
					PackItem( new ominousCrystal() );

				if ( 0.15 > Utility.RandomDouble() )
					PackItem( new Skull() );

				if ( 0.2 > Utility.RandomDouble() )
					PackItem( new RibCage() );

				if ( 0.25 > Utility.RandomDouble() )
					PackItem( new Spine() );
			}

			ControlSlots = 0;
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

		public override void OnDamage( int amount, Mobile from, bool willKill )
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
		}

		public override bool BardImmune{ get{ return !Core.AOS || Controlled; } }
		public override bool Unprovokable { get { return Core.SE; } }
		public override bool AreaPeaceImmune { get { return Core.SE; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public BoneClaw( Serial serial ) : base( serial )
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