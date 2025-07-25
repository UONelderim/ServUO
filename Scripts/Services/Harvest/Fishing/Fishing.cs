using Server.Engines.Quests;
using Server.Engines.Quests.Collector;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System;
using System.Collections.Generic;

namespace Server.Engines.Harvest
{
    public class Fishing : HarvestSystem
    {
        private static Fishing m_System;

        public static Fishing System
        {
            get
            {
                if (m_System == null)
                    m_System = new Fishing();

                return m_System;
            }
        }

        private readonly HarvestDefinition m_Definition;

        public HarvestDefinition Definition => m_Definition;

        private Fishing()
        {
            HarvestResource[] res;
            HarvestVein[] veins;

            #region Fishing
            HarvestDefinition fish = new HarvestDefinition
            {

                // Resource banks are every 8x8 tiles
                BankWidth = 8,
                BankHeight = 8,

                // Every bank holds from 5 to 15 fish
                MinTotal = 5,
                MaxTotal = 15,

                // A resource bank will respawn its content every 10 to 20 minutes
                MinRespawn = TimeSpan.FromMinutes(10.0),
                MaxRespawn = TimeSpan.FromMinutes(20.0),

                // Skill checking is done on the Fishing skill
                Skill = SkillName.Fishing,

                // Set the list of harvestable tiles
                Tiles = m_WaterTiles,
                SpecialTiles = m_LavaTiles,
                RangedTiles = true,

                // Players must be within 4 tiles to harvest
                MaxRange = 4,

                // One fish per harvest action
                ConsumedPerHarvest = 1,
                ConsumedPerFeluccaHarvest = 1,

                // The fishing
                EffectActions = [6],
                EffectSounds = [],
                EffectCounts = [1],
                EffectDelay = TimeSpan.Zero,
                EffectSoundDelay = TimeSpan.FromSeconds(8.0),

                NoResourcesMessage = 503172, // The fish don't seem to be biting here.
                FailMessage = 503171, // You fish a while, but fail to catch anything.
                TimedOutOfRangeMessage = 500976, // You need to be closer to the water to fish!
                OutOfRangeMessage = 500976, // You need to be closer to the water to fish!
                PackFullMessage = 503176, // You do not have room in your backpack for a fish.
                ToolBrokeMessage = 503174 // You broke your fishing pole.
            };

            res =
            [
	            new HarvestResource(00.0, 00.0, 120.0, 1043297, typeof(Fish))
            ];

            veins =
            [
	            new HarvestVein(100.0, 0.0, res[0], null)
            ];

            fish.Resources = res;
            fish.Veins = veins;

            fish.BonusResources =
            [
	            new BonusHarvestResource(0, 97.0, null, null), //set to same chance as mining ml gems
			    new BonusHarvestResource(80.0, 2.0, 1113764, typeof(DelicateScales)),
                new BonusHarvestResource(80.0, 1.0, 1072597, typeof(WhitePearl))
            ];

            m_Definition = fish;
            Definitions.Add(fish);
            #endregion
        }

        public override void OnConcurrentHarvest(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            from.SendLocalizedMessage(500972); // You are already fishing.
        }

        private class MutateEntry
        {
            public double m_ReqSkill, m_MinSkill, m_MaxSkill;
            public bool m_DeepWater;
            public Type[] m_Types;

            public MutateEntry(double reqSkill, double minSkill, double maxSkill, bool deepWater, params Type[] types)
            {
                m_ReqSkill = reqSkill;
                m_MinSkill = minSkill;
                m_MaxSkill = maxSkill;
                m_DeepWater = deepWater;
                m_Types = types;
            }
        }

        private static readonly MutateEntry[] m_MutateTable =
        [
	        new MutateEntry( 80.0,  80.0,  1865.0,  true, typeof( SpecialFishingNet ) ),
            new MutateEntry( 90.0,  80.0,  1875.0,  true, typeof( TreasureMap ) ),
            new MutateEntry( 100.0,  80.0,  750.0,  true, typeof( MessageInABottle ) ),
            new MutateEntry( 80.0,  80.0,  4080.0,  true, typeof( BigFish ) ),
            new MutateEntry( 0.0, 125.0, -2375.0, false, typeof( PrizedFish ), typeof( WondrousFish ), typeof( TrulyRareFish ), typeof( PeculiarFish ) ),
            new MutateEntry( 0.0, 125.0,  -420.0, false, typeof( Boots ), typeof( Shoes ), typeof( Sandals ), typeof( ThighBoots ) ),
            new MutateEntry( 80.0,  80.0, 2500.0, false, typeof( MudPuppy ), typeof( RedHerring) ),
            new MutateEntry( 0.0, 200.0,  -200.0, false, [null])
        ];

