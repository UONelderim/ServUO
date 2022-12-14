//
// ** Basic Trap Framework (BTF)
// ** Author: Lichbane
//
using System;
using Server.Regions;

namespace Server.Items
{
  	public class TrapDetector : Item
    {
        private Mobile m_Player;

        [Constructable]
        public TrapDetector() : base(0x1B73)
        {
            Weight = 7.0;
            Name = "Wykrywacz pułapek";

        }

        public override void OnDoubleClick(Mobile from)
		{
            m_Player = from;
            if (IsChildOf(from.Backpack))
                from.SendMessage("Najpierw umiesć wykrywacz pułapek na ziemi");

            else if (!from.InRange(this.GetWorldLocation(), 2))
                from.SendMessage("Stoisz za daleko od wykrywacza");
            else
            {
                from.SendMessage("Aktywujesz wykrywacz pułapek");
                from.PlaySound(0x2F3); // Earthquake Sound

                bool trapfound = false;
                Point3D location = ((Item)this).Location;
                double Skill = m_Player.Skills[SkillName.DetectHidden].Value;
                int range = (int)(Skill / 10.0);

                IPooledEnumerable itemsInRange = m_Player.Map.GetItemsInRange(location, range);
                foreach (Item item in itemsInRange)
                {
                    if (item is BaseTinkerTrap)
                    {
                        BaseTinkerTrap trap = (BaseTinkerTrap)item;
                        double detectMin = trap.DisarmingSkillReq - 10;
                        double detectMax = trap.DisarmingSkillReq + 10;
                        if ((m_Player.CheckTargetSkill(SkillName.DetectHidden, trap, detectMin, detectMax)) || (trap.Owner == from))
                        {
                            trap.Visible = true;
                            trapfound = true;
                        }
                    }
                }
                itemsInRange.Free();

                //
                // Report back!
                //
                if (trapfound)
                    m_Player.SendMessage(120, "Twój wykrywacz pułapek zaczyna drżeć. Chyba coś znalazł... Bądź ostrożny!");
                else
                    m_Player.SendMessage(0x69, "Twój wykrywacz nic nie znalazł. Prawdopodobnie jest bezpiecznie...");

                //
                // 10% of the device burning out (rather than charges)
                //
                if (0.1 > Utility.RandomDouble())
                {
                    m_Player.PlaySound(0x5C);
                    m_Player.SendMessage(0x59, "Twoje urządzenie się zepsuło.");
                    this.Delete();
                }
            }
        }

        public TrapDetector(Serial serial) : base(serial)
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
        }
    }
}