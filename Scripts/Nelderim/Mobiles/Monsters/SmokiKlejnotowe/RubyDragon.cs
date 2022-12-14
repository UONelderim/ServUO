using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki rubinowego smoka" )]
	public class RubyDragon : Dragon
	{
		[Constructable]
		public RubyDragon () : base()
		{
			Name = "rubinowy smok";
			BaseSoundID = 362;
            Hue= 1157;
            
            SetStr( 796, 825 );
            SetDex( 86, 105 );
            SetInt( 436, 475 );
            
            SetHits( 420, 480 );


			SetDamage( 17, 20);

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Fire, 60 );
			
			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 65, 70 );
			SetResistance( ResistanceType.Cold, 20, 35);
			SetResistance( ResistanceType.Poison, 20, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

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
			MinTameSkill = 103;
		}
		
		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
            if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
			    if( Utility.RandomDouble() < 0.05 )
				    corpse.DropItem( new DragonsHeart() );
			    if( Utility.RandomDouble() < 0.15 )
				    corpse.DropItem( new DragonsBlood() );
					
				corpse.DropItem( new Ruby(8) );
            }

			base.OnCarve(from, corpse, with);
		}
		
	    public override void AddWeaponAbilities()
        {
	        WeaponAbilities.Add( WeaponAbility.CrushingBlow, 0.3 );
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 5 );
		}
		
		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Horned; } }
		public override int Scales{ get{ return 7; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Red; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Ruby; } }

		public RubyDragon( Serial serial ) : base( serial )
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