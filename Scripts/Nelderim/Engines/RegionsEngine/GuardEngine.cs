#region References

using System;
using System.Collections;
using System.Xml;
using Nelderim.Races;
using Server.Items;
using Server.Mobiles;

#endregion

namespace Server.Nelderim
{
	public class GuardEngine
	{
		#region Pola

		private int m_MaxStr, m_MaxDex, m_MaxInt, m_Hits, m_Damage;
		private string m_Title;
		private readonly ArrayList m_Skills;
		private readonly ArrayList m_SkillMaxValue;
		private int m_PhysicalResistanceSeed;
		private int m_FireResistSeed;
		private int m_ColdResistSeed;
		private int m_PoisonResistSeed;
		private int m_EnergyResistSeed;
		private readonly ArrayList m_ItemType;
		private readonly ArrayList m_ItemHue;
		private readonly ArrayList m_BackpackItem;
		private readonly ArrayList m_BackpackItemHue;
		private readonly ArrayList m_BackpackItemAmount;
		private Type m_Mount;
		private int m_MountHue;

		public string FileName;
		public double Factor { get; }
		public double Span { get; }
		public double Female { get; }
		public int[] Races { get; }

		#endregion

		public GuardEngine(string file, double factor, double span, double female, int[] races)
		{
			Factor = factor;
			Span = span;
			Female = female;
			Races = races;
			m_Skills = new ArrayList();
			m_SkillMaxValue = new ArrayList();
			m_ItemType = new ArrayList();
			m_ItemHue = new ArrayList();
			m_BackpackItem = new ArrayList();
			m_BackpackItemHue = new ArrayList();
			m_BackpackItemAmount = new ArrayList();

			int cutFrom = file.LastIndexOf("/") + 1;
			int cutTo = file.IndexOf(".");

			FileName = file.Substring(cutFrom, cutTo - cutFrom);

			// Console.WriteLine( file );
			// Console.WriteLine( FileName );

			ReadProfile(file);
		}

