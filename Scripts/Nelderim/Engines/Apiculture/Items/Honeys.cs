using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
    public class JarHoneyGryczany : JarHoney
    {
        [Constructable]
        public JarHoneyGryczany()
            : base()
        {
            Weight = 1.0;
            Stackable = true;
            Name = "Sloik miodu gryczanego";
            Hue = 1126;
        }

        public JarHoneyGryczany(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            Stackable = true;
        }
    }

    public class JarHoneyLesny : JarHoney
    {
        [Constructable]
        public JarHoneyLesny()
            : base()
        {
            Weight = 1.0;
            Stackable = true;
            Name = "Sloik miodu lesnego";
            Hue = 2126;
        }

        public JarHoneyLesny(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            Stackable = true;
        }
    }

    public class JarHoneySpadziowy : JarHoney
    {
        [Constructable]
        public JarHoneySpadziowy()
            : base()
        {
            Weight = 1.0;
            Stackable = true;
            Name = "Sloik miodu spadziowego";
            Hue = 1214;
        }

        public JarHoneySpadziowy(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            Stackable = true;
        }
    }

    public class JarHoneyNostrzykowy : JarHoney
    {
        [Constructable]
        public JarHoneyNostrzykowy()
            : base()
        {
            Weight = 1.0;
            Stackable = true;
            Name = "Sloik miodu nostrzykowy";
            Hue = 1105;
        }

        public JarHoneyNostrzykowy(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            Stackable = true;
        }
    }
}
