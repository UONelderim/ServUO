using Server.Gumps;
using Server.Menus.Questions;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using System;

namespace Server.Engines.Help
{
    public class ContainedMenu : QuestionMenu
    {
        private readonly Mobile m_From;

        public ContainedMenu(Mobile from)
            : base("You already have an open help request. We will have someone assist you as soon as possible.  What would you like to do?", new string[] { "Leave my old help request like it is.", "Remove my help request from the queue." })
        {
            m_From = from;
        }

        public override void OnCancel(NetState state)
        {
            m_From.SendLocalizedMessage(1005306, "", 0x35); // Help request unchanged.
        }

        public override void OnResponse(NetState state, int index)
        {
            if (index == 0)
            {
                m_From.SendLocalizedMessage(1005306, "", 0x35); // Help request unchanged.
            }
            else if (index == 1)
            {
                PageEntry entry = PageQueue.GetEntry(m_From);

                if (entry != null && entry.Handler == null)
                {
                    m_From.SendLocalizedMessage(1005307, "", 0x35); // Removed help request.
                    PageQueue.Remove(entry);
                }
                else
                {
                    m_From.SendLocalizedMessage(1005306, "", 0x35); // Help request unchanged.
                }
            }
        }
    }

    public class HelpGump : Gump
	{
		[ConfigProperty("General.SupportWebsite")]
		public static string SupportWebsite { get => Config.Get("General.SupportWebsite", default(string)); set => Config.Set("General.SupportWebsite", value); }

        public static void Configure()
        {
            EventSink.HelpRequest += EventSink_HelpRequest;
        }

        private static void EventSink_HelpRequest(HelpRequestEventArgs e)
        {
            foreach (Gump g in e.Mobile.NetState.Gumps)
            {
                if (g is HelpGump)
                    return;
            }

            if (!PageQueue.CheckAllowedToPage(e.Mobile))
                return;

            if (PageQueue.Contains(e.Mobile))
                e.Mobile.SendMenu(new ContainedMenu(e.Mobile));
            else
                e.Mobile.SendGump(new HelpGump(e.Mobile));
        }

        private static bool IsYoung(Mobile m)
        {
            if (m is PlayerMobile)
                return ((PlayerMobile)m).Young;

            return false;
        }

        private static bool CheckCombat(Mobile m)
        {
            for (int i = 0; i < m.Aggressed.Count; ++i)
            {
                AggressorInfo info = m.Aggressed[i];

                if (DateTime.UtcNow - info.LastCombatTime < TimeSpan.FromSeconds(30.0))
                    return true;
            }

            return false;
        }

