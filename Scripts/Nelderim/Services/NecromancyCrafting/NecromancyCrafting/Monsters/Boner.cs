using Server.Engines.Craft;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "resztki koscieja" )]
	public class Boner : BaseCreature, BonerCrystal.INecroPet

	{
		public override bool IsScaredOfScaryThings => false;

		public override bool IsBondable => false;

		[Constructable]
		public Boner() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "kosciej";
			Body = 308;
			BaseSoundID = 0x48D;

			SetStr( 500, 750 );
			SetDex( 176, 195 );
			SetInt( 136, 160 );

			SetHits( 500, 1000 );

			SetDamage( 35, 50 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Cold, 50 );

			SetResistance( ResistanceType.Physical, 35, 75 );
			SetResistance( ResistanceType.Fire, 20, 60 );
			SetResistance( ResistanceType.Cold, 30, 90 );
			SetResistance( ResistanceType.Poison, 20, 100 );
			SetResistance( ResistanceType.Energy, 30, 60 );
			
			SetSkill( SkillName.MagicResist, 45.1, 70.0 );
			SetSkill( SkillName.Tactics, 85.1, 100.0 );
			SetSkill( SkillName.Wrestling, 85.1, 95.0 );
			
			Fame = 10000;
			Karma = -10000;

			PackItem( new Bones() );
			if( Utility.RandomDouble() < DefNecromancyCrafting.PowderDropChance )
				PackItem( new BonerPowder() );
			
			ControlSlots = 5;
		}

		public override bool DeleteOnRelease => true;

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

		public override bool AutoDispel => !Controlled;
		public override bool BleedImmune => true;
		public override bool BardImmune => Controlled;
		public override bool Unprovokable => true;
		public override bool AreaPeaceImmune => true;
		public override Poison PoisonImmune => Poison.Lethal;

		public Boner( Serial serial ) : base( serial )
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
