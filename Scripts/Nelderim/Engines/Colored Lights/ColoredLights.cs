using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Items
{
    public class LightRed : Item
    {
        [Constructable]
        public LightRed()
            : base(0x40fe)
        {
            this.Light = LightType.Circle150;
        }
        
        [Constructable]
        public LightRed( LightType light)
            : base(0x40fe)
        {
            this.Movable = true;
            this.Light = light;
        }

        public LightRed(Serial serial)
            : base(serial)
        {
        }
        public override void OnDoubleClick(Mobile m)
        {
            if (this.Light == LightType.Circle150)
            {
                this.Light = LightType.Circle225;
            }
            else if (this.Light == LightType.Circle225)
            {
                this.Light = LightType.Circle300;
            }
            else if (this.Light == LightType.Circle300)
            {
                this.Light = LightType.Circle375;
            }
            else if (this.Light == LightType.Circle375)
            {
                this.Light = LightType.Circle450;
            }
            else if (this.Light == LightType.Circle450)
            {
                this.Light = LightType.DarkCircle300;
            }
            else if (this.Light == LightType.DarkCircle300)
            {
                this.Light = LightType.Circle150;
            }
            ReleaseWorldPackets();
            Delta(ItemDelta.Update);
        }
        public override bool OnMoveOver(Mobile m)
        {
            if (m.Player)
            {
                Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
                if (m.IsStaff())
                {
                    m.SendMessage("Found the light editor");
                    if (token == null)
                    {
                        m.SendMessage("If you want to edit this light carry a StaffLightEditor.");
                        return true;
                    }
                    else
                    {
                        m.SendMessage("Setting to movable");
                        this.Movable = true;
                        return true;
                    }
                }
                else
                {
                    this.Movable = false;
                }
            }

            return base.OnMoveOver(m);
        }
        public override bool OnMoveOff(Mobile m)
        {
            if (m.IsStaff())
                this.Movable = false;

            return true;
        }
        public override bool OnDragLift(Mobile m)
        {
            Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
            m.SendMessage("Found the light editor");
            if (m.IsStaff())
            {
                if (token == null)
                {
                    this.Movable = false;
                    return false;
                }
                else
                {
                    this.Movable = true;
                    return true;
                }
            }
            else
            {
                this.Movable = false;
                return false;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        goto case 1;
                    }
                case 1:
                    {
                        goto case 0;
                    }
                case 0:
                    {
                        break;
                    }
            }
        }
    }
    public class LightBlue : Item
    {
        [Constructable]
        public LightBlue()
            : base(0x40ff)
        {
            this.Light = LightType.Circle150;
        }

        [Constructable]
        public LightBlue(LightType light)
            : base(0x40ff)
        {
            this.Movable = true;
            this.Light = light;
        }

        public LightBlue(Serial serial)
            : base(serial)
        {
        }
        public override void OnDoubleClick(Mobile m)
        {
            if (this.Light == LightType.Circle150)
            {
                this.Light = LightType.Circle225;
            }
            else if (this.Light == LightType.Circle225)
            {
                this.Light = LightType.Circle300;
            }
            else if (this.Light == LightType.Circle300)
            {
                this.Light = LightType.Circle375;
            }
            else if (this.Light == LightType.Circle375)
            {
                this.Light = LightType.Circle450;
            }
            else if (this.Light == LightType.Circle450)
            {
                this.Light = LightType.DarkCircle300;
            }
            else if (this.Light == LightType.DarkCircle300)
            {
                this.Light = LightType.Circle150;
            }
            ReleaseWorldPackets();
            Delta(ItemDelta.Update);
        }
        public override bool OnMoveOver(Mobile m)
        {
            if (m.Player)
            {
                Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
                if (m.IsStaff())
                {
                    m.SendMessage("Found the light editor");
                    if (token == null)
                    {
                        m.SendMessage("If you want to edit this light carry a StaffLightEditor.");
                        return true;
                    }
                    else
                    {
                        m.SendMessage("Setting to movable");
                        this.Movable = true;
                        return true;
                    }
                }
                else
                {
                    this.Movable = false;
                }
            }

            return base.OnMoveOver(m);
        }
        public override bool OnMoveOff(Mobile m)
        {
            if (m.IsStaff())
                this.Movable = false;

            return true;
        }
        public override bool OnDragLift(Mobile m)
        {
            Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
            m.SendMessage("Found the light editor");
            if (m.IsStaff())
            {
                if (token == null)
                {
                    this.Movable = false;
                    return false;
                }
                else
                {
                    this.Movable = true;
                    return true;
                }
            }
            else
            {
                this.Movable = false;
                return false;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        goto case 1;
                    }
                case 1:
                    {
                        goto case 0;
                    }
                case 0:
                    {
                        break;
                    }
            }
        }
    }
    public class LightGreen : Item
    {
        [Constructable]
        public LightGreen()
            : base(0x4100)
        {
            this.Light = LightType.Circle150;
        }

        [Constructable]
        public LightGreen(LightType light)
            : base(0x4100)
        {
            this.Movable = true;
            this.Light = light;
        }

        public LightGreen(Serial serial)
            : base(serial)
        {
        }
        public override void OnDoubleClick(Mobile m)
        {
            if (this.Light == LightType.Circle150)
            {
                this.Light = LightType.Circle225;
            }
            else if (this.Light == LightType.Circle225)
            {
                this.Light = LightType.Circle300;
            }
            else if (this.Light == LightType.Circle300)
            {
                this.Light = LightType.Circle375;
            }
            else if (this.Light == LightType.Circle375)
            {
                this.Light = LightType.Circle450;
            }
            else if (this.Light == LightType.Circle450)
            {
                this.Light = LightType.DarkCircle300;
            }
            else if (this.Light == LightType.DarkCircle300)
            {
                this.Light = LightType.Circle150;
            }
            ReleaseWorldPackets();
            Delta(ItemDelta.Update);
        }
        public override bool OnMoveOver(Mobile m)
        {
            if (m.Player)
            {
                Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
                if (m.IsStaff())
                {
                    m.SendMessage("Found the light editor");
                    if (token == null)
                    {
                        m.SendMessage("If you want to edit this light carry a StaffLightEditor.");
                        return true;
                    }
                    else
                    {
                        m.SendMessage("Setting to movable");
                        this.Movable = true;
                        return true;
                    }
                }
                else
                {
                    this.Movable = false;
                }
            }

            return base.OnMoveOver(m);
        }
        public override bool OnMoveOff(Mobile m)
        {
            if (m.IsStaff())
                this.Movable = false;

            return true;
        }
        public override bool OnDragLift(Mobile m)
        {
            Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
            m.SendMessage("Found the light editor");
            if (m.IsStaff())
            {
                if (token == null)
                {
                    this.Movable = false;
                    return false;
                }
                else
                {
                    this.Movable = true;
                    return true;
                }
            }
            else
            {
                this.Movable = false;
                return false;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        goto case 1;
                    }
                case 1:
                    {
                        goto case 0;
                    }
                case 0:
                    {
                        break;
                    }
            }
        }
    }
    public class LightPurple : Item
    {
        [Constructable]
        public LightPurple()
            : base(0x4101)
        {
            this.Light = LightType.Circle150;
        }

        [Constructable]
        public LightPurple(LightType light)
            : base(0x4101)
        {
            this.Movable = true;
            this.Light = light;
        }

        public LightPurple(Serial serial)
            : base(serial)
        {
        }
        public override void OnDoubleClick(Mobile m)
        {
            if (this.Light == LightType.Circle150)
            {
                this.Light = LightType.Circle225;
            }
            else if (this.Light == LightType.Circle225)
            {
                this.Light = LightType.Circle300;
            }
            else if (this.Light == LightType.Circle300)
            {
                this.Light = LightType.Circle375;
            }
            else if (this.Light == LightType.Circle375)
            {
                this.Light = LightType.Circle450;
            }
            else if (this.Light == LightType.Circle450)
            {
                this.Light = LightType.DarkCircle300;
            }
            else if (this.Light == LightType.DarkCircle300)
            {
                this.Light = LightType.Circle150;
            }
            ReleaseWorldPackets();
            Delta(ItemDelta.Update);
        }
        public override bool OnMoveOver(Mobile m)
        {
            if (m.Player)
            {
                Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
                if (m.IsStaff())
                {
                    m.SendMessage("Found the light editor");
                    if (token == null)
                    {
                        m.SendMessage("If you want to edit this light carry a StaffLightEditor.");
                        return true;
                    }
                    else
                    {
                        m.SendMessage("Setting to movable");
                        this.Movable = true;
                        return true;
                    }
                }
                else
                {
                    this.Movable = false;
                }
            }

            return base.OnMoveOver(m);
        }
        public override bool OnMoveOff(Mobile m)
        {
            if (m.IsStaff())
                this.Movable = false;

            return true;
        }
        public override bool OnDragLift(Mobile m)
        {
            Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
            m.SendMessage("Found the light editor");
            if (m.IsStaff())
            {
                if (token == null)
                {
                    this.Movable = false;
                    return false;
                }
                else
                {
                    this.Movable = true;
                    return true;
                }
            }
            else
            {
                this.Movable = false;
                return false;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        goto case 1;
                    }
                case 1:
                    {
                        goto case 0;
                    }
                case 0:
                    {
                        break;
                    }
            }
        }
    }
    public class LightWhite : Item
    {
        [Constructable]
        public LightWhite()
            : base(0x1646)
        {
            this.Light = LightType.Circle150;
        }

        [Constructable]
        public LightWhite(LightType light)
            : base(0x1646)
        {
            this.Movable = true;
            this.Light = light;
        }

        public LightWhite(Serial serial)
            : base(serial)
        {
        }
        public override void OnDoubleClick(Mobile m)
        {
            if (this.Light == LightType.Circle150)
            {
                this.Light = LightType.Circle225;
            }
            else if (this.Light == LightType.Circle225)
            {
                this.Light = LightType.Circle300;
            }
            else if (this.Light == LightType.Circle300)
            {
                this.Light = LightType.Circle375;
            }
            else if (this.Light == LightType.Circle375)
            {
                this.Light = LightType.Circle450;
            }
            else if (this.Light == LightType.Circle450)
            {
                this.Light = LightType.DarkCircle300;
            }
            else if (this.Light == LightType.DarkCircle300)
            {
                this.Light = LightType.Circle150;
            }
            ReleaseWorldPackets();
            Delta(ItemDelta.Update);
        }
        public override bool OnMoveOver(Mobile m)
        {
            if (m.Player)
            {
                Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
                if (m.IsStaff())
                {
                    m.SendMessage("Found the light editor");
                    if (token == null)
                    {
                        m.SendMessage("If you want to edit this light carry a StaffLightEditor.");
                        return true;
                    }
                    else
                    {
                        m.SendMessage("Setting to movable");
                        this.Movable = true;
                        return true;
                    }
                }
                else
                {
                    this.Movable = false;
                }
            }

            return base.OnMoveOver(m);
        }
        public override bool OnMoveOff(Mobile m)
        {
            if (m.IsStaff())
                this.Movable = false;

            return true;
        }
        public override bool OnDragLift(Mobile m)
        {
            Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
            m.SendMessage("Found the light editor");
            if (m.IsStaff())
            {
                if (token == null)
                {
                    this.Movable = false;
                    return false;
                }
                else
                {
                    this.Movable = true;
                    return true;
                }
            }
            else
            {
                this.Movable = false;
                return false;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
    public class LightYellow : Item
    {
        [Constructable]
        public LightYellow()
            : base(0x1647)
        {
            this.Light = LightType.Circle150;
        }

        [Constructable]
        public LightYellow(LightType light)
            : base(0x1647)
        {
            this.Movable = true;
            this.Light = light;
        }

        public LightYellow(Serial serial)
            : base(serial)
        {
        }
        public override void OnDoubleClick(Mobile m)
        {
            if (this.Light == LightType.Circle150)
            {
                this.Light = LightType.Circle225;
            }
            else if (this.Light == LightType.Circle225)
            {
                this.Light = LightType.Circle300;
            }
            else if (this.Light == LightType.Circle300)
            {
                this.Light = LightType.Circle375;
            }
            else if (this.Light == LightType.Circle375)
            {
                this.Light = LightType.Circle450;
            }
            else if (this.Light == LightType.Circle450)
            {
                this.Light = LightType.DarkCircle300;
            }
            else if (this.Light == LightType.DarkCircle300)
            {
                this.Light = LightType.Circle150;
            }
            ReleaseWorldPackets();
            Delta(ItemDelta.Update);
        }
        public override bool OnMoveOver(Mobile m)
        {
            if (m.Player)
            {
                Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
                if (m.IsStaff())
                {
                    m.SendMessage("Found the light editor");
                    if (token == null)
                    {
                        m.SendMessage("If you want to edit this light carry a StaffLightEditor.");
                        return true;
                    }
                    else
                    {
                        m.SendMessage("Setting to movable");
                        this.Movable = true;
                        return true;
                    }
                }
                else
                {
                    this.Movable = false;
                }
            }

            return base.OnMoveOver(m);
        }
        public override bool OnMoveOff(Mobile m)
        {
            if (m.IsStaff())
                this.Movable = false;

            return true;
        }
        public override bool OnDragLift(Mobile m)
        {
            Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
            m.SendMessage("Found the light editor");
            if (m.IsStaff())
            {
                if (token == null)
                {
                    this.Movable = false;
                    return false;
                }
                else
                {
                    this.Movable = true;
                    return true;
                }
            }
            else
            {
                this.Movable = false;
                return false;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        goto case 1;
                    }
                case 1:
                    {
                        goto case 0;
                    }
                case 0:
                    {
                        break;
                    }
            }
        }
    }
    public class LightDarkPurple : Item
    {
        [Constructable]
        public LightDarkPurple()
            : base(0x0e6a)
        {
            this.Light = LightType.Circle150;
        }

        [Constructable]
        public LightDarkPurple(LightType light)
            : base(0x0e6a)
        {
            this.Movable = true;
            this.Light = light;
        }

        public LightDarkPurple(Serial serial)
            : base(serial)
        {
        }
        public override void OnDoubleClick(Mobile m)
        {
            if (this.Light == LightType.Circle150)
            {
                this.Light = LightType.Circle225;
            }
            else if (this.Light == LightType.Circle225)
            {
                this.Light = LightType.Circle300;
            }
            else if (this.Light == LightType.Circle300)
            {
                this.Light = LightType.Circle375;
            }
            else if (this.Light == LightType.Circle375)
            {
                this.Light = LightType.Circle450;
            }
            else if (this.Light == LightType.Circle450)
            {
                this.Light = LightType.DarkCircle300;
            }
            else if (this.Light == LightType.DarkCircle300)
            {
                this.Light = LightType.Circle150;
            }
            ReleaseWorldPackets();
            Delta(ItemDelta.Update);
        }
        public override bool OnMoveOver(Mobile m)
        {
            if (m.Player)
            {
                Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
                if (m.IsStaff())
                {
                    m.SendMessage("Found the light editor");
                    if (token == null)
                    {
                        m.SendMessage("If you want to edit this light carry a StaffLightEditor.");
                        return true;
                    }
                    else
                    {
                        m.SendMessage("Setting to movable");
                        this.Movable = true;
                        return true;
                    }
                }
                else
                {
                    this.Movable = false;
                }
            }

            return base.OnMoveOver(m);
        }
        public override bool OnDragLift(Mobile m)
        {
            Item token = m.Backpack.FindItemByType(typeof(StaffLightEditor));
            m.SendMessage("Found the light editor");
            if (m.IsStaff())
            {
                if (token == null)
                {
                    this.Movable = false;
                    return false;
                }
                else
                {
                    this.Movable = true;
                    return true;
                }
            }
            else
            {
                this.Movable = false;
                return false;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        goto case 1;
                    }
                case 1:
                    {
                        goto case 0;
                    }
                case 0:
                    {
                        break;
                    }
            }
        }
    }
}
