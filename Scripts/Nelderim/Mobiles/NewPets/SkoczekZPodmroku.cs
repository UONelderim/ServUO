using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zlowki skoczka z podmroku " )]
	public class SkoczekZPodmroku : BaseMount
	{


	    public override TimeSpan MountAbilityDelay { get { return TimeSpan.FromHours( 1.0 ); } }

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
		public SkoczekZPodmroku() : this( "Skoczek Z Podmroku" )
		{
		}

		[Constructable]
		public SkoczekZPodmroku( string name ) : base( name, 187, 0x3EBA, AIType.AI_Mage, FightMode.Evil, 12, 5, 0.2, 0.4 )
		{
			BaseSoundID = 0x3C5;
            Hue = 2672;
			SetStr( 296, 325 );
			SetDex( 86, 105 );
			SetInt( 186, 225 );

			SetHits( 191, 210 );

			SetDamage( 16, 22 );



			SetDamageType( ResistanceType.Cold, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 35, 65 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 15, 20 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 55, 75 );

			SetSkill( SkillName.EvalInt, 80.1, 98.0 );
			SetSkill( SkillName.Magery, 100.4, 120.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 85.3, 110.0 );
			SetSkill( SkillName.Tactics, 20.1, 22.5 );
			SetSkill( SkillName.Wrestling, 30.5, 50.0 );

			Fame = 9000;
			Karma = 9000;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 102.1;
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
            if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
            {
                if (Utility.RandomDouble() < 0.20)
                    corpse.DropItem(new Ruby());
            }

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Potions );
		}

		public override int Meat{ get{ return 3; } }
		public override int Hides{ get{ return 5; } }
		public override HideType HideType{ get{ return HideType.Horned; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }
        public override PackInstinct PackInstinct{ get{ return PackInstinct.Daemon; } }

		public SkoczekZPodmroku( Serial serial ) : base( serial )
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