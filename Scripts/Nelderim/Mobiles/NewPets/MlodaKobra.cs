using System;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("zwloki mlodej kobry")]
    public class MlodaKobra : BaseCreature  // Snake
    {
        [Constructable]
        public MlodaKobra()
            : base(AIType.AI_Animal, FightMode.Aggressor, 12, 1, 0.2, 0.3)
        {
            Name = "mloda kobra";
            Body = 92;
            Hue = 358;
            BaseSoundID = 0xDB;

            SetStr( 161, 360 );
			SetDex( 151, 300 );
			SetInt( 21, 40 );

			SetHits( 97, 216 );

			SetDamage( 5, 21 );

            SetDamageType(ResistanceType.Physical, 30);
			SetDamageType(ResistanceType.Poison, 70);

            SetResistance( ResistanceType.Physical, 35, 45 );
            SetResistance( ResistanceType.Fire, 15, 40 );
			SetResistance( ResistanceType.Cold, 5, 30 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 25, 30 );

			SetSkill( SkillName.Poisoning, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 95.1, 100.0 );
			SetSkill( SkillName.Tactics, 80.1, 95.0 );
			SetSkill( SkillName.Wrestling, 85.1, 100.0 );

            Fame = 300;
            Karma = 0;

            VirtualArmor = 16;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 100.1;
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison { get { return Poison.Greater; } }

        public override bool DeathAdderCharmable { get { return true; } }

        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public MlodaKobra(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}