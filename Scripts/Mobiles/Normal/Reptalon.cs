using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki starej chimery" )]
	public class Reptalon : BaseMount
	{
		[Constructable]
		public Reptalon() : base( "stara chimera", 0x114, 0x3E90, AIType.AI_Melee, FightMode.Closest, 12, 1, 0.3, 0.35 )
		{
			BaseSoundID = 0x4FB;

			SetStr( 1001, 1025 );
			SetDex( 152, 164 );
			SetInt( 401, 478 );  //  251, 289 

			SetHits( 933, 1031 ); // 833, 931 );

			SetDamage( 25, 30 );
            
			SetDamageType( ResistanceType.Physical, 25 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Cold, 0 );
			SetDamageType( ResistanceType.Poison, 25);
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 53, 64 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 55, 65 );
			SetResistance( ResistanceType.Poison, 65, 75 );
			SetResistance( ResistanceType.Energy, 71, 83 );
			



			SetSkill( SkillName.Wrestling, 101.5, 118.2 );
			SetSkill( SkillName.Tactics, 101.7, 108.2 );
			SetSkill( SkillName.MagicResist, 99.1, 100.0 );
			SetSkill( SkillName.Anatomy, 56.4, 59.7 );
			SetSkill( SkillName.EvalInt, 50.1, 70.0 );
			SetSkill( SkillName.Magery, 50.1, 70.0 );
			
			
			VirtualArmor = 60;
			
			Tamable = true;
			ControlSlots = 5;
			Hue = 2586;
			MinTameSkill = 115.1;
			
			SetSpecialAbility(SpecialAbility.DragonBreath);
			SetWeaponAbility(WeaponAbility.ParalyzingBlow);
			SetWeaponAbility(WeaponAbility.WhirlwindAttack);
			SetWeaponAbility(WeaponAbility.CrushingBlow);
			SetWeaponAbility(WeaponAbility.MortalStrike);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 2);
			AddLoot(LootPack.LootItem<SpleenOfThePutrefier>(30.0));
		}
		
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Meat{ get{ return 5; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Horned; } }
		public override bool CanAngerOnTame{ get { return true; } }
		public override bool StatLossAfterTame{ get{ return true; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }		
		public override Poison HitPoison{ get{ return Poison.Deadly; } }

		public Reptalon( Serial serial ) : base( serial )
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
		}
	}
}
