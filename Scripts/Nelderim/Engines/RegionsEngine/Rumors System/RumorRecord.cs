using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Server.Diagnostics;

namespace Server.Nelderim
{
	public class RumorRecord : IComparable
	{
		private DateTime m_EndRumor;
		private DateTime m_StartRumor;

		public string Title { get; set; }

		public string Coppice { get; set; }

		public string Text { get; set; }

		public string KeyWord { get; set; }

		public List<string> Regions { get; }

		public List<string> ExcludedRegions { get; }

		public PriorityLevel Priority { get; set; }

		public DateTime EndRumor { get { return m_EndRumor; } set { m_EndRumor = value; } }

		public DateTime StartRumor { get { return m_StartRumor; } set { m_StartRumor = value; } }

		public Mobile RumorMobile { get; set; }

		public NewsType Type { get; set; }

		public bool Expired { get { return m_EndRumor < DateTime.Now || m_StartRumor > DateTime.Now; } }

		public RumorRecord(string title, string coppice, string text, string keyword, PriorityLevel priority,
			DateTime start, DateTime end, Mobile rumormobile, NewsType type)
		{
			Title = title;
			Coppice = coppice;
			Text = text;
			KeyWord = keyword;
			Priority = priority;
			m_StartRumor = start;
			m_EndRumor = end;
			RumorMobile = rumormobile;
			Type = type;
			Regions = new List<string>();
			ExcludedRegions = new List<string>();
		}

		public RumorRecord(RumorRecord record)
		{
			Title = record.Title + " (kopia)";
			Coppice = record.Coppice;
			Text = record.Text;
			KeyWord = record.KeyWord;
			Priority = record.Priority;
			m_StartRumor = record.StartRumor;
			m_EndRumor = record.EndRumor;
			RumorMobile = record.RumorMobile;
			Type = record.Type;
			Regions = new List<string>();
			ExcludedRegions = new List<string>();
		}

		public RumorRecord(XmlNode xNode)
		{
			Regions = new List<string>();
			ExcludedRegions = new List<string>();

			try
			{
				Title = xNode.Attributes["Title"].Value;
				if (Title == null) Title = "";
			}
			catch
			{
				Title = "";
			}

			try
			{
				Coppice = xNode.Attributes["Coppice"].Value;
				if (Coppice == null) Coppice = "";
			}
			catch
			{
				Coppice = "";
			}

			try
			{
				KeyWord = xNode.Attributes["KeyWord"].Value;
				if (KeyWord == null) KeyWord = "";
			}
			catch
			{
				KeyWord = "";
			}

			try
			{
				Text = xNode.Attributes["Text"].Value;
				if (Text == null) Text = "";
			}
			catch
			{
				Text = "";
			}

			try
			{
				int serial = -1;

				serial = Convert.ToInt32(xNode.Attributes["RumorMobile"].Value, 16);

				if (serial != -1)
					RumorMobile = World.FindMobile(new Serial(serial));
				else
					RumorMobile = null;
			}
			catch
			{
				RumorMobile = null;
			}

			try
			{
				if (xNode.ChildNodes.Count > 0)
				{
					XmlNode xRegionsName = xNode.ChildNodes[0];

					foreach (XmlNode xCom in xRegionsName.ChildNodes)
					{
						AddRegion(xCom.Attributes["Name"].Value);
					}
				}
			}
			catch
			{
			}

			try
			{
				if (xNode.ChildNodes.Count > 0)
				{
					XmlNode xRegionsName = xNode.ChildNodes[1];

					foreach (XmlNode xCom in xRegionsName.ChildNodes)
					{
						AddRegionExclude(xCom.Attributes["Name"].Value);
					}
				}
			}
			catch
			{
			}

			try
			{
				Priority = (PriorityLevel)(Int32.Parse(xNode.Attributes["Priority"].Value));
			}
			catch
			{
				Priority = PriorityLevel.None;
			}

			try
			{
				Type = (NewsType)(Int32.Parse(xNode.Attributes["Type"].Value));
			}
			catch
			{
				Type = NewsType.Rumor;
			}

			try
			{
				m_EndRumor = DateTime.ParseExact(xNode.Attributes["EndRumor"].Value, "dd.MM.yyyy HH:mm:ss",
					CultureInfo.InvariantCulture);
			}
			catch
			{
				m_EndRumor = DateTime.MinValue;
			}

			try
			{
				m_StartRumor = DateTime.ParseExact(xNode.Attributes["StartRumor"].Value, "dd.MM.yyyy HH:mm:ss",
					CultureInfo.InvariantCulture);
			}
			catch
			{
				m_StartRumor = DateTime.MinValue;
			}
		}

		public RumorRecord()
		{
			Regions = new List<string>();
			ExcludedRegions = new List<string>();
			Priority = PriorityLevel.VeryLow;
			Type = NewsType.Rumor;
		}

		public double DoublePriority(RumorType type)
		{
			double chance = 0.00;

			if (type == RumorType.BuyandSell)
			{
				switch (Priority)
				{
					case PriorityLevel.Low:
						chance = 0.01;
						break;
					case PriorityLevel.Medium:
						chance = 0.025;
						break;
					case PriorityLevel.High:
						chance = 0.05;
						break;
					case PriorityLevel.VeryHigh:
						chance = 0.1;
						break;
				}
			}
			else if (type == RumorType.Vendor)
			{
				switch (Priority)
				{
					case PriorityLevel.Medium:
						chance = 0.005;
						break;
					case PriorityLevel.High:
						chance = 0.1;
						break;
					case PriorityLevel.VeryHigh:
						chance = 0.2;
						break;
				}
			}
			else if (type == RumorType.Guard)
			{
				switch (Priority)
				{
					case PriorityLevel.High:
						chance = 0.0025;
						break;
					case PriorityLevel.VeryHigh:
						chance = 0.005;
						break;
				}
			}

			return chance;
		}

