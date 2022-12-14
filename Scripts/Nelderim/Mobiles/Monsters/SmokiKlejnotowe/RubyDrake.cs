using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki mlodego rubinowego smoka" )]
	public class RubyDrake : Drake
	{
		[Constructable]
		public RubyDrake () : base()
		{
			Name = "mlody rubinowy smok";
			BaseSoundID = 362;
            Hue= 1157;
			SetStr( 401, 460 );
			SetDex( 153, 182 );
			SetInt( 131, 150 );

			SetHits( 241, 318 );

			SetDamage( 11, 17 );

			SetDamageType( ResistanceType.Physical, 25 );
			SetDamageType( ResistanceType.Fire, 75 );

			SetResistance( ResistanceType.Physical, 45, 50 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 65.1, 80.0 );
			SetSkill( SkillName.Tactics, 65.1, 90.0 );
			SetSkill( SkillName.Wrestling, 65.1, 80.0 );

			Fame = 5500;
			Karma = -5500;

			VirtualArmor = 46;

			Tamable = false;
			ControlSlots = 2;
			MinTameSkill = 85;

			PackReg( 3 );
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
            if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
            {
                if (Utility.RandomDouble() < 0.10)
                    corpse.DropItem(new DragonsBlood());

				corpse.DropItem( new Ruby(4) );
            }

			base.OnCarve(from, corpse, with);
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 5 );
		}
		
		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int TreasureMapLevel{ get{ return 3; } }
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Horned; } }
		public override int Scales{ get{ return 7; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Red; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Ruby; } }

		public RubyDrake( Serial serial ) : base( serial )
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