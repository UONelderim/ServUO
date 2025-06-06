using Server.Network;
using System;
using System.Collections.Generic;

namespace Server.Items
{
    public struct BulletinEquip
    {
        public int itemID;
        public int hue;
        public BulletinEquip(int itemID, int hue)
        {
            this.itemID = itemID;
            this.hue = hue;
        }
    }

    [Flipable(0x1E5E, 0x1E5F)]
    public class BulletinBoard : BaseBulletinBoard
    {
        [Constructable]
        public BulletinBoard()
            : base(0x1E5E)
        {
        }

        public BulletinBoard(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public abstract class BaseBulletinBoard : Item
    {
        public static readonly bool DisplayAuthor = false;

        // Threads will be removed six hours after the last post was made
        private static readonly TimeSpan ThreadDeletionTime = TimeSpan.FromDays(30.0);
        // A player may only create a thread once every two minutes
        private static readonly TimeSpan ThreadCreateTime = TimeSpan.FromMinutes(2.0);
        // A player may only reply once every thirty seconds
        private static readonly TimeSpan ThreadReplyTime = TimeSpan.FromSeconds(30.0);
        private string m_BoardName;
        public BaseBulletinBoard(int itemID)
            : base(itemID)
        {
            m_BoardName = "bulletin board";
            Movable = false;
        }

        public BaseBulletinBoard(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string BoardName
        {
            get
            {
                return m_BoardName;
            }
            set
            {
                m_BoardName = value;
            }
        }
        public static bool CheckTime(DateTime time, TimeSpan range)
        {
            return (time + range) < DateTime.UtcNow;
        }

        public static string FormatTS(TimeSpan ts)
        {
            int totalSeconds = (int)ts.TotalSeconds;
            int seconds = totalSeconds % 60;
            int minutes = totalSeconds / 60;

            if (minutes != 0 && seconds != 0)
                return string.Format("{0} minute{1} and {2} second{3}", minutes, minutes == 1 ? "" : "s", seconds, seconds == 1 ? "" : "s");
            else if (minutes != 0)
                return string.Format("{0} minute{1}", minutes, minutes == 1 ? "" : "s");
            else
                return string.Format("{0} second{1}", seconds, seconds == 1 ? "" : "s");
        }

        public static void Initialize()
        {
            PacketHandlers.Register(0x71, 0, true, BBClientRequest);
        }

        public static void BBClientRequest(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;

            int packetID = pvSrc.ReadByte();
            BaseBulletinBoard board = World.FindItem(pvSrc.ReadSerial()) as BaseBulletinBoard;

            if (board == null || !board.CheckRange(from))
                return;

            switch (packetID)
            {
                case 3:
                    BBRequestContent(from, board, pvSrc);
                    break;
                case 4:
                    BBRequestHeader(from, board, pvSrc);
                    break;
                case 5:
                    BBPostMessage(from, board, pvSrc);
                    break;
                case 6:
                    BBRemoveMessage(from, board, pvSrc);
                    break;
            }
        }

        public static void BBRequestContent(Mobile from, BaseBulletinBoard board, PacketReader pvSrc)
        {
            BulletinMessage msg = World.FindItem(pvSrc.ReadSerial()) as BulletinMessage;

            if (msg == null || msg.Parent != board)
                return;

            from.Send(new BBMessageContent(board, msg));
        }

        public static void BBRequestHeader(Mobile from, BaseBulletinBoard board, PacketReader pvSrc)
        {
            BulletinMessage msg = World.FindItem(pvSrc.ReadSerial()) as BulletinMessage;

            if (msg == null || msg.Parent != board)
                return;

            from.Send(new BBMessageHeader(board, msg));
        }

        public static void BBPostMessage(Mobile from, BaseBulletinBoard board, PacketReader pvSrc)
        {
            BulletinMessage thread = World.FindItem(pvSrc.ReadSerial()) as BulletinMessage;

            if (thread != null && thread.Parent != board)
                thread = null;

            int breakout = 0;

            while (thread != null && thread.Thread != null && breakout++ < 10)
                thread = thread.Thread;

            DateTime lastPostTime = DateTime.MinValue;

            if (board.GetLastPostTime(from, (thread == null), ref lastPostTime))
            {
                if (!CheckTime(lastPostTime, (thread == null ? ThreadCreateTime : ThreadReplyTime)))
                {
                    if (thread == null)
                        from.SendMessage("You must wait {0} before creating a new thread.", FormatTS(ThreadCreateTime));
                    else
                        from.SendMessage("You must wait {0} before replying to another thread.", FormatTS(ThreadReplyTime));

                    return;
                }
            }

            string subject = pvSrc.ReadUTF8StringSafe(pvSrc.ReadByte());

            if (subject.Length == 0)
                return;

            string[] lines = new string[pvSrc.ReadByte()];

            if (lines.Length == 0)
                return;

            for (int i = 0; i < lines.Length; ++i)
                lines[i] = pvSrc.ReadUTF8StringSafe(pvSrc.ReadByte());

            board.PostMessage(from, thread, subject, lines);
        }

        public static void BBRemoveMessage(Mobile from, BaseBulletinBoard board, PacketReader pvSrc)
        {
            BulletinMessage msg = World.FindItem(pvSrc.ReadSerial()) as BulletinMessage;

            if (msg == null || msg.Parent != board)
                return;

            if (from.AccessLevel < AccessLevel.GameMaster && msg.Poster != from)
                return;

            msg.Delete();
        }

        public virtual void Cleanup()
        {
            List<Item> items = Items;

            for (int i = items.Count - 1; i >= 0; --i)
            {
                if (i >= items.Count)
                    continue;

                BulletinMessage msg = items[i] as BulletinMessage;

                if (msg == null)
                    continue;

                msg.Cleanup();

                if (msg.Thread == null && CheckTime(msg.LastPostTime, ThreadDeletionTime))
                {
                    msg.Delete();
                    RecurseDelete(msg); // A root-level thread has expired
                }
            }
        }

        public virtual bool GetLastPostTime(Mobile poster, bool onlyCheckRoot, ref DateTime lastPostTime)
        {
            List<Item> items = Items;
            bool wasSet = false;

            for (int i = 0; i < items.Count; ++i)
            {
                BulletinMessage msg = items[i] as BulletinMessage;

                if (msg == null || msg.Poster != poster)
                    continue;

                if (onlyCheckRoot && msg.Thread != null)
                    continue;

                if (msg.Time > lastPostTime)
                {
                    wasSet = true;
                    lastPostTime = msg.Time;
                }
            }

            return wasSet;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (CheckRange(from))
            {
                Cleanup();

                NetState state = from.NetState;

                state.Send(new BBDisplayBoard(this));

                ContainerContent.Send(state, this);
            }
            else
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            }
        }

        public virtual bool CheckRange(Mobile from)
        {
            if (from.AccessLevel >= AccessLevel.GameMaster)
                return true;

            return (from.Map == Map && from.InRange(GetWorldLocation(), 2));
        }

        public void PostMessage(Mobile from, BulletinMessage thread, string subject, string[] lines)
        {
            if (thread != null)
                thread.LastPostTime = DateTime.UtcNow;

            AddItem(new BulletinMessage(from, thread, subject, lines));

            AcquireItems().Sort(delegate(Item a, Item b) {
                BulletinMessage msgA = a as BulletinMessage;
                BulletinMessage msgB = b as BulletinMessage;
                if (msgA == null || msgB == null)
                    return 0;
                return msgB.Time.CompareTo(msgA.Time);
            });
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            writer.Write(m_BoardName);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_BoardName = reader.ReadString();
                        break;
                    }
            }
        }

