using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Gumps
{
    public class VoodooSpellGump : Gump
    {
        private readonly VoodooDoll _doll;
        private readonly Mobile     _from;

        public VoodooSpellGump(VoodooDoll doll, Mobile from)
            : base(50, 50)
        {
            _doll = doll;
            _from = from;

            AddPage(0);
            AddBackground(0, 0, 220, 240, 9270);

            // Nagłówek
            AddLabel(20, 15, 1152, "Guślarstwo - Wybierz czar");

            // Lista dostępnych czarów
            var spells = new List<VoodooSpellType>
            {
                VoodooSpellType.Stab,
                VoodooSpellType.Curse,
                VoodooSpellType.Disease,
                VoodooSpellType.Link
            };

            int y = 50;
            foreach (var type in spells)
            {
                // ID == wartość enuma (1..4)
                int buttonID = (int)type;

                // Polskie nazwy
                string name = type switch
                {
                    VoodooSpellType.Stab    => "Ukłucie",
                    VoodooSpellType.Curse   => "Klątwa",
                    VoodooSpellType.Disease => "Choroba",
                    VoodooSpellType.Link    => "Nawiąż połączenie",
                    _                       => type.ToString()
                };

                AddButton(10, y, 4005, 4007, buttonID, GumpButtonType.Reply, 0);
                AddLabel(45, y, 1152, name);
                y += 30;
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            int id = info.ButtonID;

            // Czy buttonID to poprawna wartość enum?
            if (!Enum.IsDefined(typeof(VoodooSpellType), id))
                return;

            var spell = (VoodooSpellType)id;

            // Dla wszystkich czarów poza Link wymagamy aktywnego połączenia
            if (spell != VoodooSpellType.Link && !_doll.HasActiveLink)
            {
                _from.SendMessage("Brak aktywnego połączenia – najpierw użyj „Nawiąż połączenie”.");
                return;
            }

            // Właściwe wywołanie managera
            VoodooManager.CastSpell(spell, _doll, _from);
        }
    }
}
