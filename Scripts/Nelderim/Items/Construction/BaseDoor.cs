using System.Collections.Generic;
using Nelderim;
using System.Net.Sockets;
using Server.Network;

namespace Server.Items
{
    public partial class BaseDoor : ILockpickable
    {
        public virtual void LockPick(Mobile from)
        {
            if (typeof(BaseHouseDoor).IsAssignableFrom(GetType()))
            {
                from.SendMessage("Chcialbys...");
                return;
            }
            if (Open)
            {
                from.SendMessage("Po co wywazac otwarte drzwi?");
                return;
            }
            if (from.AccessLevel == AccessLevel.Player && !from.InRange(GetWorldLocation(), 2))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return;
            }

            Use(from, true);
        }

        public Mobile Picker
        {
            get { return null; }
            set { }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxLockLevel
        {
            get { return BaseDoorExt.Get(this).MaxLockLevel; }
            set { BaseDoorExt.Get(this).MaxLockLevel = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int LockLevel
        {
            get { return BaseDoorExt.Get(this).LockLevel; }
            set { BaseDoorExt.Get(this).LockLevel = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RequiredSkill
        {
            get { return BaseDoorExt.Get(this).RequiredSkill; }
            set { BaseDoorExt.Get(this).RequiredSkill = value; }
        }
    }

    class BaseDoorExt() : NExtension<BaseDoorExtInfo>("BaseDoor")
    {
        public static void Configure()
        {
            Register(new BaseDoorExt());
        }
    }

    class BaseDoorExtInfo : NExtensionInfo
    {
        public int MaxLockLevel { get; set; }
        public int LockLevel { get; set; }
        public int RequiredSkill { get; set; }

        public override void Serialize(GenericWriter writer)
        {
            writer.Write((int)0); // version

            writer.Write((int)LockLevel);
            writer.Write((int)MaxLockLevel);
            writer.Write((int)RequiredSkill);
        }

        public override void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();

            LockLevel = reader.ReadInt();
            MaxLockLevel = reader.ReadInt();
            RequiredSkill = reader.ReadInt();
        }
    }
}
