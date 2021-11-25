using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki wilczego wojownika" )]
	public class WolfWarrior : BaseCreature
	{
		[Constructable]
		public WolfWarrior() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			
			Name = "wilczy wojownik";
			Hue = 2940;
			Body = 0x190;
			

			SetStr( 96, 220 );
			SetDex( 30, 45 );
			SetInt( 51, 65 );
			SetHits( 200, 250 );
			SetDamage( 14, 16 );

			SetDamageType( ResistanceType.Physical, 100 );
			
			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );


			SetSkill( SkillName.Anatomy, 125.0 );
			SetSkill( SkillName.Fencing, 46.0, 77.5 );
			SetSkill( SkillName.Macing, 35.0, 57.5 );
			SetSkill( SkillName.Poisoning, 60.0, 82.5 );
			SetSkill( SkillName.MagicResist, 83.5, 92.5 );
			SetSkill( SkillName.Wrestling, 125.0 );
			SetSkill( SkillName.Tactics, 125.0 );
			SetSkill( SkillName.Lumberjacking, 125.0 );

			Fame = 3000;
			Karma = -3000;


	            LeatherCap Helm = new LeatherCap ();
				Helm.Hue = 2707;
				Helm.LootType = LootType.Blessed;
				Helm.ItemID = 5445;
				Helm.Name = "wilcza maska";
				AddItem( Helm ); 
			StuddedChest Chest = new StuddedChest ();
				Chest.Hue = 2707;
				Chest.LootType = LootType.Blessed;
				AddItem ( Chest ); 
			StuddedGorget Gorget = new StuddedGorget ();
				Gorget.Hue = 2707;
				Gorget.LootType = LootType.Blessed;
				AddItem( Gorget );
			BoneGloves Gloves = new BoneGloves ();
				Gloves.Hue = 2707;
				Gloves.LootType = LootType.Blessed;
				AddItem( Gloves );
			BoneArms Arms = new BoneArms ();
				Arms.Hue = 2707;
				Arms.LootType = LootType.Blessed;
				AddItem( Arms );
			BoneLegs Legs = new BoneLegs ();
				Legs.Hue = 2707;
				Legs.LootType = LootType.Blessed;
				AddItem ( Legs );
			Katana sword = new Katana ();
			sword.Hue = 424;
			sword.Movable = false;
			Legs.LootType = LootType.Blessed;
			EquipItem ( sword );
			
			MetalShield Shield = new MetalShield ();
			Shield.Hue = 424;
			Shield.Movable = false;
			Legs.LootType = LootType.Blessed;
			EquipItem ( Shield );


			Cloak Cloa = new Cloak();
				Cloa.Hue = 2707;
				Cloa.LootType = LootType.Blessed;
				AddItem ( Cloa );


                	Item hair = new Item( Utility.RandomList( 0x203C ) );
               		hair.Hue = Utility.RandomHairHue();
			hair.Layer = Layer.Hair; 
			hair.Movable = false; 
			AddItem( hair ); 

		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.Meager );
		
		}
        public override double AttackMasterChance { get { return 0.05; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } } 
		public override bool ShowFameTitle{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }

		public WolfWarrior( Serial serial ) : base( serial )
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
