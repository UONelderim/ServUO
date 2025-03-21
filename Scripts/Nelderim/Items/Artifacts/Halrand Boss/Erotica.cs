
using System;
using Server;

namespace Server.Items
{
    public class Erotica : FemaleStuddedChest
    {
        public override int BasePhysicalResistance{ get{ return 3; } }
        public override int BaseColdResistance{ get{ return 12; } }
        public override int BaseFireResistance{ get{ return 12; } }
        public override int BaseEnergyResistance{ get{ return 13; } }
        public override int BasePoisonResistance{ get{ return 18; } }
        public override int InitMinHits{ get{ return 255; } }
        public override int InitMaxHits{ get{ return 255; } }
        
        public override int LabelNumber => 3070059;//Ciegna

        [Constructable]
        public Erotica()
        {
            Name = "Ciegna";
            Hue = 25;
            Attributes.BonusInt = 5;
            Attributes.DefendChance = 10;
            Attributes.SpellDamage = 5;
            Attributes.LowerManaCost = 8;
        }

        public Erotica(Serial serial) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    } 
} 
