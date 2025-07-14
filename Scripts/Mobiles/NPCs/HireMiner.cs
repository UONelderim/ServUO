using System;
using Server.Mobiles;
using Server.Items;
using Server;
using Server.Targeting;
using System.Linq;
using Server.Regions;
using Server.Spells;
using Server.PathAlgorithms;

namespace Server.Mobiles
{
    public class HireMiner : TrainableHire
    {
	    private DateTime m_NextMineTime;
	    private static readonly TimeSpan BaseMineInterval = TimeSpan.FromMinutes(10);
	    private const int MaxBackpackItems = 100;
	    private bool m_ShouldWork = true;
	    private static readonly int ScanRange = 30;
	    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(30);

        private static readonly Point3D[] MiningSpots = new Point3D[]
        {
            new Point3D(5197, 1147, 0)
        };

        private Point3D? m_TargetMiningSpot = null;
        private Point3D? m_LastFailedSpot = null;
        private DateTime m_MovementStartTime;
        private int m_MiningAttempts = 0;
        private int m_MinesInPlace = 0;
        private bool m_AtWorkSpot = false;
        private WayPoint m_CurrentWorkWaypoint;
        private static readonly Random rand = new Random();

        private void CreateAndAssignWaypoint(Point3D origin)
        {
	        DeleteCurrentWaypoint();

	        Map map = this.Map;
	        int dx = rand.Next(3, 6) * (rand.Next(2) == 0 ? 1 : -1);
	        int dy = rand.Next(3, 6) * (rand.Next(2) == 0 ? 1 : -1);
	        Point3D location = new Point3D(origin.X + dx, origin.Y + dy, origin.Z);

	        Point3D[] candidateSpots = new Point3D[]
	        {
		        location,
		        new Point3D(location.X + 1, location.Y, location.Z),
		        new Point3D(location.X - 1, location.Y, location.Z),
		        new Point3D(location.X, location.Y + 1, location.Z),
		        new Point3D(location.X, location.Y - 1, location.Z)
	        };

	        foreach (var spot in candidateSpots)
	        {
		        if (map.CanFit(spot, 1, false, false))
		        {
			        m_CurrentWorkWaypoint = new WayPoint();
			        m_CurrentWorkWaypoint.MoveToWorld(spot, map);
			        this.CurrentWayPoint = m_CurrentWorkWaypoint; // <-- tylko to wystarczy
			        m_TargetMiningSpot = spot;
			        m_MovementStartTime = DateTime.UtcNow;
			        return;
		        }
	        }

	        Say("Nie mogę znaleźć odpowiedniego miejsca na ścieżkę, panie...");
	        m_LastFailedSpot = origin;
        }




        private void DeleteCurrentWaypoint()
        {
            if (m_CurrentWorkWaypoint != null && !m_CurrentWorkWaypoint.Deleted)
            {
                m_CurrentWorkWaypoint.Delete();
                m_CurrentWorkWaypoint = null;
            }
        }

        private void GoTo(Point3D location)
        {
	        if (this.Location == location)
		        return;

	        Say("Wyruszam do wyznaczonego punktu...");
	        this.ControlOrder = OrderType.None;

	        WayPoint wp = new WayPoint();
	        Map map = this.Map;

	        Point3D[] candidateSpots = new Point3D[]
	        {
		        new Point3D(location.X, location.Y, location.Z),
		        new Point3D(location.X + 1, location.Y, location.Z),
		        new Point3D(location.X - 1, location.Y, location.Z),
		        new Point3D(location.X, location.Y + 1, location.Z),
		        new Point3D(location.X, location.Y - 1, location.Z)
	        };

	        foreach (var spot in candidateSpots)
	        {
		        if (map.CanFit(spot, 1, false, false))
		        {
			        wp.MoveToWorld(spot, map);
			        this.CurrentWayPoint = wp;
			        return;
		        }
	        }

	        Say("Nie mogę znaleźć odpowiedniego miejsca na ścieżkę, panie...");
	        m_LastFailedSpot = location;
        }

        private double GetDistance(Point3D a, Point3D b)
        {
	        int dx = a.X - b.X;
	        int dy = a.Y - b.Y;
	        return Math.Sqrt(dx * dx + dy * dy);
        }

