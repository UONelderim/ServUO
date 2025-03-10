using Server.Commands;
using Server.Mobiles;
using Server.Network;
using System;
using System.Collections.Generic;
using Nelderim.Towns;

namespace Server.Gumps
{
    public class WhoGump : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public static readonly int TextHue = PropsConfig.TextHue;
        public static readonly int TextOffsetX = PropsConfig.TextOffsetX;
        public static readonly int OffsetGumpID = PropsConfig.OffsetGumpID;
        public static readonly int HeaderGumpID = PropsConfig.HeaderGumpID;
        public static readonly int EntryGumpID = PropsConfig.EntryGumpID;
        public static readonly int BackGumpID = PropsConfig.BackGumpID;
        public static readonly int SetGumpID = PropsConfig.SetGumpID;
        public static readonly int SetWidth = PropsConfig.SetWidth;
        public static readonly int SetOffsetX = PropsConfig.SetOffsetX, SetOffsetY = PropsConfig.SetOffsetY;
        public static readonly int SetButtonID1 = PropsConfig.SetButtonID1;
        public static readonly int SetButtonID2 = PropsConfig.SetButtonID2;
        public static readonly int PrevWidth = PropsConfig.PrevWidth;
        public static readonly int PrevOffsetX = PropsConfig.PrevOffsetX, PrevOffsetY = PropsConfig.PrevOffsetY;
        public static readonly int PrevButtonID1 = PropsConfig.PrevButtonID1;
        public static readonly int PrevButtonID2 = PropsConfig.PrevButtonID2;
        public static readonly int NextWidth = PropsConfig.NextWidth;
        public static readonly int NextOffsetX = PropsConfig.NextOffsetX, NextOffsetY = PropsConfig.NextOffsetY;
        public static readonly int NextButtonID1 = PropsConfig.NextButtonID1;
        public static readonly int NextButtonID2 = PropsConfig.NextButtonID2;
        public static readonly int OffsetSize = PropsConfig.OffsetSize;
        public static readonly int EntryHeight = PropsConfig.EntryHeight;
        public static readonly int BorderSize = PropsConfig.BorderSize;
        public static bool OldStyle = PropsConfig.OldStyle;
        private static readonly bool PrevLabel = false;
        private static readonly bool NextLabel = false;
        private static readonly int PrevLabelOffsetX = PrevWidth + 1;
        private static readonly int PrevLabelOffsetY = 0;
        private static readonly int NextLabelOffsetX = -29;
        private static readonly int NextLabelOffsetY = 0;
        private static readonly int EntryWidth = 180;
        private static readonly int EntryCount = 15;
        private static readonly int TotalWidth = OffsetSize + SetWidth + OffsetSize + EntryWidth + OffsetSize + GoWidth + 
                                                 RaceEntryWidth + OffsetSize + TownEntryWidth + OffsetSize + GuildEntryWidth + 
                                                 OffsetSize + BringWidth;
        private static readonly int TotalHeight = OffsetSize + ((EntryHeight + OffsetSize) * (EntryCount + 1));
        private static readonly int BackWidth = BorderSize + TotalWidth + BorderSize;
        private static readonly int BackHeight = BorderSize + TotalHeight + BorderSize;
        private readonly Mobile m_Owner;
        private readonly List<Mobile> m_Mobiles;
        private int m_Page;

        public bool EC => m_Owner != null && m_Owner.NetState != null && m_Owner.NetState.IsEnhancedClient;
        
        private const int RaceEntryWidth = 100;

        private const int GoWidth = 24;
        public const int GoButtonID1 = 2224;
        public const int GoButtonID2 = 2224;

        private const int BringWidth = 24;
        public const int BringButtonID1 = 2223;
        public const int BringButtonID2 = 2223;

        private const int TownEntryWidth = 80;

        private const int GuildEntryWidth = 80;

        public WhoGump(Mobile owner, string filter)
            : this(owner, BuildList(owner, filter), 0)
        {
        }

        public WhoGump(Mobile owner, List<Mobile> list, int page)
            : base(GumpOffsetX, GumpOffsetY)
        {
            owner.CloseGump(typeof(WhoGump));

            m_Owner = owner;
            m_Mobiles = list;

            Initialize(page);
        }

        public static void Initialize()
        {
            CommandSystem.Register("Who", AccessLevel.Counselor, WhoList_OnCommand);
            CommandSystem.Register("WhoList", AccessLevel.Counselor, WhoList_OnCommand);
        }

        public static List<Mobile> BuildList(Mobile owner, string filter)
        {
            if (filter != null && (filter = filter.Trim()).Length == 0)
                filter = null;
            else
            {
                filter = filter?.ToLower();
            }

            List<Mobile> list = new List<Mobile>();
            List<NetState> states = NetState.Instances;

            for (int i = 0; i < states.Count; ++i)
            {
                Mobile m = states[i].Mobile;

                if (m != null && (m == owner || !m.Hidden || owner.AccessLevel >= m.AccessLevel || (m is PlayerMobile && ((PlayerMobile)m).VisibilityList.Contains(owner))))
                {
                    if (filter != null && (m.Name == null || m.Name.ToLower().IndexOf(filter) < 0))
                        continue;

                    list.Add(m);
                }
            }

            list.Sort(InternalComparer.Instance);

            return list;
        }

