using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Server.Nelderim
{
	public class NelderimGuardProfile
	{
		public string Name { get; }
		private double _Factor = 1.0f;
		private double _Span = 1.0f;
		private int _MaxStr, _MaxDex, _MaxInt, _Hits, _Damage;
		private string _Title;
		private string _NonHumanName;
        private GuardMode _GuardMode;
        private int _FightMode;
        private int _NonHumanBody;
        private int _NonHumanSound;
        private int _NonHumanHue;
		private List<SkillName> _Skills;
		private List<double> _SkillMaxValue;
		private int _PhysicalResistanceSeed;
		private int _FireResistSeed;
		private int _ColdResistSeed;
		private int _PoisonResistSeed;
		private int _EnergyResistSeed;
		private List<Type> _ItemType;
		private List<ushort> _ItemHue;
		private List<Type> _BackpackItem;
		private List<ushort> _BackpackItemHue;
		private List<int> _BackpackItemAmount;
		private Type _Mount;
		private int _MountHue;

		public NelderimGuardProfile(string name)
		{
			Name = name;
			_Skills = new List<SkillName>();
			_SkillMaxValue = new List<double>();
			_ItemType = new List<Type>();
			_ItemHue = new List<ushort>();
			_BackpackItem = new List<Type>();
			_BackpackItemHue = new List<ushort>();
			_BackpackItemAmount = new List<int>();

			ReadProfile(name);
		}

		private void ReadProfile(string name) {
			try
			{
				var file = Path.Combine(NelderimRegionSystem.BaseDir, "Profiles", name + ".xml");
				XmlReaderSettings settings = new XmlReaderSettings();
				settings.ValidationType = ValidationType.DTD;
				settings.IgnoreWhitespace = true;

				XmlReader xml = XmlReader.Create(file, settings);

				XmlDocument doc = new XmlDocument();
				doc.Load(xml);

				XmlElement reader;
				XmlNodeList nodes;

				nodes = doc.GetElementsByTagName("title");
				if (nodes.Count >= 1)
					_Title = (nodes.Item(0) as XmlElement).GetAttribute("value");

				nodes = doc.GetElementsByTagName("nonHuman");
				if (nodes.Count >= 1)
				{
					reader = nodes.Item(0) as XmlElement;
                    if (reader != null)
					{
						string attr;

						attr = reader.GetAttribute("body");
                        if (!String.IsNullOrEmpty(attr))
							_NonHumanBody = XmlConvert.ToInt32(attr);

						attr = reader.GetAttribute("sound");
						if (!String.IsNullOrEmpty(attr))
							_NonHumanSound = XmlConvert.ToInt32(attr);

						attr = reader.GetAttribute("hue");
						if (!String.IsNullOrEmpty(attr))
							_NonHumanHue = XmlConvert.ToInt32(attr);

                        _NonHumanName = reader.GetAttribute("name");
                    }
				}

				_FightMode = (int) FightMode.Criminal; // default

                nodes = doc.GetElementsByTagName("behavior");
				if (nodes.Count >= 1)
				{
					reader = nodes.Item(0) as XmlElement;
					if (reader != null)
					{
                        string attr;

                        attr = reader.GetAttribute("fightMode");
                        if (!String.IsNullOrEmpty(attr))
                            _FightMode = XmlConvert.ToInt32(attr);

                        var guardModeName = reader.GetAttribute("guardMode");
                        if (!Enum.TryParse(guardModeName, out _GuardMode))
                        {
	                        Console.WriteLine($"ERROR: Unable to parse guard mode for {name}");
                        }
                    }
				}


                foreach (XmlElement skill in doc.GetElementsByTagName("skill")) {
					_Skills.Add((SkillName)XmlConvert.ToInt32(skill.GetAttribute("index")));
					_SkillMaxValue.Add(XmlConvert.ToDouble(skill.GetAttribute("base")) * _Factor);
				}

				foreach (XmlElement stat in doc.GetElementsByTagName("stat"))
					switch (stat.GetAttribute("name")) {
						case "str":
							_MaxStr = (int)(XmlConvert.ToInt32(stat.GetAttribute("value")) * _Factor);
							break;
						case "dex":
							_MaxDex = (int)(XmlConvert.ToInt32(stat.GetAttribute("value")) * _Factor);
							break;
						case "int":
							_MaxInt = (int)(XmlConvert.ToInt32(stat.GetAttribute("value")) * _Factor);
							break;
					}

				reader = doc.GetElementsByTagName("hits").Item(0) as XmlElement;
				_Hits = XmlConvert.ToInt32(reader.GetAttribute("value"));


				reader = doc.GetElementsByTagName("damage").Item(0) as XmlElement;
				_Damage = (int)(XmlConvert.ToInt32(reader.GetAttribute("value")) * _Factor);


				reader = doc.GetElementsByTagName("resistances").Item(0) as XmlElement;
				_PhysicalResistanceSeed = (int)(XmlConvert.ToInt32(reader.GetAttribute("physical")) * _Factor);
				_FireResistSeed = (int)(XmlConvert.ToInt32(reader.GetAttribute("fire")) * _Factor);
				_ColdResistSeed = (int)(XmlConvert.ToInt32(reader.GetAttribute("cold")) * _Factor);
				_PoisonResistSeed = (int)(XmlConvert.ToInt32(reader.GetAttribute("poison")) * _Factor);
				_EnergyResistSeed = (int)(XmlConvert.ToInt32(reader.GetAttribute("energy")) * _Factor);

				foreach (XmlElement layer in doc.GetElementsByTagName("layer")) {
					int index = XmlConvert.ToInt32(layer.GetAttribute("index"));

					if (index != 7 && index != 5) {
						_ItemType.Add(ScriptCompiler.FindTypeByFullName(layer.GetAttribute("item")));
						_ItemHue.Add(XmlConvert.ToUInt16(layer.GetAttribute("hue")));

						if (layer.HasChildNodes && _ItemType[_ItemType.Count - 1] == typeof(Backpack)) {
							foreach (XmlElement backpackItem in layer.ChildNodes) {
								_BackpackItem.Add(ScriptCompiler.FindTypeByFullName(backpackItem.GetAttribute("type")));
								_BackpackItemHue.Add(XmlConvert.ToUInt16(backpackItem.GetAttribute("hue")));
								_BackpackItemAmount.Add(XmlConvert.ToInt32(backpackItem.GetAttribute("amount")));
							}
						}
					}
				}

				reader = doc.GetElementsByTagName("mount").Item(0) as XmlElement;

				if (!reader.HasAttribute("mounted")) {
					_Mount = ScriptCompiler.FindTypeByFullName(reader.GetAttribute("type"), false);
					_MountHue = XmlConvert.ToInt32(reader.GetAttribute("hue"));
				} else {
					_Mount = null;
					_MountHue = 0;
				}

				xml.Close();
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
		}

		private bool IsHuman()
		{
			return _NonHumanBody == 0;
		}

		public void Make(BaseNelderimGuard target) {
			foreach (Layer layer in Enum.GetValues(typeof(Layer))) {
				Item item = target.FindItemOnLayer(layer);

				if (item != null)
					item.Delete();
			}

			if (target.Mounted) {
				if (target.Mount is Mobile) {
					BaseMount mount;
					mount = (BaseMount)target.Mount;
					mount.Delete();
				} else if (target.Mount is Item) {
					Item mount;
					mount = (Item)target.Mount;
					mount.Delete();
				}
			}

			target.ActiveSpeed /= _Factor;
			target.PassiveSpeed /= _Factor;

			target.FightMode = (FightMode) _FightMode;
			target.GuardMode = _GuardMode;

			BaseCreature bc = target;

			bc.SetStr((int)(_MaxStr * _Factor * _Span), (int)(_MaxStr * _Factor));
			bc.SetDex((int)(_MaxDex * _Factor * _Span), (int)(_MaxDex * _Factor));
			bc.SetInt((int)(_MaxInt * _Factor * _Span), (int)(_MaxInt * _Factor));

			bc.SetHits((int)(_Hits * _Factor * _Span), (int)(_Hits * _Factor));

			bc.SetDamage((int)(_Damage * _Factor * _Span), (int)(_Damage * _Factor));

			bc.SetResistance(ResistanceType.Physical, (int)(_PhysicalResistanceSeed * _Factor * _Span), (int)(_PhysicalResistanceSeed * _Factor));
			bc.SetResistance(ResistanceType.Fire, (int)(_FireResistSeed * _Factor * _Span), (int)(_FireResistSeed * _Factor));
			bc.SetResistance(ResistanceType.Cold, (int)(_ColdResistSeed * _Factor * _Span), (int)(_ColdResistSeed * _Factor));
			bc.SetResistance(ResistanceType.Poison, (int)(_PoisonResistSeed * _Factor * _Span), (int)(_PoisonResistSeed * _Factor));
			bc.SetResistance(ResistanceType.Energy, (int)(_EnergyResistSeed * _Factor * _Span), (int)(_EnergyResistSeed * _Factor));

			for (int i = 0; i < _Skills.Count; i++)
				bc.SetSkill(_Skills[i], _SkillMaxValue[i] * _Factor * _Span, _SkillMaxValue[i] * _Factor);

			for (int i = 0; i < _ItemType.Count; i++) {
				Item item = (Item)Activator.CreateInstance(_ItemType[i], false);
				item.Hue = _ItemHue[i];
				item.LootType = LootType.Blessed;
				item.InvalidateProperties();
				bc.EquipItem(item);
			}

			Item backpack = bc.FindItemOnLayer(Layer.Backpack);

			if (backpack == null) {
				backpack = new Backpack();
				bc.AddItem(backpack);
			} else if (!(backpack is Container)) {
				backpack.Delete();
				backpack = new Backpack();
				bc.AddItem(backpack);
			}

			backpack.Movable = false;

			for (int i = 0; i < _BackpackItem.Count; i++) {
				Item item = (Item)Activator.CreateInstance(_BackpackItem[i], false);
				item.Hue = _BackpackItemHue[i];
				item.Amount = _BackpackItemAmount[i];

				if (item.Stackable == false)
					item.LootType = LootType.Blessed;

				item.InvalidateProperties();
				(backpack as Container).DropItem(item);
			}

			backpack.InvalidateProperties();

			if (_Mount != null && IsHuman()) {

				object someMount = Activator.CreateInstance(_Mount);

				if (someMount is BaseMount) {
					BaseMount mount = (BaseMount)someMount;
					mount.Hue = _MountHue;
					mount.Rider = target;
					mount.ControlMaster = target;
					mount.Controlled = true;
					mount.InvalidateProperties();
				} else if (someMount is EtherealMount) {
					EtherealMount mount = (EtherealMount)someMount;
					mount.Hue = _MountHue;
					mount.Rider = target;
					mount.InvalidateProperties();
				}
			}

			if (IsHuman())
            {
                target.Body = target.Female ? 401 : 400;

                target.Race.MakeRandomAppearance(target);
                target.Name = NameList.RandomName(target.Race.Name.ToLower() + "_" + (target.Female ? "female" : "male"));

                target.SpeechHue = Utility.RandomDyedHue();
                target.Title = _Title;
            }
            else
            {
                target.Name = _NonHumanName;
                target.Body = _NonHumanBody != 0 ? _NonHumanBody : 400; // sanity (Body==0 makes mobile invisible)
                target.BaseSoundID = _NonHumanSound;
                target.Hue = _NonHumanHue;
            }

			target.InvalidateProperties();
		}
	}
}
