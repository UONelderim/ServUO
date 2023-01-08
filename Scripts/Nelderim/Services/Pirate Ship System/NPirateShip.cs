using System;
using Server;
using Server.Items;

namespace Server.Multis
{
    public class NPirateShip : BaseBoat
    {
        public override int NorthID => 0x4014;

        public override int EastID => 0x4015;

        public override int SouthID => 0x4016;

        public override int WestID => 0x4017;

        public override int HoldDistance => 5;

        public override int TillerManDistance => -5;

        public override Point2D StarboardOffset => new Point2D(2, -1);

        public override Point2D PortOffset => new Point2D(-2, -1);

        public override Point3D MarkOffset => new Point3D(0, 0, 3);

        public override BaseDockedBoat DockedBoat => new LargeDockedDragonBoat(this);

        [Constructable]
        public NPirateShip() : base(Direction.North)
        {
            Name = "statek piratow";
        }

        public NPirateShip(Serial serial) : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }
    }

    public class NPirateShipDeed : BaseBoatDeed
    {
        public override int LabelNumber => 1041210;

        public override BaseBoat Boat => new NPirateShip();

        [Constructable]
        public NPirateShipDeed() : base(0x4014, new Point3D(0, -1, -5))
        {
            Name = "statek piratow";
            Hue = 1157;
        }

        public NPirateShipDeed(Serial serial) : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }
    }

    public class NPirateShipDocked : BaseDockedBoat
    {
        public override BaseBoat Boat => new LargeDragonBoat(Direction.North);

        public NPirateShipDocked(BaseBoat boat) : base(0x4014, new Point3D(0, -1, -5), boat)
        {
        }

        public NPirateShipDocked(Serial serial) : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }
    }
}