        private static readonly MutateEntry[] m_SiegeMutateTable =
        [
	        new MutateEntry( 80.0,  80.0,  1865.0,  true, typeof( SpecialFishingNet ) ),
            new MutateEntry( 0.0, 200.0,  -200.0, false, [null]),
            new MutateEntry( 100.0,  80.0,  1865.0,  true, typeof( MessageInABottle ) ),
            new MutateEntry( 80.0,  80.0,  4080.0,  true, typeof( BigFish ) ),
            new MutateEntry( 0.0, 125.0, -2375.0, false, typeof( PrizedFish ), typeof( WondrousFish ), typeof( TrulyRareFish ), typeof( PeculiarFish ) ),
            new MutateEntry( 0.0, 105.0,  -420.0, false, typeof( Boots ), typeof( Shoes ), typeof( Sandals ), typeof( ThighBoots ) ),
            new MutateEntry( 80.0,  80.0, 2500.0, false, typeof( MudPuppy ), typeof( RedHerring) ),
            new MutateEntry( 0.0, 200.0,  -200.0, false, [null])
        ];

        private static readonly MutateEntry[] m_LavaMutateTable =
        [
	        new MutateEntry( 0.0,  80.0, 333, false, typeof(StoneFootwear)),
            new MutateEntry( 80.0, 80.0, 333, false, typeof(CrackedLavaRockEast), typeof(CrackedLavaRockSouth)),
            new MutateEntry( 85.0, 80.0, 333, false, typeof(StonePaver)),
            new MutateEntry( 80.0, 80.0, 4080, false, typeof(BaseWeapon))
        ];

        public override bool SpecialHarvest(Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc)
        {
            PlayerMobile player = from as PlayerMobile;

            Container pack = from.Backpack;

            if (player != null)
            {
                QuestSystem qs = player.Quest;

                if (qs is CollectorQuest)
                {
                    QuestObjective obj = qs.FindObjective(typeof(FishPearlsObjective));

                    if (obj != null && !obj.Completed)
                    {
                        if (Utility.RandomDouble() < 0.5)
                        {
                            player.SendLocalizedMessage(1055086, "", 0x59); // You pull a shellfish out of the water, and find a rainbow pearl inside of it.

                            obj.CurProgress++;
                        }
                        else
                        {
                            player.SendLocalizedMessage(1055087, "", 0x2C); // You pull a shellfish out of the water, but it doesn't have a rainbow pearl.
                        }

                        return true;
                    }
                }

                if (from.Region.IsPartOf("Underworld"))
                {
                    foreach (BaseQuest quest in player.Quests)
                    {
                        if (quest is SomethingFishy && Utility.RandomDouble() < 0.1)
                        {
                            Item red = new RedHerring();
                            from.AddToBackpack(red);
                            player.SendLocalizedMessage(1095047, "", 0x23); // You pull a shellfish out of the water, but it doesn't have a rainbow pearl.
                            return true;
                        }

                        if (quest is ScrapingtheBottom && Utility.RandomDouble() < 0.1)
                        {
                            Item mug = new MudPuppy();
                            from.AddToBackpack(mug);
                            player.SendLocalizedMessage(1095064, "", 0x23); // You pull a shellfish out of the water, but it doesn't have a rainbow pearl.
                            return true;
                        }
                    }
                }

                #region High Seas Charydbis
                if (tool is FishingPole pole && CharydbisSpawner.SpawnInstance != null && CharydbisSpawner.SpawnInstance.IsSummoned)
                {
                    Item oracle = from.Backpack.FindItemByType(typeof(OracleOfTheSea));
                    CharydbisSpawner sp = CharydbisSpawner.SpawnInstance;

                    if (oracle != null && sp != null)
                    {
                        if (from.Map != sp.Map)
                            from.SendLocalizedMessage(1150861); //Charybdis have never been seen in these waters, try somewhere else.

                        else if (pole.BaitType == typeof(Charydbis) && from.Skills[SkillName.Fishing].Value >= 100)
                        {
                            if (sp.Charydbis == null && !sp.HasSpawned && sp.CurrentLocation.Contains(loc))
                            {
                                Multis.BaseBoat boat = Multis.BaseBoat.FindBoatAt(from, from.Map);
                                sp.SpawnCharydbis(from, loc, sp.Map, boat);
                                sp.HasSpawned = true;
                                pole.OnFishedHarvest(from, true);
                                return true;
                            }
                            else if (sp.LastLocation.Contains(loc))
                            {
                                from.SendLocalizedMessage(1150862); //The charybdis has moved on from this location, consult Oracle Of The Seas again.
                            }
                        }
                        else
                            from.SendLocalizedMessage(1150858); //You see a few bubbles, but no charybdis.
                    }
                }
                #endregion
            }

            return false;
        }