        [Constructable]
        public HireMiner() : base(AIType.AI_Melee)
        {
            Title = "górnik najemny";
            SetSkill(SkillName.Mining, 60.0);
            m_NextMineTime = DateTime.UtcNow + BaseMineInterval;
            BodyValue = 400;
            Race = Race.NTamael;
        }

        public HireMiner(Serial serial) : base(serial) { }

         public override void OnThink()
        {
            base.OnThink();

            if (!m_ShouldWork)
                return;

            if (!InMiningRegion())
            {
                Say("Nie czuję zapachu kopalni... Muszę się tam udać!");

                if (m_TargetMiningSpot == null)
                {
                    Say("Wyczuwam zapach rud... zmierzam ku kopalni!");
                    CreateAndAssignWaypoint(Location);
                }

                return;
            }

            if (m_TargetMiningSpot != null && GetDistance(Location, m_TargetMiningSpot.Value) < 2)
            {
                Say("Dotarłem na miejsce wydobycia.");
                this.ControlOrder = OrderType.Stay;
                this.ControlTarget = null;
                m_AtWorkSpot = true;
            }

            if (m_TargetMiningSpot != null && !m_AtWorkSpot && DateTime.UtcNow - m_MovementStartTime > Timeout)
            {
                Say("Nie mogę dotrzeć do celu... Szukam innego miejsca.");
                CreateAndAssignWaypoint(Location);
                m_MiningAttempts = 0;
                m_MinesInPlace = 0;
                m_AtWorkSpot = false;
            }

            double efficiency = 1.0 - (100 - Happiness) * 0.005;
            TimeSpan adjusted = TimeSpan.FromTicks((long)(BaseMineInterval.Ticks / efficiency));

            if (DateTime.UtcNow >= m_NextMineTime && m_AtWorkSpot && ControlOrder == OrderType.Stay)
            {
                if (WithdrawPayment())
                {
                    if (MineOre())
                    {
                        m_MiningAttempts = 0;
                        m_MinesInPlace++;

                        DeleteCurrentWaypoint();

                        if (m_MinesInPlace >= 3)
                        {
                            Say("Przemieszczam się do nowej żyły rudy...");
                            CreateAndAssignWaypoint(Location);
                            m_MiningAttempts = 0;
                            m_MinesInPlace = 0;
                            m_AtWorkSpot = false;
                        }
                    }
                    else
                    {
                        m_MiningAttempts++;

                        if (m_MiningAttempts >= 2)
                        {
                            Say("Szukam innego miejsca...");
                            CreateAndAssignWaypoint(Location);
                            m_MiningAttempts = 0;
                            m_MinesInPlace = 0;
                            m_AtWorkSpot = false;
                        }
                    }
                }

                m_NextMineTime = DateTime.UtcNow + adjusted;
            }

            NameMod = m_ShouldWork && ControlOrder == OrderType.Stay ? "[pracuje] górnik najemny" : String.Empty;
        }

        private Point3D? FindNearestMiningSpot()
        {
            return MiningSpots
                .Where(p => GetDistance(Location, p) <= 3)
                .OrderBy(p => GetDistance(Location, p))
                .FirstOrDefault();
        }

        private bool MineOre()
        {
            if (!HasRequiredTool(out Pickaxe pickaxe))
            {
                Say("Mój kilof się gdzieś zawieruszył! Nie mogę pracować.");
                m_ShouldWork = false;
                return false;
            }

            Animate(11, 5, 1, true, false, 0);
            Effects.SendLocationEffect(Location, Map, 0x376A, 10);
            SayRandomMiningQuote();

            if (Utility.RandomDouble() < 0.05)
            {
                pickaxe.Delete();
                Say("Ach, mój kilof się rozpadł!");
            }

            if (Backpack == null || Backpack.Items.Count >= MaxBackpackItems)
            {
                Say("Nie mam już miejsca w jukach! Muszę przestać.");
                m_ShouldWork = false;
                ControlOrder = OrderType.Follow;
                return false;
            }

            int bonus = Math.Max(0, Level - 1);
            Item ore = CreateOreByLevel(Level);

            Mobile owner = GetOwner();
            if (owner?.BankBox != null)
            {
                Say("Wysyłam rudę do skarbca mojego pana.");
                owner.BankBox.DropItem(ore);
            }
            else
            {
                Backpack.DropItem(ore);
            }

            return true;
        }

