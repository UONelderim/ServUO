// Scroll Of Resurrection v1.0.1
// Author: Felladrin
// Started: 2007-07-07
// Updated: 2016-01-23

using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Felladrin.Items
{
    public class ScrollOfResurrection : Item
    {
        [Constructable]
        public ScrollOfResurrection() : base(0x227B)
        {
            Name = "Scroll of Resurrection";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendMessage("Kogo chcesz wskrzesic?");
            from.BeginTarget(2, false, TargetFlags.Beneficial, OnTarget);
        }

        void OnTarget(Mobile from, object obj)
        {
            var mob = obj as Mobile;
            var corpse = obj as Corpse;

            if (mob == null && corpse == null)
            {
                from.SendMessage(33, "Tego nie wskrzesisz!");
                return;
            }

            if (corpse != null && corpse.Owner != null)
                mob = corpse.Owner;

            if (mob.IsDeadBondedPet && mob is BaseCreature)
            {
                ((BaseCreature)mob).ResurrectPet();
            }
            else if (mob.Alive)
            {
                from.SendMessage(33, "Ta istota nie jest martwa!");
                return;
            }
            else
            {
                mob.Resurrect();
            }

            mob.PlaySound(0x214);
            mob.FixedEffect(0x376A, 10, 16);

            Consume();
            from.Emote("*An Corp*");
            from.SendMessage(67, "To stworzenie zostalo przywrocone do zycia.");
        }

        public ScrollOfResurrection(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
