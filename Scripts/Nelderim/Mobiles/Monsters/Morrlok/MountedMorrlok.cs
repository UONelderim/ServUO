using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class JezdziecMorrlok : BaseCreature
	{
		public override double DifficultyScalar{ get{ return 1.10; } }

		[Constructable]
		public JezdziecMorrlok () : base( AIType.AI_Melee, FightMode.Strongest, 12, 1, 0.1, 0.4 )
		{			
			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- jezdziec";
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new Skirt( Utility.RandomNeutralHue() ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}
			

			SetStr( 300, 400 );
			SetDex( 60, 120 );
			SetInt( 100, 165 );
			SetHits( 200, 300 );

			SetDamage( 12, 16 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );


			SetSkill( SkillName.Poisoning, 60.0, 82.5 );
			SetSkill( SkillName.MagicResist, 57.5, 80.0 );
			SetSkill( SkillName.Swords, 90.0, 120.0 );
			SetSkill( SkillName.Tactics, 90.0, 102.5 );
			SetSkill( SkillName.Anatomy, 90.0, 105.2 );

			Fame = 5000;
			Karma = -5000;
			
			VirtualArmor = 40;
			
			NorseHelm Helm = new NorseHelm ();
			Helm.Hue = 2106;
			Helm.Movable = false;
			EquipItem( Helm );
			
			ChainChest chest = new ChainChest ();
			chest.Hue = 2106;
			chest.Movable = false;
			EquipItem ( chest );
			
			PlateGorget Gorget = new PlateGorget ();
			Gorget.Hue = 2106;
			Gorget.Movable = false;
			EquipItem( Gorget );
			
			RingmailGloves Gloves = new RingmailGloves ();
			Gloves.Hue = 2106;
			Gloves.Movable = false;
			EquipItem( Gloves );
			
			RingmailLegs legs = new RingmailLegs ();
			legs.Hue = 2106;
			legs.Movable = false;
			EquipItem ( legs );

			Boots Boot = new Boots ();
			Boot.Hue = 1109;
			EquipItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 1146;
			EquipItem ( Cloa );
			
			Robe Rob = new Robe ();
			Rob.Hue = 1146;
			EquipItem ( Rob );

			Longsword sword = new Longsword ();
			sword.Hue = 2106;
			sword.Movable = false;
			EquipItem ( sword );
			
			MetalShield Shield = new MetalShield ();
			Shield.Hue = 2106;
			Shield.Movable = false;
			EquipItem ( Shield );

			Horse mount = new Horse();

			mount.ControlMaster = this as Mobile;
			mount.Controlled = true;
			mount.InvalidateProperties();
			
			mount.Rider = this;

            SetWeaponAbility( WeaponAbility.Disarm );
		}

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
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }


		public JezdziecMorrlok ( Serial serial ) : base( serial )
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
