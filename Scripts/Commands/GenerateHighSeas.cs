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

            ProfessionalBountyBoard board;

            board = new ProfessionalBountyBoard();
            board.MoveToWorld(new Point3D(1556, 1823, 0), Map.Felucca);
            WeakEntityCollection.Add(name, board);

            XmlSpawner sp;
            string toSpawn = "FishMonger";

            //Tasandora
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1544, 1829, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Orod
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(622, 2084, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Tirassa
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(2051, 2770, -5), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Ferion
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(985, 1084, 1), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Garlan
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(933, 600, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //ArtTrader
            sp = new XmlSpawner(toSpawn);
            sp.MoveToWorld(new Point3D(1531, 1515, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Lotharn

            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1870, 580, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Podmrok_C
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
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1544, 1829, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);
            
            //Orod
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(622, 2084, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Tirassa
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(2051, 2770, -5), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Ferion
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(985, 1084, 1), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Garlan
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(933, 600, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //ArtTrader
            sp = new XmlSpawner(toSpawn)
            {
	            SpawnRange = 1,
	            HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1531, 1515, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Lotharn
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 5
            };
            sp.MoveToWorld(new Point3D(1870, 580, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            //Podmrok_C
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 0,
                HomeRange = 0
            };
            sp.MoveToWorld(new Point3D(5653, 3594, 10), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            toSpawn = "GBBigglesby/Name/Stary Wichura/Title/- wlasciciel akcji w Kompanii Handlowej";
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 10
            };
            sp.MoveToWorld(new Point3D(1542, 1872, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            toSpawn = "CrabFisher";
            sp = new XmlSpawner(toSpawn)
            {
                SpawnRange = 1,
                HomeRange = 15
            };
            sp.MoveToWorld(new Point3D(1544, 1807, 0), Map.Felucca);
            sp.Respawn();
            WeakEntityCollection.Add(name, sp);

            toSpawn = "SeaMarketTavernKeeper";
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

            Rectangle2D bound = Regions.SeaMarketRegion.MarketBounds[0];

            bouy1.MoveToWorld(new Point3D(bound.X, bound.Y, -5), Map.Felucca);
            WeakEntityCollection.Add(name, bouy1);

            bouy2.MoveToWorld(new Point3D(bound.X + bound.Width, bound.Y, -5), Map.Felucca);
            WeakEntityCollection.Add(name, bouy2);

            bouy3.MoveToWorld(new Point3D(bound.X + bound.Width, bound.Y + bound.Height, -5), Map.Felucca);
            WeakEntityCollection.Add(name, bouy3);

            bouy4.MoveToWorld(new Point3D(bound.X, bound.Y + bound.Height, -5), Map.Felucca);
            WeakEntityCollection.Add(name, bouy4);

            Console.WriteLine("High Seas Content generated.");
        }
    }
}