        public override Type MutateType(Type type, Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource)
        {
            if (FishInfo.IsRareFish(type))
                return type;

            bool deepWater = IsDeepWater(loc, map);
            bool junkproof = HasTypeHook(tool, HookType.JunkProof);

            double skillBase = from.Skills[SkillName.Fishing].Base;
            double skillValue = from.Skills[SkillName.Fishing].Value;

            MutateEntry[] table = Siege.SiegeShard ? m_SiegeMutateTable : m_MutateTable;

            for (int i = 0; i < table.Length; ++i)
            {
                MutateEntry entry = m_MutateTable[i];

                // RedHerring / MudPuppy
                if (i == 6 && (from.Region == null || !from.Region.IsPartOf("Underworld")))
                    continue;

                if (junkproof && i == 5 && 0.80 >= Utility.RandomDouble())
                    continue;

                if (!deepWater && entry.m_DeepWater)
                    continue;

                if (skillBase >= entry.m_ReqSkill)
                {
                    double chance = (skillValue - entry.m_MinSkill) / (entry.m_MaxSkill - entry.m_MinSkill);

                    if (chance > Utility.RandomDouble())
                        return entry.m_Types[Utility.Random(entry.m_Types.Length)];
                }
            }

            return type;
        }

        private bool IsDeepWater(Point3D p, Map map)
        {
            return SpecialFishingNet.FullValidation(map, p.X, p.Y);
        }

        public override bool CheckResources(Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, bool timed)
        {
            Container pack = from.Backpack;

            if (pack != null)
            {
                foreach (SOS sos in pack.FindItemsByType<SOS>())
                {
					if ((from.Map == Map.Felucca || from.Map == Map.Trammel) && from.InRange(sos.TargetLocation, 60))
                        return true;
                }
            }

            return base.CheckResources(from, tool, def, map, loc, timed);
        }

        public override Item Construct(Type type, Mobile from, Item tool)
        {
            // Searing Weapon Support, handled elsewhere
            if (type == typeof(BaseWeapon))
                return null;

            if (type == typeof(TreasureMap))
            {
                return new TreasureMap(0, from.Map == Map.Felucca ? Map.Felucca : Map.Trammel);
            }
            else if (type == typeof(MessageInABottle))
            {
                return new MessageInABottle(from.Map == Map.Felucca ? Map.Felucca : Map.Trammel);
            }
            else if (type == typeof(WhitePearl))
            {
                return new WhitePearl();
            }

            Container pack = from.Backpack;

            if (pack != null)
            {
                foreach (SOS sos in pack.FindItemsByType<SOS>())
                {
					if ((from.Map == Map.Felucca || from.Map == Map.Trammel) && from.InRange(sos.TargetLocation, 60))
                    {
                        Item preLoot = null;
                        bool dredge = HasTypeHook(tool, HookType.Dredging);

                        switch (Utility.Random(17))
                        {
                            case 0: // Body parts
                            case 1:
                                {
                                    int[] list =
                                    [
	                                    0x1CDD, 0x1CE5, // arm
                                        0x1CE0, 0x1CE8, // torso
                                        0x1CE1, 0x1CE9, // head
                                        0x1CE2, 0x1CEC // leg
                                    ];

                                    preLoot = new ShipwreckedItem(Utility.RandomList(list), dredge);
                                    break;
                                }
                            case 2: // Bone parts
                            case 3:
                                {
                                    int[] list =
                                    [
	                                    0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3, 0x1AE4, // skulls
                                        0x1B09, 0x1B0A, 0x1B0B, 0x1B0C, 0x1B0D, 0x1B0E, 0x1B0F, 0x1B10, // bone piles
                                        0x1B15, 0x1B16 // pelvis bones
                                    ];

                                    preLoot = new ShipwreckedItem(Utility.RandomList(list), dredge);
                                    break;
                                }
                            case 4: // Paintings and portraits
                            case 5:
                                {
                                    preLoot = new ShipwreckedItem(Utility.Random(0xE9F, 10), dredge);
                                    break;
                                }
                            case 6: // Pillows
                            case 7:
                                {
                                    preLoot = new ShipwreckedItem(Utility.Random(0x13A4, 11), dredge);
                                    break;
                                }
                            case 8: // Shells
                            case 9:
                                {
                                    preLoot = new ShipwreckedItem(Utility.Random(0xFC4, 9), dredge);
                                    break;
                                }
                            case 10: //Hats
                            case 11:
                                {
                                    if (Utility.RandomBool())
                                        preLoot = new SkullCap();
                                    else
                                        preLoot = new TricorneHat();

                                    break;
                                }
                            case 12: // Misc
                            case 13:
                                {
                                    int[] list =
                                    [
	                                    0x1EB5, // unfinished barrel
                                        0xA2A, // stool
                                        0xC1F, // broken clock
                                        0x1047, 0x1048, // globe
                                        0x1EB1, 0x1EB2, 0x1EB3, 0x1EB4 // barrel staves
                                    ];

                                    if (Utility.Random(list.Length + 1) == 0)
                                        preLoot = new Candelabra();
                                    else
                                        preLoot = new ShipwreckedItem(Utility.RandomList(list), dredge);

                                    break;
                                }
                            #region High Seas
                            case 14:
                                {
                                    int[] list =
                                    [
	                                    0x1E19, 0x1E1A, 0x1E1B, //Fish heads
                                        0x1E2A, 0x1E2B,         //Oars
                                        0x1E71, 0x1E7A,         //Unfinished drawers
                                        0x1E75 //Unfinished legs
                                    ];

                                    double ran = Utility.RandomDouble();

                                    if (ran < 0.05)
                                        preLoot = new YellowPolkaDotBikini();
                                    else if (ran < 0.25)
                                        preLoot = new ShipwreckedItem(list[Utility.RandomMinMax(3, 7)], dredge);
                                    else
                                        preLoot = new ShipwreckedItem(list[Utility.Random(3)], dredge);
                                    break;
                                }
                                #endregion
                        }

                        if (preLoot != null)
                        {
                            if (preLoot is IShipwreckedItem item)
                                item.IsShipwreckedItem = true;

                            return preLoot;
                        }

                        LockableContainer chest;

                        if (0.01 > Utility.RandomDouble())
                        {
                            chest = new ShipsStrongbox(sos.Level);
                        }
                        else
                        {
                            switch (sos.Level)
                            {
                                case 0: chest = new SOSChest(Utility.RandomBool() ? 0xE43 : 0xE41); break;
                                case 1: chest = new SOSChest(0xA306); break;
                                case 2: chest = new SOSChest(Utility.RandomBool() ? 0xE43 : 0xE41); break;
                                case 3: chest = new SOSChest(0xA308); break;
                                default:
                                    if (.33 > Utility.RandomDouble())
                                    {
                                        chest = new SOSChest(0xA30A);
                                    }
                                    else
                                    {
                                        chest = new SOSChest(Utility.RandomBool() ? 0xE41 : 0xE43)
                                        {
                                            Hue = 0x481
                                        };
                                    }
                                    break;
                            }
                        }

                        Fill(from, chest, sos.Level);
                        sos.OnSOSComplete(chest);

                        if (sos.IsAncient)
                            chest.DropItem(new FabledFishingNet());
                        else
                            chest.DropItem(new SpecialFishingNet());

                        chest.Movable = true;
                        chest.Locked = false;
                        chest.TrapType = TrapType.None;
                        chest.TrapPower = 0;
                        chest.TrapLevel = 0;

                        chest.IsShipwreckedItem = true;
                        
                        if (from is PlayerMobile pm)
                        {
	                        pm.Statistics.SOSChestsFished.Increment(sos.Level);
                        }

                        sos.Delete();

                        return chest;
                    }
                }
            }

            return base.Construct(type, from, tool);
        }