        private void RecurseDelete(BulletinMessage msg)
        {
            List<Item> found = new List<Item>();
            List<Item> items = Items;

            for (int i = items.Count - 1; i >= 0; --i)
            {
                if (i >= items.Count)
                    continue;

                BulletinMessage check = items[i] as BulletinMessage;

                if (check == null)
                    continue;

                if (check.Thread == msg)
                {
                    check.Delete();
                    found.Add(check);
                }
            }

            for (int i = 0; i < found.Count; ++i)
                RecurseDelete((BulletinMessage)found[i]);
        }
    }

    public class BulletinMessage : Item
    {
        private Mobile m_Poster;
        private string m_Subject;
        private DateTime m_Time, m_LastPostTime;
        private BulletinMessage m_Thread;
        private string m_PostedName;
        private int m_PostedBody;
        private int m_PostedHue;
        private BulletinEquip[] m_PostedEquip;
        private string[] m_Lines;
        public BulletinMessage(Mobile poster, BulletinMessage thread, string subject, string[] lines)
            : base(0xEB0)
        {
            Movable = false;

            m_Poster = poster;
            m_Subject = removePrefixFromSubject(subject);
            m_Time = DateTime.UtcNow;
            m_LastPostTime = m_Time;
            m_Thread = thread;
            m_PostedName = m_Poster.Name;
            m_PostedBody = m_Poster.Body;
            m_PostedHue = m_Poster.Hue;
            m_Lines = lines;

            List<BulletinEquip> list = new List<BulletinEquip>();

            for (int i = 0; i < poster.Items.Count; ++i)
            {
                Item item = poster.Items[i];

                if (item.Layer >= Layer.OneHanded && item.Layer <= Layer.Mount)
                    list.Add(new BulletinEquip(item.ItemID, item.Hue));
            }

            m_PostedEquip = list.ToArray();
        }

