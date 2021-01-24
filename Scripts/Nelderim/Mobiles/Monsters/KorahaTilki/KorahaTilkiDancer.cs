using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class KorahaTilkiDancer : BaseCreature
	{

		[Constructable]
		public KorahaTilkiDancer() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.15, 0.3 )
		{

			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Tilki Tancerka";
			Hue = Utility.RandomSkinHue();
			Body = 0x191;
			Female = true;
			Name=NameList.RandomName( "female" );

			
			SetStr( 160, 200 );
			SetDex( 80, 120 );
			SetInt( 40, 60 );

			SetDamage( 10, 12 );


			SetDamageType( ResistanceType.Physical, 100 );
			
			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 45, 50 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );


			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Fencing, 100.0 );
			SetSkill( SkillName.MagicResist, 70.0, 80.0 );
			SetSkill( SkillName.Swords, 100.0 );
			SetSkill( SkillName.Macing, 100.0, 120.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Ninjitsu, 100.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 35;
			
			Bandana Helm = new Bandana ();
			Helm.Hue = 1319;
			Helm.Movable = false;
			EquipItem( Helm ); 
			
			FemaleLeafChest chest = new FemaleLeafChest ();
			chest.Hue = 1319;
			chest.Movable = false;
			EquipItem ( chest );	
			
			LeafTonlet legs = new LeafTonlet ();
			legs.Hue = 1319;
			legs.Movable = false;
			EquipItem ( legs );			
	
			LeafGloves Gloves = new LeafGloves ();
			Gloves.Hue = 1319;
			Gloves.Movable = false;
			EquipItem( Gloves );

			Sandals Boot = new Sandals ();
			Boot.Hue = 48;
			AddItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 48;
			Cloa.Movable = false;
			AddItem ( Cloa );
			
			BodySash sash = new BodySash();
			sash.Hue = 48;
			sash.Movable = false;
			EquipItem ( sash );

			Tessen sword = new Tessen ();
			sword.Hue = 1437;
			sword.Movable = false;
			EquipItem ( sword );

            SetWeaponAbility( WeaponAbility.TalonStrike );
            SetWeaponAbility( WeaponAbility.Feint );
        }
			
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		
		public KorahaTilkiDancer( Serial serial ) : base( serial )
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
