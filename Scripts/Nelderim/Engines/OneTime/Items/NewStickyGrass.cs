using Server.Mobiles;

namespace Server
{
    class NewStickyGrass : Item
    {
        private int Time = Utility.Random(1, 3);

        private int Count { get; set; }
        private int OrgZ { get; set; }

        [Constructable]
        public NewStickyGrass() : base()
        {
            Count = 0;
            OrgZ = 999;

            Visible = false;

            switch (Utility.Random(2))
            {
                case 0: ItemID = 3378; break;
                case 1: ItemID = 3379; break;
            }
        }

        public NewStickyGrass(Serial serial) : base(serial)
        {
            Count = 0;

            Visible = false;
        }

        public void TimerTick()
        {
            if (OrgZ == 999)
            {
                OrgZ = Z;
            }

            RunCheck();
        }

        private void RunCheck()
        {
            if (Count >= Time)
            {
                bool IsPlayer = IsPlayerClose();

                if (IsPlayer)
                {
                    Visible = true;

                    if (ItemID == 3378)
                        ItemID = 3379;
                    else
                        ItemID = 3378;

                    int Grab = Utility.Random(1, 5);

                    if (Grab == 1)
                    {
                        if (Z > OrgZ)
                        {
                            Z--;
                            Hue = 0;
                            Visible = false;
                        }
                        else
                        {
                            Z++;
                            Hue = 67;
                        }
                    }
                }
                else
                {
                    if (Z > OrgZ)
                    {
                        Z--;
                    }

                    Hue = 0;
                    Visible = false;
                }

                Count = 0;
            }

            Count++;
        }

        private bool IsPlayerClose()
        {
            IPooledEnumerable Mobs = GetMobilesInRange(2);

            bool IsPlayer = false;

            if (Mobs != null)
            {
                foreach (var mob in Mobs)
                {
                    if (mob is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)mob;
                        bool GoodLoad = true;

                        if (X == pm.X)
                        {
                            if (Y == pm.Y)
                                GoodLoad = false;
                        }

                        if (X == pm.X + 2 || X == pm.X - 2)
                        {
                            if (Y == pm.Y + 2 || Y == pm.Y - 2)
                            {
                                GoodLoad = false;
                            }
                        }

                        if (GoodLoad)
                            IsPlayer = true;
                    }
                }
                Mobs.Free();
            }

            return IsPlayer;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write(Time);

            writer.Write(OrgZ);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            Time = reader.ReadInt();

            OrgZ = reader.ReadInt();
        }
    }
}