        public override bool Give(Mobile m, Item item, bool placeAtFeet)
        {
            if (item is TreasureMap || item is MessageInABottle || item is SpecialFishingNet)
            {
                BaseCreature serp;

                if (0.25 > Utility.RandomDouble())
                    serp = new DeepSeaSerpent();
                else
                    serp = new SeaSerpent();

                int x = m.X, y = m.Y;

                Map map = m.Map;

                for (int i = 0; map != null && i < 20; ++i)
                {
                    int tx = m.X - 10 + Utility.Random(21);
                    int ty = m.Y - 10 + Utility.Random(21);

                    LandTile t = map.Tiles.GetLandTile(tx, ty);

                    if (t.Z == -5 && ((t.ID >= 0xA8 && t.ID <= 0xAB) || (t.ID >= 0x136 && t.ID <= 0x137)) && !Spells.SpellHelper.CheckMulti(new Point3D(tx, ty, -5), map))
                    {
                        x = tx;
                        y = ty;
                        break;
                    }
                }

                serp.MoveToWorld(new Point3D(x, y, -5), map);

                serp.Home = serp.Location;
                serp.RangeHome = 10;

                serp.PackItem(item);

                m.SendLocalizedMessage(503170); // Uh oh! That doesn't look like a fish!

                return true; // we don't want to give the item to the player, it's on the serpent
            }

            if (item is BigFish || item is LockableContainer)
                placeAtFeet = true;

            #region High Seas
            if (item is RareFish fish)
            {
	            if (FishInfo.IsRareFish(fish.GetType()))
                {
                    fish.Fisher = m;
                    fish.DateCaught = DateTime.Now;
                    fish.Stackable = false;

                    fish.Weight = Math.Max(1, 200 - (int)Math.Sqrt(Utility.RandomMinMax(0, 40000)));
                }
            }
            #endregion

            return base.Give(m, item, placeAtFeet);
        }

