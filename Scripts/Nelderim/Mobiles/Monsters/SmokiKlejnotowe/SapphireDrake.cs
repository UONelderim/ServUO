using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki mlodego szafirowego smoka" )]
	public class SapphireDrake : Drake
	{
		[Constructable]
		public SapphireDrake () : base()
		{
			Name = "mlody szafirowy smok";
			BaseSoundID = 362;
            
			Hue = 2061;
			
			SetStr( 411, 440 );
			SetDex( 153, 162 );
			SetInt( 121, 140 );

			SetHits( 241, 288 );

			SetDamage( 11, 17 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

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

			

			Tamable = false;
			ControlSlots = 2;
			MinTameSkill = 84.0;

			SetSpecialAbility(SpecialAbility.DragonBreath);
		}
		
		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
            if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
            {
                if (Utility.RandomDouble() < 0.10)
                    corpse.DropItem(new DragonsBlood());

				corpse.DropItem( new Sapphire(2) );
				corpse.DropItem( new StarSapphire(2) );
            }

			base.OnCarve(from, corpse, with);
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 5 );
			AddLoot(LootPack.LootItem<PazurStarozytnegoDiamentowegoSmoka>(30.0));
		}
		
		
		public override int TreasureMapLevel{ get{ return 3; } }
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override int Scales{ get{ return 7; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Blue; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Sapphire | FoodType.StarSapphire; } }

		public SapphireDrake( Serial serial ) : base( serial )
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
