using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Spells.Fifth;
using Server.Mobiles;

namespace Server.Items
{

    public class DisguiseKitNelderim : Item
    {
        public override int LabelNumber { get { return 1045040; } } // Zestaw do kamuflazu

        [Constructable]
        public DisguiseKitNelderim()
            : base(0xE05)
        {
            Weight = 1.0;
        }

        public DisguiseKitNelderim(Serial serial)
            : base(serial)
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

        public bool ValidateUse(Mobile from)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (!IsChildOf(from.Backpack))
            {
                // That must be in your pack for you to use it.
                from.SendLocalizedMessage(1042001);
            }
            else if (pm == null || pm.NpcGuild != NpcGuild.ThievesGuild)
            {
                // Only Members of the thieves guild are trained to use this item.
                from.SendLocalizedMessage(501702);
            }
            /* // zabojcy moga uzywac peruki
            else if ( Stealing.SuspendOnMurder && pm.Kills > 0 )
            {
                // You are currently suspended from the thieves guild.  They would frown upon your actions.
                from.SendLocalizedMessage( 501703 );
            }
            */
            else if (!from.CanBeginAction(typeof(IncognitoSpell)))
            {
                // You cannot disguise yourself while incognitoed.
                from.SendLocalizedMessage(501704);
            }
            // Kamuflaz dozwolony rownoczesnie z polimorfia dzieki osobnej zmiennej HueDisguise w PlayerMobile
            /*else if (TransformationSpellHelper.UnderTransformation(from))
            {
                // You cannot disguise yourself while in that form.
                from.SendLocalizedMessage(1061634);
            }*/
            else if (from.BodyMod == 183 || from.BodyMod == 184)
            {
                // You cannot disguise yourself while wearing body paint
                from.SendLocalizedMessage(1040002);
            }
            // Kamuflaz dozwolony rownoczesnie z polimorfia dzieki osobnej zmiennej HueDisguise w PlayerMobile
            /*else if (!from.CanBeginAction(typeof(PolymorphSpell)) || from.IsBodyMod)
            {
                // You cannot disguise yourself while polymorphed.
                from.SendLocalizedMessage(501705);
            }*/
            else
            {
                return true;
            }

            return false;
        }

