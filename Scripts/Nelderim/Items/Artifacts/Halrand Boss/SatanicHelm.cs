// Created with UO Armor Generator
// Created On: 2/9/2010 12:34:10 PM
// By: dxmonkey

using System;
using Server;

namespace Server.Items
{
    public class SatanicHelm : DragonHelm
    {
        public override int BasePhysicalResistance{ get{ return 18; } }
        public override int BaseColdResistance{ get{ return 2; } }
        public override int BaseFireResistance{ get{ return 33; } }
        public override int BaseEnergyResistance{ get{ return 14; } }
        public override int BasePoisonResistance{ get{ return 6; } }
        public override int InitMinHits{ get{ return 255; } }
        public override int InitMaxHits{ get{ return 255; } }
        
        public override int LabelNumber => 3070060;//Helm Smierci
        
        [Constructable]
        public SatanicHelm()
        {
            Name = "Helm Smierci";
            Hue = 1157;
            StrRequirement = 70;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 5;
            //Random Resonance:
            switch (Utility.Random(5))
            {
	            case 0:
		            AbsorptionAttributes.ResonanceCold = 10;
		            break;
	            case 1:
		            AbsorptionAttributes.ResonanceFire = 10;
		            break;
	            case 2:
		            AbsorptionAttributes.ResonanceKinetic = 10;
		            break;
	            case 3:
		            AbsorptionAttributes.ResonancePoison = 10;
		            break;
	            case 4:
		            AbsorptionAttributes.ResonanceEnergy = 10;
		            break;

        }
			}

        public SatanicHelm(Serial serial) : base( serial )
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
    } // End Class
} // End Namespace
