using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "zwloki poteznego licza" )]
	public class Vecna : BaseCreature
	{
		private bool m_Stunning;

		public override bool IsScaredOfScaryThings{ get{ return false; } }
		public override bool IsScaryToPets{ get{ return true; } }

		public override bool IsBondable{ get{ return true; } }

		[Constructable]
		public Vecna() : this( false, 1.0 )
		{
		}

		[Constructable]
		public Vecna( bool summoned, double scalar ) : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "starozytny licz";
			Body = 78;
			BaseSoundID = 412;
			VirtualArmor = ((int)(16*scalar));

			SetStr( (int)(216*scalar), (int)(305*scalar) );
			SetDex( (int)(96*scalar), (int)(115*scalar) );
			SetInt( (int)(966*scalar), (int)(1045*scalar) );

			SetHits( (int)(560*scalar), (int)(595*scalar) );

			SetDamage( (int)(15*scalar), (int)(27*scalar) );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, (int)(55*scalar), (int)(65*scalar) );
			SetResistance( ResistanceType.Fire, (int)(25*scalar), (int)(30*scalar) );
			SetResistance( ResistanceType.Cold, (int)(50*scalar), (int)(60*scalar) );
			SetResistance( ResistanceType.Poison, (int)(50*scalar), (int)(60*scalar) );
			SetResistance( ResistanceType.Energy, (int)(25*scalar), (int)(30*scalar) );

			if ( summoned )
				SetResistance( ResistanceType.Fire, (int)(25*scalar), (int)(30*scalar) );
			else
				SetResistance( ResistanceType.Fire, (int)(25*scalar), (int)(30*scalar) );

			SetResistance( ResistanceType.Cold, (int)(50*scalar), (int)(60*scalar) );
			SetResistance( ResistanceType.Poison, (int)(50*scalar), (int)(60*scalar) );
			SetResistance( ResistanceType.Energy, (int)(25*scalar), (int)(30*scalar) );

			SetSkill( SkillName.EvalInt, (120.1*scalar), (130.0*scalar) );
			SetSkill( SkillName.Magery, (120.1*scalar), (130.0*scalar) );
			SetSkill( SkillName.Meditation, (100.1*scalar), (101.0*scalar) );
			SetSkill( SkillName.Poisoning, (100.1*scalar), (101.0*scalar) );
			SetSkill( SkillName.MagicResist, (175.2*scalar), (200.0*scalar) );
			SetSkill( SkillName.Tactics, (90.1*scalar), (100.0*scalar) );
			SetSkill( SkillName.Wrestling, (75.1*scalar), (100.0*scalar) );

			if ( summoned )
			{
				Fame = 5000;
				Karma = -5000;
			}
			else
			{
				Fame = 23000;
				Karma = -23000;
			}

			if ( !summoned )
			{
				PackItem( new Bones() );

				if ( 0.1 > Utility.RandomDouble() )
					PackItem( new Phylacery() );

				if ( 0.2 > Utility.RandomDouble() )
					PackItem( new MaliceCrystal() );

				if ( 0.25 > Utility.RandomDouble() )
					PackItem( new SorrowCrystal() );
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

	/*	public override void OnDamage( int amount, Mobile from, bool willKill )
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
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public Vecna( Serial serial ) : base( serial )
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