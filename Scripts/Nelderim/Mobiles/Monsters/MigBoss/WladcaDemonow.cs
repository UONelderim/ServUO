using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki azatotha - wladcy demonow" )]
	public class WladcaDemonow : BaseCreature
	{
		public override bool BardImmune{ get{ return true; } }
        public override double AttackMasterChance { get { return 0.15; } }
        public override double SwitchTargetChance { get { return 0.15; } }
		public override double DispelDifficulty{ get{ return 135.0; } }
		public override double DispelFocus{ get{ return 45.0; } }
		public override bool AutoDispel{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		
		[Constructable]
		public WladcaDemonow () : base( AIType.AI_Boss, FightMode.Closest, 11, 1, 0.25, 0.5 )
		{
			Name = "azatoth - wladca demonow ";
			Body = 0x310;
			Hue = 2585;
			BaseSoundID = 357;

			SetStr( 1286, 1385 );
			SetDex( 210, 265 );
			SetInt( 800, 900 );

			SetMana( 8000 );
			SetHits( 18000 );

			SetDamage( 26, 35 );

			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 75 );

			SetResistance( ResistanceType.Physical, 90, 100 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 90, 100 );
			SetResistance( ResistanceType.Energy, 90, 100 );

			SetSkill( SkillName.Anatomy, 25.1, 50.0 );
			SetSkill( SkillName.EvalInt, 110 );
			SetSkill( SkillName.Magery, 115.5, 130.0 );
			SetSkill( SkillName.Meditation, 105.1, 120.0 );
			SetSkill( SkillName.MagicResist, 120.5, 130.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 34000;
			Karma = -34000;

			VirtualArmor = 90;
			AddItem( new LightSource() );

            SetWeaponAbility( WeaponAbility.BleedAttack );
		}

        public override void OnCarve(Mobile from, Corpse corpse, Item with)
        {
            if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
            {
                corpse.DropItem(new Bloodspawn());
                corpse.DropItem(new DaemonBone());
            }

            base.OnCarve(from, corpse, with);
        }

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

            ArtifactHelper.ArtifactDistribution(this);
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss ); 
		}
		

		public WladcaDemonow( Serial serial ) : base( serial )
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