        public override void SendSuccessTo(Mobile from, Item item, HarvestResource resource)
        {
            if (item is BigFish fish)
            {
                from.SendLocalizedMessage(1042635); // Your fishing pole bends as you pull a big fish from the depths!

                fish.Fisher = from;
                fish.DateCaught = DateTime.Now;
            }

            #region Stygian Abyss
            else if (item is RedHerring)
                from.SendLocalizedMessage(1095047, null, 0x23); // You take the Red Herring and put it into your pack.  The only thing more surprising than the fact that there is a fish called the Red Herring is the fact that you fished for it!
            else if (item is MudPuppy)
                from.SendLocalizedMessage(1095064, null, 0x23); // You take the Mud Puppy and put it into your pack.  Not surprisingly, it is very muddy.
            #endregion

            else if (item is WoodenChest || item is MetalGoldenChest)
            {
                from.SendLocalizedMessage(503175); // You pull up a heavy chest from the depths of the ocean!
            }
            else
            {
                int number;
                string name;

                if (item is BaseMagicFish)
                {
                    number = 1008124;
                    name = "a mess of small fish";
                }
                else if (item is Fish)
                {
                    number = 1008124;
                    name = item.ItemData.Name;
                }
                else if (item is BaseShoes)
                {
                    number = 1008124;
                    name = item.ItemData.Name;
                }
                else if (item is TreasureMap)
                {
                    number = 1008125;
                    name = "a sodden piece of parchment";
                }
                else if (item is MessageInABottle)
                {
                    number = 1008125;
                    name = "a bottle, with a message in it";
                }
                else if (item is SpecialFishingNet)
                {
                    number = 1008125;
                    name = "a special fishing net"; // TODO: this is just a guess--what should it really be named?
                }
                else if (item is BaseHighseasFish)
                {
                    if (FishInfo.IsRareFish(item.GetType()))
                    {
                        from.SendLocalizedMessage(1043297, "a rare fish");
                    }
                    else if (item.LabelNumber < 1)
                    {
                        from.SendLocalizedMessage(1043297, "a fish");
                    }
                    else
                        from.SendLocalizedMessage(1043297, string.Format("#{0}", item.LabelNumber));

                    return;
                }
                else if (item.LabelNumber > 0)
                {
                    from.SendLocalizedMessage(1043297, string.Format("#{0}", item.LabelNumber));
                    return;
                }
                else
                {
                    number = 1043297;

                    if ((item.ItemData.Flags & TileFlag.ArticleA) != 0)
                        name = "a " + item.ItemData.Name;
                    else if ((item.ItemData.Flags & TileFlag.ArticleAn) != 0)
                        name = "an " + item.ItemData.Name;
                    else
                        name = item.ItemData.Name;
                }

                from.SendLocalizedMessage(number, name);
            }
        }

        public override void StartHarvesting(Mobile from, Item tool, object toHarvest)
        {
            if (from != null && tool != null && tool.IsChildOf(from.Backpack))
            {
                Item item = from.FindItemOnLayer(Layer.OneHanded);
                Item item2 = from.FindItemOnLayer(Layer.TwoHanded);

                if (item != null)
                    from.AddToBackpack(item);

                if (item2 != null)
                    from.AddToBackpack(item2);

                Timer.DelayCall(TimeSpan.FromMilliseconds(250), () =>
                {
                    from.EquipItem(tool);
                });
            }

            base.StartHarvesting(from, tool, toHarvest);
        }

        public override void OnHarvestStarted(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            base.OnHarvestStarted(from, tool, def, toHarvest);

            int tileID;

            if (GetHarvestDetails(from, tool, toHarvest, out tileID, out var map, out var loc))
                Timer.DelayCall(TimeSpan.FromSeconds(1.5),
                    delegate
                    {
                        from.RevealingAction();

                        int sound = 0x364;
                        int effect = 0x352D;

                        if (IsLavaHarvest(tool, toHarvest))
                        {
                            sound = 0x60A;
                            effect = 0x1A75;
                        }

                        Effects.SendLocationEffect(loc, map, effect, 16, 4);
                        Effects.PlaySound(loc, map, sound);
                    });
        }

        public override void OnHarvestFinished(Mobile from, Item tool, HarvestDefinition def, HarvestVein vein, HarvestBank bank, HarvestResource resource, object harvested)
        {
            base.OnHarvestFinished(from, tool, def, vein, bank, resource, harvested);

            from.RevealingAction();
        }

        public override object GetLock(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            return this;
        }

        public override bool BeginHarvesting(Mobile from, Item tool)
        {
            if (!base.BeginHarvesting(from, tool))
                return false;

            from.SendLocalizedMessage(500974); // What water do you want to fish in?
            return true;
        }

        public override bool CheckHarvest(Mobile from, Item tool)
        {
            if (!base.CheckHarvest(from, tool))
                return false;

            if (from.Mounted || from.Flying)
            {
                from.SendLocalizedMessage(500971); // You can't fish while riding!
                return false;
            }

            return true;
        }

