using Server.Items;

namespace Server.Mobiles
{
    [CorpseName( "zwloki goblina" )]
    public class Goblin : BaseCreature
    {
        [Constructable]
        public Goblin() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
        {
            Name = "goblin";
            Body = 182;
            Hue = 2212;
            BaseSoundID = 1114;

            SetStr( 60 );
            SetDex( 20 );
            SetInt( 50 );

            SetHits( 70, 90 );

            SetDamage( 6, 8 );

            SetDamageType( ResistanceType.Physical, 100 );
            SetDamageType( ResistanceType.Cold, 0 );
            SetDamageType( ResistanceType.Fire, 0 );
            SetDamageType( ResistanceType.Energy, 0 );
            SetDamageType( ResistanceType.Poison, 0 );

            SetResistance( ResistanceType.Physical, 35 );
            SetResistance( ResistanceType.Fire, 36 );
            SetResistance( ResistanceType.Cold, 29 );
            SetResistance( ResistanceType.Poison, 35 );
            SetResistance( ResistanceType.Energy, 35 );

            SetSkill( SkillName.Tactics, 21.9, 37.8 );
            SetSkill( SkillName.Wrestling, 87.4, 94.2 );
            SetSkill( SkillName.Anatomy, 52.1, 57.8 );

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 2;

            if ( Utility.RandomDouble() < 0.3 )
                PackItem( new BowstringCannabis() );

        }

        public override bool CanRummageCorpses { get { return true; } }

        public Goblin( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    }
}
