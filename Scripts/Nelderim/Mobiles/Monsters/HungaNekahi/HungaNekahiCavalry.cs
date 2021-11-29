using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class HungaNekahiCavalry : BaseCreature
	{

		[Constructable]
		public HungaNekahiCavalry () : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.1, 0.4 )
		{			
			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Nekahi Kawalerzysta";
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
			

			SetStr( 380, 460 );
			SetDex( 100, 140 );
			SetInt( 100, 160 );
			SetHits( 280, 340 );

			SetDamage( 18, 24 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 45 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 90.0, 110.0 );
			SetSkill( SkillName.Tactics, 100.0, 110.0 );
			SetSkill( SkillName.Anatomy, 100.0, 110.0 );
			SetSkill( SkillName.Healing, 90.1, 100.0 );
			SetSkill( SkillName.Macing, 120.0, 130.0 );
			SetSkill( SkillName.Wrestling, 100.0, 110.0 );

			Fame = 8000;
			Karma = -8000;
			
			VirtualArmor = 50;
			
			PlateHelm Helm = new PlateHelm ();
			Helm.Hue = 556;
			Helm.Movable = false;
			EquipItem( Helm );
			
			PlateChest chest = new PlateChest ();
			chest.Hue = 556;
			chest.Movable = false;
			EquipItem ( chest );
			
			PlateGorget Gorget = new PlateGorget ();
			Gorget.Hue = 556;
			Gorget.Movable = false;
			EquipItem( Gorget );
			
			PlateArms Arms = new PlateArms ();
			Arms.Hue = 556;
			Arms.Movable = false;
			EquipItem( Arms );
			
			PlateGloves Gloves = new PlateGloves ();
			Gloves.Hue = 556;
			Gloves.Movable = false;
			EquipItem( Gloves );
			
			PlateLegs legs = new PlateLegs ();
			legs.Hue = 556;
			legs.Movable = false;
			EquipItem ( legs );

			Boots Boot = new Boots ();
			Boot.Hue = 33;
			EquipItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 33;
			EquipItem ( Cloa );
			
			BodySash sash = new BodySash();
			sash.Hue = 33;
			sash.Movable = false;
			EquipItem ( sash );
			
			Maul sword = new Maul ();
			sword.Hue = 589;
			sword.Movable = false;
			EquipItem ( sword );
			
			OrderShield Shield = new OrderShield ();
			Shield.Hue = 589;
			Shield.Movable = false;
			EquipItem ( Shield );
			
			Beetle mount = new Beetle();

			mount.ControlMaster = this as Mobile;
			mount.Controlled = true;
			mount.InvalidateProperties();
			
			mount.Rider = this;

            SetWeaponAbility( WeaponAbility.ConcussionBlow );
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


		public HungaNekahiCavalry ( Serial serial ) : base( serial )
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