        public override bool CheckHarvest(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            if (!base.CheckHarvest(from, tool, def, toHarvest))
                return false;

            if (from.Mounted || from.Flying)
            {
                from.SendLocalizedMessage(500971); // You can't fish while riding!
                return false;
            }

            return true;
        }

        private static readonly int[] m_WaterTiles =
        [
	        0x00A8, 0x00AB,
            0x0136, 0x0137,
            0x5797, 0x579C,
            0x746E, 0x7485,
            0x7490, 0x74AB,
            0x74B5, 0x75D5
        ];

        #region HighSeas
        public static int[] LavaTiles => m_LavaTiles;
		private static readonly int[] m_LavaTiles =
		[
			0x1F4, 0x1F5,
			0x1F6, 0x1F7,

			4846, 4847,
			4848, 4849,
			4850, 4852,
			4853, 4854,
			4855, 4856,
			4857, 4858,
			4859, 4560,
			4561, 4562,
			4864, 4865,
			4866, 4867,
			4868, 4870,
			4871, 4872,
			4873, 4874,
			4876, 4877,
			4878, 4879,
			4880, 4882,
			4883, 4884,
			4885, 4886,
			4888, 4889,
			4890, 4891,
			4892
		];

        public override bool GetHarvestDetails(Mobile from, Item tool, object toHarvest, out int tileID, out Map map, out Point3D loc)
        {
            bool lava = HasTypeHook(tool, HookType.Lava);

            if (toHarvest is Static @static && !@static.Movable)
            {
	            if (lava)
                    tileID = @static.ItemID;
                else
                    tileID = (@static.ItemID & 0x3FFF) | 0x4000;

                map = @static.Map;
                loc = @static.GetWorldLocation();
            }
            else if (toHarvest is StaticTarget target)
            {
	            if (lava)
                    tileID = target.ItemID;
                else
                    tileID = (target.ItemID & 0x3FFF) | 0x4000;

                map = from.Map;
                loc = target.Location;
            }
            else if (toHarvest is LandTarget harvest)
            {
	            tileID = harvest.TileID;
                map = from.Map;
                loc = harvest.Location;
            }
            else
            {
                tileID = 0;
                map = null;
                loc = Point3D.Zero;
                return false;
            }

            //Lava tile, no lava hook
            if (ValidateSpecialTile(tileID) && !lava)
                return false;

            return (map != null && map != Map.Internal);
        }

        public override HarvestDefinition GetDefinition(int tileID, Item tool)
        {
            if (tool is FishingPole pole)
            {
	            bool usingLavaHook = HasTypeHook(pole, HookType.Lava);

                if (usingLavaHook && ValidateSpecialTile(tileID))
                    return GetDefinitionFromSpecialTile(tileID);
            }

            return base.GetDefinition(tileID, tool);
        }

        public bool ValidateSpecialTile(int tileID)
        {
            foreach (int id in m_LavaTiles)
            {
                if (tileID == id)
                    return true;
            }
            return false;
        }

        public bool IsLavaHarvest(Item tool, object toHarvest)
        {
            int id = 0;

            if (!HasTypeHook(tool, HookType.Lava))
                return false;

            if (toHarvest is StaticTarget target)
                id = target.ItemID;
            else if (toHarvest is LandTarget landTarget)
                id = landTarget.TileID;
            else if (toHarvest is Static @static && !@static.Movable)
                id = @static.ItemID;

            return ValidateSpecialTile(id);
        }

        public bool IsLavaHarvest(Item tool, int id)
        {
            if (!HasTypeHook(tool, HookType.Lava))
                return false;

            return ValidateSpecialTile(id);
        }

        public override void OnToolUsed(Mobile from, Item tool, bool caughtsomething)
        {
            if (tool is FishingPole pole)
                pole.OnFishedHarvest(from, caughtsomething);
        }

        public static bool HasTypeHook(Item tool, HookType type)
        {
            if (tool is FishingPole pole)
                return pole.HookType == type;
            return false;
        }

        public override Type GetResourceType(Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource)
        {
            Type type = FishInfo.GetSpecialItem(from, tool, loc, false);

            if (type == null)
                type = base.GetResourceType(from, tool, def, map, loc, resource);

            return type;
        }

