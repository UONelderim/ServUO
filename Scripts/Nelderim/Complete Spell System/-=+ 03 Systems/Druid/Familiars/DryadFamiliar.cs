using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.Druid
{
	[CorpseName( "zwłoki driady" )]
	public class DryadFamiliar : BaseCreature
	{
		[Constructable]
		public DryadFamiliar () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "driada";
			Body = 0x10A;
			Hue = 33770;
			BaseSoundID = 0x4B0;

			SetStr( 400 );
			SetDex( 400 );
			SetInt( 200 );

			SetHits( 445 );
			SetStam( 100 );

			SetDamage( 12, 18 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 70, 80 );

			SetSkill( SkillName.Meditation, 110.0 );
			SetSkill( SkillName.EvalInt, 110.0 );
			SetSkill( SkillName.Magery, 110.0 );
			SetSkill( SkillName.MagicResist, 110.0 );
			SetSkill( SkillName.Tactics, 110.0 );
			SetSkill( SkillName.Wrestling, 110.0 );

			VirtualArmor = 45;
			ControlSlots = 2;
		}

		public DryadFamiliar( Serial serial ) : base( serial )
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
