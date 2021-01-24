using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki ognistej wyverny" )]
	public class OgnistaWyverna : BaseCreature
	{
		[Constructable]
		public OgnistaWyverna () : base( AIType.AI_Melee, FightMode.Weakest, 12, 1, 0.2, 0.4 )
		{
			Name = "ognista wyverna";
			Body = 62;
			BaseSoundID = 362;
			
			Hue = 1568;
			SetStr( 172, 230 );
			SetDex( 153, 172 );
			SetInt( 51, 90 );

			SetHits( 125, 151 );

			SetDamage( 8, 18 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 80 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 30, 60 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 65.1, 80.0 );
			SetSkill( SkillName.Tactics, 85.1, 99.0 );
			SetSkill( SkillName.Wrestling, 85.1, 99.0 );

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 40;
		}

        public override void OnCarve(Mobile from, Corpse corpse, Item with)
        {
            if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
            {
                if (Utility.RandomDouble() < 0.10)
                    corpse.DropItem(new VolcanicAsh());
            }

            base.OnCarve(from, corpse, with);
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.MedScrolls );
		}

		public override bool ReacquireOnMovement{ get{ return true; } }
        public override double AttackMasterChance { get { return 0.45; } }
		public override int Meat{ get{ return 4; } }
		public override int Hides{ get{ return 8; } }
		public override HideType HideType{ get{ return HideType.Horned; } }

		public override int GetAttackSound()
		{
			return 713;
		}

		public override int GetAngerSound()
		{
			return 718;
		}

		public override int GetDeathSound()
		{
			return 716;
		}

		public override int GetHurtSound()
		{
			return 721;
		}

		public override int GetIdleSound()
		{
			return 725;
		}

		public OgnistaWyverna( Serial serial ) : base( serial )
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