        public override void OnDoubleClick(Mobile from)
        {
            Race race = from.Race;
            if (from is PlayerMobile && ((PlayerMobile)from).RaceMod != null && ((PlayerMobile)from).RaceMod != Race.DefaultRace)
                race = ((PlayerMobile)from).RaceMod;

            if (ValidateUse(from))
                from.SendGump(new RaceDisguiseGump(from, race, this));
        }
    }

    public class RaceDisguiseGump : RaceChoiceGump
    {
        private DisguiseKitNelderim m_Kit;
        private string m_Name;
        private static Race[] RacesList = new Race[] { Race.NTamael, Race.NJarling, Race.NNaur, Race.NElf, Race.NDrow, Race.NKrasnolud };

        public RaceDisguiseGump(Mobile from, Race race, DisguiseKitNelderim kit)
            : this(from, race, kit, RandomName(from), from.Hue, from.HairHue, from.HairItemID, from.FacialHairItemID)
        {
            m_Kit = kit;
        }

        private RaceDisguiseGump(Mobile from, Race race, DisguiseKitNelderim kit, string name, int skinHue, int hairHue, int hairItemID, int facialHairItemID)
            : base(from, race, skinHue, hairHue, hairItemID, facialHairItemID)
        {
            m_Kit = kit;
            m_Name = name;

            DrawName();
        }

        private static string RandomName(Mobile from)
        {
            return NameList.RandomName(from.Female ? "female" : "male");
        }

        public override void DrawBackground()
        {
            AddBackground(0, 0, 605, 570, 9200);
            AddImageTiled(5, 5, 595, 560, 9274);
            AddLabel(20, 10, 930, "TYMCZASOWE PRZEBRANIE POSTACI");

            // przyciski wyboru rasy
            AddHtml(390, 350, 203, 25, "Barwy makijazu dla rasy:", true, false);
            int x = 400;
            int y = 380;
            for (int i = 0; i < RacesList.Length; i++)
            {
                if (m_Race == RacesList[i])
                    AddButton(x, y + 2, 4005, 4007, 500 + i, GumpButtonType.Reply, 0);
                else
                    AddButton(x, y + 2, 4017, 4019, 500 + i, GumpButtonType.Reply, 0);                
                AddHtml(x + 35, y, 100, 28, RacesList[i].GetName(Cases.Mianownik ), true, false);
                y += 30;
            }

            // Apply / Cancel
            AddButton(457, 540, 0xef, 0xf0, (int)ButtonID.Apply, GumpButtonType.Reply, 0);
            AddButton(527, 540, 242, 243, (int)ButtonID.Cancel, GumpButtonType.Reply, 0);
        }

        protected virtual void DrawName()
        {
            // przycisk wyboru imienia
            AddHtml(390, 265, 203, 25, "Wybrane imie:", true, false);
            AddButton(400, 297, 4005, 4007, 600, GumpButtonType.Reply, 0);
            AddHtml(440, 295, 120, 28, m_Name, true, false);
        }

        public override void ExtendedResponse(RelayInfo info)
        {
            if (info.ButtonID < 600)
            {
                // wybor rasy
                int index = info.ButtonID - 500;
                if (index >= 0 && index < RacesList.Length)
                {
                    m_Race = RacesList[index];
                    // zmien aktualny kolor skory aby uniknac niezgodnosci koloru z informacja w paperdolu
                    m_SkinHue = m_Race.SkinHues[0];
                    m_HairHue = m_Race.HairHues[0];

                }
            }
            else if (info.ButtonID < 700)
            {
                // wybor imienia
                m_Name = RandomName(m_From);
            }

            m_From.SendGump(new RaceDisguiseGump(m_From, m_Race, m_Kit, m_Name, m_SkinHue, m_HairHue, m_HairItemID, m_FacialHairItemID));
        }

        public override void ApplyAppearance(Race race, int skinHue, int hairHue, int hairID, int facialHairID)
        {
            if (!AllowAppearanceChange())
            {
                if (m_From != null)
                    m_From.SendLocalizedMessage(501704); // You cannot disguise yourself while incognitoed.
                return;
            }

            // zmiana wygladu:
            if (!(m_From is PlayerMobile))
                return;

            PlayerMobile pm = (PlayerMobile)m_From;
            pm.SetHairMods(hairID, facialHairID);
            pm.RaceMod = race;
            pm.HueDisguise = skinHue;

            m_From.NameMod = m_Name;

            m_From.SendLocalizedMessage(501706); // Kamuflaz rozpadnie sie po 2 godzinach.

            // start timer 2h
            m_Timers[m_From] = Timer.DelayCall(TimeSpan.FromHours(2.0), new TimerStateCallback(OnDisguiseExpire), m_From);
        }

        public override bool AllowAppearanceChange()
		{
            return (m_From == null) ? false : m_From.CanBeginAction(typeof(IncognitoSpell));
		}

        public static void OnDisguiseExpire(object state)
        {
            Mobile m = (Mobile)state;

            StopTimer(m);

            m.NameMod = null;

            if (m is PlayerMobile)
            {
                ((PlayerMobile)m).SetHairMods(-1, -1);
                ((PlayerMobile)m).RaceMod = null;
                ((PlayerMobile)m).HueDisguise = -1;
            }

            m.SendLocalizedMessage(1045038);    // Twoj kamuflaz rozpadl sie z uplywem czasu!
        }

        public static bool IsDisguised(Mobile m)
        {
            return m_Timers.Contains(m);
        }

        public static bool StopTimer(Mobile m)
        {
            Timer t = (Timer)m_Timers[m];

            if (t != null)
            {
                t.Stop();
                m_Timers.Remove(m);
            }

            return (t != null);
        }

        private static Hashtable m_Timers = new Hashtable();
    }
	
    

}
