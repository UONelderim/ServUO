using Server;
using System;

namespace Server.Items
{
    public class EpauletteBearingTheCrestOfBlackthorn6 : Cloak
    {
        public override bool IsArtifact { get { return true; } }
        
        [Constructable]
        public EpauletteBearingTheCrestOfBlackthorn6()
        {
            ReforgedSuffix = ReforgedSuffix.Blackthorn;
            ItemID = 0x9985;            
            Attributes.BonusHits = 3;
            Attributes.BonusInt = 5;
            Hue = 2019;            
        }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }       

        public EpauletteBearingTheCrestOfBlackthorn6(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}