using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class BagusGagakLightCav : BaseCreature
	{
		[Constructable]
		public BagusGagakLightCav () : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.1, 0.4 )
		{			
			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Gagak Kawalerzysta";
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
			

			SetStr( 280, 340 );
			SetDex( 120, 150 );
			SetInt( 80, 120 );
			SetHits( 200, 240 );

			SetDamage( 12, 16 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 35, 45 );
			SetResistance( ResistanceType.Energy, 30, 40 );


			SetSkill( SkillName.Poisoning, 60.0, 82.5 );
			SetSkill( SkillName.MagicResist, 80.0, 90.0 );
			SetSkill( SkillName.Swords, 90.0, 110.0 );
			SetSkill( SkillName.Tactics, 90.0, 110.0 );
			SetSkill( SkillName.Anatomy, 90.0, 105.2 );
			SetSkill( SkillName.Healing, 80.1, 100.0 );
			SetSkill( SkillName.Fencing, 80.0, 90.0 );

			Fame = 6000;
			Karma = -6000;
			
			VirtualArmor = 45;

            SetWeaponAbility( WeaponAbility.Dismount );
			
			BoneHelm Helm = new BoneHelm ();
			Helm.Hue = 2101;
			Helm.Movable = false;
			EquipItem( Helm );
			
			BoneChest chest = new BoneChest ();
			chest.Hue = 2101;
			chest.Movable = false;
			EquipItem ( chest );
			
			BoneGloves Gloves = new BoneGloves ();
			Gloves.Hue = 2101;
			Gloves.Movable = false;
			EquipItem( Gloves );
			
			BoneLegs legs = new BoneLegs ();
			legs.Hue = 2101;
			legs.Movable = false;
			EquipItem ( legs );

			Boots Boot = new Boots ();
			Boot.Hue = 262;
			EquipItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 262;
			EquipItem ( Cloa );
			
			BodySash sash = new BodySash();
			sash.Hue = 262;
			sash.Movable = false;
			EquipItem ( sash );
			
			BladedStaff sword = new BladedStaff ();
			sword.Hue = 424;
			sword.Movable = false;
			EquipItem ( sword );
			
			ForestOstard mount = new ForestOstard();

			mount.ControlMaster = this as Mobile;
			mount.Controlled = true;
			mount.InvalidateProperties();
			
			mount.Rider = this;
		}

        public override double WeaponAbilityChance => 0.3;

        public override bool OnBeforeDeath()
		{
			IMount mount = this.Mount;

			if ( mount != null )
				mount.Rider = null;

			if ( mount is Mobile )
				((Mobile)mount).Kill();

			return base.OnBeforeDeath();
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

        public override double AttackMasterChance { get { return 0.10; } }
		public override bool ShowFameTitle{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }


		public BagusGagakLightCav ( Serial serial ) : base( serial )
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
