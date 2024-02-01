using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Nelderim.Time;
using Server.Mobiles;

namespace Server.Engines.Tournament
{
    public class RankComparer : IComparer<TournamentCompetitorRecord>
    {
        public int Compare(TournamentCompetitorRecord l, TournamentCompetitorRecord r)
        {
            if (l == null && r == null)
                return 0;

            if (l == null)
                return 1;

            if (r == null)
                return -1;

            if (l.Status > r.Status)
                return -1;
            if (l.Status < r.Status)
                return 1;

            if (l.Place1 > r.Place1)
                return -1;
            if (l.Place1 < r.Place1)
                return 1;

            if (l.Place2 > r.Place2)
                return -1;
            if (l.Place2 < r.Place2)
                return 1;

            if (l.Place3 > r.Place3)
                return -1;
            if (l.Place3 < r.Place3)
                return 1;

            return 0;
        }
    }

    public class TournamentRecord
    {
        DateTime m_StartDate;
        DateTime m_StopDate;
        string m_ArenaName;
        string m_TournamentName;
        string m_RegionName;
        string m_RewardName;
        string m_1stName;
        string m_2ndName;
        string m_3rdName;
        string m_4thName;
        TournamentClass m_Class;

        public TournamentRecord(Tournament t)
        {
            if (t == null || t.Progress != TournamentProgress.Finished)
            {
                m_StartDate = DateTime.Now;
                m_StopDate = DateTime.Now;
                m_ArenaName = null;
                m_TournamentName = null;
                m_RegionName = null;
                m_RewardName = null;
                m_1stName = null;
                m_2ndName = null;
                m_3rdName = null;
                m_4thName = null;
                m_Class = TournamentClass.Normal;
                return;
            }

            m_StartDate = t.Owner.TournamentStart;
            m_StopDate = t.EndTime;
            m_ArenaName = t.Owner.ArenaName;
            m_TournamentName = t.Owner.TrnName;
            m_RegionName = t.Owner.Region.Name;
            m_RewardName = t.Owner.TrnRewardName;
            m_Class = t.Owner.TrnClass;

            for (int i = 1; i <= 4; i++)
            {
                Mobile m = t.GetWinner(i);
                string name = null;

                if (m != null)
                    name = m.Name;

                switch (i)
                {
                    case 1:
                        m_1stName = name;
                        break;
                    case 2:
                        m_2ndName = name;
                        break;
                    case 3:
                        m_3rdName = name;
                        break;
                    case 4:
                        m_4thName = name;
                        break;
                }
            }
        }

        public TournamentRecord(XmlElement doc)
        {
            try
            {
                XmlElement xml = doc.GetElementsByTagName("start").Item(0) as XmlElement;
                m_StartDate = new DateTime(XmlConvert.ToInt64(xml.GetAttribute("value")));

                xml = doc.GetElementsByTagName("stop").Item(0) as XmlElement;
                m_StopDate = new DateTime(XmlConvert.ToInt64(xml.GetAttribute("value")));

                xml = doc.GetElementsByTagName("arena").Item(0) as XmlElement;
                m_ArenaName = xml.GetAttribute("value");

                xml = doc.GetElementsByTagName("tournament").Item(0) as XmlElement;
                m_TournamentName = xml.GetAttribute("value");

                xml = doc.GetElementsByTagName("region").Item(0) as XmlElement;
                m_RegionName = xml.GetAttribute("value");

                try
                {
                    xml = doc.GetElementsByTagName("class").Item(0) as XmlElement;
                    m_Class = (TournamentClass)XmlConvert.ToInt32(xml.GetAttribute("value"));
                }
                catch
                {
                    m_Class = TournamentClass.Normal;
                }

                #region reward

                try
                {
                    xml = doc.GetElementsByTagName("reward").Item(0) as XmlElement;
                    m_RewardName = xml.GetAttribute("value");
                }
                catch
                {
                    m_RewardName = null;
                }

                #endregion

                #region place1

                try
                {
                    xml = doc.GetElementsByTagName("place1").Item(0) as XmlElement;
                    m_1stName = xml.GetAttribute("value");
                }
                catch
                {
                    m_1stName = null;
                }

                #endregion

                #region place2

                try
                {
                    xml = doc.GetElementsByTagName("place2").Item(0) as XmlElement;
                    m_2ndName = xml.GetAttribute("value");
                }
                catch
                {
                    m_2ndName = null;
                }

                #endregion

                #region place3

                try
                {
                    xml = doc.GetElementsByTagName("place3").Item(0) as XmlElement;
                    m_3rdName = xml.GetAttribute("value");
                }
                catch
                {
                    m_3rdName = null;
                }

                #endregion

                #region place4

                try
                {
                    xml = doc.GetElementsByTagName("place4").Item(0) as XmlElement;
                    m_4thName = xml.GetAttribute("value");
                }
                catch
                {
                    m_4thName = null;
                }

                #endregion
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());

                m_StartDate = DateTime.Now;
                m_StopDate = DateTime.Now;
                m_ArenaName = null;
                m_TournamentName = null;
                m_RegionName = null;
                m_RewardName = null;
                m_1stName = null;
                m_2ndName = null;
                m_3rdName = null;
                m_4thName = null;
            }
        }