        public void Initialize(int page)
        {
            m_Page = page;

            int count = m_Mobiles.Count - (page * EntryCount);

            if (count < 0)
                count = 0;
            else if (count > EntryCount)
                count = EntryCount;

            int totalHeight = OffsetSize + ((EntryHeight + OffsetSize) * (count + 1));

            AddPage(0);

            AddBackground(0, 0, BackWidth, BorderSize + totalHeight + BorderSize, BackGumpID);
            AddImageTiled(BorderSize, BorderSize, TotalWidth - (OldStyle ? SetWidth + OffsetSize : 0), totalHeight, OffsetGumpID);

            int x = BorderSize + OffsetSize;
            int y = BorderSize + OffsetSize;

            int emptyWidth = TotalWidth - PrevWidth - NextWidth - (OffsetSize * 4) - (OldStyle ? SetWidth + OffsetSize : 0);

            if (!OldStyle)
                AddImageTiled(x - (OldStyle ? OffsetSize : 0), y, emptyWidth + (OldStyle ? OffsetSize * 2 : 0), EntryHeight, EntryGumpID);

            AddLabel(x + TextOffsetX, y, TextHue, string.Format("Page {0} of {1} ({2})", page + 1, (m_Mobiles.Count + EntryCount - 1) / EntryCount, m_Mobiles.Count));

            x += emptyWidth + OffsetSize;

            if (OldStyle)
                AddImageTiled(x, y, TotalWidth - (OffsetSize * 3) - SetWidth, EntryHeight, HeaderGumpID);
            else
                AddImageTiled(x, y, PrevWidth, EntryHeight, HeaderGumpID);

            if (page > 0)
            {
                AddButton(x + PrevOffsetX, y + PrevOffsetY, PrevButtonID1, PrevButtonID2, 1, GumpButtonType.Reply, 0);

                if (PrevLabel)
                    AddLabel(x + PrevLabelOffsetX, y + PrevLabelOffsetY, TextHue, "Previous");
            }

            x += PrevWidth + OffsetSize;

            if (!OldStyle)
                AddImageTiled(x, y, NextWidth, EntryHeight, HeaderGumpID);

            if ((page + 1) * EntryCount < m_Mobiles.Count)
            {
                AddButton(x + NextOffsetX, y + NextOffsetY, NextButtonID1, NextButtonID2, 2, GumpButtonType.Reply, 1);

                if (NextLabel)
                    AddLabel(x + NextLabelOffsetX, y + NextLabelOffsetY, TextHue, "Next");
            }

            for (int i = 0, index = page * EntryCount; i < EntryCount && index < m_Mobiles.Count; ++i, ++index)
            {
                x = BorderSize + OffsetSize;
                y += EntryHeight + OffsetSize;

                Mobile m = m_Mobiles[index];
                
                if (SetGumpID != 0)
	                AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);

                if (m.NetState != null && !m.Deleted)
	                AddButton(x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, 3 + (i * 3) + 0, GumpButtonType.Reply, 0);

                x += SetWidth + OffsetSize;

                AddImageTiled(x, y, EntryWidth, EntryHeight, EntryGumpID);
                AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, GetHueFor(m), m.Deleted ? "(deleted)" : m.Name);

                x += EntryWidth + OffsetSize;
                
                if (SetGumpID != 0)
	                AddImageTiled(x, y, GoWidth, EntryHeight, SetGumpID);

                if (m.NetState != null && !m.Deleted)
	                AddButton(x + SetOffsetX, y + SetOffsetY + 2, GoButtonID1, GoButtonID2, 3 + (i * 3) + 1, GumpButtonType.Reply, 0);

