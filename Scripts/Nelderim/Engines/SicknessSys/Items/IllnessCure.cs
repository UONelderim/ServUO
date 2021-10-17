using Server.Mobiles;

namespace Server.SicknessSys
{
    public class IllnessCure : Item
    {
        private IllnessType IllType { get; set; }

        [Constructable]
        public IllnessCure(VirusCell cell) : base(0xF07)
        {
            IllType = cell.Illness;

            Name = "a " + cell.Sickness + " cure";
            Hue = cell.PM.Hue;
            Weight = 1.0;

            Movable = false;

            if (cell.InDebug == true && cell.GM != null)
            {
                cell.GM.SendMessage(120, "Potion Created : " + Name + " : " + Hue);
            }
        }

        public IllnessCure(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.HasFreeHand())
            {
                Item cell = from.Backpack.FindItemByType(typeof(VirusCell));

                if (cell != null)
                {
                    VirusCell Cell = cell as VirusCell;

                    if (Cell.Illness == IllType)
                    {
                        if (Cell.InDebug == true && Cell.GM != null)
                        {
                            Cell.GM.SendMessage(120, "Drank Potion");
                        }

                        SicknessCure.Cure(from as PlayerMobile, Cell);

                        Delete();
                    }
                    else
                    {
                        from.SendMessage("You are not infected with " + Cell.Illness);
                    }
                }
                else
                {
                    from.SendMessage("You are not infected with any illness!");
                }
            }
            else
            {
                from.SendMessage("a free hand is needed to drink the potion!");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write((int)IllType);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            IllType = (IllnessType)reader.ReadInt();
        }
    }
}
