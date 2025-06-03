using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki diamentowego smoka" )]
	public class DiamondDragon : Dragon
	{
		[Constructable]
		public DiamondDragon () : base()
		{
			Name = "diamentowy smok";
			BaseSoundID = 362;
            Hue = 1154;
            
            SetStr( 796, 825 );
            SetDex( 86, 105 );
            SetInt( 436, 475 );
            
            SetHits( 400, 420 );

			SetDamage( 18, 20 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Fire, 50 );
			SetDamageType( ResistanceType.Cold, 0 );
			SetDamageType( ResistanceType.Poison, 50);
			SetDamageType( ResistanceType.Energy, 0 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.EvalInt, 90.0, 110.0 );
			SetSkill( SkillName.Magery, 90.0, 120.0 );
			SetSkill( SkillName.MagicResist, 99.1, 110.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 75.1, 100.0 );
			SetSkill( SkillName.Meditation, 70.0, 100.0 );
			SetSkill( SkillName.Anatomy, 70.0, 100.0 );

			Fame = 15000;
			Karma = -15000;

			Tamable = false;
			ControlSlots = 3;
			MinTameSkill = 105.2;
			
			SetSpecialAbility(SpecialAbility.DragonBreath);
		}
		
		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
            if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
			    if( Utility.RandomDouble() < 0.05 )
				    corpse.DropItem( new DragonsHeart() );
			    if( Utility.RandomDouble() < 0.15 )
				    corpse.DropItem( new DragonsBlood() );
					
				corpse.DropItem( new Diamond(8) );
            }

			base.OnCarve(from, corpse, with);
		}
		

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 5 );
			AddLoot(LootPack.LootItem<CrushedCrystals>(80.0));
		}
		
		
		public override bool AutoDispel{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Scales{ get{ return 7; } }
		public override ScaleType ScaleType{ get{ return ScaleType.White; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Diamond; } }

		public DiamondDragon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			
			if ( version < 1)
				SetDamage( 16, 19 );
		}
	}
}
