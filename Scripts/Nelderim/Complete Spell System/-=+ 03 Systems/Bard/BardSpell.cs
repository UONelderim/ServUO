#region References

using System;
using System.Collections.Generic;
using System.Linq;
using Server.Commands; // Potrzebne do rejestracji komendy
using Server.Gumps;    // Potrzebne do okna wyboru
using Server.Items;
using Server.Spells;
using Server.Network; // Potrzebne do NetState w Gump.OnResponse

#endregion

namespace Server.ACC.CSS.Systems.Bard
{
    #region Preference Token Item

    /// <summary>
    /// Ten przedmiot służy jako "token" przechowujący preferencje gracza co do typu instrumentu.
    /// Jest to rozwiązanie kompatybilne ze starszymi wersjami ServUO, które nie posiadają systemu Attachment.
    /// Przedmiot jest niewidzialny i przechowywany w banku gracza.
    /// </summary>
    public class BardPreferenceToken : Item
    {
        private Type m_PreferredInstrumentType;

        public Type PreferredInstrumentType { get { return m_PreferredInstrumentType; } set { m_PreferredInstrumentType = value; } }

        public override string DefaultName { get { return "Token preferencji barda"; } }

        public BardPreferenceToken() : base(0x0E74) // Używamy ID przedmiotu, który może być niewidoczny lub nieistotny
        {
            Weight = 0.0;
            Movable = false;
            Visible = false; // Ukryj przedmiot przed graczem
            LootType = LootType.Blessed; // Zabezpiecz przedmiot
        }

        // Konstruktor do deserializacji (ładowania zapisu)
        public BardPreferenceToken(Serial serial) : base(serial)
        {
        }
        
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Niezwykle ważne jest wywołanie metody bazowej
            writer.Write(0); // Wersja
            writer.Write(m_PreferredInstrumentType != null ? m_PreferredInstrumentType.FullName : null);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Niezwykle ważne jest wywołanie metody bazowej
            int version = reader.ReadInt();
            string typeName = reader.ReadString();

