using Server.Commands;
using Server.Engines.Points;
using Server.Engines.SeasonalEvents;
using Server.Items;
using Server.Mobiles;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Server.Engines.CityLoyalty
{
    public enum City
    {
        Tasandora,
        Twierdza,
        Tirassa,
        LDelmah,
        Orod,
        Lotharn,
        Garlan,
        ArtTrader,
        Celendir
    }

    public enum LoyaltyRating
    {
        Disfavored,
        Disliked,
        Detested,
        Loathed,
        Despised,
        Reviled,
        Scorned,
        Shunned,
        Villified,
        Abhorred,
        Unknown,
        Doubted,
        Distrusted,
        Disgraced,
        Denigrated,
        Commended,
        Esteemed,
        Respected,
        Honored,
        Admired,
        Adored,
        Lauded,
        Exalted,
        Revered,
        Venerated
    }

    [Flags]
    public enum CityTitle
    {
        None = 0x00000000,
        Citizen = 0x00000002,
        Knight = 0x00000004,
        Baronet = 0x00000008,
        Baron = 0x00000010,
        Viscount = 0x00000020,
        Earl = 0x00000040,
        Marquis = 0x00000080,
        Duke = 0x00000100,
    }

    public enum TradeDeal
    {
        None = 0,
        GuildOfArcaneArts = 1154044,
        SocietyOfClothiers = 1154045,
        BardicCollegium = 1154046,
        OrderOfEngineers = 1154048,
        GuildOfHealers = 1154049,
        MaritimeGuild = 1154050,
        MerchantsAssociation = 1154051,
        MiningCooperative = 1154052,
        LeagueOfRangers = 1154053,
        GuildOfAssassins = 1154054,
        WarriorsGuild = 1154055,
    }

    [PropertyObject]
    public class CityLoyaltySystem : PointsSystem
    {
        public static readonly bool Enabled = Config.Get("CityLoyalty.Enabled", true);
        public static readonly int CitizenJoinWait = Config.Get("CityLoyalty.JoinWait", 7);
        public static readonly int BannerCost = Config.Get("CityLoyalty.BannerCost", 250000);
        public static readonly int BannerCooldownDuration = Config.Get("CityLoyalty.BannerCooldown", 24);
        public static readonly int TradeDealCostPeriod = Config.Get("CityLoyalty.TradeDealPeriod", 7);
        public static readonly int TradeDealCooldown = Config.Get("CityLoyalty.TradeDealCooldown", 7);
        public static readonly int TradeDealCost = Config.Get("CityLoyalty.TradeDealCost", 2000000);
        public static readonly int TradeDealUtilizationPeriod = Config.Get("CityLoyalty.TradeDealUtilizationPeriod", 24);
        public static readonly int MaxBallotBoxes = Config.Get("CityLoyalty.MaxBallotBoxes", 10);
        public static readonly int AnnouncementPeriod = Config.Get("CityLoyalty.AnnouncementPeriod", 48);

        public static readonly TimeSpan LoveAtrophyDuration = TimeSpan.FromHours(40);
        public static Map SystemMap => Siege.SiegeShard ? Map.Felucca : Map.Trammel;

        public bool ArtisanFestivalActive => SeasonalEventSystem.IsActive(EventType.ArtisanFestival);
        public static readonly bool AwakeingEventActive = false;

        public override TextDefinition Name => new TextDefinition(string.Format("{0}", City.ToString()));
        public override bool AutoAdd => false;
        public override double MaxPoints => double.MaxValue;
        public override PointsType Loyalty => PointsType.None;
        public override bool ShowOnLoyaltyGump => false;

        public override string ToString()
        {
            return Name.String;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public City City { get; private set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public CityDefinition Definition { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public CityElection Election { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime TradeDealStart { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextTradeDealCheck { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanUtilize { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public GuardCaptain Captain { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public CityHerald Herald { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public TradeMinister Minister { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public CityStone Stone { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public CityMessageBoard Board { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Headline { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Body { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime PostedOn { get; set; }

        private Mobile _Governor;
        private Mobile _GovernorElect;
        private bool _PendingGovernor;
        private long _Treasury;
        private TradeDeal _ActiveTradeDeal;
        private int _CompletedTrades;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile GovernorElect
        {
            get { return _GovernorElect; }
            set
            {
                if (value != null && Governor != null)
                    Governor = null;

                _GovernorElect = value;

                if (Stone != null)
                    Stone.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Governor
        {
            get { return _Governor; }
            set
            {
                if (_Governor != null && _Governor != value && _Governor.NetState != null)
                    _Governor.SendLocalizedMessage(1154071); // King Blackthorn thanks you for your service. You have been removed from the Office of the Governor.

                if (value == _GovernorElect)
                    _GovernorElect = null;

                if (value != null && value != _Governor)
                    HeraldMessage(1154070, value.Name); // Hear Ye! Hear Ye! ~1_NAME~ hath accepted the Office of Governor! King Blackthorn congratulates Governor ~1_NAME~! 

                _PendingGovernor = false;
                _Governor = value;

                if (Stone != null)
                    Stone.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PendingGovernor
        {
            get { return _PendingGovernor; }
            set
            {
                if (value && _GovernorElect != null)
                    _GovernorElect = null;

                _PendingGovernor = value;

                if (Stone != null)
                    Stone.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public long Treasury
        {
            get { return _Treasury; }
            set
            {
                _Treasury = value;

                if (Stone != null)
                    Stone.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TradeDeal ActiveTradeDeal
        {
            get { return _ActiveTradeDeal; }
            set
            {
                _ActiveTradeDeal = value;

                if (Stone != null)
                    Stone.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int CompletedTrades
        {
            get { return _CompletedTrades; }
            set
            {
                _CompletedTrades = value;

                if (Stone != null)
                    Stone.InvalidateProperties();
            }
        }

        private Dictionary<Mobile, DateTime> CitizenWait { get; set; }

        public CityLoyaltySystem(City city)
        {
            City = city;

            Election = new CityElection(this);
            CitizenWait = new Dictionary<Mobile, DateTime>();

            Cities.Add(this);
        }

        public bool IsGovernor(Mobile m)
        {
            return m.AccessLevel > AccessLevel.GameMaster || m == Governor;
        }

        public override PointsEntry GetSystemEntry(PlayerMobile pm)
        {
            return new CityLoyaltyEntry(pm, City);
        }

        public bool IsCitizen(Mobile from, bool staffIsCitizen = true)
        {
            if (from.AccessLevel > AccessLevel.Player && staffIsCitizen)
                return true;

            CityLoyaltyEntry entry = GetPlayerEntry<CityLoyaltyEntry>(from);

            return entry != null && entry.IsCitizen;
        }

        public int GetCitizenCount()
        {
            int count = 0;

            foreach (CityLoyaltyEntry entry in PlayerTable.OfType<CityLoyaltyEntry>())
            {
                if (entry.IsCitizen)
                    count++;
            }

            return count;
        }

        public void DeclareCitizen(Mobile from)
        {
            CityLoyaltyEntry entry = GetPlayerEntry<CityLoyaltyEntry>(from, true);

            entry.DeclareCitizenship();
        }

        public void RenounceCitizenship(Mobile from)
        {
            CityLoyaltyEntry entry = GetPlayerEntry<CityLoyaltyEntry>(from, true);

            if (entry != null)
            {
                entry.RenounceCitizenship();

                if (from == Governor)
                {
                    _Governor = null;

                    if (Stone != null)
                        Stone.InvalidateProperties();
                }

                if (from == GovernorElect)
                {
                    _GovernorElect = null;

                    if (Stone != null)
                        Stone.InvalidateProperties();
                }

                CitizenWait[from] = DateTime.UtcNow + TimeSpan.FromDays(CitizenJoinWait);
            }
        }

        public virtual void AwardHate(Mobile from, double hate)
        {
            CityLoyaltyEntry entry = GetPlayerEntry<CityLoyaltyEntry>(from, true);

            if (entry.Love > 10)
            {
                double convert = entry.Hate / 75;
                entry.Love -= (int)convert;
                entry.Hate += (int)convert;
            }

            entry.Hate += (int)hate;

            if (entry.ShowGainMessage)
            {
                from.SendLocalizedMessage(1152321, Definition.Name); // Your deeds in the city of ~1_name~ are worthy of censure.
            }

            if (from == Governor && entry.LoyaltyRating < LoyaltyRating.Unknown)
                Governor = null;

            if (from == GovernorElect && entry.LoyaltyRating < LoyaltyRating.Unknown)
                GovernorElect = null;
        }

        public virtual void AwardLove(Mobile from, double love, bool message = true)
        {
            CityLoyaltyEntry entry = GetPlayerEntry<CityLoyaltyEntry>(from, true);

            if (entry.Hate > 10)
            {
                double convert = entry.Hate / 75;
                entry.Neutrality += (int)convert;
                entry.Hate -= (int)convert;
            }

            if (AwakeingEventActive)
            {
                foreach (CityLoyaltySystem sys in Cities.Where(s => s.City != this.City))
                {
                    CityLoyaltyEntry e = sys.GetPlayerEntry<CityLoyaltyEntry>(from, true);

                    if (e.Love > 10)
                    {
                        double convert = (double)e.Love / 75.0;

                        if (convert > 0.0)
                        {
                            e.Love -= (int)convert;
                            e.Neutrality += (int)convert;
                        }
                    }
                }
            }

            if (message && entry.ShowGainMessage)
            {
                from.SendLocalizedMessage(1152320, Definition.Name); // Your deeds in the city of ~1_name~ are worthy of praise.
            }

            entry.Love += (int)love;
        }

        public virtual LoyaltyRating GetLoyaltyRating(Mobile from)
        {
            return GetLoyaltyRating(from, GetPlayerEntry<CityLoyaltyEntry>(from as PlayerMobile));
        }

        public virtual LoyaltyRating GetLoyaltyRating(Mobile from, CityLoyaltyEntry entry)
        {
            if (entry != null)
            {
                int love = entry.Love;
                int hate = entry.Hate;
                int neut = entry.Neutrality;

                if (hate > 0 && hate > love && hate > neut)
                {
                    return GetHateRating(hate);
                }
                else if (neut > 0 && neut > love && neut > hate)
                {
                    return GetNeutralRating(neut);
                }
                else if (love > 0)
                {
                    return GetLoveRating(love);
                }
            }

            return LoyaltyRating.Unknown;
        }

        public virtual LoyaltyRating GetHateRating(int points)
        {
            return GetRating(points, _LoveHatePointsTable, _HateLoyaltyTable);
        }

        public virtual LoyaltyRating GetNeutralRating(int points)
        {
            return GetRating(points, _NuetralPointsTable, _NuetralLoyaltyTable);
        }

        public virtual LoyaltyRating GetLoveRating(int points)
        {
            return GetRating(points, _LoveHatePointsTable, _LoveLoyaltyTable);
        }

        private LoyaltyRating GetRating(int points, int[][] table, LoyaltyRating[][] loyaltytable)
        {
            LoyaltyRating rating = LoyaltyRating.Unknown;

            for (int i = 0; i < table.Length; i++)
            {
                for (int j = 0; j < table[i].Length; j++)
                {
                    if (points >= table[i][j])
                        rating = loyaltytable[i][j];
                }
            }

            return rating;
        }

        public void AddToTreasury(Mobile m, int amount, bool givelove = false)
        {
            Treasury += amount;

            if (Stone != null)
                Stone.InvalidateProperties();

            if (givelove)
            {
                AwardLove(m, amount / 250);
            }
        }

        public virtual bool HasTitle(Mobile from, CityTitle title)
        {
            CityLoyaltyEntry entry = GetPlayerEntry<CityLoyaltyEntry>(from);

            if (entry == null)
                return false;

            return (entry.Titles & title) != 0;
        }

        public virtual void AddTitle(Mobile from, CityTitle title)
        {
            CityLoyaltyEntry entry = GetPlayerEntry<CityLoyaltyEntry>(from);

            if (entry == null)
                return;

            entry.AddTitle(title);
        }

        public int GetTitleCost(CityTitle title)
        {
            switch (title)
            {
                default:
                case CityTitle.Citizen: return 0;
                case CityTitle.Knight: return 10000;
                case CityTitle.Baronet: return 100000;
                case CityTitle.Baron: return 1000000;
                case CityTitle.Viscount: return 2000000;
                case CityTitle.Earl: return 5000000;
                case CityTitle.Marquis: return 10000000;
                case CityTitle.Duke: return 50000000;
            }
        }

        public bool HasMinRating(Mobile from, CityTitle title)
        {
            CityLoyaltyEntry entry = GetPlayerEntry<CityLoyaltyEntry>(from);

            if (entry == null)
                return false;

            return entry.LoyaltyRating >= GetMinimumRating(title);
        }

        public LoyaltyRating GetMinimumRating(CityTitle title)
        {
            switch (title)
            {
                default:
                case CityTitle.Citizen: return LoyaltyRating.Disfavored;
                case CityTitle.Knight: return LoyaltyRating.Commended;
                case CityTitle.Baronet: return LoyaltyRating.Esteemed;
                case CityTitle.Baron: return LoyaltyRating.Respected;
                case CityTitle.Viscount: return LoyaltyRating.Admired;
                case CityTitle.Earl: return LoyaltyRating.Adored;
                case CityTitle.Marquis: return LoyaltyRating.Revered;
                case CityTitle.Duke: return LoyaltyRating.Venerated;
            }
        }

        public void OnNewTradeDeal(TradeDeal newtradedeal)
        {
            if (ActiveTradeDeal == TradeDeal.None)
            {
                NextTradeDealCheck = DateTime.UtcNow + TimeSpan.FromDays(TradeDealCostPeriod);
            }
            else if (newtradedeal == TradeDeal.None)
            {
                NextTradeDealCheck = DateTime.MinValue;
                TradeDealStart = DateTime.MinValue;
            }
            else
                TradeDealStart = DateTime.UtcNow;

            ActiveTradeDeal = newtradedeal;

            if (Stone != null)
                Stone.InvalidateProperties();

            foreach (CityLoyaltyEntry player in PlayerTable.OfType<CityLoyaltyEntry>())
            {
                if (player.UtilizingTradeDeal)
                    player.UtilizingTradeDeal = false;
            }
        }

        public void TryUtilizeTradeDeal(Mobile m)
        {
            CityLoyaltyEntry entry = GetPlayerEntry<CityLoyaltyEntry>(m, true);

            if (entry != null)
            {
                if (ActiveTradeDeal == TradeDeal.None)
                {
                    HeraldMessage(m, 1154064); // The City doth nay currently have a Trade Deal in place, perhaps ye should petition the Governor to make such a deal...
                }
                else if (entry.UtilizingTradeDeal)
                {
                    HeraldMessage(m, 1154063); // Thou hath already claimed the benefit of the Trade Deal today!
                }
                else if (entry.LoyaltyRating < LoyaltyRating.Respected)
                {
                    HeraldMessage(m, 1154062); // Begging thy pardon, but thou must be at least Respected within the City to claim the benefits of a Trade Deal!
                }
                else
                {
                    entry.UtilizingTradeDeal = true;
                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.CityTradeDeal, 1154168, 1154169, new TextDefinition((int)ActiveTradeDeal), true));

                    ActivateTradeDeal(m, ActiveTradeDeal);

                    m.Delta(MobileDelta.WeaponDamage);

                    m.SendLocalizedMessage(1154075); // You gain the benefit of your City's Trade Deal!
                }
            }
        }

        public void HeraldMessage(int message)
        {
            HeraldMessage(message, "");
        }

        public void HeraldMessage(string message)
        {
            if (Herald != null)
                Herald.Say(message);
        }

        public void HeraldMessage(int message, string args)
        {
            if (Herald != null)
            {
                Herald.Say(message, args);
            }
        }

        public void HeraldMessage(Mobile to, int message)
        {
            if (Herald != null)
                Herald.SayTo(to, message);
            else
                to.SendLocalizedMessage(message);
        }

        public bool CanAdd(Mobile from)
        {
            if (CitizenWait.ContainsKey(from))
            {
                if (CitizenWait[from] < DateTime.UtcNow)
                {
                    RemoveWaitTime(from);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public int NextJoin(Mobile from)
        {
            if (CitizenWait.ContainsKey(from))
            {
                return (int)(CitizenWait[from] - DateTime.UtcNow).TotalDays;
            }

            return 0;
        }

        public void RemoveWaitTime(Mobile from)
        {
            if (CitizenWait.ContainsKey(from))
            {
                CitizenWait.Remove(from);
            }
        }

        public static void Initialize()
        {
            EventSink.Login += OnLogin;
            Timer.DelayCall(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10), OnTick);

            CommandSystem.Register("ElectionStartTime", AccessLevel.Administrator, e => Gumps.BaseGump.SendGump(new ElectionStartTimeGump(e.Mobile as PlayerMobile)));
            CommandSystem.Register("RemoveWait", AccessLevel.Administrator, e =>
                {
                    foreach (CityLoyaltySystem city in Cities)
                    {
                        city.RemoveWaitTime(e.Mobile);
                    }
                });

            CommandSystem.Register("SystemInfo", AccessLevel.Administrator, e =>
            {
                if (e.Mobile is PlayerMobile)
                {
                    e.Mobile.CloseGump(typeof(SystemInfoGump));
                    Gumps.BaseGump.SendGump(new SystemInfoGump((PlayerMobile)e.Mobile));
                }
            });
        }

        private static DateTime _NextAtrophy;

        public static List<CityLoyaltySystem> Cities { get; private set; } = new List<CityLoyaltySystem>();

        public static bool HasTradeDeal(Mobile m, TradeDeal deal)
        {
            CityLoyaltySystem sys = GetCitizenship(m, false);

            if (sys != null)
            {
                CityLoyaltyEntry entry = sys.GetPlayerEntry<CityLoyaltyEntry>(m, true);

                return sys.ActiveTradeDeal == deal && entry != null && entry.UtilizingTradeDeal;
            }

            return false;
        }

        public static void OnLogin(LoginEventArgs e)
        {
            if (!Enabled)
                return;

            PlayerMobile pm = e.Mobile as PlayerMobile;

            if (pm == null)
                return;

            CityLoyaltySystem sys = GetCitizenship(pm);

            if (sys != null)
            {
                if (sys.ActiveTradeDeal != TradeDeal.None)
                {
                    CityLoyaltyEntry entry = sys.GetPlayerEntry<CityLoyaltyEntry>(pm, true);

                    if (entry != null && entry.UtilizingTradeDeal)
                    {
                        BuffInfo.AddBuff(pm, new BuffInfo(BuffIcon.CityTradeDeal, 1154168, 1154169, new TextDefinition((int)sys.ActiveTradeDeal), true));
                        ActivateTradeDeal(pm, sys.ActiveTradeDeal);
                    }
                }

                int message;

                if (pm.LastOnline + LoveAtrophyDuration > DateTime.UtcNow)
                {
                    message = 1152913; // The moons of Trammel and Felucca align to preserve your virtue status and city loyalty.
                }
                else
                {
                    message = 1152912; // The moons of Trammel and Felucca fail to preserve your virtue status and city loyalty.
                }

                Timer.DelayCall(TimeSpan.FromSeconds(.7), () =>
                {
                    pm.SendLocalizedMessage(message);
                });
            }
        }

        public static void ActivateTradeDeal(Mobile m, TradeDeal deal)
        {
            switch (deal)
            {
                case TradeDeal.OrderOfEngineers: m.AddStatMod(new StatMod(StatType.Dex, string.Format("TradeDeal_{0}", StatType.Dex), 3, TimeSpan.Zero)); break;
                case TradeDeal.MiningCooperative: m.AddStatMod(new StatMod(StatType.Str, string.Format("TradeDeal_{0}", StatType.Str), 3, TimeSpan.Zero)); break;
                case TradeDeal.LeagueOfRangers: m.AddStatMod(new StatMod(StatType.Int, string.Format("TradeDeal_{0}", StatType.Int), 3, TimeSpan.Zero)); break;
            }
        }

        public static void RemoveTradeDeal(Mobile m)
        {
            m.RemoveStatMod(string.Format("TradeDeal_{0}", StatType.Dex));
            m.RemoveStatMod(string.Format("TradeDeal_{0}", StatType.Str));
            m.RemoveStatMod(string.Format("TradeDeal_{0}", StatType.Int));
        }

        public static void OnBODTurnIn(Mobile from, int gold)
        {
            if (!Enabled)
                return;

            CityLoyaltySystem city = Cities.FirstOrDefault(c => c.Definition.Region != null && c.Definition.Region.IsPartOf(from.Region));

            if (city != null)
            {
                city.AwardLove(from, Math.Max(10, gold / 100));
            }
        }

        public static void OnSpawnCreatureKilled(BaseCreature killed, int spawnLevel)
        {
            if (!Enabled || killed == null)
                return;

            List<DamageStore> rights = killed.GetLootingRights();

            rights.ForEach(store =>
                {
                    CityLoyaltySystem city = GetCitizenship(store.m_Mobile, false);

                    if (city != null)
                        city.AwardLove(store.m_Mobile, 1 * (spawnLevel + 1), 0.10 > Utility.RandomDouble());
                });
        }

        public static bool CanAddCitizen(Mobile from)
        {
            if (from.AccessLevel > AccessLevel.Player)
                return true;

            foreach (CityLoyaltySystem city in Cities)
            {
                if (!city.CanAdd(from))
                    return false;
            }
            return true;
        }

        public static int NextJoinCity(Mobile from)
        {
            foreach (CityLoyaltySystem city in Cities)
            {
                if (!city.CanAdd(from))
                {
                    return city.NextJoin(from);
                }
            }

            return 0;
        }

        public static void OnTick()
        {
            foreach (CityLoyaltySystem sys in Cities)
            {
                List<Mobile> list = new List<Mobile>(sys.CitizenWait.Keys);

                foreach (Mobile m in list)
                {
                    if (sys.CitizenWait[m] < DateTime.UtcNow)
                    {
                        sys.RemoveWaitTime(m);
                    }
                }

                ColUtility.Free(list);

                if (DateTime.UtcNow > _NextAtrophy)
                {
                    if (AwakeingEventActive)
                    {
                        sys.PlayerTable.ForEach(t =>
                        {
                            CityLoyaltyEntry entry = t as CityLoyaltyEntry;

                            if (entry != null && entry.Player != null)
                            {
                                PlayerMobile owner = entry.Player;

                                entry.Neutrality -= entry.Neutrality / 50;
                                entry.Hate -= entry.Hate / 50;

                                if (owner.LastOnline + LoveAtrophyDuration < DateTime.UtcNow)
                                    entry.Love -= entry.Love / 75;
                            }
                        });
                    }

                    _NextAtrophy = DateTime.UtcNow + TimeSpan.FromDays(1);
                }

                if (sys.NextTradeDealCheck != DateTime.MinValue && sys.NextTradeDealCheck < DateTime.UtcNow)
                {
                    if (sys.Treasury >= TradeDealCost)
                    {
                        sys.Treasury -= TradeDealCost;
                        sys.NextTradeDealCheck = DateTime.UtcNow + TimeSpan.FromDays(TradeDealCostPeriod);
                    }
                    else
                    {
                        sys.OnNewTradeDeal(TradeDeal.None);
                    }
                }

                foreach (CityLoyaltyEntry entry in sys.PlayerTable.OfType<CityLoyaltyEntry>())
                {
                    entry.CheckTradeDeal();
                }

                if (sys.Election != null)
                {
                    sys.Election.OnTick();
                }
                else
                {
                    sys.Election = new CityElection(sys);
                }

                if (sys.Stone != null)
                {
                    sys.Stone.InvalidateProperties();
                }
            }

            CityTradeSystem.OnTick();
        }

        public static bool HasCitizenship(Mobile from)
        {
            return Cities.Any(sys => sys.IsCitizen(from));
        }

        public static bool HasCitizenship(Mobile from, City city)
        {
            CityLoyaltySystem sys = Cities.FirstOrDefault(s => s.City == city);

            return sys != null && sys.IsCitizen(from);
        }

        public static CityLoyaltySystem GetCitizenship(Mobile from, bool staffIsCitizen = true)
        {
            return Cities.FirstOrDefault(sys => sys.IsCitizen(from, staffIsCitizen));
        }

        public static bool ApplyCityTitle(PlayerMobile pm, ObjectPropertyList list, string prefix, int loc)
        {
            if (loc == 1154017)
            {
                CityLoyaltySystem city = GetCitizenship(pm);

                if (city != null)
                {
                    CityLoyaltyEntry entry = city.GetPlayerEntry<CityLoyaltyEntry>(pm, true);

                    if (entry != null && !string.IsNullOrEmpty(entry.CustomTitle))
                    {
                        prefix = string.Format("{0} {1} the {2}", prefix, pm.Name, entry.CustomTitle);
                        list.Add(1154017, string.Format("{0}\t{1}", prefix, city.Definition.Name)); // ~1_TITLE~ of ~2_CITY~
                        return true;
                    }
                }
            }
            else
            {
                list.Add(1151487, "{0} \t{1} the \t#{2}", prefix, pm.Name, loc); // ~1NT_PREFIX~~2NT_NAME~~3NT_SUFFIX~
                return true;
            }

            return false;
        }

        public static bool HasCustomTitle(PlayerMobile pm, out string str)
        {
            CityLoyaltySystem city = GetCitizenship(pm);
            str = null;

            if (city != null)
            {
                CityLoyaltyEntry entry = city.GetPlayerEntry<CityLoyaltyEntry>(pm, true);

                if (entry != null && !string.IsNullOrEmpty(entry.CustomTitle))
                    str = string.Format("{0}\t{1}", entry.CustomTitle, city.Definition.Name);
            }

            return str != null;
        }

        public static City GetRandomCity()
        {
            switch (Utility.Random(11))
            {
                default:
                case 0: return City.Tasandora;
                case 1: return City.Twierdza;
                case 2: return City.Tirassa;
                case 3: return City.LDelmah;
                case 4: return City.Orod;
                case 5: return City.Lotharn;
                case 6: return City.Garlan;
                case 7: return City.ArtTrader;
                case 10: return City.Celendir;
            }
        }

        public static int GetTitleLocalization(Mobile from, CityTitle title, City city)
        {
            return (1152739 + (int)city * 16) + TitleIndex(title, from.Female);
        }

        private static int TitleIndex(CityTitle title, bool female)
        {
            switch (title)
            {
                default:
                case CityTitle.Citizen: return !female ? 0 : 1;
                case CityTitle.Knight: return !female ? 2 : 3;
                case CityTitle.Baronet: return !female ? 4 : 5;
                case CityTitle.Baron: return !female ? 6 : 7;
                case CityTitle.Viscount: return !female ? 8 : 9;
                case CityTitle.Earl: return !female ? 10 : 11;
                case CityTitle.Marquis: return !female ? 12 : 13;
                case CityTitle.Duke: return !female ? 14 : 15;
            }
        }

        public static int BannerLocalization(City city)
        {
            switch (city)
            {
                default: return 0;
                case City.Tasandora: return 1098171;
                case City.Twierdza: return 1098172;
                case City.Tirassa: return 1098173;
                case City.LDelmah: return 1098174;
                case City.Orod: return 1098175;
                case City.Lotharn: return 1098170;
                case City.Garlan: return 1098178;
                case City.ArtTrader: return 1098177;
                case City.Celendir: return 1098176;
            }
        }

        public static int GetCityLocalization(City city)
        {
            switch (city)
            {
                default: return 0;
                case City.Tasandora: return 1011344;
                case City.Twierdza: return 1011028;
                case City.Tirassa: return 1011343;
                case City.LDelmah: return 1011032;
                case City.Orod: return 1011031;
                case City.Lotharn: return 1011029;
                case City.Garlan: return 1011347;
                case City.ArtTrader: return 1011345;
                case City.Celendir: return 1011030;
            }
        }

        public static int CityLocalization(City city)
        {
            return GetCityInstance(city).Definition.LocalizedName;
        }

        public static int RatingLocalization(LoyaltyRating rating)
        {
            switch (rating)
            {
                default: return 1152115; // Unknown
                case LoyaltyRating.Disfavored: return 1152118;
                case LoyaltyRating.Disliked: return 1152122;
                case LoyaltyRating.Detested: return 1152123;
                case LoyaltyRating.Loathed: return 1152128;
                case LoyaltyRating.Despised: return 1152129;
                case LoyaltyRating.Reviled: return 1152130;
                case LoyaltyRating.Scorned: return 1152136;
                case LoyaltyRating.Shunned: return 1152137;
                case LoyaltyRating.Villified: return 1152138;
                case LoyaltyRating.Abhorred: return 1152139;
                case LoyaltyRating.Unknown: return 1152115;
                case LoyaltyRating.Doubted: return 1152117;
                case LoyaltyRating.Distrusted: return 1152121;
                case LoyaltyRating.Disgraced: return 1152127;
                case LoyaltyRating.Denigrated: return 1152135;
                case LoyaltyRating.Commended: return 1152116;
                case LoyaltyRating.Esteemed: return 1152120;
                case LoyaltyRating.Respected: return 1152119;
                case LoyaltyRating.Honored: return 1152126;
                case LoyaltyRating.Admired: return 1152125;
                case LoyaltyRating.Adored: return 1152124;
                case LoyaltyRating.Lauded: return 1152134;
                case LoyaltyRating.Exalted: return 1152133;
                case LoyaltyRating.Revered: return 1152132;
                case LoyaltyRating.Venerated: return 1152131;
            }
        }

        public static int[][] _LoveHatePointsTable =
        {
            new int[] { 250 }, 								// Tier 1
			new int[] { 500, 1000 }, 						// Tier 2
			new int[] { 5000, 10000, 25000 }, 				// Tier 3
			new int[] { 100000, 250000, 500000, 1000000  }, // Tier 4
		};

        public static int[][] _NuetralPointsTable =
        {
            new int[] { 250 }, 								// Tier 1
			new int[] { 1000 }, 							// Tier 2
			new int[] { 25000 }, 							// Tier 3
			new int[] { 1000000 }, 							// Tier 4
		};

        public static LoyaltyRating[][] _LoveLoyaltyTable =
        {
            new LoyaltyRating[] { LoyaltyRating.Commended }, 																		// Tier 1
			new LoyaltyRating[] { LoyaltyRating.Esteemed, LoyaltyRating.Respected }, 												// Tier 2
			new LoyaltyRating[] { LoyaltyRating.Honored, LoyaltyRating.Admired, LoyaltyRating.Adored }, 							// Tier 3
			new LoyaltyRating[] { LoyaltyRating.Lauded, LoyaltyRating.Exalted, LoyaltyRating.Revered, LoyaltyRating.Venerated  },   // Tier 4
		};

        public static LoyaltyRating[][] _HateLoyaltyTable =
        {
            new LoyaltyRating[] { LoyaltyRating.Disfavored }, 																		// Tier 1
			new LoyaltyRating[] { LoyaltyRating.Disliked, LoyaltyRating.Detested }, 											    // Tier 2
			new LoyaltyRating[] { LoyaltyRating.Loathed, LoyaltyRating.Despised, LoyaltyRating.Reviled }, 							// Tier 3
			new LoyaltyRating[] { LoyaltyRating.Scorned, LoyaltyRating.Shunned, LoyaltyRating.Villified, LoyaltyRating.Abhorred  }, // Tier 4
		};

        public static LoyaltyRating[][] _NuetralLoyaltyTable =
        {
            new LoyaltyRating[] { LoyaltyRating.Doubted }, 							// Tier 1
			new LoyaltyRating[] { LoyaltyRating.Distrusted }, 						// Tier 2
			new LoyaltyRating[] { LoyaltyRating.Disgraced }, 						// Tier 3
			new LoyaltyRating[] { LoyaltyRating.Denigrated }, 						// Tier 4
		};

        public static bool IsLove(LoyaltyRating rating)
        {
            foreach (LoyaltyRating[] ratings in _LoveLoyaltyTable)
            {
                foreach (LoyaltyRating r in ratings)
                {
                    if (r == rating)
                        return true;
                }
            }

            return false;
        }

        public static CityLoyaltySystem GetCityInstance(City city)
        {
            switch (city)
            {
                default: return null;
                case City.Tasandora: return Tasandora;
                case City.Twierdza: return Twierdza;
                case City.Tirassa: return Tirassa;
                case City.LDelmah: return LDelmah;
                case City.Orod: return Orod;
                case City.Lotharn: return Lotharn;
                case City.Garlan: return Garlan;
                case City.ArtTrader: return ArtTrader;
                case City.Celendir: return Celendir;
            }
        }

        public static bool IsSetup()
        {
            return Cities.FirstOrDefault(c => c.CanUtilize) != null;
        }

        public static void OnTradeComplete(Mobile from, TradeEntry entry)
        {
            CityLoyaltySystem dest = GetCityInstance(entry.Destination);
            CityLoyaltySystem origin = GetCityInstance(entry.Origin);
            int gold = entry.CalculateGold();

            if (gold > 0)
            {
                origin.AddToTreasury(from, gold);
                from.SendLocalizedMessage(1154761, string.Format("{0}\t{1}", gold.ToString("N0", CultureInfo.GetCultureInfo("en-US")), origin.Definition.Name)); // ~1_val~ gold has been deposited into the ~2_NAME~ City treasury for your efforts!
            }

            if (entry.Distance > 0)
            {
                origin.AwardLove(from, 150);
                dest.AwardLove(from, 150);
            }

            origin.CompletedTrades++;

            if (CityTradeSystem.KrampusEncounterActive)
            {
                KrampusEvent.Instance.OnTradeComplete(from, entry);
            }
        }

        public static void OnSlimTradeComplete(Mobile from, TradeEntry entry)
        {
            CityLoyaltySystem dest = GetCityInstance(entry.Destination);
            CityLoyaltySystem origin = GetCityInstance(entry.Origin);

            if (entry.Distance > 0)
            {
                origin.AwardHate(from, 25);
                dest.AwardHate(from, 25);
            }
        }

        public static Tasandora Tasandora { get; set; }
        public static Twierdza Twierdza { get; set; }
        public static Tirassa Tirassa { get; set; }
        public static LDelmah LDelmah { get; set; }
        public static Orod Orod { get; set; }
        public static Lotharn Lotharn { get; set; }
        public static Garlan Garlan { get; set; }
        public static ArtTrader ArtTrader { get; set; }
        public static Celendir Celendir { get; set; }

        public static CityTradeSystem CityTrading { get; set; }

        public static void ConstructSystems()
        {
            Tasandora = new Tasandora();
            Twierdza = new Twierdza();
            Tirassa = new Tirassa();
            LDelmah = new LDelmah();
            Orod = new Orod();
            Lotharn = new Lotharn();
            Garlan = new Garlan();
            ArtTrader = new ArtTrader();
            Celendir = new Celendir();

            CityTrading = new CityTradeSystem();
        }

        public override void Serialize(GenericWriter writer)
        {
            writer.Write((int)City);

            base.Serialize(writer);
            writer.Write(2);

            writer.Write(CitizenWait.Count);
            foreach (KeyValuePair<Mobile, DateTime> kvp in CitizenWait)
            {
                writer.Write(kvp.Key);
                writer.Write(kvp.Value);
            }

            writer.Write(CompletedTrades);
            writer.Write(Governor);
            writer.Write(GovernorElect);
            writer.Write(PendingGovernor);
            writer.Write(Treasury);
            writer.Write((int)ActiveTradeDeal);
            writer.Write(TradeDealStart);
            writer.Write(NextTradeDealCheck);
            writer.Write(CanUtilize);

            writer.Write(Headline);
            writer.Write(Body);
            writer.Write(PostedOn);

            if (Election != null)
            {
                writer.Write(0);
                Election.Serialize(writer);
            }
            else
                writer.Write(1);
        }

        public override void Deserialize(GenericReader reader)
        {
            City = (City)reader.ReadInt();

            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                case 1:
                    {
                        int count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                        {
                            Mobile m = reader.ReadMobile();
                            DateTime dt = reader.ReadDateTime();

                            if (m != null && dt > DateTime.UtcNow)
                                CitizenWait[m] = dt;
                        }
                    }
                    goto case 0;
                case 0:
                    {
                        CompletedTrades = reader.ReadInt();
                        Governor = reader.ReadMobile();
                        GovernorElect = reader.ReadMobile();
                        PendingGovernor = reader.ReadBool();
                        Treasury = reader.ReadLong();
                        ActiveTradeDeal = (TradeDeal)reader.ReadInt();
                        TradeDealStart = reader.ReadDateTime();
                        NextTradeDealCheck = reader.ReadDateTime();
                        CanUtilize = reader.ReadBool();

                        Headline = reader.ReadString();
                        Body = reader.ReadString();
                        PostedOn = reader.ReadDateTime();

                        if (reader.ReadInt() == 0)
                            Election = new CityElection(this, reader);
                        else
                            Election = new CityElection(this);
                    }
                    break;
            }

            if (version == 0 && City == City.Twierdza)
            {
                int count = reader.ReadInt();
                for (int i = 0; i < count; i++)
                {
                    Mobile m = reader.ReadMobile();
                    DateTime dt = reader.ReadDateTime();

                    if (m != null && dt > DateTime.UtcNow)
                        CitizenWait[m] = dt;
                }
            }

            // City Bulletin Board Location
            if (version == 1)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                    {
                        Board = new CityMessageBoard(City, 0xA0C5);
                        Board.MoveToWorld(Definition.BoardLocation, SystemMap);
                        Console.WriteLine("City Message Board for {0} Converted!", City.ToString());
                    });
            }
        }
    }

    public class Tasandora : CityLoyaltySystem
    {
        public override PointsType Loyalty => PointsType.Moonglow;

        public Tasandora() : base(City.Tasandora)
        {
            Definition = new CityDefinition(
                             City.Tasandora,
                             new Point3D(1422, 2087, 0),
                             new Point3D(1409, 2099, 0),
                             new Point3D(1420, 2105, 0),
                             new Point3D(1442, 2098, 0),
                             new Point3D(1445, 2062, 0),
                             "Tasandora",
                             1011344,
                             1154524
                             );
        }
    }

    public class Twierdza : CityLoyaltySystem
    {
        public override PointsType Loyalty => PointsType.Britain;

        public Twierdza() : base(City.Twierdza)
        {
            Definition = new CityDefinition(
                             City.Twierdza,
                             new Point3D(2515, 1806, 0),
                             new Point3D(2516, 1816, 0),
                             new Point3D(2536, 1829, 0),
                             new Point3D(2549, 1818, 0),
                             new Point3D(2520, 1801, 0),
                             "Twierdza",
                             1011028,
                             1154521
                             );
        }
    }

    public class Tirassa : CityLoyaltySystem
    {
        public override PointsType Loyalty => PointsType.Jhelom;

        public Tirassa() : base(City.Tirassa)
        {
            Definition = new CityDefinition(
                             City.Tirassa,
                             new Point3D(1993, 2724, 0),
                             new Point3D(1973, 2738, 0),
                             new Point3D(1964, 2731, -7),
                             new Point3D(1963, 2729, 5),
                             new Point3D(2030, 2757, 0),
                             "Tirassa",
                             1114146,
                             1154522
                             );
        }
    }

    public class LDelmah : CityLoyaltySystem
    {
        public override PointsType Loyalty => PointsType.Yew;

        public LDelmah() : base(City.LDelmah)
        {
            Definition = new CityDefinition(
                             City.LDelmah,
                             new Point3D(5721,3342, 0),
                             new Point3D(5723, 3323, 0),
                             new Point3D(5745,3328, 0),
                             new Point3D(5753, 3328, 0),
                             new Point3D(5741, 3313, 0),
                             "L'Delmah",
                             1011032,
                             1154529
                             );
        }
    }

    public class Orod : CityLoyaltySystem
    {
        public override PointsType Loyalty => PointsType.Minoc;

        public Orod() : base(City.Orod)
        {
            Definition = new CityDefinition(
                             City.Orod,
                             new Point3D(856, 1880, 0),
                             new Point3D(843, 1897, 0),
                             new Point3D(853, 1897, 0),
                             new Point3D(865, 1888, 0),
                             new Point3D(829, 1904, 0),
                             "Orod",
                             1011031,
                             1154523
                             );
        }
    }

    public class Lotharn : CityLoyaltySystem
    {
        public override PointsType Loyalty => PointsType.Trinsic;

        public Lotharn() : base(City.Lotharn)
        {
            Definition = new CityDefinition(
                             City.Lotharn,
                             new Point3D(1975, 564, 0),
                             new Point3D(1975, 589, 1),
                             new Point3D(1982, 597, 1),
                             new Point3D(1973, 606, 0),
                             new Point3D(1956, 603, 0),
                             "Lotharn",
                             1011029,
                             1154527
                             );
        }
    }

    public class Garlan : CityLoyaltySystem
    {
        public override PointsType Loyalty => PointsType.SkaraBrae;

        public Garlan() : base(City.Garlan)
        {
            Garlan = this;
            Definition = new CityDefinition(
                             City.Garlan,
                             new Point3D(928, 661, 30),
                             new Point3D(919, 679, 2),
                             new Point3D(926, 684, -1),
                             new Point3D(913, 686, 2),
                             new Point3D(940, 683, 1),
                             "Garlan",
                             1114138,
                             1154526
                             );
        }
    }

    public class ArtTrader : CityLoyaltySystem
    {
        public override PointsType Loyalty => PointsType.NewMagincia;

        public ArtTrader() : base(City.ArtTrader)
        {
            ArtTrader = this;
            Definition = new CityDefinition(
                             City.ArtTrader,
                             new Point3D(1512, 1505, 10),
                             new Point3D(1512, 15144, 10),
                             new Point3D(1520, 1513, 10),
                             new Point3D(1526, 1515, 0),
                             new Point3D(1517, 1512, 5),
                             "Wolny port handlarzy zwojami",
                             3070070,
                             1154525
                             );
        }
    }

    public class Celendir : CityLoyaltySystem
    {
        public override PointsType Loyalty => PointsType.Vesper;

        public Celendir() : base(City.Celendir)
        {
            Celendir = this;
            Definition = new CityDefinition(
                             City.Celendir,
                             new Point3D(2016, 2077, 0),
                             new Point3D(2046, 2038, 5),
                             new Point3D(2070, 2037, 0),
                             new Point3D(2053, 2046, 0),
                             new Point3D(2049, 2044, 0),
                             "Celendir",
                             1114144,
                             1154528
                             );
        }
    }
}
