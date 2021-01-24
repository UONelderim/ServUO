using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "zwloki wielkiego demona chaosu" )]
	public class GreaterChaosDaemon : BaseCreature
	{
		public override double DifficultyScalar{ get{ return 1.20; } }

		[Constructable]
		public GreaterChaosDaemon() : base( AIType.AI_Melee, FightMode.Weakest, 12, 1, 0.2, 0.4 )
		{
			Name = "wielki demon chaosu";
			Body = 792;
			Hue = 1569;
			BaseSoundID = 0x3E9;

			SetStr( 406, 630 );
			SetDex( 171, 200 );
			SetInt( 56, 80 );

			SetHits( 591, 710 );

			SetDamage( 35, 40 );

			SetDamageType( ResistanceType.Physical, 85 );
			SetDamageType( ResistanceType.Fire, 15 );

			SetResistance( ResistanceType.Physical, 70, 80 );
			SetResistance( ResistanceType.Fire, 70, 80 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 95.1, 105.0 );
			SetSkill( SkillName.Tactics, 100.1, 120.0 );
			SetSkill( SkillName.Wrestling, 95.1, 120.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 65;

            SetWeaponAbility( WeaponAbility.CrushingBlow );
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

		public GreaterChaosDaemon( Serial serial ) : base( serial )
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
