using System;
using Server.Items;

namespace Server.Mobiles
{
	public class rzezimieszek : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }
		
private DateTime m_Spoken;
		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( m.Alive && m is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile)m;
					
				int range = 2;
				
				if ( range >= 0 && InRange( m, range ) && !InRange( oldLocation, range ) && DateTime.Now >= m_Spoken + TimeSpan.FromSeconds( 10 ) )
				{
                    switch ( Utility.Random( 12 ) )
				{
					case 0: Say( "STÓJ! KURWISYNU!" ); break;
					case 1: Say( "Potnę Cię szlamo!" ); break;
					case 2: Say( "Zabiję Ci matkę!" ); break;
					case 3: Say( "Pieniądze albo życie! Haha i tak wezmę oba!" ); break;
					case 4: Say( "Zaraz się przekonasz co to dobre rżnięcie!" ); break;
					case 6: Say( "Spierdalaj bo zabije!" ); break;
					case 7: Say( "Gdzie Ci tak kurwa śpieszno!" ); break;
					case 8: Say( "Nikt Cie tu nie usłyszy." ); break;
					case 9: Say( "Dawaj co masz albo przetrące Ci łeb!" ); break;
					case 10: Say( "Wychędoże Cie w oczodoły!" ); break;
					case 11: Say( "Wracaj tu kurwisynu!" ); break;
								
				}			
					m_Spoken = DateTime.Now;
				}
			}
		}		
		[Constructable]
		public rzezimieszek() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Title = "- rzezimieszek";
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

			SetStr( 86, 100 );
			SetDex( 81, 95 );
			SetInt( 61, 75 );

			SetDamage( 10, 23 );

			SetSkill( SkillName.Fencing, 66.0, 97.5 );
			SetSkill( SkillName.Macing, 65.0, 87.5 );
			SetSkill( SkillName.MagicResist, 25.0, 47.5 );
			SetSkill( SkillName.Swords, 65.0, 87.5 );
			SetSkill( SkillName.Tactics, 65.0, 87.5 );
			SetSkill( SkillName.Wrestling, 15.0, 37.5 );

			Fame = 1000;
			Karma = -1000;

			AddItem( new Boots( Utility.RandomNeutralHue() ) );
			AddItem( new FancyShirt());
			AddItem( new Bandana());

			switch ( Utility.Random( 7 ))
			{
				case 0: AddItem( new Longsword() ); break;
				case 1: AddItem( new Cutlass() ); break;
				case 2: AddItem( new Broadsword() ); break;
				case 3: AddItem( new Axe() ); break;
				case 4: AddItem( new Club() ); break;
				case 5: AddItem( new Dagger() ); break;
				case 6: AddItem( new Spear() ); break;
			}

			Utility.AssignRandomHair( this );
		}

		public override void GenerateLoot() {
			AddLoot(LootPack.Poor);
		}

		public override bool AlwaysMurderer{ get{ return true; } }

		public rzezimieszek( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}