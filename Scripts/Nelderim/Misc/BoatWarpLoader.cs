using Server;
using Server.Misc;
using Server.Regions;
using System;
using System.IO;
using System.Xml;
using Server.Mobiles;

public static class BoatWarpRegionLoader
{
    public static void Initialize()
    {
        string path = Path.Combine(Core.BaseDirectory, "Data/BoatWarpRegions.xml");

        if (!File.Exists(path))
        {
            Console.WriteLine("BoatWarpRegions.xml not found.");
            return;
        }

        try
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNodeList regionNodes = doc.SelectNodes("/BoatWarpRegions/Region");

            foreach (XmlNode node in regionNodes)
            {
                string name = node.Attributes["name"].Value;
                Map map = Map.Parse(node.Attributes["map"].Value);
                int x = int.Parse(node.Attributes["x"].Value);
                int y = int.Parse(node.Attributes["y"].Value);
                int width = int.Parse(node.Attributes["width"].Value);
                int height = int.Parse(node.Attributes["height"].Value);
                int destinationIndex = int.Parse(node.Attributes["destinationIndex"].Value);
                bool showBoundaries = bool.Parse(node.Attributes["showBoundaries"].Value);

                new BoatWarpRegion(name, map, new Rectangle2D(x, y, width, height), destinationIndex, showBoundaries);
                Console.WriteLine($"[BoatWarpRegion] Registered: {name}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading BoatWarpRegions.xml: " + ex);
        }
    }
}
