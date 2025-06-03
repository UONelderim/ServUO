//Edits by Raist/Tass23 2/2/2017
using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "zwłoki animowanej laleczki guslarza" )]
	public class AnimatedVoodooDoll : BaseCreature
	{
		public Mobile m_CursedPerson;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile CursedPerson
		{
			get   { return m_CursedPerson; }
			set { m_CursedPerson = value; }
		}
		
		private bool m_Stunning;
		public override bool DeleteOnRelease{ get{ return true; } }
		public override bool Commandable{ get{ return false; } }
		public override bool IsScaredOfScaryThings{ get{ return false; } }
		public override bool IsScaryToPets{ get{ return true; } }

		public override bool IsBondable{ get{ return false; } }

		//[Constructable]
		public AnimatedVoodooDoll( Mobile m, bool summoned ) : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name ="Upiór";
			CursedPerson = m;

			Body = 3;
			Hue = 2983;
			BaseSoundID = 471;

			SetStr( 216, 305 );
			SetDex( 96, 115 );
			SetInt( 966, 1045 );

			SetHits( 560, 595 );

			SetDamage( 19, 27 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 25, 30 );

			if ( Controlled || Summoned )
				SetResistance( ResistanceType.Fire, 25, 30 );
			else
				SetResistance( ResistanceType.Fire, 25, 30 );

			SetSkill( SkillName.Poisoning, 100.1, 101.0 );
			SetSkill( SkillName.MagicResist, 175.2, 200.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 75.1, 100.0 );

			if ( Controlled || Summoned )
			{
				Fame = 500;
				Karma = -500;
			}
			else
			{
				Fame = 2300;
				Karma = -2300;
			}
			ControlSlots = 3;
		}

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

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties(list);
			list.Add(1049646); // (summoned)

			if (CursedPerson != null)
				list.Add("wskrzeszona laleczka {0}", CursedPerson.Name);

			if (SummonMaster != null)
				list.Add("<BASEFONT COLOR=#cc33ff>Utworzone przez {0}", SummonMaster.Name);
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

		public override bool BardImmune{ get{ return Controlled; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		
		public AnimatedVoodooDoll( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( m_CursedPerson );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_CursedPerson = reader.ReadMobile();
		}
	}
}