		public bool CheckRegions(string regionName)
		{
			try
			{
				foreach (string reg in Regions)
				{
					if (reg == regionName)
					{
						foreach (string reg2 in ExcludedRegions)
						{
							if (reg2 == regionName)
								return true;
						}

						return false;
					}
				}

				return true;
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}

			return false;
		}

		public void AddRegion(string regionName)
		{
			try
			{
				RegionsEngineRegion region = RegionsEngine.GetRegion(regionName);

				foreach (string reg in ExcludedRegions)
				{
					if (reg == region.Name)
					{
						ExcludedRegions.Remove(reg);
						return;
					}
				}

				foreach (string reg in Regions)
				{
					if (reg == region.Name)
						return;
				}

				// ArrayList string
				if (region != null)
				{
					Regions.Add(regionName);
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}

		public void AddRegionExclude(string regionName)
		{
			try
			{
				RegionsEngineRegion region = RegionsEngine.GetRegion(regionName);

				foreach (string reg in ExcludedRegions)
				{
					if (reg == region.Name)
						return;
				}

				// ArrayList string
				if (region != null)
				{
					ExcludedRegions.Add(regionName);
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}

		public XmlNode GetXmlNode(XmlDocument dom)
		{
			XmlNode xNode = dom.CreateElement("Rumor");

			try
			{
				XmlAttribute title = dom.CreateAttribute("Title");
				title.Value = Title;
				xNode.Attributes.Append(title);
				XmlAttribute coppice = dom.CreateAttribute("Coppice");
				coppice.Value = Coppice;
				xNode.Attributes.Append(coppice);
				XmlAttribute keyword = dom.CreateAttribute("KeyWord");
				keyword.Value = KeyWord;
				xNode.Attributes.Append(keyword);
				XmlAttribute text = dom.CreateAttribute("Text");
				text.Value = Text;
				xNode.Attributes.Append(text);
				XmlAttribute mobile = dom.CreateAttribute("RumorMobile");
				mobile.Value = RumorMobile?.Serial.ToString() ?? "";
				xNode.Attributes.Append(mobile);
			}
			catch(Exception e)
			{
				ExceptionLogging.LogException(e);
			}

			try
			{
				if (Regions.Count > 0)
				{
					XmlNode xRegions = dom.CreateElement("Regions");

					foreach (string r in Regions)
					{
						XmlNode xCom = dom.CreateElement("Region");

						XmlAttribute com = dom.CreateAttribute("Name");
						com.Value = r;
						xCom.Attributes.Append(com);

						xRegions.AppendChild(xCom);
					}

					xNode.AppendChild(xRegions);
				}

				if (Regions.Count > 0)
				{
					XmlNode xRegions = dom.CreateElement("ExcludedRegions");

					foreach (string r in ExcludedRegions)
					{
						XmlNode xCom = dom.CreateElement("Region");

						XmlAttribute com = dom.CreateAttribute("Name");
						com.Value = r;
						xCom.Attributes.Append(com);

						xRegions.AppendChild(xCom);
					}

					xNode.AppendChild(xRegions);
				}

				XmlAttribute priority = dom.CreateAttribute("Priority");
				priority.Value = ((int)Priority).ToString();
				xNode.Attributes.Append(priority);
				XmlAttribute type = dom.CreateAttribute("Type");
				type.Value = ((int)Type).ToString();
				xNode.Attributes.Append(type);
				XmlAttribute endrumor = dom.CreateAttribute("EndRumor");
				endrumor.Value = m_EndRumor.ToString("dd.MM.yyyy HH:mm:ss");
				xNode.Attributes.Append(endrumor);
				XmlAttribute startrumor = dom.CreateAttribute("StartRumor");
				startrumor.Value = m_StartRumor.ToString("dd.MM.yyyy HH:mm:ss");
				xNode.Attributes.Append(startrumor);
			}
			catch (Exception e)
			{
				ExceptionLogging.LogException(e);
			}

			return xNode;
		}

		public bool IsValid()
		{
			if (Title.Length == 0 && Type != NewsType.News)
				return false;

			if (Coppice.Length == 0 && Type == NewsType.Rumor)
				return false;

			if (KeyWord.Length == 0 && Type == NewsType.Rumor)
				return false;

			if (Text.Length < 10)
				return false;

			return true;
		}

		public int CompareTo(object o)
		{
			if (o == null)
				return 1;

			if (!(o is RumorRecord))
				throw new ArgumentException();

			RumorRecord rec = (RumorRecord)o;

			if (!Expired && IsValid() && (rec.Expired || !rec.IsValid()))
				return 1;

			if ((Expired || !IsValid()) && !rec.Expired && rec.IsValid())
				return -1;

			if (Type > rec.Type)
				return 1;

			if (Type < rec.Type)
				return -1;

			if (Priority > rec.Priority)
				return 1;

			if (Priority < rec.Priority)
				return -1;

			return m_EndRumor.CompareTo(rec.EndRumor);
		}
	}
}
