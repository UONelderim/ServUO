using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "martwy motyl" )]
	public class Motyl : BaseCreature
	{
		[Constructable]
		public Motyl() : base( AIType.AI_Animal, FightMode.Aggressor, 7, 1, 0.2, 0.4 )
		{
			Name = "motylek";
			Body = 903;
			Hue = Utility.RandomList( 2469, 2467, 2465, 1152, 2342, 2351, 2484, 2489 );

			SetStr( 1, 2 );
			SetDex( 8, 43 );
			SetInt( 9, 37 );

			SetHits( 1, 2 );
			SetMana( 0 );

			SetDamage( 1 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 1, 2 );

			SetSkill( SkillName.MagicResist, 22.1, 47.0 );
			SetSkill( SkillName.Tactics, 0.2, 1.0 );
			SetSkill( SkillName.Wrestling, 0.2, 1.0 );

			Fame = 0;
			Karma = 1;

			VirtualArmor = 1;

		
		}

		
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public Motyl(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
