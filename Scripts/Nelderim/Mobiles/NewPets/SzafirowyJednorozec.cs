using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki szafirowego jednorozca" )]
	public class SzafirowyJednorozec : BaseMount
	{
		public override bool AllowMaleRider => false;
		public override bool AllowMaleTamer => true;

		public override bool InitialInnocent => true;

		public override TimeSpan MountAbilityDelay => TimeSpan.FromHours( 1.0 );

		public override void OnDisallowedRider( Mobile m )
		{
			m.SendLocalizedMessage( 1042318 ); // The unicorn refuses to allow you to ride it.
		}

		public override bool DoMountAbility( int damage, Mobile attacker )
		{
			if( Rider == null || attacker == null )	//sanity
				return false;

			if( Rider.Poisoned && ((Rider.Hits - damage) < 40) )
			{
				Poison p = Rider.Poison;

				if( p != null )
				{
					int chanceToCure = 10000 + (int)(this.Skills[SkillName.Magery].Value * 75) - ((p.Level + 1) * (p.Level < 4 ? 3300 : 3100));
					chanceToCure /= 100;

					if( chanceToCure > Utility.Random( 100 ) )
					{
						if( Rider.CurePoison( this ) )	//TODO: Confirm if mount is the one flagged for curing it or the rider is
						{
							Rider.LocalOverheadMessage( Server.Network.MessageType.Regular, 0x3B2, true, "Twoje zwierze wyczuwa, że jesteś w niebezpieczeństwie i chroni Cię przed nim" );
							Rider.FixedParticles( 0x373A, 10, 15, 5012, EffectLayer.Waist );
							Rider.PlaySound( 0x1E0 );	// Cure spell effect.
							Rider.PlaySound( 0xA9 );		// Unicorn's whinny.

							return true;
						}
					}
				}
			}

			return false;
		}

		[Constructable]
		public SzafirowyJednorozec() : this( "szafirowy jednorozec" )
		{
		}

		[Constructable]
		public SzafirowyJednorozec( string name ) : base( name, 0x7A, 0x3EB4, AIType.AI_Mage, FightMode.Evil, 12, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x4BC;
			Hue = 1558;

			SetStr( 296, 325 );
			SetDex( 96, 115 );
			SetInt( 186, 225 );

			SetHits( 191, 210 );

			SetDamage( 16, 22 );

			SetDamageType( ResistanceType.Physical, 45 );
			SetDamageType( ResistanceType.Energy, 55 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 25, 40 );
			SetResistance( ResistanceType.Cold, 25, 40 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 55, 60 );

			SetSkill( SkillName.EvalInt, 80.1, 90.0 );
			SetSkill( SkillName.Magery, 60.2, 80.0 );
			SetSkill( SkillName.Necromancy, 60.2, 80.0 );
			SetSkill( SkillName.SpiritSpeak, 60.2, 80.0 );
			SetSkill( SkillName.Meditation, 50.1, 60.0 );
			SetSkill( SkillName.MagicResist, 75.3, 90.0 );
			SetSkill( SkillName.Tactics, 20.1, 22.5 );
			SetSkill( SkillName.Wrestling, 80.5, 92.5 );

			Fame = 9000;
			Karma = -9000;
			

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 96.1;
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
            if( !IsBonded && !corpse.Carved && !IsChampionSpawn)
            {
			    if ( Utility.RandomDouble() < 0.20 )
				    corpse.DropItem( new BowstringUnicorn() );
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.Potions );
		}

		public override TribeType Tribe => TribeType.Fey;
		public override Poison PoisonImmune => Poison.Lethal;
		public override int Meat => 3;
		public override int Hides => 5;
		public override HideType HideType => HideType.Horned;
		public override FoodType FavoriteFood => FoodType.FruitsAndVegies | FoodType.GrainsAndHay;

		public SzafirowyJednorozec( Serial serial ) : base( serial )
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