        public override string ToString()
        {
            string output = "";

            output += m_RegionName + " - " + m_ArenaName + "\n";
            output += m_TournamentName + (m_RewardName != null ? " o trofeum - " + m_RewardName + "\n" : "\n");

            switch (m_Class)
            {
                case TournamentClass.Normal:
                    output += "Turniej klasyczny\n";
                    break;
                case TournamentClass.Masters:
                    output += "Turniej mistrzow\n";
                    break;
                case TournamentClass.Open:
                    output += "Turniej otwarty\n";
                    break;
                case TournamentClass.Private:
                    output += "Turniej prywatny\n";
                    break;
            }

            output += "Poczatek turnieju: " +
                      new NDateTime(m_StartDate).ToString(NDateTimeFormat.LongIs) + "\n";
            output += "Koniec turnieju: " + new NDateTime(m_StopDate).ToString(NDateTimeFormat.LongIs) +
                      "\n\n";
            output += "Zwyciezcy: \n";
            if (m_1stName != null)
                output += "\n#1 miejsce - " + m_1stName;
            if (m_2ndName != null)
                output += "\n#2 miejsce - " + m_2ndName;
            if (m_3rdName != null)
                output += "\n#3 miejsce - " + m_3rdName;
            if (m_4thName != null)
                output += "\n#4 miejsce - " + m_4thName;

            output += "\n";

            return output;
        }

