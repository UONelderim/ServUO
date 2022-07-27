using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwolki snieznego ki-rina " )]
	public class SnieznyKirin : BaseMount
	{
		public override bool AllowFemaleRider => false;
		public override bool AllowFemaleTamer => true;

		public override bool InitialInnocent => true;

		public override TimeSpan MountAbilityDelay => TimeSpan.FromHours( 1.0 );

		public override void OnDisallowedRider( Mobile m )
		{
			m.SendLocalizedMessage( 1042319 ); // The Ki-Rin refuses your attempts to mount it.
		}

		public override bool DoMountAbility( int damage, Mobile attacker )
		{
			if( Rider == null || attacker == null )	//sanity
				return false;

			if( (Rider.Hits - damage) < 30 && Rider.Map == attacker.Map && Rider.InRange( attacker, 18 ) )	//Range and map checked here instead of other base fuction because of abiliites that don't need to check this
			{
				attacker.BoltEffect( 0 );
				// 35~100 damage, unresistable, by the Ki-rin.
				attacker.Damage( Utility.RandomMinMax( 35, 100 ), this, false );	//Don't inform mount about this damage, Still unsure wether or not it's flagged as the mount doing damage or the player.  If changed to player, without the extra bool it'd be an infinite loop

				Rider.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1042534 );	// Your mount calls down the forces of nature on your opponent.
				Rider.FixedParticles( 0, 0, 0, 0x13A7, EffectLayer.Waist );
				Rider.PlaySound( 0xA9 );	// Ki-rin's whinny.
				return true;
			}

			return false;
		}

		[Constructable]
		public SnieznyKirin() : this( "sniezny ki-rin" )
		{
		}

		[Constructable]
		public SnieznyKirin( string name ) : base( name, 132, 0x3EAD, AIType.AI_Mage, FightMode.Evil, 12, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x3C5;
			Hue = 1153;

			SetStr( 296, 325 );
			SetDex( 86, 105 );
			SetInt( 186, 225 );

			SetHits( 191, 250 );

			SetDamage( 18, 24 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Cold, 50 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 45, 55 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.EvalInt, 80.1, 90.0 );
			SetSkill( SkillName.Magery, 60.4, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 85.3, 100.0 );
			SetSkill( SkillName.Tactics, 20.1, 22.5 );
			SetSkill( SkillName.Wrestling, 80.5, 92.5 );

			Fame = 9000;
			Karma = 9000;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 96.1;
			
			SetWeaponAbility(WeaponAbility.ParalyzingBlow);
		}
		
		public override double WeaponAbilityChance => 0.133;

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
            if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
            {
                if (Utility.RandomDouble() < 0.20)
                    corpse.DropItem(new BowstringKirin());
            }

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Potions );
		}

		public override TribeType Tribe => TribeType.Fey;

		public override int Meat => 3;
		public override int Hides => 5;
		public override HideType HideType => HideType.Horned;
		public override FoodType FavoriteFood => FoodType.FruitsAndVegies | FoodType.GrainsAndHay;

		public SnieznyKirin( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version == 0 )
				AI = AIType.AI_Mage;
		}
	}
}
