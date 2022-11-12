using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "resztki mumii" )]
	public class PesantMummy : BaseCreature
	{
		private bool m_Stunning;

		public override bool IsScaredOfScaryThings{ get{ return false; } }
		//public override bool IsScaryToPets{ get{ return true; } }

		public override bool IsBondable{ get{ return false; } }

		[Constructable]
		public PesantMummy() : this( false, 1.0 )
		{
		}

		[Constructable]
		public PesantMummy( bool summoned, double scalar ) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "mumia";
			Body = 154;
			BaseSoundID = 471;
			VirtualArmor = ((int)(10*scalar));

			SetStr( (int)(346*scalar), (int)(370*scalar) );
			SetDex( (int)(71*scalar), (int)(90*scalar) );
			SetInt( (int)(26*scalar), (int)(40*scalar) );

			SetHits( (int)(208*scalar), (int)(222*scalar) );

			SetDamage( (int)(13*scalar), (int)(23*scalar) );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Cold, 60 );

			SetResistance( ResistanceType.Physical, (int)(45*scalar), (int)(55*scalar) );
			SetResistance( ResistanceType.Fire, (int)(10*scalar), (int)(20*scalar) );
			SetResistance( ResistanceType.Cold, (int)(50*scalar), (int)(60*scalar) );
			SetResistance( ResistanceType.Poison, (int)(20*scalar), (int)(30*scalar) );
			SetResistance( ResistanceType.Energy, (int)(20*scalar), (int)(30*scalar) );

			if ( summoned )
				SetResistance( ResistanceType.Fire, (int)(10*scalar), (int)(20*scalar) );
			else
				SetResistance( ResistanceType.Fire, (int)(10*scalar), (int)(20*scalar) );

			SetResistance( ResistanceType.Cold, (int)(50*scalar), (int)(60*scalar) );
			SetResistance( ResistanceType.Poison, (int)(20*scalar), (int)(30*scalar) );
			SetResistance( ResistanceType.Energy, (int)(20*scalar), (int)(30*scalar) );

			SetSkill( SkillName.MagicResist, (15.1*scalar), (40.0*scalar) );
			SetSkill( SkillName.Tactics, (35.1*scalar), (50.0*scalar) );
			SetSkill( SkillName.Wrestling, (35.1*scalar), (50.0*scalar) );

			if ( summoned )
			{
				Fame = 400;
				Karma = -400;
			}
			else
			{
				Fame = 4000;
				Karma = -4000;
			}

			if ( !summoned )
			{
				PackItem( new Bones() );

				if ( 0.1 > Utility.RandomDouble() )
					PackItem( new wickedCrystal() );

				if ( 0.15 > Utility.RandomDouble() )
					PackItem( new Skull() );

				if ( 0.2 > Utility.RandomDouble() )
					PackItem( new Bandage() );

				if ( 0.25 > Utility.RandomDouble() )
					PackItem( new Bandage() );
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

		public PesantMummy( Serial serial ) : base( serial )
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