        private Item CreateOreByLevel(int level)
        {
            int roll = Utility.Random(100);
            Item ore;

            if (level >= 10 && roll < 10)
                ore = new ValoriteOre(Utility.RandomMinMax(1, 2));
            else if (level >= 8 && roll < 20)
                ore = new VeriteOre(Utility.RandomMinMax(1, 2));
            else if (level >= 6 && roll < 35)
                ore = new AgapiteOre(Utility.RandomMinMax(1, 2));
            else if (level >= 4 && roll < 50)
                ore = new GoldOre(Utility.RandomMinMax(1, 2));
            else if (level >= 2 && roll < 70)
                ore = new ShadowIronOre(Utility.RandomMinMax(1, 2));
            else
                ore = new IronOre(Utility.RandomMinMax(1, 2));

            if (Utility.RandomDouble() < 0.15)
                ore.Name = (ore.Name ?? ore.GetType().Name) + " (oczyszczona)";

            Say($"Znalazłem {ore.Name?.ToLowerInvariant() ?? ore.GetType().Name.ToLowerInvariant()}!");
            return ore;
        }

        private bool HasRequiredTool(out Pickaxe tool)
        {
            tool = Backpack?.FindItemByType(typeof(Pickaxe)) as Pickaxe;
            return tool != null;
        }

        private void SayRandomMiningQuote()
        {
            if (Utility.RandomDouble() > 0.25) return;
            string[] quotes =
            {
                "Kopię dalej!",
                "Znowu kamień...",
                "Żyły są głębiej!",
                "To wygląda na coś cennego.",
                "Złoto? Nie, to tylko piryt...",
                "Słońce nie dochodzi tu wcale.",
                "Kilof się ślizga...",
                "To idzie do przetopienia.",
                "Głęboko, ale warto.",
                "Ruda zawsze się przyda."
            };
            Say(quotes[Utility.Random(quotes.Length)]);
        }

        private bool InMiningRegion()
        {
            return Region is MiningRegion;
        }

        private bool WithdrawPayment()
        {
            int cost = 5 + Level;
            Mobile owner = GetOwner();
            if (owner == null || !Banker.Withdraw(owner, cost))
            {
                Say("Brakuje mi zapłaty z twojego skarbca, mój panie.");
                return false;
            }

            return true;
        }

            public override void OnSpeech(SpeechEventArgs e)
        {
            base.OnSpeech(e);

            if (!e.Handled && e.Mobile.InRange(this, 8))
            {
                string speech = e.Speech.ToLower();

                if (e.Mobile == GetOwner())
                {
                    if (speech.Contains("pracuj"))
                    {
                        Say("Zaczynam swą robotę, panie!");
                        m_ShouldWork = true;
                        CreateAndAssignWaypoint(Location);
                        ControlOrder = OrderType.None;
                        e.Handled = true;
                    }
                    else if (speech.Contains("przestań pracować") || speech.Contains("follow") || speech.Contains("stay"))
                    {
                        Say("Odpocznę chwilę, jak rozkażesz.");
                        m_ShouldWork = false;
                        ControlOrder = OrderType.Follow;
                        DeleteCurrentWaypoint();
                        e.Handled = true;
                    }
                }

                if (e.Mobile.AccessLevel >= AccessLevel.GameMaster)
                {
                    if (speech.Contains("direction"))
                    {
                        Say($"Aktualny kierunek: {Direction}");
                        e.Handled = true;
                    }
                    else if (speech.Contains("location"))
                    {
                        Say($"Moja pozycja: {Location.X}, {Location.Y}, {Location.Z}");
                        e.Handled = true;
                    }
                    else if (speech.Contains("region"))
                    {
                        Say($"Region: {(Region != null ? Region.Name : "brak")}");
                        e.Handled = true;
                    }
                    else if (speech.Contains("target"))
                    {
                        Say(m_TargetMiningSpot.HasValue ? $"Cel: {m_TargetMiningSpot.Value.X}, {m_TargetMiningSpot.Value.Y}, {m_TargetMiningSpot.Value.Z}" : "Brak ustawionego celu.");
                        e.Handled = true;
                    }
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);
            writer.Write(m_NextMineTime);
            writer.Write(m_ShouldWork);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextMineTime = reader.ReadDateTime();
            m_ShouldWork = version >= 1 && reader.ReadBool();
        }
    }
}