        public void ToHTML(StreamWriter op)
        {
            try
            {
                op.Write("         <tr><td bgcolor=\"black\"><font color=\"white\">");
                op.Write(m_ArenaName);
                op.Write("</td><td bgcolor=\"black\"><font color=\"white\">");
                op.Write(m_RegionName);
                op.Write("</td><td bgcolor=\"black\"><font color=\"white\">");
                op.Write(m_TournamentName);
                op.Write("         </tr>");

                string klasa = m_Class == TournamentClass.Normal ? "Turniej Klasyczny" :
                    m_Class == TournamentClass.Masters ? "Turniej Mistrzow" :
                    m_Class == TournamentClass.Open ? "Turniej Otwarty" : "Turniej Fundowany";
                string reward = m_RewardName == null ? "trofeum nie bylo wreczane" : m_RewardName;

                op.Write("         <tr><td bgcolor=\"black\"><font color=\"white\">");
                op.Write(m_StartDate.Date);
                op.Write("</td><td bgcolor=\"black\"><font color=\"white\">");
                op.Write(klasa);
                op.Write("</td><td bgcolor=\"black\"><font color=\"white\">");
                op.Write(reward);
                op.Write("         </tr>");

                op.Write("         <tr><td bgcolor=\"black\"><font color=\"white\">");
                op.Write(1);
                op.Write("</td><td>");
                op.Write(m_1stName);
                op.Write("         </tr>");

                op.Write("         <tr><td bgcolor=\"black\"><font color=\"white\">");
                op.Write(2);
                op.Write("</td><td>");
                op.Write(m_2ndName);
                op.Write("         </tr>");

                op.Write("         <tr><td bgcolor=\"black\"><font color=\"white\">");
                op.Write(3);
                op.Write("</td><td>");
                op.Write(m_3rdName);
                op.Write("</td><td>");
                op.Write(m_4thName);
                op.Write("         </tr>");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        public bool Write(XmlTextWriter xml)
        {
            try
            {
                xml.WriteStartElement("Record");

                xml.WriteStartElement("start");
                xml.WriteAttributeString("value", m_StartDate.Ticks.ToString());
                xml.WriteEndElement();

                xml.WriteStartElement("stop");
                xml.WriteAttributeString("value", m_StopDate.Ticks.ToString());
                xml.WriteEndElement();

                xml.WriteStartElement("arena");
                xml.WriteAttributeString("value", m_ArenaName);
                xml.WriteEndElement();

                xml.WriteStartElement("tournament");
                xml.WriteAttributeString("value", m_TournamentName);
                xml.WriteEndElement();

                xml.WriteStartElement("region");
                xml.WriteAttributeString("value", m_RegionName);
                xml.WriteEndElement();

                xml.WriteStartElement("class");
                xml.WriteAttributeString("value", ((int)m_Class).ToString());
                xml.WriteEndElement();

                if (m_RewardName != null)
                {
                    xml.WriteStartElement("reward");
                    xml.WriteAttributeString("value", m_RewardName);
                    xml.WriteEndElement();
                }

                if (m_1stName != null)
                {
                    xml.WriteStartElement("place1");
                    xml.WriteAttributeString("value", m_1stName);
                    xml.WriteEndElement();
                }

                if (m_2ndName != null)
                {
                    xml.WriteStartElement("place2");
                    xml.WriteAttributeString("value", m_2ndName);
                    xml.WriteEndElement();
                }

                if (m_3rdName != null)
                {
                    xml.WriteStartElement("place3");
                    xml.WriteAttributeString("value", m_3rdName);
                    xml.WriteEndElement();
                }

                if (m_4thName != null)
                {
                    xml.WriteStartElement("place4");
                    xml.WriteAttributeString("value", m_4thName);
                    xml.WriteEndElement();
                }

                xml.WriteEndElement();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                return false;
            }

            return true;
        }
    }

    public class TournamentCompetitorRecord
    {
        private Serial m_Serial;
        private int m_Status;
        private int m_1sts;
        private int m_2nds;
        private int m_3rds;

        public Mobile Competitor
        {
            get => World.FindMobile(m_Serial);
            set
            {
                if (value == null)
                    m_Serial = new Serial();
                else
                    m_Serial = value.Serial;
            }
        }

        public int Status => m_Status;
        public int Place1 => m_1sts;
        public int Place2 => m_2nds;
        public int Place3 => m_3rds;
        public Serial Serial => m_Serial;

        public TournamentCompetitorRecord(Mobile mob, List<TournamentCompetitorRecord> list)
        {
            m_Serial = mob.Serial;
            m_Status = 0;
            m_1sts = 0;
            m_2nds = 0;
            m_3rds = 0;

            try
            {
                foreach (TournamentCompetitorRecord tcr in list)
                {
                    if (tcr.Competitor == mob)
                    {
                        m_Serial = tcr.Serial;
                        m_Status = tcr.Status;
                        m_1sts = tcr.Place1;
                        m_2nds = tcr.Place2;
                        m_3rds = tcr.Place3;
                        break;
                    }
                }
            }
            catch
            {
                Console.WriteLine("TournamentCompetitorRecord Error");
            }
        }

        public TournamentCompetitorRecord(Mobile m)
        {
            m_Serial = m.Serial;
            m_Status = 0;
            m_1sts = 0;
            m_2nds = 0;
            m_3rds = 0;
        }

        public TournamentCompetitorRecord(XmlElement doc)
        {
            try
            {
                XmlElement xml = doc.GetElementsByTagName("serial").Item(0) as XmlElement;
                m_Serial = new Serial(XmlConvert.ToInt32(xml.GetAttribute("value")));

                xml = doc.GetElementsByTagName("status").Item(0) as XmlElement;
                m_Status = XmlConvert.ToInt32(xml.GetAttribute("value"));

                xml = doc.GetElementsByTagName("place1").Item(0) as XmlElement;
                m_1sts = XmlConvert.ToInt32(xml.GetAttribute("value"));

                xml = doc.GetElementsByTagName("place2").Item(0) as XmlElement;
                m_2nds = XmlConvert.ToInt32(xml.GetAttribute("value"));

                xml = doc.GetElementsByTagName("place3").Item(0) as XmlElement;
                m_3rds = XmlConvert.ToInt32(xml.GetAttribute("value"));
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());

                m_Serial = new Serial();
                m_Status = 0;
                m_1sts = 0;
                m_2nds = 0;
                m_3rds = 0;
            }
        }

        public override string ToString()
        {
            return Competitor != null
                ? Competitor.Name + " [" + m_Status + "] (" + m_1sts + "-" + m_2nds + "-" + m_3rds + ")"
                : "brak danych";
        }

        public void ToHTML(StreamWriter op, int rank)
        {
            try
            {
                op.Write("         <tr><td bgcolor=\"black\"><font color=\"white\">");
                op.Write(rank);
                op.Write("</td><td>");
                op.Write(Competitor.Name);
                op.Write("</td><td>");
                op.Write(Status);
                op.Write("</td><td>");
                op.Write(Place1);
                op.Write("</td><td>");
                op.Write(Place2);
                op.Write("</td><td>");
                op.Write(Place3);
                op.WriteLine("</td></tr>");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        // strour - krotki turniej, gdzie za 1 runde nie dostaje sie rankow za miejsce
        public void Update(int rank, int place, bool stour)
        {
            switch (place)
            {
                case 1:
                    m_1sts++;
                    m_2nds--;
                    break;
                case 2:
                {
                    m_2nds++;
                    if (!stour)
                        m_3rds--;
                    break;
                }
                case 3:
                    m_3rds++;
                    break;
            }

            m_Status -= rank / 2;
            m_Status += rank;
        }

        public bool Write(XmlTextWriter xml)
        {
            try
            {
                xml.WriteStartElement("Competitor");

                xml.WriteStartElement("serial");
                xml.WriteAttributeString("value", m_Serial.Value.ToString());
                xml.WriteEndElement();

                xml.WriteStartElement("status");
                xml.WriteAttributeString("value", m_Status.ToString());
                xml.WriteEndElement();

                xml.WriteStartElement("place1");
                xml.WriteAttributeString("value", m_1sts.ToString());
                xml.WriteEndElement();

                xml.WriteStartElement("place2");
                xml.WriteAttributeString("value", m_2nds.ToString());
                xml.WriteEndElement();

                xml.WriteStartElement("place3");
                xml.WriteAttributeString("value", m_3rds.ToString());
                xml.WriteEndElement();

                xml.WriteEndElement();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                return false;
            }

            return true;
        }
    }

    public class TournamentStatistics
    {
        private static List<TournamentRecord> m_Records;
        private static List<TournamentCompetitorRecord> m_Competitors;

        static TournamentStatistics()
        {
            m_Records = new List<TournamentRecord>();
            m_Competitors = new List<TournamentCompetitorRecord>();
        }

        public static void Initialize()
        {
            if (!Directory.Exists("Logi/Turnieje"))
                Directory.CreateDirectory("Logi/Turnieje");

            Console.Write("Turnieje: wgrywam statystyki...");
            if (TournamentStatistics.Load())
                Console.WriteLine("OK");
            else
                Console.WriteLine("ERROR");
        }

        private static bool Load()
        {
            try
            {
                #region init

                XmlDocument doc = new XmlDocument();
                doc.Load("Data/Tournaments.xml");

                #endregion

                #region Records

                foreach (XmlElement record in doc.GetElementsByTagName("Record"))
                {
                    m_Records.Add(new TournamentRecord(record));
                }

                #endregion

                #region Competitors

                foreach (XmlElement record in doc.GetElementsByTagName("Competitor"))
                {
                    m_Competitors.Add(new TournamentCompetitorRecord(record));
                }

                #endregion

                Clean();
                WriteHTML();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                return false;
            }

            return true;
        }

        private static bool Save()
        {
            try
            {
                #region init

                XmlTextWriter xml = new XmlTextWriter("Data/Tournaments.xml", System.Text.Encoding.UTF8);
                xml.Indentation = 1;
                xml.IndentChar = '\t';
                xml.Formatting = Formatting.Indented;

                xml.WriteStartDocument(true);

                #endregion

                xml.WriteStartElement("statistics");

                #region Records

                xml.WriteStartElement("records");

                foreach (TournamentRecord tr in m_Records)
                {
                    if (!tr.Write(xml))
                    {
                        Console.WriteLine("Turniej: [stats] blad zapisu");
                        Console.WriteLine(tr.ToString());
                    }
                }

                xml.WriteEndElement();

                #endregion

                #region Competitors

                Clean();
                m_Competitors.Sort(new RankComparer());

                xml.WriteStartElement("competitors");

                foreach (TournamentCompetitorRecord tr in m_Competitors)
                {
                    if (!tr.Write(xml))
                    {
                        Console.WriteLine("Turniej: [stats] blad zapisu");
                        Console.WriteLine(tr.ToString());
                    }
                }

                xml.WriteEndElement();
                xml.WriteEndElement();

                #endregion

                xml.WriteEndDocument();
                xml.Flush();
                xml.Close();

                WriteHTML();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                return false;
            }

            return true;
        }

        public static bool UpdateRank(Mobile mob, int round, int fights, bool won, TournamentClass tclass)
        {
            if (mob == null)
                return false;

            #region Znajduje Mobile'a na liscie m_Records, jesli brak, to tworzy nowy rekord

            TournamentCompetitorRecord rec = Find(mob);

            if (rec == null)
            {
                rec = new TournamentCompetitorRecord(mob);
                m_Competitors.Add(rec);
            }

            #endregion

            #region Obliczenie rankingu

            int rounds = (int)Math.Round(Math.Log((double)(fights + 1), 2));
            int place = won ? 1 : round == rounds ? 2 : round == rounds - 1 ? 3 : 0;
            int rank = (int)(Math.Pow(2, (double)(round - 1)) * (won ? 2 : 1));

            if (tclass == TournamentClass.Masters)
                rank *= 2;
            else if (tclass == TournamentClass.Open)
                rank /= 2;

            // Console.WriteLine( "sub {3} | rounds {0} | place {1} | rank {2}", rounds, place, rank , sub );

            #endregion

            #region Update pozycji

            rec.Update(rank, place, rounds < 3);

            #endregion

            m_Competitors.Sort(new RankComparer());
            Save();

            return true;
        }

        public static bool UpdateRecords(Tournament t)
        {
            m_Records.Add(new TournamentRecord(t));
            m_Competitors.Sort(new RankComparer());

            Save();

            return true;
        }

        private static TournamentCompetitorRecord Find(Mobile mob)
        {
            foreach (TournamentCompetitorRecord tcr in m_Competitors)
            {
                if (tcr.Serial == mob.Serial)
                    return tcr;
            }

            return null;
        }

        private static void Clean()
        {
            try
            {
                for (int i = m_Competitors.Count - 1; i >= 0; i--)
                {
                    TournamentCompetitorRecord tcr = m_Competitors[i];

                    if (tcr.Competitor == null || tcr.Status < 1 || !(tcr.Competitor is PlayerMobile))
                        m_Competitors.RemoveAt(i);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private static void Sort()
        {
            try
            {
                Clean();
                m_Competitors.Sort(new RankComparer());
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        public static string PrintCompetitors()
        {
            string output = "";

            foreach (TournamentCompetitorRecord tcr in m_Competitors)
            {
                output += tcr + "\n";
            }

            return output;
        }

        public static string PrintCompetitors(int count)
        {
            string output = "";

            Sort();

            for (int i = 0; i < count && i < m_Competitors.Count; i++)
            {
                TournamentCompetitorRecord tcr = m_Competitors[i];

                output += "#" + (i + 1) + " " + tcr + "\n";
            }

            return output;
        }

        public static string PrintRecords()
        {
            string output = "";

            foreach (TournamentRecord tr in m_Records)
            {
                output += tr + "\n";
            }

            return output;
        }

        public static string PrintRecords(int count)
        {
            string output = "";

            for (int i = count >= m_Records.Count ? m_Records.Count - 1 : count - 1; i >= 0; i--)
            {
                TournamentRecord tcr = m_Records[i];

                output += tcr + "\n";
            }

            return output;
        }

        public static void Clasify(List<TournamentCompetitor> list)
        {
            List<TournamentCompetitor> tmpl = new List<TournamentCompetitor>();

            foreach (TournamentCompetitor tc in list)
            {
                Console.WriteLine(tc.Competitor.Name);
                TournamentCompetitor ntc = new TournamentCompetitor(tc.Competitor);
                ntc.Confirmed = true;
                tmpl.Add(ntc);
            }

            try
            {
                List<TournamentCompetitorRecord> internalList = new List<TournamentCompetitorRecord>();

                foreach (TournamentCompetitor tc in list)
                    internalList.Add(new TournamentCompetitorRecord(tc.Competitor, m_Competitors));

                internalList.Sort(new RankComparer());
                list.Clear();

                foreach (TournamentCompetitorRecord tcr in internalList)
                {
                    TournamentCompetitor tc = new TournamentCompetitor(tcr.Competitor);
                    tc.Confirmed = true;
                    list.Add(tc);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                list = tmpl;
            }
        }

        public static void WriteHTML()
        {
            try
            {
                using (StreamWriter op = new StreamWriter("Logi/Turnieje/ranks.html"))
                {
                    op.WriteLine("<html>");
                    op.WriteLine("   <head>");
                    op.WriteLine("      <title>Nelderim - rankingi turniejowe</title>");
                    op.WriteLine("   </head>");
                    op.WriteLine("   <body bgcolor=\"white\">");
                    op.WriteLine("      <h1>Nelderim - rankingi turniejowe (aktualizacja {0})</h1>", DateTime.Now);
                    op.WriteLine("      Calkowita liczba uczestnikow wszystkich turniejow Nelderim: {0}<br>",
                        m_Competitors.Count);
                    op.WriteLine("      <table width=\"100%\">");
                    op.WriteLine("         <tr>");
                    op.WriteLine(
                        "            <td bgcolor=\"black\"><font color=\"white\">nr</font></td><td bgcolor=\"black\"><font color=\"white\">Imie</font></td><td bgcolor=\"black\"><font color=\"white\">Punkty</font></td><td bgcolor=\"black\"><font color=\"white\">1 miejsc</font></td><td bgcolor=\"black\"><font color=\"white\">2 miejsc</font></td><td bgcolor=\"black\"><font color=\"white\">3 miejsc</font></td>");
                    op.WriteLine("         </tr>");

                    for (int i = 0; i < m_Competitors.Count; i++)
                    {
                        TournamentCompetitorRecord rec = m_Competitors[i];
                        rec.ToHTML(op, i + 1);
                    }

                    op.WriteLine("      </table>");
                    op.WriteLine("   </body>");
                    op.WriteLine("</html>");
                }

                using (StreamWriter op = new StreamWriter("Logi/Turnieje/history.html"))
                {
                    op.WriteLine("<html>");
                    op.WriteLine("   <head>");
                    op.WriteLine("      <title>Nelderim - historia turniejow</title>");
                    op.WriteLine("   </head>");
                    op.WriteLine("   <body bgcolor=\"white\">");
                    op.WriteLine("      <h1>Nelderim - historia turniejow (aktualizacja {0})</h1>", DateTime.Now);
                    op.WriteLine("      Calkowita liczba zakonczonych turniejow Nelderim: {0}<br>", m_Records.Count);
                    op.WriteLine("      <table width=\"100%\">");

                    for (int i = m_Records.Count - 1; i >= 0; i--)
                    {
                        TournamentRecord rec = m_Records[i];
                        rec.ToHTML(op);
                    }

                    op.WriteLine("      </table>");
                    op.WriteLine("   </body>");
                    op.WriteLine("</html>");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
