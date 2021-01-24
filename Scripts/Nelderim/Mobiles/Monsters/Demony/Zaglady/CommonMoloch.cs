using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki demona zaglady" )]
	public class CommonMoloch : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.15; } }

		[Constructable]
		public CommonMoloch() : base( AIType.AI_Melee, FightMode.Strongest, 11, 1, 0.2, 0.4 )
		{
			Name = "demon zaglady";
			Body = 0x311;
			BaseSoundID = 0x300;


			SetStr( 531, 660 );
			SetDex( 266, 285 );
			SetInt( 141, 165 );

			SetHits( 671, 700 );

			SetDamage( 20, 25 );

			SetResistance( ResistanceType.Physical, 60, 70 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 65.1, 75.0 );
			SetSkill( SkillName.Tactics, 75.1, 90.0 );
			SetSkill( SkillName.Wrestling, 70.1, 90.0 );

			Fame = 7500;
			Karma = -7500;

			VirtualArmor = 32;
			
			PackItem( new SulfurousAsh( Utility.RandomMinMax( 5, 8 ) ) );

            SetWeaponAbility( WeaponAbility.WhirlwindAttack );
		}

        public override double WeaponAbilityChance => 0.5;

        public override bool BardImmune { get { return false; } }

        public override void OnCarve(Mobile from, Corpse corpse, Item with)
        {
            if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
            {
                if (Utility.RandomDouble() < 0.08)
                    corpse.DropItem(new Bloodspawn());
                if (Utility.RandomDouble() < 0.15)
                    corpse.DropItem(new DaemonBone());
            }
            base.OnCarve(from, corpse, with);
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}
        
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }

		public CommonMoloch( Serial serial ) : base( serial )
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
