namespace Server.Items
{
  	public class TrapDetector : Item
    {

        [Constructable]
        public TrapDetector() : base(0x1B73)
        {
            Weight = 7.0;
            Name = "Wykrywacz pułapek";
        }

        public override void OnDoubleClick(Mobile from)
		{
            if (IsChildOf(from.Backpack))
                from.SendMessage("Najpierw umiesć wykrywacz pułapek na ziemi");
            else if (!from.InRange(GetWorldLocation(), 2))
                from.SendMessage("Stoisz za daleko od wykrywacza");
            else
            {
                from.SendMessage("Aktywujesz wykrywacz pułapek");
                from.PlaySound(0x2F3); // Earthquake Sound

                var found = false;
                var range = (int)(from.Skills[SkillName.DetectHidden].Value / 10.0);

                IPooledEnumerable eable = from.Map.GetItemsInRange(Location, range);
                foreach (Item item in eable)
                {
	                if (item is not BaseTinkerTrap trap) continue;
	                
	                double detectMin = trap.DisarmingSkillReq - 25;
	                double detectMax = trap.DisarmingSkillReq + 25;
	                if (trap.Owner == from || from.CheckTargetSkill(SkillName.DetectHidden, trap, detectMin, detectMax)) 
	                {
		                trap.Visible = true;
		                found = true;
	                }
                }
                eable.Free();

                if (found)
	                from.SendMessage(120, "Twój wykrywacz pułapek zaczyna drżeć. Chyba coś znalazł... Bądź ostrożny!");
                else
	                from.SendMessage(0x69, "Twój wykrywacz nic nie znalazł. Prawdopodobnie jest bezpiecznie...");

                if (0.01 > Utility.RandomDouble())
                {
	                from.PlaySound(0x5C);
	                from.SendMessage(0x59, "Twoje urządzenie się zepsuło.");
                    Delete();
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