                x += GoWidth + OffsetSize;
                
                
                AddImageTiled(x, y, RaceEntryWidth, EntryHeight, SetGumpID);
                AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, GetRaceHueFor(m), m.Race.Name);

                // Obywatelstwo
                x += RaceEntryWidth + OffsetSize;

                AddImageTiled(x, y, TownEntryWidth, EntryHeight, SetGumpID);
                AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, GetTownHueFor(m), GetTownFor(m));

                x += TownEntryWidth + OffsetSize;

                //Gildia
                AddImageTiled(x, y, GuildEntryWidth, EntryHeight, SetGumpID);
                AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, GetGuildHueFor(m), GetGuildFor(m));

                x += GuildEntryWidth + OffsetSize;

                // Bring
                if (SetGumpID != 0)
	                AddImageTiled(x, y, BringWidth, EntryHeight, SetGumpID);

                if (m.NetState != null && !m.Deleted)
	                AddButton(x + SetOffsetX, y + SetOffsetY + 2, BringButtonID1, BringButtonID2, 3 + (i * 3) + 2, GumpButtonType.Reply, 0);
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            switch (info.ButtonID)
            {
                case 0: // Closed
                    {
                        return;
                    }
                case 1: // Previous
                    {
                        if (m_Page > 0)
                            from.SendGump(new WhoGump(from, m_Mobiles, m_Page - 1));

                        break;
                    }
                case 2: // Next
                    {
                        if ((m_Page + 1) * EntryCount < m_Mobiles.Count)
                            from.SendGump(new WhoGump(from, m_Mobiles, m_Page + 1));

                        break;
                    }
                default:
                    {
                        int index = (m_Page * EntryCount) + (info.ButtonID - 3) / 3;
                        var button = (info.ButtonID - 3) % 3;

                        if (index >= 0 && index < m_Mobiles.Count)
                        {
                            Mobile m = m_Mobiles[index];

                            if (m.Deleted)
                            {
                                from.SendMessage("That player has deleted their character.");
                                from.SendGump(new WhoGump(from, m_Mobiles, m_Page));
                            }
                            else if (m.NetState == null)
                            {
                                from.SendMessage("That player is no longer online.");
                                from.SendGump(new WhoGump(from, m_Mobiles, m_Page));
                            }
                            else if (m == from || !m.Hidden || from.AccessLevel >= m.AccessLevel || (m is PlayerMobile && ((PlayerMobile)m).VisibilityList.Contains(from)))
                            {
	                            switch (button)
	                            {
		                            case 0:
			                            from.SendGump(new ClientGump(from, m.NetState));
			                            break;
		                            case 1: 
			                            from.MoveToWorld(m.Location, m.Map);
			                            break;
		                            case 2:
			                            m.MoveToWorld(from.Location, from.Map);
			                            break;
	                            }
                            }
                            else
                            {
                                from.SendMessage("You cannot see them.");
                                from.SendGump(new WhoGump(from, m_Mobiles, m_Page));
                            }
                        }

                        break;
                    }
            }
        }

        [Usage("WhoList [filter]")]
        [Aliases("Who")]
        [Description("Lists all connected clients. Optionally filters results by name.")]
        private static void WhoList_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new WhoGump(e.Mobile, e.ArgString));
        }

        private int GetHueFor(Mobile m)
        {
            switch (m.AccessLevel)
            {
                case AccessLevel.Owner:
                case AccessLevel.CoOwner:
                case AccessLevel.Developer:
                case AccessLevel.Administrator:
                    return EC ? 0x51D : 0x516;
                case AccessLevel.Seer:
                    return EC ? 0x142 : 0x144;
                case AccessLevel.GameMaster:
                    return EC ? 0x11 : 0x21;
                case AccessLevel.Decorator:
                    return 0x2;
                case AccessLevel.VIP:
                case AccessLevel.Player:
                default:
                    {
                        if (m.Murderer)
                            return EC ? 0x20 : 0x21;
                        else if (m.Criminal)
                            return EC ? 0x3AE : 0x3B1;

                        return EC ? 0x5C : 0x58;
                    }
            }
        }
        
        public static int GetRaceHueFor(Mobile m)
        {
	        if (m.Deleted)
		        return 945;
	        
	        if ( m.Race == Race.None)
		        return 945;
	        if ( m.Race == Race.NTamael )
		        return 88;
	        if ( m.Race == Race.NJarling )
		        return 54;
	        if ( m.Race == Race.NNaur )
		        return 36;
	        if ( m.Race == Race.NElf )
		        return 945;
	        if ( m.Race == Race.NDrow )
		        return 945;
	        if ( m.Race == Race.NKrasnolud )
		        return 945;

	        return 945;
        }
        
        public static int GetTownHueFor(Mobile m)
        {
	        if (m.Deleted)
		        return 945;

	        return TownDatabase.IsCitizenOfWhichTown(m) switch
	        {
		        Towns.None => 902,
		        Towns.Orod => 84,
		        Towns.Garlan => 51,
		        Towns.Twierdza => 65,
		        Towns.LDelmah => 35,
		        Towns.Lotharn => 2702,
		        Towns.Tirassa => 0,
		        _ => 945
	        };
        }
        
        public static string GetTownFor(Mobile m)
        {
	        if (m.Deleted)
		        return "(deleted)";

	        var mobileTown = TownDatabase.IsCitizenOfWhichTown(m);
	        if (mobileTown == Towns.None)
		        return "------";
	        
	        return mobileTown.ToString();
        }
        
        public static int GetGuildHueFor(Mobile m)
        {
	        if (m.Deleted || m.Guild == null)
		        return 945;
	        
	        return 167;
        }
        
        public static string GetGuildFor(Mobile m)
        {
	        if (m.Deleted || m.Guild == null)
		        return "( --- )";
	        
		    return $"[ {m.Guild.Abbreviation} ]";
        }

        private class InternalComparer : IComparer<Mobile>
        {
            public static readonly IComparer<Mobile> Instance = new InternalComparer();

            public int Compare(Mobile x, Mobile y)
            {
                if (x == null || y == null)
                    throw new ArgumentException();

                if (x.AccessLevel > y.AccessLevel)
                    return -1;
                else if (x.AccessLevel < y.AccessLevel)
                    return 1;
                else
                    return Insensitive.Compare(x.Name, y.Name);
            }
        }
    }
}
