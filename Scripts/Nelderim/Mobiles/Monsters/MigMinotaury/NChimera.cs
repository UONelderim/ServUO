using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
[CorpseName( "zwloki chimery" )]
	public class NChimera : BaseMount
	{
		[Constructable]
		public NChimera() : this( "Chimera" )
		{
		}
		[Constructable]
		public NChimera( string name ) : base( name, 276, 0x3e90, AIType.AI_BattleMage, FightMode.Aggressor, 12, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x4FB;
		
			SetStr( 296, 325 );
			SetDex( 186, 205 );
			SetInt( 236, 275 );

			SetDamage( 12, 18 );

			SetHits( 278, 295 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 55, 65 );
			SetResistance( ResistanceType.Poison, 65, 75 );
			SetResistance( ResistanceType.Energy, 55, 65 );

			SetSkill( SkillName.EvalInt, 50.1, 70.0 );
			SetSkill( SkillName.Magery, 50.1, 70.0 );
			SetSkill( SkillName.MagicResist, 99.1, 100.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 92.5 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 60;

			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 93.9;

            SetWeaponAbility( WeaponAbility.MortalStrike );
            SetWeaponAbility( WeaponAbility.WhirlwindAttack );
            SetWeaponAbility( WeaponAbility.CrushingBlow );
            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 1 );
			AddLoot( LootPack.Gems, 2 );
		}

		public override int GetAngerSound()
		{
			if ( !Controlled )
				return 0x16A;

			return base.GetAngerSound();
		}

		public override int Meat{ get{ return 5; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		// 07.04.21 :: Emfor
		public override bool BardImmune{ get{ return false; } } 
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }		
		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		
		public NChimera( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( BaseSoundID == 0x16A )
				BaseSoundID = 0xA8;
		}
	}
}