		private void ReadProfile(string file)
		{
			// Console.WriteLine( "{0}", file );
			try
			{
				#region init

				XmlReaderSettings settings = new XmlReaderSettings();
				settings.ValidationType = ValidationType.DTD;
				settings.IgnoreWhitespace = true;
				//settings.ValidationEventHandler += new ValidationEventHandler( ValidationCallBack );

				XmlReader xml = XmlReader.Create(file, settings); //XmlTextReader( file );

				//xml.WhitespaceHandling = WhitespaceHandling.None;

				//XmlValidatingReader validXML = new XmlValidatingReader(xml);
				//validXML.ValidationType = ValidationType.DTD;

				XmlDocument doc = new XmlDocument();
				doc.Load(xml);

				#endregion

				#region base

				XmlElement reader;

				reader = doc.GetElementsByTagName("title").Item(0) as XmlElement;
				m_Title = reader.GetAttribute("value");

				#endregion

				#region skills

				foreach (XmlElement skill in doc.GetElementsByTagName("skill"))
				{
					m_Skills.Add((SkillName)XmlConvert.ToInt32(skill.GetAttribute("index")));
					m_SkillMaxValue.Add(XmlConvert.ToDouble(skill.GetAttribute("base")) * Factor);
				}

				#endregion

				#region stats

				foreach (XmlElement stat in doc.GetElementsByTagName("stat"))
					switch (stat.GetAttribute("name"))
					{
						case "str":
							m_MaxStr = (int)(XmlConvert.ToInt32(stat.GetAttribute("value")) * Factor);
							break;
						case "dex":
							m_MaxDex = (int)(XmlConvert.ToInt32(stat.GetAttribute("value")) * Factor);
							break;
						case "int":
							m_MaxInt = (int)(XmlConvert.ToInt32(stat.GetAttribute("value")) * Factor);
							break;
					}

				reader = doc.GetElementsByTagName("hits").Item(0) as XmlElement;
				m_Hits = XmlConvert.ToInt32(reader.GetAttribute("value"));


				reader = doc.GetElementsByTagName("damage").Item(0) as XmlElement;
				m_Damage = (int)(XmlConvert.ToInt32(reader.GetAttribute("value")) * Factor);

				#endregion

				#region resistances

				reader = doc.GetElementsByTagName("resistances").Item(0) as XmlElement;
				m_PhysicalResistanceSeed = (int)(XmlConvert.ToInt32(reader.GetAttribute("physical")) * Factor);
				m_FireResistSeed = (int)(XmlConvert.ToInt32(reader.GetAttribute("fire")) * Factor);
				m_ColdResistSeed = (int)(XmlConvert.ToInt32(reader.GetAttribute("cold")) * Factor);
				m_PoisonResistSeed = (int)(XmlConvert.ToInt32(reader.GetAttribute("poison")) * Factor);
				m_EnergyResistSeed = (int)(XmlConvert.ToInt32(reader.GetAttribute("energy")) * Factor);

				#endregion

				#region equipment

				foreach (XmlElement layer in doc.GetElementsByTagName("layer"))
				{
					int index = XmlConvert.ToInt32(layer.GetAttribute("index"));

					if (index != 7 && index != 5)
					{
						m_ItemType.Add(layer.GetAttribute("item"));
						m_ItemHue.Add(XmlConvert.ToInt32(layer.GetAttribute("hue")));

						if (layer.HasChildNodes && (string)m_ItemType[m_ItemType.Count - 1] == "Server.Items.Backpack")
						{
							// Console.WriteLine( "Backpack!" );

							foreach (XmlElement backpackItem in layer.ChildNodes)
							{
								m_BackpackItem.Add(backpackItem.GetAttribute("type"));
								m_BackpackItemHue.Add(XmlConvert.ToInt32(backpackItem.GetAttribute("hue")));
								m_BackpackItemAmount.Add(XmlConvert.ToInt32(backpackItem.GetAttribute("amount")));
							}
						}
					}
				}

				#endregion

				#region mount

				//Console.WriteLine( "mount" );	
				reader = doc.GetElementsByTagName("mount").Item(0) as XmlElement;

				if (!reader.HasAttribute("mounted"))
				{
					m_Mount = ScriptCompiler.FindTypeByFullName(reader.GetAttribute("type"), false);
					m_MountHue = XmlConvert.ToInt32(reader.GetAttribute("hue"));
				}
				else
				{
					m_Mount = null;
					m_MountHue = 0;
				}

				#endregion

				xml.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		public void Make(BaseNelderimGuard target)
		{
			#region czyscimy istniejacy ekwipunek i konie

			foreach (Layer layer in Enum.GetValues(typeof(Layer)))
			{
				Item item = target.FindItemOnLayer(layer);

				if (item != null)
					item.Delete();
			}

			if (target.Mounted)
			{
				if (target.Mount is Mobile)
				{
					BaseMount mount;
					mount = (BaseMount)target.Mount;
					mount.Delete();
				}
				else if (target.Mount is Item)
				{
					Item mount;
					mount = (Item)target.Mount;
					mount.Delete();
				}
			}

			#endregion

			#region poprawiamy bazowe wartosci

			target.ActiveSpeed /= Factor;
			target.PassiveSpeed /= Factor;

			#endregion


			#region statystyki

			BaseCreature bc = target;

			bc.SetStr((int)(m_MaxStr * Factor * Span), (int)(m_MaxStr * Factor));
			bc.SetDex((int)(m_MaxDex * Factor * Span), (int)(m_MaxDex * Factor));
			bc.SetInt((int)(m_MaxInt * Factor * Span), (int)(m_MaxInt * Factor));

			bc.SetHits((int)(m_Hits * Factor * Span), (int)(m_Hits * Factor));

			bc.SetDamage((int)(m_Damage * Factor * Span), (int)(m_Damage * Factor));

			#endregion

			#region odpornosci

			bc.SetResistance(ResistanceType.Physical, (int)(m_PhysicalResistanceSeed * Factor * Span),
				(int)(m_PhysicalResistanceSeed * Factor));
			bc.SetResistance(ResistanceType.Fire, (int)(m_FireResistSeed * Factor * Span),
				(int)(m_FireResistSeed * Factor));
			bc.SetResistance(ResistanceType.Cold, (int)(m_ColdResistSeed * Factor * Span),
				(int)(m_ColdResistSeed * Factor));
			bc.SetResistance(ResistanceType.Poison, (int)(m_PoisonResistSeed * Factor * Span),
				(int)(m_PoisonResistSeed * Factor));
			bc.SetResistance(ResistanceType.Energy, (int)(m_EnergyResistSeed * Factor * Span),
				(int)(m_EnergyResistSeed * Factor));

			#endregion

			#region skille

			for (int i = 0; i < m_Skills.Count; i++)
				bc.SetSkill((SkillName)m_Skills[i], (double)m_SkillMaxValue[i] * Factor * Span,
					(double)m_SkillMaxValue[i] * Factor);

			#endregion

			#region przedmioty

			for (int i = 0; i < m_ItemType.Count; i++)
			{
				Item item = (Item)Activator.CreateInstance(
					ScriptCompiler.FindTypeByFullName(m_ItemType[i] as string, false));
				item.Hue = (int)m_ItemHue[i];
				item.LootType = LootType.Blessed;
				item.InvalidateProperties();
				bc.EquipItem(item);
			}

			Item backpack = bc.FindItemOnLayer(Layer.Backpack);

			if (backpack == null)
			{
				backpack = new Backpack();
				bc.AddItem(backpack);
			}
			else if (!(backpack is Container))
			{
				backpack.Delete();
				backpack = new Backpack();
				bc.AddItem(backpack);
			}

			backpack.Movable = false;

			for (int i = 0; i < m_BackpackItem.Count; i++)
			{
				Item item = (Item)Activator.CreateInstance(
					ScriptCompiler.FindTypeByFullName(m_BackpackItem[i] as string, false));
				item.Hue = (int)m_BackpackItemHue[i];
				item.Amount = (int)m_BackpackItemAmount[i];

				if (item.Stackable == false)
					item.LootType = LootType.Blessed;

				item.InvalidateProperties();
				(backpack as Container).DropItem(item);
			}

			backpack.InvalidateProperties();

			#endregion

			#region mount

			if (m_Mount != null)
			{
				object someMount = Activator.CreateInstance(m_Mount);

				if (someMount is BaseMount)
				{
					BaseMount mount = (BaseMount)someMount;
					mount.Hue = m_MountHue;
					mount.Rider = target;
					mount.ControlMaster = target;
					mount.Controlled = true;
					mount.InvalidateProperties();
				}
				else if (someMount is EtherealMount)
				{
					EtherealMount mount = (EtherealMount)someMount;
					mount.Hue = m_MountHue;
					mount.Rider = target;
					mount.InvalidateProperties();
				}
			}

			#endregion

			#region inicjalizacja podstawowych pol

			int rand = Utility.Random(0, 99);
			int cumsum = 0, index = 0;

			Mobile mob = target;

			mob.Female = (Utility.RandomDouble() < Female) ? true : false;
			mob.Body = (mob.Female) ? 401 : 400;

			for (int i = 0; i < NRace.AllRaces.Count; i++)
			{
				if ((cumsum += Races[i]) > rand)
				{
					index = i;
					break;
				}
			}

			Race guardRace = NRace.AllRaces[index];
			mob.Race = guardRace;
			guardRace.MakeRandomAppearance(mob);
			mob.Name = NameList.RandomName(guardRace.Name.ToLower() + "_" + (mob.Female ? "female" : "male"));

			mob.SpeechHue = Utility.RandomDyedHue();
			mob.Title = m_Title;

			#endregion

			mob.InvalidateProperties();
		}
	}
}
