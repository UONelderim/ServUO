using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "zwloki mrocznego pajaka" )]
	public class DeathSpiderFamiliar : BaseFamiliar
	{
		public DeathSpiderFamiliar()
		{
			Name = "mroczny pajak";
			Hue = 2406;
			Body = 11;
			BaseSoundID = 1170;

			SetStr( 200 );
			SetDex( 100 );
			SetInt( 100 );

			SetHits( 200 );
			SetStam( 160 );
			SetMana( 100 );

			SetDamage( 12, 20 );

			SetDamageType( ResistanceType.Physical, 100 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 60, 70 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 80, 90 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Wrestling, 70.0, 80.0 );
			SetSkill( SkillName.Tactics, 70.0, 80.0 );
			SetSkill( SkillName.Poisoning, 40.0, 50.0 );

			ControlSlots = 1;
			PackItem( new SpidersSilk( 10 ) );

		}

		

		public DeathSpiderFamiliar( Serial serial ) : base( serial )
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
