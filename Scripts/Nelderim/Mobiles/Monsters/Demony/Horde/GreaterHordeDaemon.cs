using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "zwloki wielkiego demona hordy" )]
	public class GreaterHordeDaemon : BaseCreature
	{
		public override double DifficultyScalar{ get{ return 1.20; } }
		[Constructable]
		public GreaterHordeDaemon () : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{
			Name = "wielki demon hordy";
			Body = 796;
			BaseSoundID = 357;


			SetStr( 616, 740 );
			SetDex( 131, 160 );
			SetInt( 121, 155 );

			SetHits( 1510, 1824 );

			SetDamage( 20, 35 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 45, 55 );
			SetResistance( ResistanceType.Cold, 45, 55 );
			SetResistance( ResistanceType.Poison, 45, 55 );
			SetResistance( ResistanceType.Energy, 45, 55 );

			SetSkill( SkillName.MagicResist, 80.0 );
			SetSkill( SkillName.Tactics, 100.1, 110.0 );
			SetSkill( SkillName.Wrestling, 100.1, 110.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 58;

			AddItem( new LightSource() );
		}
		
		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
            if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
            {
                if (Utility.RandomDouble() < 0.10)
                    corpse.DropItem(new Bloodspawn());
                if (Utility.RandomDouble() < 0.30)
                    corpse.DropItem(new DaemonBone());
            }
			base.OnCarve(from, corpse, with);
		}
		
		public override void GenerateLoot()
		{
			// 07.01.2013 :: szczaw :: usuniecie PackGold
			//PackGold(1000, 2000 );
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.FilthyRich );
		}

		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override bool BardImmune{ get{ return false; } }

		public override int GetIdleSound()
		{
			return 338;
		}

		public override int GetAngerSound()
		{
			return 338;
		}

		public override int GetDeathSound()
		{
			return 338;
		}

		public override int GetAttackSound()
		{
			return 406;
		}

		public override int GetHurtSound()
		{
			return 194;
		}

		public GreaterHordeDaemon( Serial serial ) : base( serial )
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