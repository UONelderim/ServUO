using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class KorahaTilkiSpearman : BaseCreature
	{

		[Constructable]
		public KorahaTilkiSpearman() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{

			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Tilki Wlocznik";
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
			}
			
			SetStr( 160, 200 );
			SetDex( 100, 120 );
			SetInt( 50, 65 );
			
			SetHits( 200, 240 );

			SetDamage( 14, 18 );


			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Fire, 40 );
			
			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 55, 65 );
			SetResistance( ResistanceType.Cold, 40, 45 );
			SetResistance( ResistanceType.Poison, 35, 50 );
			SetResistance( ResistanceType.Energy, 35, 50 );


			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Swords, 50.0, 70.0 );
			SetSkill( SkillName.MagicResist, 80.0, 90.0 );
			SetSkill( SkillName.Fencing, 90.0, 110.0);
			SetSkill( SkillName.Tactics, 100.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 40;
			
			LeatherCap Helm = new LeatherCap ();
			Helm.Hue = 1319; 
			Helm.Movable = false;
			EquipItem( Helm ); 
			
			StuddedChest chest = new StuddedChest ();
			chest.Hue = 1319;
			chest.Movable = false;
			EquipItem ( chest );
			
			StuddedGorget Gorget = new StuddedGorget ();
			Gorget.Hue = 1319;
			Gorget.Movable = false;
			EquipItem( Gorget );
			
			StuddedGloves Gloves = new StuddedGloves ();
			Gloves.Hue = 1319;
			Gloves.Movable = false;
			EquipItem( Gloves );
			
			StuddedLegs legs = new StuddedLegs ();
			legs.Hue = 1319;
			legs.Movable = false;
			EquipItem ( legs );

			Boots Boot = new Boots ();
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

			Spear sword = new Spear ();
			sword.Hue = 1437;
			sword.Movable = false;
			EquipItem ( sword );

            SetWeaponAbility( WeaponAbility.ParalyzingBlow );
		}

        public override double WeaponAbilityChance => 0.3;

        public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }


		public KorahaTilkiSpearman( Serial serial ) : base( serial )
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