        public BulletinMessage(Serial serial)
            : base(serial)
        {
        }

        public Mobile Poster => m_Poster;
        public BulletinMessage Thread => m_Thread;
        public string Subject => m_Subject;
        public DateTime Time => m_Time;
        public DateTime LastPostTime
        {
            get
            {
                return m_LastPostTime;
            }
            set
            {
                m_LastPostTime = value;
            }
        }
        public string PostedName => m_PostedName;
        public int PostedBody => m_PostedBody;
        public int PostedHue => m_PostedHue;
        public BulletinEquip[] PostedEquip => m_PostedEquip;
        public string[] Lines => m_Lines;
        public string GetTimeAsString()
        {
            return m_Time.ToString("dd MMM");
        }

        public override bool CheckTarget(Mobile from, Targeting.Target targ, object targeted)
        {
            return false;
        }

        public override bool IsAccessibleTo(Mobile check)
        {
            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            writer.Write(m_Poster);
            writer.Write(m_Subject);
            writer.Write(m_Time);
            writer.Write(m_LastPostTime);
            writer.Write(m_Thread != null);
            writer.Write(m_Thread);
            writer.Write(m_PostedName);
            writer.Write(m_PostedBody);
            writer.Write(m_PostedHue);

            writer.Write(m_PostedEquip.Length);

            for (int i = 0; i < m_PostedEquip.Length; ++i)
            {
                writer.Write(m_PostedEquip[i].itemID);
                writer.Write(m_PostedEquip[i].hue);
            }

            writer.Write(m_Lines.Length);

            for (int i = 0; i < m_Lines.Length; ++i)
                writer.Write(m_Lines[i]);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                case 0:
                    {
                        m_Poster = reader.ReadMobile();
                        m_Subject = reader.ReadString();
                        m_Time = reader.ReadDateTime();
                        m_LastPostTime = reader.ReadDateTime();
                        bool hasThread = reader.ReadBool();
                        m_Thread = reader.ReadItem() as BulletinMessage;
                        m_PostedName = reader.ReadString();
                        m_PostedBody = reader.ReadInt();
                        m_PostedHue = reader.ReadInt();

                        m_PostedEquip = new BulletinEquip[reader.ReadInt()];

                        for (int i = 0; i < m_PostedEquip.Length; ++i)
                        {
                            m_PostedEquip[i].itemID = reader.ReadInt();
                            m_PostedEquip[i].hue = reader.ReadInt();
                        }

                        m_Lines = new string[reader.ReadInt()];

                        for (int i = 0; i < m_Lines.Length; ++i)
                            m_Lines[i] = reader.ReadString();

                        if (hasThread && m_Thread == null)
                            Delete();

                        if (version == 0)
                            ValidationQueue<BulletinMessage>.Add(this);

                        break;
                    }
            }
        }

        public void Validate()
        {
            if (!(Parent is BulletinBoard && ((BulletinBoard)Parent).Items.Contains(this)))
                Delete();
        }

        public void Cleanup()
        {
            // clean legacy entries:
            m_Subject = removePrefixFromSubject(m_Subject);
        }

