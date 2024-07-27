using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zgliszcza potwora" )]
	public class ChiefParoxysmusAvatar: BaseCreature
	{
		[Constructable]
		public ChiefParoxysmusAvatar() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{
			Name = "Avatar Paroxymusa";
			Body = 0x100;

			SetStr( 1232, 1400 );
			SetDex( 76, 82 );
			SetInt( 76, 85 );

			SetHits( 5000 );

			SetDamage( 22, 26 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Physical, 70, 75 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 50, 60 );
			
			SetSkill( SkillName.Wrestling, 100.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.MagicResist, 120.0 );
			SetSkill( SkillName.Anatomy, 120.0 );
			SetSkill( SkillName.Poisoning, 90.0 );

			SetAreaEffect(AreaEffect.PoisonBreath);
		}
		
		public ChiefParoxysmusAvatar( Serial serial ) : base( serial )
		{
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 8 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }

		public override int GetDeathSound()	{ return 0x56F; }
		public override int GetAttackSound() { return 0x570; }
		public override int GetIdleSound() { return 0x571; }
		public override int GetAngerSound() { return 0x572; }
		public override int GetHurtSound() { return 0x573; }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
		}
	}
}
