using Server.Commands;
using Server.Engines.Quests;
using Server.Mobiles;
using System;

namespace Server.Items
{
    public static class GenerateHighSeas
    {
        public static void Initialize()
        {
            CommandSystem.Register("DecorateHS", AccessLevel.Administrator, GenerateDeco);
            CommandSystem.Register("DeleteHS", AccessLevel.Administrator, DeleteHS);

            CommandSystem.Register("CharydbisSpawner", AccessLevel.Administrator, Spawner);
        }

        public static void Spawner(CommandEventArgs e)
        {
            if (CharydbisSpawner.SpawnInstance == null)
                e.Mobile.SendMessage("Charydbis spawner does not exist.");
            else
                e.Mobile.SendGump(new Gumps.PropertiesGump(e.Mobile, CharydbisSpawner.SpawnInstance));
        }

        public static void DeleteHS(CommandEventArgs e)
        {
            WeakEntityCollection.Delete("highseas");

            if (CharydbisSpawner.SpawnInstance != null)
                CharydbisSpawner.SpawnInstance.Active = false;

            if (BountyQuestSpawner.Instance != null)
                BountyQuestSpawner.Instance.Active = false;
        }

        public static void GenerateDeco(CommandEventArgs e)
        {
            string name = "highseas";

            CharydbisSpawner.GenerateCharydbisSpawner();
            BountyQuestSpawner.GenerateShipSpawner();

            CorgulAltar altar;

            altar = new CorgulAltar();
            altar.MoveToWorld(new Point3D(3830, 1908, 0), Map.Felucca);
            WeakEntityCollection.Add(name, altar);

            //altar = new CorgulAltar();
           // altar.MoveToWorld(new Point3D(2453, 865, 0), Map.Trammel);
           // WeakEntityCollection.Add(name, altar);

            ProfessionalBountyBoard board;

            //board = new ProfessionalBountyBoard();
           // board.MoveToWorld(new Point3D(4544, 2298, -1), Map.Trammel);
           // WeakEntityCollection.Add(name, board);

            board = new ProfessionalBountyBoard();
            board.MoveToWorld(new Point3D(1556, 1823, 0), Map.Felucca);
            WeakEntityCollection.Add(name, board);
/*
            LocalizedSign sign;

            sign = new LocalizedSign(3025, 1152653); //The port of Zento Parking Area
            sign.MoveToWorld(new Point3D(713, 1359, 53), Map.Tokuno);
            WeakEntityCollection.Add(name, sign);

            sign = new LocalizedSign(3023, 1149821); //Winds Tavern
            sign.MoveToWorld(new Point3D(4548, 2300, -6), Map.Trammel);
            WeakEntityCollection.Add(name, sign);

            sign = new LocalizedSign(3023, 1149821); //Winds Tavern
            sign.MoveToWorld(new Point3D(4548, 2300, -6), Map.Felucca);
            WeakEntityCollection.Add(name, sign);

            sign = new LocalizedSign(3023, 1149820); //General Store
            sign.MoveToWorld(new Point3D(4543, 2317, -3), Map.Trammel);
            WeakEntityCollection.Add(name, sign);

            sign = new LocalizedSign(3023, 1149820); //General Store
            sign.MoveToWorld(new Point3D(4543, 2317, -3), Map.Felucca);
            WeakEntityCollection.Add(name, sign);*/

            XmlSpawner sp;
            string toSpawn = "FishMonger";

            //Tasadnora
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            //sp.MoveToWorld(new Point3D(1482, 1754, -2), Map.Trammel);
           // sp.Respawn();
           // WeakEntityCollection.Add(name, sp);

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1544, 1829, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Orod
           /* sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(622, 2084, 0), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(622, 2084, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Tirassa
            /*sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(2061, 2855, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(2051, 2770, -5), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Ferion
            /*sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(3009, 826, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(985, 1084, 1), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Garlan
            /*sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1373, 3885, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(933, 600, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //ArtTrader
            /*sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(641, 2234, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn);
            sp.MoveToWorld(new Point3D(1531, 1515, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Lotharn
           /* sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(5827, 3258, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1870, 580, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Podmrok_C
            /*sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 0,
                HomeRange = 0
            };
            sp.MoveToWorld(new Point3D(4552, 2299, -1), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 0,
                HomeRange = 0
            };
            sp.MoveToWorld(new Point3D(5653, 3594, 10), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);
            
            toSpawn = "SeaMarketOfficer";
            //Tasadnora
            sp = new XmlSpawner(toSpawn)
            {
	            SpawnRange = 3,
	            HomeRange = 6
            };
            //sp.MoveToWorld(new Point3D(1482, 1754, -2), Map.Trammel);
            // sp.Respawn();
            // WeakEntityCollection.Add(name, sp);

            toSpawn = "DocksAlchemist";

                        //Tasadnora
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            //sp.MoveToWorld(new Point3D(1482, 1754, -2), Map.Trammel);
           // sp.Respawn();
           // WeakEntityCollection.Add(name, sp);

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1544, 1829, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            sp = new XmlSpawner(toSpawn)
            {
	            SpawnRange = 1,
	            HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1544, 1829, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Orod
           /* sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(622, 2084, 0), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(622, 2084, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Tirassa
            /*sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(2061, 2855, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(2051, 2770, -5), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Ferion
            /*sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(3009, 826, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(985, 1084, 1), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Garlan
            /*sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1373, 3885, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(933, 600, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //ArtTrader
            /*sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(641, 2234, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn);
            sp.MoveToWorld(new Point3D(1531, 1515, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Lotharn
           /* sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(5827, 3258, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1870, 580, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Podmrok_C
            /*sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 0,
                HomeRange = 0
            };
            sp.MoveToWorld(new Point3D(4552, 2299, -1), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 0,
                HomeRange = 0
            };
            sp.MoveToWorld(new Point3D(5653, 3594, 10), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            toSpawn = "GBBigglesby";

           /* sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1559, 1853, 0), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1559, 1853, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            toSpawn = "GBBigglesby/Name/Stary Wichura/Title/- wlasciciel akcji w Kompanii Handlowej";

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 10
            };
            sp.MoveToWorld(new Point3D(1542, 1872, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

          /*  toSpawn = "BoatPainter";

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 15
            };
            sp.MoveToWorld(new Point3D(4552, 2337, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 15
            };
            sp.MoveToWorld(new Point3D(4552, 2337, -2), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            toSpawn = "Banker";

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(4554, 2315, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(4554, 2315, -2), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            toSpawn = "CrabFisher";

            /*sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 15
            };
            sp.MoveToWorld(new Point3D(4552, 2336, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 15
            };
            sp.MoveToWorld(new Point3D(4552, 2336, -2), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 15
            };
            sp.MoveToWorld(new Point3D(4552, 2378, -2), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 15
            };
            sp.MoveToWorld(new Point3D(1544, 1807, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

           /* toSpawn = "DockMaster";

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 10
            };
            sp.MoveToWorld(new Point3D(1530, 1852, 0), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 10
            };
            sp.MoveToWorld(new Point3D(4565, 2307, -2), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            toSpawn = "SeaMarketTavernKeeper";

            /*sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(4544, 2302, -1), Map.Trammel);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);*/

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1573, 1821, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            SeaMarketBuoy bouy1 = new SeaMarketBuoy();
            SeaMarketBuoy bouy2 = new SeaMarketBuoy();
            SeaMarketBuoy bouy3 = new SeaMarketBuoy();
            SeaMarketBuoy bouy4 = new SeaMarketBuoy();
            SeaMarketBuoy bouy5 = new SeaMarketBuoy();
            SeaMarketBuoy bouy6 = new SeaMarketBuoy();
            SeaMarketBuoy bouy7 = new SeaMarketBuoy();
            SeaMarketBuoy bouy8 = new SeaMarketBuoy();

            Rectangle2D bound = Regions.SeaMarketRegion.MarketBounds[0];

            bouy1.MoveToWorld(new Point3D(bound.X, bound.Y, -5), Map.Felucca);
            //bouy2.MoveToWorld(new Point3D(bound.X, bound.Y, -5), Map.Trammel);
            WeakEntityCollection.Add(name, bouy1);
           // WeakEntityCollection.Add(name, bouy2);

            bouy3.MoveToWorld(new Point3D(bound.X + bound.Width, bound.Y, -5), Map.Felucca);
           // bouy4.MoveToWorld(new Point3D(bound.X + bound.Width, bound.Y, -5), Map.Trammel);
            WeakEntityCollection.Add(name, bouy3);
           // WeakEntityCollection.Add(name, bouy4);

            bouy5.MoveToWorld(new Point3D(bound.X + bound.Width, bound.Y + bound.Height, -5), Map.Felucca);
           // bouy6.MoveToWorld(new Point3D(bound.X + bound.Width, bound.Y + bound.Height, -5), Map.Trammel);
            WeakEntityCollection.Add(name, bouy5);
            //WeakEntityCollection.Add(name, bouy6);

            bouy7.MoveToWorld(new Point3D(bound.X, bound.Y + bound.Height, -5), Map.Felucca);
            //bouy8.MoveToWorld(new Point3D(bound.X, bound.Y + bound.Height, -5), Map.Trammel);
            WeakEntityCollection.Add(name, bouy7);
            //WeakEntityCollection.Add(name, bouy8);

            Console.WriteLine("High Seas Content generated.");
        }
    }
}