        public static String removePrefixFromSubject(String subject)
        {
            if (subject == null)
                return null;

            var prefixToRemove = "RE: ";
            while (subject.StartsWith(prefixToRemove, StringComparison.OrdinalIgnoreCase))
                subject = subject.Substring(prefixToRemove.Length);

            return subject;
        }
    }

    public class BBDisplayBoard : Packet
    {
        public BBDisplayBoard(BaseBulletinBoard board)
            : base(0x71)
        {
            string name = board.BoardName;

            if (name == null)
                name = "";

            EnsureCapacity(38);

            byte[] buffer = Utility.UTF8.GetBytes(name);

            m_Stream.Write((byte)0x00); // PacketID
            m_Stream.Write(board.Serial); // Bulletin board serial

            // Bulletin board name
            if (buffer.Length >= 29)
            {
                m_Stream.Write(buffer, 0, 29);
                m_Stream.Write((byte)0);
            }
            else
            {
                m_Stream.Write(buffer, 0, buffer.Length);
                m_Stream.Fill(30 - buffer.Length);
            }
        }
    }

    public class BBMessageHeader : Packet
    {
        public BBMessageHeader(BaseBulletinBoard board, BulletinMessage msg)
            : base(0x71)
        {
            string poster = SafeString(BaseBulletinBoard.DisplayAuthor ? msg.PostedName : string.Empty);
            string subject = SafeString(msg.Subject);
            string time = SafeString(msg.GetTimeAsString());

            EnsureCapacity(22 + poster.Length + subject.Length + time.Length);

            m_Stream.Write((byte)0x01); // PacketID
            m_Stream.Write(board.Serial); // Bulletin board serial
            m_Stream.Write(msg.Serial); // Message serial

            BulletinMessage thread = msg.Thread;

            if (thread == null)
                m_Stream.Write(0); // Thread serial--root
            else
                m_Stream.Write(thread.Serial); // Thread serial--parent

            WriteString(poster);
            WriteString(subject);
            WriteString(time);
        }

        public void WriteString(string v)
        {
            byte[] buffer = Utility.UTF8.GetBytes(v);
            int len = buffer.Length + 1;

            if (len > 255)
                len = 255;

            m_Stream.Write((byte)len);
            m_Stream.Write(buffer, 0, len - 1);
            m_Stream.Write((byte)0);
        }

        public string SafeString(string v)
        {
            if (v == null)
                return string.Empty;

            return v;
        }
    }

    public class BBMessageContent : Packet
    {
        public BBMessageContent(BaseBulletinBoard board, BulletinMessage msg)
            : base(0x71)
        {
            string poster = SafeString(BaseBulletinBoard.DisplayAuthor ? msg.PostedName : string.Empty);
            string subject = SafeString(msg.Subject);
            string time = SafeString(msg.GetTimeAsString());

            EnsureCapacity(22 + poster.Length + subject.Length + time.Length);

            m_Stream.Write((byte)0x02); // PacketID
            m_Stream.Write(board.Serial); // Bulletin board serial
            m_Stream.Write(msg.Serial); // Message serial

            WriteString(poster);
            WriteString(subject);
            WriteString(time);

            m_Stream.Write((short)msg.PostedBody);
            m_Stream.Write((short)msg.PostedHue);

            int len = msg.PostedEquip.Length;

            if (len > 255)
                len = 255;

            m_Stream.Write((byte)len);

            for (int i = 0; i < len; ++i)
            {
                BulletinEquip eq = msg.PostedEquip[i];

                m_Stream.Write((short)eq.itemID);
                m_Stream.Write((short)eq.hue);
            }

            len = msg.Lines.Length;

            if (len > 255)
                len = 255;

            m_Stream.Write((byte)len);

            for (int i = 0; i < len; ++i)
                WriteString(msg.Lines[i], true);
        }

        public void WriteString(string v)
        {
            WriteString(v, false);
        }

        public void WriteString(string v, bool padding)
        {
            byte[] buffer = Utility.UTF8.GetBytes(v);
            int tail = padding ? 2 : 1;
            int len = buffer.Length + tail;

            if (len > 255)
                len = 255;

            m_Stream.Write((byte)len);
            m_Stream.Write(buffer, 0, len - tail);

            if (padding)
                m_Stream.Write((short)0); // padding compensates for a client bug
            else
                m_Stream.Write((byte)0);
        }

        public string SafeString(string v)
        {
            if (v == null)
                return string.Empty;

            return v;
        }
    }
}
