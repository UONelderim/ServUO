using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class VitVargBerserker : BaseCreature
	{

		[Constructable]
		public VitVargBerserker () : base( AIType.AI_Melee, FightMode.Weakest, 14, 2, 0.15, 0.3 )
		{			
			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Varg Berserker";
			Hue = Race.RandomSkinHue();
			
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

			SetStr( 320, 380 );
			SetDex( 90, 140 );
			SetInt( 100, 140 );
			SetHits( 460, 500 );
			
			SetDamage( 20, 30 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 45 );
			SetResistance( ResistanceType.Cold, 55, 60 );
			SetResistance( ResistanceType.Poison, 45, 55 );
			SetResistance( ResistanceType.Energy, 45, 55 );


			SetSkill( SkillName.MagicResist, 57.5, 80.0 );
			SetSkill( SkillName.Swords, 90.0, 120.0 );
			SetSkill( SkillName.Tactics, 90.0, 102.5 );
			SetSkill( SkillName.Anatomy, 90.0, 105.2 );

			Fame = 5000;
			Karma = -5000;
			
			VirtualArmor = 40;
			
			BoneHelm Helm = new BoneHelm ();
			Helm.Hue = 856;
			Helm.Movable = false;
			EquipItem( Helm );
			
			BoneChest chest = new BoneChest ();
			chest.Hue = 856;
			chest.Movable = false;
			EquipItem ( chest );
			
			BoneGloves Gloves = new BoneGloves ();
			Gloves.Hue = 856;
			Gloves.Movable = false;
			EquipItem( Gloves );
			
			BoneLegs legs = new BoneLegs ();
			legs.Hue = 856;
			legs.Movable = false;
			EquipItem ( legs );

			ThighBoots Boot = new ThighBoots ();
			Boot.Hue = 1109;
			EquipItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 288;
			Cloa.Movable = false;
			AddItem ( Cloa );
			
			BodySash sash = new BodySash();
			sash.Hue = 288;
			sash.Movable = false;
			EquipItem ( sash );

			PaladinSword sword = new PaladinSword();
			sword.Hue = 781;
			sword.Movable = false;
			EquipItem ( sword );

            SetWeaponAbility( WeaponAbility.Disarm );
		}

        public override double WeaponAbilityChance => 0.3;

        public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

        public override double AttackMasterChance { get { return 0.10; } }
		public override bool ShowFameTitle{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }


		public VitVargBerserker ( Serial serial ) : base( serial )
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