        public HelpGump(Mobile from)
            : base(0, 0)
        {
            from.CloseGump(typeof(HelpGump));

            bool isYoung = IsYoung(from);

            AddBackground(50, 25, 540, 430, 2600);

            AddPage(0);

            AddHtmlLocalized(150, 50, 360, 40, 1001002, false, false); // <CENTER><U>Ultima Online Help Menu</U></CENTER>
            AddButton(425, 415, 2073, 2072, 0, GumpButtonType.Reply, 0); // Close

            AddPage(1);

            if (isYoung)
            {
                AddButton(80, 75, 5540, 5541, 9, GumpButtonType.Reply, 2);
                AddHtml(110, 75, 450, 58, @"<BODY><BASEFONT COLOR=BLACK><u>Mlody Gracz - Transport do Nowego Haven.</u> Wybierz te opcje, jesli chcesz zostac przetransportowany do Nowego Haven.</BODY>", true, true);

                AddButton(80, 140, 5540, 5541, 1, GumpButtonType.Reply, 2);
                AddHtml(110, 140, 450, 58, @"<u>Ogólne pytanie dotyczące Ultima Online.</u> Wybierz tę opcję, jeśli masz ogólne pytanie dotyczące rozgrywki, potrzebujesz pomocy w nauce korzystania z umiejętności, lub chcesz przeszukać Bazę Wiedzy UO lub zapytaj na Discord.", true, true);

                AddButton(80, 205, 5540, 5541, 2, GumpButtonType.Reply, 0);
                AddHtml(110, 205, 450, 58, @"<u>Moj bohater jest fizycznie uwieziony w grze.</u> Ta opcja obejmuje tylko przypadki, gdy twoj bohater jest fizycznie uwieziony w miejscu, z ktorego nie moze sie wydostac. Ta opcja bedzie dzialac tylko dwa razy w ciagu 24 godzin. UWAGA! Naduzycia beda karane wedle regulaminu", true, true);

                AddButton(80, 270, 5540, 5541, 0, GumpButtonType.Page, 3);
                AddHtml(110, 270, 450, 58, @"<u>Inny gracz mnie neka.</u> Inny gracz werbalnie neka twoja postac Aby dowiedziec sie, co stanowi nekaniem, odwiedz " + (SupportWebsite == null ? "https://nelderim.pl/regulamin-2/" : SupportWebsite) + ", a jesli wiesz co jest problemem, napsiz do nas na Discord na priv.", true, true);

                AddButton(80, 335, 5540, 5541, 0, GumpButtonType.Page, 2);
                AddHtml(110, 335, 450, 58, @"<u>Inne.</u> Jesli masz problem w grze, ktory nie pasuje do zadnej z innych kategorii lub nie jest omowiony w regulaminie, napisz do nas na Discordzie (pod adresem  " + (SupportWebsite == null ? "https://discord.gg/GDyGncD" : SupportWebsite) + "), w wiadomosci prywantej do [NT] opisz swoj problem.", true, true);
            }
            else
            {
                AddButton(80, 90, 5540, 5541, 1, GumpButtonType.Reply, 2);
                AddHtml(110, 90, 450, 74, @"<u><u>Ogolne pytanie dotyczace Ultima Online.</u> Wybierz te opcje, jesli masz ogolne pytanie dotyczace rozgrywki, potrzebujesz pomocy w nauce korzystania z umiejetnosci, lub chcesz przeszukac baze wiedzy UO..", true, true);

                AddButton(80, 170, 5540, 5541, 2, GumpButtonType.Reply, 0);
                AddHtml(110, 170, 450, 74, @"<u>Moj bohater jest fizycznie uwieziony w grze.</u> Ta opcja obejmuje tylko przypadki, gdy twoj bohater jest fizycznie uwieziony w miejscu, z ktorego nie moze sie wydostac. Ta opcja bedzie dzialac tylko dwa razy w ciagu 24 godzin. UWAGA! Naduzycia beda karane wedle regulaminu""", true, true);

                AddButton(80, 250, 5540, 5541, 0, GumpButtonType.Page, 3);
                AddHtml(110, 250, 450, 74, @"<u>Inny gracz mnie neka.</u> Inny gracz werbalnie neka twoja postac Aby dowiedziec sie, co stanowi nekaniem, odwiedz " + (SupportWebsite == null ? "https://nelderim.pl/regulamin-2/" : SupportWebsite) + ", a jesli wiesz co jest problemem, napsiz do nas na Discord na priv.", true, true);

                AddButton(80, 330, 5540, 5541, 0, GumpButtonType.Page, 2);
                AddHtml(110, 330, 450, 74, @"<u>Inne.</u> Jesli masz problem w grze, ktory nie pasuje do zadnej z innych kategorii lub nie jest omowiony w regulaminie, napisz do nas na Discordzie (pod adresem  " + (SupportWebsite == null ? "https://discord.gg/GDyGncD" : SupportWebsite) + "), w wiadomosci prywantej do [NT] opisz swoj problem.", true, true);
            }

            AddPage(2);

            AddButton(80, 90, 5540, 5541, 3, GumpButtonType.Reply, 0);
            AddHtml(110, 90, 450, 74, @"<u>Wszelkie bledy nalezy zglaszac na Discord ekipie [NT]. ", true, true);

            AddButton(80, 170, 5540, 5541, 4, GumpButtonType.Reply, 0);
            AddHtml(110, 170, 450, 74, @"<u>Sugestia.</u> Nalezy je zgłaszać na Discord w dziale #sugestie. ", true, true);

            AddButton(80, 250, 5540, 5541, 5, GumpButtonType.Reply, 0);
            AddHtml(110, 250, 450, 74, @"<u>Zarzadzenie kontem</u> Zapomniales hasla? Masz problem z kontem? Napisz na Discrd do kogos z [NT].", true, true);

            AddButton(80, 330, 5540, 5541, 6, GumpButtonType.Reply, 0);
            AddHtml(110, 330, 450, 74, @"<u>Inne.</u> Jesli masz problem w grze, ktory nie pasuje do zadnej z innych kategorii lub nie jest omowiony w regulaminie, napisz do nas na Discordzie (pod adresem " + (SupportWebsite == null ? "https://discord.gg/GDyGncD" : SupportWebsite) + ", w wiadomosci prywantej do [NT] opisz swoj problem. ", true, true);

            AddPage(3);

            AddButton(80, 90, 5540, 5541, 7, GumpButtonType.Reply, 0);
            AddHtmlLocalized(110, 90, 450, 145, 1062572, true, true); /* <U><CENTER>Another player is harassing me (or Exploiting).</CENTER></U><BR>
            * VERBAL HARASSMENT<BR>
            * Use this option when another player is verbally harassing your character.
            * Verbal harassment behaviors include but are not limited to, using bad language, threats etc..
            * Before you submit a complaint be sure you understand what constitutes harassment
            * <A HREF="http://uo.custhelp.com/cgi-bin/uo.cfg/php/enduser/std_adp.php?p_faqid=40">– what is verbal harassment? -</A>
            * and that you have followed these steps:<BR>
            * 1. You have asked the player to stop and they have continued.<BR>
            * 2. You have tried to remove yourself from the situation.<BR>
            * 3. You have done nothing to instigate or further encourage the harassment.<BR>
            * 4. You have added the player to your ignore list.
            * <A HREF="http://uo.custhelp.com/cgi-bin/uo.cfg/php/enduser/std_adp.php?p_faqid=138">- How do I ignore a player?</A><BR>
            * 5. You have read and understand Origin’s definition of harassment.<BR>
            * 6. Your account information is up to date. (Including a current email address)<BR>
            * *If these steps have not been taken, GMs may be unable to take action against the offending player.<BR>
            * **A chat log will be review by a GM to assess the validity of this complaint.
            * Abuse of this system is a violation of the Rules of Conduct.<BR>
            * EXPLOITING<BR>
            * Use this option to report someone who may be exploiting or cheating.
            * <A HREF="http://uo.custhelp.com/cgi-bin/uo.cfg/php/enduser/std_adp.php?p_faqid=41">– What constitutes an exploit?</a>
            */

            AddButton(80, 240, 5540, 5541, 8, GumpButtonType.Reply, 0);
            AddHtmlLocalized(110, 240, 450, 145, 1062573, true, true); /* <U><CENTER>Another player is harassing me using game mechanics.</CENTER></U><BR>
            * <BR>
            * PHYSICAL HARASSMENT<BR>
            * Use this option when another player is harassing your character using game mechanics.
            * Physical harassment includes but is not limited to luring, Kill Stealing, and any act that causes a players death in Trammel.
            * Before you submit a complaint be sure you understand what constitutes harassment
            * <A HREF="http://uo.custhelp.com/cgi-bin/uo.cfg/php/enduser/std_adp.php?p_faqid=59"> – what is physical harassment?</A>
            * and that you have followed these steps:<BR>
            * 1. You have asked the player to stop and they have continued.<BR>
            * 2. You have tried to remove yourself from the situation.<BR>
            * 3. You have done nothing to instigate or further encourage the harassment.<BR>
            * 4. You have added the player to your ignore list.
            * <A HREF="http://uo.custhelp.com/cgi-bin/uo.cfg/php/enduser/std_adp.php?p_faqid=138"> - how do I ignore a player?</A><BR>
            * 5. You have read and understand Origin’s definition of harassment.<BR>
            * 6. Your account information is up to date. (Including a current email address)<BR>
            * *If these steps have not been taken, GMs may be unable to take action against the offending player.<BR>
            * **This issue will be reviewed by a GM to assess the validity of this complaint.
            * Abuse of this system is a violation of the Rules of Conduct.
            */

            AddButton(150, 390, 5540, 5541, 0, GumpButtonType.Page, 1);
            AddHtmlLocalized(180, 390, 335, 40, 1001015, false, false); // NO  - I meant to ask for help with another matter.
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            PageType type = (PageType)(-1);

            switch (info.ButtonID)
            {
                case 0: // Close/Cancel
                    {
                        from.SendLocalizedMessage(501235, "", 0x35); // Help request aborted.

                        break;
                    }
                case 1: // General question
                    {
                        type = PageType.Question;
                        break;
                    }
                case 2: // Stuck
                    {
                        BaseHouse house = BaseHouse.FindHouseAt(from);

                        if (house != null)
                        {
                            from.Location = house.BanLocation;
                        }
                        else if (CityLoyalty.CityTradeSystem.HasTrade(from))
                        {
                            from.SendLocalizedMessage(1151733); // You cannot do that while carrying a Trade Order.
                        }
                        else if (from.Region.IsPartOf<Regions.Jail>())
                        {
                            from.SendLocalizedMessage(1114345, "", 0x35); // You'll need a better jailbreak plan than that!
                        }
                        else if (from.Region.CanUseStuckMenu(from) && !CheckCombat(from) && !from.Frozen && !from.Criminal)
                        {
                            StuckMenu menu = new StuckMenu(from, from, true);

                            menu.BeginClose();

                            from.SendGump(menu);
                        }
                        else
                        {
                            type = PageType.Stuck;
                        }

                        break;
                    }
                case 3: // Report bug or contact Origin
                    {
                        type = PageType.Bug;
                        break;
                    }
                case 4: // Game suggestion
                    {
                        type = PageType.Suggestion;
                        break;
                    }
                case 5: // Account management
                    {
                        type = PageType.Account;
                        break;
                    }
                case 6: // Other
                    {
                        type = PageType.Other;
                        break;
                    }
                case 7: // Harassment: verbal/exploit
                    {
                        type = PageType.VerbalHarassment;
                        break;
                    }
                case 8: // Harassment: physical
                    {
                        type = PageType.PhysicalHarassment;
                        break;
                    }
                case 9: // Young player transport
                    {
                        if (IsYoung(from))
                        {
                            if (from.Region.IsPartOf<Regions.Jail>())
                            {
                                from.SendLocalizedMessage(1114345, "", 0x35); // You'll need a better jailbreak plan than that!
                            }
                            else if (CityLoyalty.CityTradeSystem.HasTrade(from))
                            {
                                from.SendLocalizedMessage(1151733); // You cannot do that while carrying a Trade Order.
                            }
                            else if (from.Region.IsPartOf("Haven"))
                            {
                                from.SendLocalizedMessage(1041529); // You're already in Haven
                            }
                            else
                            {
                                from.MoveToWorld(new Point3D(3503, 2574, 14), Map.Trammel);
                            }
                        }

                        break;
                    }
            }

            if (type != (PageType)(-1) && PageQueue.CheckAllowedToPage(from))
                from.SendGump(new PagePromptGump(from, type));
        }
    }
}
