using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    /// <summary>
    /// An immovable amulet which, when clicked, lets players "pick it up".
    /// Each pickup spawns a configurable set of vampires; the 3rd time a boss.
    /// After the boss dies, its corpse is looted for a note + a relic.
    /// </summary>
    public class SpawnAmulet : Item
    {
        private int _pickupCount;

        // ────────── CONFIGURATION ──────────

        /// <summary>ID of the amulet graphic.</summary>
        public static readonly int DefaultItemID = 0x108A;

        /// <summary>Types of standard mobs to spawn on pickups 1 & 2.</summary>
        public static readonly Type[] VampireTypes =
        {
            typeof(VampireBat),  // flying bat-vampire
            typeof(Mongbat)      // standard mongbat
        };

        /// <summary>Type of boss to spawn on 3rd pickup.</summary>
        public static readonly Type BossType = typeof(BossVampire);

        /// <summary>Text that goes into the note left in the boss corpse.</summary>
        public static readonly string NoteText = "Znalazłeś notatkę w ciele wampira...";

        /// <summary>Possible relic graphics (hair, fang, blood vial).</summary>
        public static readonly int[] RelicItemIDs =
        {
            0x2AA, // vampire hair
            0x2AC, // vampire fang
            0x2AD  // vampire blood vial
        };

        [Constructable]
        public SpawnAmulet() : base(DefaultItemID)
        {
            Name    = "Mroczny Amulet";
            Movable = false;
            _pickupCount = 0;
        }

        public SpawnAmulet(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);              // version
            writer.Write(_pickupCount); 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _pickupCount = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }

            from.SendGump(new ConfirmPickupGump(this, from));
        }

        /// <summary>
        /// Called by the Gump when the player confirms pickup.
        /// </summary>
        public void DoPickup(Mobile from)
        {
            _pickupCount++;

            // Remove the amulet (simulate pickup)
            Delete();

            // Decide what to spawn
            if (_pickupCount < 3)
            {
                SpawnCreatures(from, VampireTypes);
            }
            else
            {
                // 3rd time → spawn boss
                SpawnCreatures(from, new[]{ BossType });
            }
        }

        private void SpawnCreatures(Mobile target, Type[] types)
        {
            foreach (Type t in types)
            {
                if (!typeof(BaseCreature).IsAssignableFrom(t))
                    continue;

                BaseCreature bc = (BaseCreature)Activator.CreateInstance(t);
                bc.MoveToWorld(target.Location, target.Map);
                bc.Combatant = target;
            }
        }
    }

    /// <summary>
    /// Confirmation Gump “Czy na pewno chcesz podnieść Amulet?”
    /// </summary>
    public class ConfirmPickupGump : Gump
    {
        private readonly SpawnAmulet _amulet;
        private readonly Mobile _from;

        public ConfirmPickupGump(SpawnAmulet amulet, Mobile from)
            : base(50, 50)
        {
            _amulet = amulet;
            _from   = from;

            AddBackground(0, 0, 200, 100, 0x13BE);
            AddHtml(10, 10, 180, 40, "Czy na pewno chcesz podnieść Amulet?", false, false);
            AddButton(30, 60, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
            AddHtml(65, 60, 100, 20, "<BODY><CENTER>Podnieś</CENTER></BODY>", false, false);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                _amulet.DoPickup(_from);
            }
        }
    }

    /// <summary>
    /// Boss vampire (inherits from CountDracula) with extra HP and custom loot.
    /// </summary>
    public class BossVampire : CountDracula
    {
        public BossVampire()
        {
            Name         = "Wielki Wampir";
            Hits         = 10000;
            HitsMaxSeed  = 10000;
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Drop the note
            var paper = new SimpleNote
            {
                Name       = "Notatka Wampira",
                NoteString = SpawnAmulet.NoteText
            };
            c.DropItem(paper);

            // Drop a random relic
            int id = Utility.RandomList(SpawnAmulet.RelicItemIDs);
            var relic = new GenericRelic(id);
            c.DropItem(relic);
        }
    }

    /// <summary>
    /// Simple wrapper for any relic graphic.
    /// </summary>
    public class GenericRelic : Item
    {
        public GenericRelic(int itemID) : base(itemID)
        {
            Name    = "Relikwia Wampira";
            Movable = true;
        }

        public GenericRelic(Serial serial) : base(serial) { }

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
}
