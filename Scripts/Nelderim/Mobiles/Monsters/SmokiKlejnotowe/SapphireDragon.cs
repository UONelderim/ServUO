using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki szafirowego smoka" )]
	public class SapphireDragon : Dragon
	{
		[Constructable]
		public SapphireDragon () : base()
		{
			Name = "szafirowy smok";
			BaseSoundID = 362;
            
			Hue = 2061;

			SetStr( 796, 825 );
			SetDex( 86, 105 );
			SetInt( 436, 475 );
			
            SetHits( 415, 455 );

			SetDamage( 16, 18);

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );
			
			SetResistance( ResistanceType.Physical, 50, 60 );
			SetResistance( ResistanceType.Fire, 30, 45 );
			SetResistance( ResistanceType.Cold, 20, 40 );
			SetResistance( ResistanceType.Poison, 45, 65 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.EvalInt, 90.0, 110.0 );
			SetSkill( SkillName.Magery, 90.0, 120.0 );
			SetSkill( SkillName.Bushido, 99.1, 110.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 75.1, 100.0 );
			SetSkill( SkillName.Meditation, 70.0, 100.0 );
			SetSkill( SkillName.Anatomy, 70.0, 100.0 );

			Fame = 15000;
			Karma = -15000;

			Tamable = false;
			ControlSlots = 3;
			MinTameSkill = 102;
		}
		
		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
            if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
			    if( Utility.RandomDouble() < 0.05 )
				    corpse.DropItem( new DragonsHeart() );
			    if( Utility.RandomDouble() < 0.15 )
				    corpse.DropItem( new DragonsBlood() );
					
				corpse.DropItem( new Sapphire(4) );
				corpse.DropItem( new StarSapphire(4) );
            }

			base.OnCarve(from, corpse, with);
		}
		
	    public override void AddWeaponAbilities()
        {
            WeaponAbilities.Add( WeaponAbility.ParalyzingBlow, 0.4 );
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 5 );
		}
		
		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int TreasureMapLevel{ get{ return 3; } }
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override int Scales{ get{ return 7; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Blue; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Sapphire | FoodType.StarSapphire; } }

		public SapphireDragon( Serial serial ) : base( serial )
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