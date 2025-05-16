using System.Collections.Generic;
using Nelderim.Towns;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
    /// <summary>
    /// Represents the soul of a town (Dusza miasta) that players can interact with to view
    /// resources or join the associated town.
    /// Left-click will prompt a JoinTownGump when applicable.
    /// </summary>
    public class DuszaMiasta : Item
    {
        /// <summary>
        /// Localization number for "Dusza miasta".
        /// </summary>
        public override int LabelNumber => 1063735;

        /// <summary>
        /// The town linked to this soul.
        /// </summary>
        public Towns Town = Towns.None;

        [Constructable]
        public DuszaMiasta() : base(0x1184)
        {
            Movable = false;
            Hue = 2935;
        }

        public DuszaMiasta(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(2); // version
            writer.Write((int)Town);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 2)
            {
                Town = (Towns)reader.ReadInt();
            }
        }

        /// <summary>
        /// Handles single clicks: show join prompt if player is not already a member of the town.
        /// Now uses TownDatabase for persistent membership.
        /// </summary>
        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            if (Town != Towns.None && !TownDatabase.IsCitizenOfGivenTown(from, Town))
            {
                from.SendGump(new JoinTownGump(from, this));
            }
        }

        /// <summary>
        /// Handles double-clicks by showing the town resources gump.
        /// </summary>
        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Information, from, this));
        }
    }

    /// <summary>
    /// Gump that prompts the player to join the town.
    /// </summary>
    public class JoinTownGump : Gump
    {
        private readonly Mobile _from;
        private readonly DuszaMiasta _soul;

        private const int GumpWidth = 220;
        private const int GumpHeight = 120;

        public JoinTownGump(Mobile from, DuszaMiasta soul)
            : base(50, 50)
        {
            _from = from;
            _soul = soul;

            AddPage(0);
            AddBackground(0, 0, GumpWidth, GumpHeight, 0x13BE);

            // Title
            AddLabel(20, 15, 0, "Join Town");
            // Question text
            AddLabel(20, 35, 0, $"Do you wish to join {GetTownName(soul.Town)}?");

            // Yes button
            AddButton(50, 70, 0xF7, 0xF8, 1, GumpButtonType.Reply, 0);
            AddLabel(85, 72, 0, "Yes");

            // No button
            AddButton(130, 70, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0);
            AddLabel(165, 72, 0, "No");
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (_soul.Deleted)
                return;

            // Player chose "Yes"
            if (info.ButtonID == 1)
            {
                // Attempt to add citizen via TownDatabase
                if (TownDatabase.AddCitizen(_from, _soul.Town))
                {
                    _from.SendLocalizedMessage(1063736); // You have joined the town.
                }
                else
                {
                    _from.SendMessage("You are already a member of this town.");
                }
            }
        }

        /// <summary>
        /// Utility to get a friendly name for the Towns enum.
        /// </summary>
        private static string GetTownName(Towns town)
        {
            // TODO: Replace with localized names or cliloc lookups.
            return town.ToString();
        }
    }
}