        public override void FinishHarvesting(Mobile from, Item tool, HarvestDefinition def, object toHarvest, object locked)
        {
            //Lava fishing needs to have its own set of rules.
            if (IsLavaHarvest(tool, toHarvest))
            {
                from.EndAction(locked);

                if (!CheckHarvest(from, tool))
                    return;

                if (!GetHarvestDetails(from, tool, toHarvest, out var tileID, out var map, out var loc))
                {
                    OnBadHarvestTarget(from, tool, toHarvest);
                    return;
                }
                else if (!def.Validate(tileID) && !def.ValidateSpecial(tileID))
                {
                    OnBadHarvestTarget(from, tool, toHarvest);
                    return;
                }

                if (!CheckRange(from, tool, def, map, loc, true))
                    return;
                else if (!CheckResources(from, tool, def, map, loc, true))
                    return;
                else if (!CheckHarvest(from, tool, def, toHarvest))
                    return;

                HarvestBank bank = def.GetBank(map, loc.X, loc.Y);

                if (bank == null)
                    return;

                HarvestVein vein = bank.Vein;

                if (vein == null)
                    return;

                Type type = null;

                HarvestResource resource = MutateResource(from, tool, def, map, loc, vein, vein.PrimaryResource, vein.FallbackResource);

                if (from.CheckSkill(def.Skill, resource.MinSkill, resource.MaxSkill))
                {
                    //Special eye candy item
                    type = GetSpecialLavaItem(from, tool, map, loc, toHarvest);

                    //Special fish
                    if (type == null)
                        type = FishInfo.GetSpecialItem(from, tool, loc, IsLavaHarvest(tool, tileID));

                    if (type != null)
                    {
                        Item item = Construct(type, from, tool);

                        if (item == null)
                        {
                            type = null;
                        }
                        else
                        {
                            if (from.AccessLevel < AccessLevel.Counselor)
                                bank.Consume(Convert.ToInt32(map != null && map.Rules == MapRules.FeluccaRules ? Math.Ceiling(item.Amount / 2.0) : item.Amount), from);

                            if (Give(from, item, true))
                            {
                                SendSuccessTo(from, item, null);
                            }
                            else
                            {
                                SendPackFullTo(from, item, def, null);
                                item.Delete();
                            }
                        }
                    }
                }

                if (type == null)
                {
                    def.SendMessageTo(from, def.FailMessage);

                    double skill = from.Skills[SkillName.Fishing].Value / 50;

                    if (0.5 / skill > Utility.RandomDouble())
                        OnToolUsed(from, tool, false);
                }
                else
                    OnToolUsed(from, tool, true);

                OnHarvestFinished(from, tool, def, vein, bank, null, null);
            }
            else
                base.FinishHarvesting(from, tool, def, toHarvest, locked);
        }

        public override bool CheckHarvestSkill(Map map, Point3D loc, Mobile from, HarvestResource res, HarvestDefinition def)
        {
            bool deepWater = IsDeepWater(loc, map);
            double value = from.Skills[SkillName.Fishing].Value;

            if (deepWater && value < 75.0) // can't fish here yet
                return from.Skills[def.Skill].Value >= res.ReqSkill;

            if (!deepWater && value >= 75.0) // you can fish, but no gains!
                return true;

            return base.CheckHarvestSkill(map, loc, from, res, def);
        }

        public Type GetSpecialLavaItem(Mobile from, Item type, Map map, Point3D pnt, object toHarvest)
        {
            Type newType = null;

            double skillBase = from.Skills[SkillName.Fishing].Base;
            double skillValue = Math.Min(120.0, from.Skills[SkillName.Fishing].Value);

            //Same method as mutate entries
            for (int i = 0; i < m_LavaMutateTable.Length; ++i)
            {
                MutateEntry entry = m_LavaMutateTable[i];

                if (skillBase >= entry.m_ReqSkill)
                {
                    double chance = (skillValue - entry.m_MinSkill) / (entry.m_MaxSkill - entry.m_MinSkill);

                    if (map != null && map.Rules == MapRules.FeluccaRules)
                        chance *= 1.5;

                    if (chance > Utility.RandomDouble())
                    {
                        newType = entry.m_Types[Utility.Random(entry.m_Types.Length)];

                        if (newType == typeof(BaseWeapon))
                        {
                            BaseWeapon wep = Loot.RandomWeapon();

                            if (wep != null)
                            {
                                wep.AttachSocket(new SearingWeapon(wep));

                                from.AddToBackpack(wep);
                                from.SendMessage("You have pulled out an item : mysterious weapon");
                                return typeof(BaseWeapon);
                            }
                        }
                    }
                }
            }

            return newType;
        }
        #endregion
        
