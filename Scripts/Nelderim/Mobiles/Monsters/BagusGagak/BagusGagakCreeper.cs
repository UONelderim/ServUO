using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki wielkiego pelzacza" )]
	public class BagusGagakCreeper : BaseCreature
	{
		public override bool IgnoreYoungProtection { get { return Core.ML; } }

		[Constructable]
		public BagusGagakCreeper() : base( AIType.AI_BattleMage, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{
			Name = "Pelzacz Bagus Gagak";
			Body = 313;
			Hue = 2594;
			BaseSoundID = 0xE0;

			SetStr( 1300, 1400 );
			SetDex( 200 );
			SetInt( 500 );

			SetHits( 4000 );

			SetDamage( 25, 30 );

			SetDamageType( ResistanceType.Physical, 70 );
			SetDamageType( ResistanceType.Poison, 30 );

			SetResistance( ResistanceType.Physical, 60 );
			SetResistance( ResistanceType.Fire, 55 );
			SetResistance( ResistanceType.Cold, 55 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 50 );

			SetSkill( SkillName.DetectHidden, 80.0 );
			SetSkill( SkillName.EvalInt, 110.0 );
			SetSkill( SkillName.Magery, 110.0 );
			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.Poisoning, 100.0 );
			SetSkill( SkillName.MagicResist, 120.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 40;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

        /* wylaczamy artefakty poki co: sa to artefakty jak z Doom
		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			if ( !Summoned && !NoKillAwards && DemonKnight.CheckArtifactChance( this ) )
				DemonKnight.DistributeArtifact( this );
		} */

		public override bool BardImmune{ get{ return !Core.SE; } }
		public override bool Unprovokable{ get{ return Core.SE; } }
		public override bool Uncalmable{ get{ return Core.SE; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }

		public override int TreasureMapLevel{ get{ return 1; } }

		public BagusGagakCreeper( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 471 )
				BaseSoundID = 0xE0;
		}
	}
}