            if (!string.IsNullOrEmpty(typeName))
            {
                m_PreferredInstrumentType = ScriptCompiler.FindTypeByFullName(typeName);
            }
        }
    }

    #endregion

    #region Gump for Making a Choice

    /// <summary>
    /// Gump (okno) pozwalające graczowi wybrać preferowany typ instrumentu.
    /// </summary>
    public class BardInstrumentChoiceGump : Gump
    {
        private readonly Mobile m_From;
        private readonly List<Type> m_InstrumentTypes;

        public BardInstrumentChoiceGump(Mobile from, List<Type> instrumentTypes) : base(50, 50)
        {
            m_From = from;
            m_InstrumentTypes = instrumentTypes;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 320, 80 + (instrumentTypes.Count * 25) + 40, 9270);
            AddAlphaRegion(10, 10, 300, 60 + (instrumentTypes.Count * 25) + 40);
            AddLabel(20, 20, 1152, "Wybierz preferowany typ instrumentu:");

            for (int i = 0; i < instrumentTypes.Count; i++)
            {
                Type type = instrumentTypes[i];
                // Button ID = i + 1, aby uniknąć ID 0 (które oznacza zamknięcie gumpa)
                AddButton(20, 60 + (i * 25), 4005, 4007, i + 1, GumpButtonType.Reply, 0);
                AddLabel(55, 60 + (i * 25), 1153, type.Name);
            }

            // Opcja do wyczyszczenia wyboru
            AddButton(20, 60 + (instrumentTypes.Count * 25), 4014, 4016, 999, GumpButtonType.Reply, 0);
            AddLabel(55, 60 + (instrumentTypes.Count * 25), 1150, "Wyczyść mój wybór");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_From == null || m_From.BankBox == null)
                return;

            int buttonId = info.ButtonID;

            if (buttonId == 0) // Zamknięto gump
                return;
            
            // Najpierw usuń stary token, jeśli istnieje w banku
            Item oldToken = m_From.BankBox.FindItemByType(typeof(BardPreferenceToken));
            if (oldToken != null)
            {
                oldToken.Delete();
            }

            if (buttonId == 999) // Wybrano "Wyczyść"
            {
                m_From.SendMessage("Twój wybór instrumentu został wyczyszczony.");
            }
            else if (buttonId > 0 && buttonId <= m_InstrumentTypes.Count)
            {
                Type selectedType = m_InstrumentTypes[buttonId - 1];
                
                // Stwórz nowy token i umieść go w banku gracza
                BardPreferenceToken newToken = new BardPreferenceToken();
                newToken.PreferredInstrumentType = selectedType;
                m_From.BankBox.DropItem(newToken);

                m_From.SendMessage("Wybrałeś {0} jako preferowany typ instrumentu. Twój wybór został zapisany.", selectedType.Name);
            }
        }
    }

    #endregion

    public abstract class BardSpell : CSpell
    {
        #region Command Registration
        
        static BardSpell()
        {
            CommandSystem.Register("BardPref", AccessLevel.Player, new CommandEventHandler(OnBardPrefCommand));
        }

        [Usage("BardPref")]
        [Description("Otwiera menu wyboru preferowanego instrumentu dla pieśni barda.")]
        private static void OnBardPrefCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            Container pack = from.Backpack;

            if (pack == null)
            {
                from.SendMessage("Nie posiadasz plecaka!");
                return;
            }

            // Znajdź unikalne typy instrumentów w plecaku gracza
            List<Type> instrumentTypes = pack.FindItemsByType(typeof(BaseInstrument))
                .Select(item => item.GetType())
                .Distinct()
                .ToList();

            if (instrumentTypes.Count == 0)
            {
                from.SendMessage("Nie masz w plecaku żadnych instrumentów, aby dokonać wyboru.");
                return;
            }

            from.SendGump(new BardInstrumentChoiceGump(from, instrumentTypes));
        }

        #endregion

        public abstract SpellCircle Circle { get; }

        public BardSpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
        }

        public override SkillName CastSkill
        {
            get
            {
                double disco = Caster.Skills.Discordance.Value;
                double peace = Caster.Skills.Peacemaking.Value;
                double provo = Caster.Skills.Provocation.Value;

                if (disco >= peace && disco >= provo)
                    return SkillName.Discordance;
                
                return peace >= provo ? SkillName.Peacemaking : SkillName.Provocation;
            }
        }

        public override SkillName DamageSkill { get { return SkillName.Musicianship; } }
        public override bool ClearHandsOnCast { get { return false; } }
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(3 * CastDelaySecondsPerTick); } }

        public override void GetCastSkills(out double min, out double max)
        {
            min = RequiredSkill;
            max = RequiredSkill + 30.0;
        }

        public override int GetMana()
        {
            return RequiredMana;
        }

        public override bool CheckCast()
        {
            if (Caster.BankBox == null)
            {
                Caster.SendMessage("Wystąpił błąd: nie można uzyskać dostępu do Twojego banku.");
                return false;
            }

            // Krok 1: Sprawdź, czy gracz dokonał wyboru, szukając tokena w banku.
            var token = Caster.BankBox.FindItemByType(typeof(BardPreferenceToken)) as BardPreferenceToken;

            if (token == null || token.PreferredInstrumentType == null)
            {
                Caster.SendMessage("Musisz najpierw wybrać preferowany typ instrumentu. Użyj komendy [BardPref, aby to zrobić.");
                return false;
            }

            Container pack = Caster.Backpack;
            if (pack == null)
            {
                Caster.SendMessage("Nie posiadasz plecaka!");
                return false;
            }

            Type preferredType = token.PreferredInstrumentType;

            // Krok 2: Znajdź najlepszy instrument WYBRANEGO TYPU w plecaku.
            BaseInstrument instrumentToUse = pack.FindItemsByType(preferredType)
                .OfType<BaseInstrument>()
                .Where(inst => inst.UsesRemaining > 0)
                .OrderByDescending(inst => inst.UsesRemaining)
                .FirstOrDefault();

            if (instrumentToUse == null)
            {
                Caster.SendMessage("Nie posiadasz w plecaku instrumentu typu {0} z wystarczającą ilością ładunków.", preferredType.Name);
                return false;
            }

            // Krok 3: Poprawna logika zużycia ładunków.
            instrumentToUse.UsesRemaining--;

            if (instrumentToUse.UsesRemaining <= 0)
            {
                string instrumentName = string.IsNullOrEmpty(instrumentToUse.Name) ? "instrument" : instrumentToUse.Name;
                Caster.SendMessage("Zużyłeś ostatni ładunek swojego instrumentu ({0}), który rozpadł się w pył.", instrumentName);
                instrumentToUse.Delete();
            }

            // Krok 4: Sukces!
            Caster.PlaySound(instrumentToUse.SuccessSound);
            return true;
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(CastDelay);
        }

        public virtual bool CheckResisted(Mobile target)
        {
            double n = GetResistPercent(target) / 100.0;
            if (n <= 0.0) return false;
            if (n >= 1.0) return true;
            int maxSkill = (1 + (int)Circle) * 10 + (1 + ((int)Circle / 6)) * 25;
            if (target.Skills[SkillName.MagicResist].Value < maxSkill)
                target.CheckSkill(SkillName.MagicResist, 0.0, 120.0);
            return (n >= Utility.RandomDouble());
        }

        public virtual double GetResistPercent(Mobile target)
        {
            return GetResistPercentForCircle(target, Circle);
        }

        public virtual double GetResistPercentForCircle(Mobile target, SpellCircle circle)
        {
            double firstPercent = target.Skills[SkillName.MagicResist].Value / 5.0;
            double secondPercent = target.Skills[SkillName.MagicResist].Value -
                                   (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);
            return Math.Max(firstPercent, secondPercent) / 2.0;
        }
    }
}