        public static void Fill(Mobile from, LockableContainer cont, int level)
        {
	        level = Math.Max(1, Math.Min(4, level));
            Map map = from.Map;
            int luck = from is PlayerMobile mobile ? mobile.RealLuck : from.Luck;

            cont.Movable = false;
            cont.Locked = true;
            int count;

            cont.TrapType = TrapType.ExplosionTrap;
            cont.TrapPower = level * 25;
            cont.TrapLevel = level;

            switch (level)
            {
                case 1:
                    cont.RequiredSkill = 5;
                    break;
                case 2:
                    cont.RequiredSkill = 45;
                    break;
                case 3:
                    cont.RequiredSkill = 65;
                    break;
                case 4:
                    cont.RequiredSkill = 75;
                    break;
            }

            cont.LockLevel = cont.RequiredSkill - 10;
            cont.MaxLockLevel = cont.RequiredSkill + 40;

            cont.DropItem(new Gold(level * 50));

            switch (level)
            {
                default: count = 10; break;
                case 1: count = Utility.RandomMinMax(1, 3); break; 
                case 2: count = Utility.RandomMinMax(5, 7); break;
            }

            for (int i = 0; i < count; ++i)
                cont.DropItem(Loot.RandomScroll(0, 63, SpellbookType.Regular));

            switch (level)
            {
                case 1:
                    count = Utility.RandomMinMax(2, 6);
                    break;
                case 2:
                    count = Utility.RandomMinMax(10, 15);
                    break;
                case 3:
                    count = Utility.RandomMinMax(15, 20);
                    break;
                case 4:
                    count = Utility.RandomMinMax(15, 20);
                    break;
                default:
                    count = 0;
                    break;
            }

            for (int i = 0; i < count; ++i)
            {
                Item item = Loot.RandomArmorOrShieldOrWeaponOrJewelry();

                if (item != null)
                {
                    TreasureMapChest.GetBudgetForLevel(level, out var min, out var max);

                    RunicReforging.GenerateRandomItem(item, luck, min, max, map);

                    cont.DropItem(item);
                }
            }

            switch (level)
            {
                default: count = Utility.RandomMinMax(25, 30); break;
                case 1: count = Utility.RandomMinMax(5, 10); break;
                case 2: count = Utility.RandomMinMax(10, 20); break;
            }

            for (int i = 0; i < count; i++)
            {
                cont.DropItemStacked(Loot.RandomPossibleReagent());
            }

            count = (level * 1); 

            for (int i = 0; i < count; i++)
            {
                cont.DropItem(Loot.RandomGem());
            }

            Item arty = null;
            Item special = null;
            Item newSpecial = null;

            if (0.004 * level > Utility.RandomDouble())
                arty = Loot.Construct(m_SOSArtifacts);
            if (0.006 * level > Utility.RandomDouble())
                special = Loot.Construct(m_SOSDecor);
            else if (0.009 * level > Utility.RandomDouble())
                special = new TreasureMap(Utility.RandomMinMax(level, Math.Min(7, level + 1)), cont.Map);

            if (level >= 4)
            {
                switch (Utility.Random(4))
                {
                    case 0: newSpecial = new AncientAquariumFishNet(); break;
                    case 1: newSpecial = new LiveRock(); break;
                    case 2: newSpecial = new SaltedSerpentSteaks(); break;
                    case 3: newSpecial = new OceanSapphire(); break;
                }
            }

            if (arty != null)
            {
                Container pack = new Backpack
                {
                    Hue = 1278
                };

                pack.DropItem(arty);
                cont.DropItem(pack);
            }

            if (special != null)
                cont.DropItem(special);

            if (newSpecial != null)
                cont.DropItem(newSpecial);

            int rolls = 2;

            RefinementComponent.Roll(cont, rolls, 0.10);
        }

        private static Item GetRandomSpecial(int level, Map map)
        {
            Item special;

            switch (Utility.Random(6)) //zmien na 8 po wprowadzeniu receptur
            {
                default:
             /*   case 0: special = new CreepingVine(); break;
                case 1: special = new MessageInABottle(); break;
                case 2: special = GetRandomRecipe(); break; 
                case 3: special = new Skeletonkey(); break;
                case 4: special = new TastyTreat(5); break;
                case 5: special = new TreasureMap(Utility.RandomMinMax(level, Math.Min(7, level + 1)), map); break;
                case 6: special = GetRandomRecipe(); break;
                case 7: special = ScrollOfTranscendence.CreateRandom(1, 5); break;*/  //ODKMONETOWAĆ PO WPROWADZENIU RECPTUR
                
                case 0: special = new CreepingVine(); break;
                case 1: special = new MessageInABottle(); break;
                case 2: special = new Skeletonkey(); break;
                case 3: special = new TastyTreat(5); break;
                case 4: special = new TreasureMap(Utility.RandomMinMax(level, Math.Min(7, level + 1)), map); break;
                case 5: special = ScrollOfTranscendence.CreateRandom(1, 5); break;
            }

            return special;
        }
        
        public static Type[] SOSArtifacts => m_SOSArtifacts;
        private static readonly Type[] m_SOSArtifacts =
        [
	        typeof(AntiqueWeddingDress),
	        typeof(KelpWovenLeggings),
	        typeof(RunedDriftwoodBow),
	        typeof(ValkyrieArmor)
        ];
        public static Type[] SOSDecor => m_SOSDecor;
        private static readonly Type[] m_SOSDecor =
        [
	        typeof(GrapeVine),
	        typeof(LargeFishingNet)
        ];
    }
}
