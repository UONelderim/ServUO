using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki mlodego szmaragdowego smoka" )]
	public class EmeraldDrake : Drake
	{
		[Constructable]
		public EmeraldDrake () : base()
		{
			Name = "mlody szmaragdowy smok";
			BaseSoundID = 362;
            Hue= 1368;
			SetStr( 431, 450 );
			SetDex( 153, 172 );
			SetInt( 131, 140 );

			SetHits( 241, 308 );

			SetDamage( 11, 17 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

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
			MinTameSkill = 85.5;

			PackReg( 3 );
		}
	
		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
            if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
            {
                if (Utility.RandomDouble() < 0.10)
                    corpse.DropItem(new DragonsBlood());

				corpse.DropItem( new Emerald(4) );
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
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Scales{ get{ return 7; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Green; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Emerald; } }

		public EmeraldDrake( Serial serial ) : base( serial )
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