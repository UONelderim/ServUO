#region References

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server.Gumps;
using Server.Network;

#endregion

namespace Server.ACC.CM
{
	public enum Pages
	{
		Home,
		ListMobiles,
		ListItems,
		ListModulesFor
	}

	public class CMGumpParams : ACCGumpParams
	{
		public Pages PageName;
		public Serial SelectedSerial;
		public int PageIndex;
	}

	public class CentralMemory : ACCSystem
	{
		internal static List<Type> m_Types = new List<Type>();

		public static Dictionary<Serial, ModuleList> DictionaryOfModuleLists { get; private set; } =
			new Dictionary<Serial, ModuleList>();

		public static void Configure()
		{
			ACC.RegisterSystem("Server.ACC.CM.CentralMemory");
		}

		public static bool Running
		{
			get { return ACC.SysEnabled("Server.ACC.CM.CentralMemory"); }
		}

		#region BasicOverrides

		public override string Name() { return "Central Memory"; }

		public override void Enable()
		{
			Console.WriteLine("{0} has just been enabled!", Name());
		}

		public override void Disable()
		{
			Console.WriteLine("{0} has just been disabled!", Name());
		}

		#endregion //BasicOverrides

		#region Methods

		public static void Flush()
		{
			List<Serial> RemoveList = new List<Serial>();

			foreach (Serial s in DictionaryOfModuleLists.Keys)
			{
				if (s.IsItem)
				{
					Item i = World.FindItem(s);
					if (i == null || i.Deleted)
						RemoveList.Add(s);
				}
				else if (s.IsMobile)
				{
					Mobile m = World.FindMobile(s);
					if (m == null || m.Deleted)
						RemoveList.Add(s);
				}

				if (DictionaryOfModuleLists[s].Count == 0)
					RemoveList.Add(s);
			}

			foreach (Serial s in RemoveList)
			{
				Remove(s);
			}

			RemoveList.Clear();

			Console.Write("Flushed...");
		}

		public static bool Contains(Serial ser)
		{
			return DictionaryOfModuleLists.ContainsKey(ser);
		}

		public static bool ContainsModule(Serial ser, Type type)
		{
			if (Contains(ser))
			{
				if (DictionaryOfModuleLists[ser] != null)
					return DictionaryOfModuleLists[ser].Contains(type);
			}

			return false;
		}

		public static void Add(Serial ser)
		{
			if (Contains(ser))
				return;

			DictionaryOfModuleLists.Add(ser, new ModuleList(ser));
		}

		public static void Add(Serial ser, ModuleList list)
		{
			DictionaryOfModuleLists[ser] = list;
		}

		public static void AddModule(Module mod)
		{
			if (!DictionaryOfModuleLists.ContainsKey(mod.Owner))
				Add(mod.Owner);

			DictionaryOfModuleLists[mod.Owner].Add(mod);
		}

		public static void AddModule(Serial ser, Type type)
		{
			if (!DictionaryOfModuleLists.ContainsKey(ser))
				Add(ser);

			DictionaryOfModuleLists[ser].Add(type);
		}

		public static void ChangeModule(Serial ser, Module mod)
		{
			if (!DictionaryOfModuleLists.ContainsKey(ser))
				Add(ser);

			DictionaryOfModuleLists[ser].Change(mod);
		}

		public static void AppendModule(Serial ser, Module mod, bool negatively)
		{
			if (!DictionaryOfModuleLists.ContainsKey(ser))
				Add(ser);
			else if (!ContainsModule(ser, mod.GetType()))
			{
				AddModule(mod);
			}
			else
			{
				DictionaryOfModuleLists[ser].Append(mod, negatively);
			}
		}

		public static void Remove(Serial ser)
		{
			DictionaryOfModuleLists.Remove(ser);
		}

		public static void RemoveModule(Serial ser, Module mod)
		{
			if (DictionaryOfModuleLists.ContainsKey(ser))
				DictionaryOfModuleLists[ser].Remove(mod);
		}

		public static void RemoveModule(Serial ser, Type type)
		{
			if (DictionaryOfModuleLists.ContainsKey(ser))
				DictionaryOfModuleLists[ser].Remove(type);
		}

		public static Module GetModule(Serial ser, Type type)
		{
			if (DictionaryOfModuleLists.ContainsKey(ser))
				return DictionaryOfModuleLists[ser].Get(type);

			return null;
		}

		public static List<Mobile> GetMobiles()
		{
			List<Mobile> list = new List<Mobile>();
			foreach (Serial s in DictionaryOfModuleLists.Keys)
			{
				if (s.IsMobile)
				{
					Mobile m = World.FindMobile(s);
					if (m != null && !m.Deleted)
						list.Add(m);
				}
			}

			return list;
		}

		public static List<Item> GetItems()
		{
			List<Item> list = new List<Item>();
			foreach (Serial s in DictionaryOfModuleLists.Keys)
			{
				if (s.IsItem)
				{
					Item i = World.FindItem(s);
					if (i != null && !i.Deleted)
						list.Add(i);
				}
			}

			return list;
		}

		#endregion //Methods

		private CMGumpParams Params;
		private List<Mobile> m_MobileList;
		private List<Item> m_ItemList;
		private List<Module> m_ModuleList;

		#region Gump

		public override void Gump(Mobile from, Gump gump, ACCGumpParams subParams)
		{
			gump.AddButton(190, 40, 2445, 2445, 101, GumpButtonType.Reply, 0);
			gump.AddLabel(204, 42, 1153, "List Mobiles");
			gump.AddButton(310, 40, 2445, 2445, 102, GumpButtonType.Reply, 0);
			gump.AddLabel(331, 42, 1153, "List Items");
			gump.AddButton(430, 40, 2445, 2445, 103, GumpButtonType.Reply, 0);
			gump.AddLabel(450, 42, 1153, "List Types");
			//			gump.AddButton( 190, 70, 2445, 2445, 104, GumpButtonType.Reply, 0 );
			//			gump.AddLabel(  208, 72, 1153, "Add Module" );
			//			gump.AddButton( 310, 70, 2445, 2445, 105, GumpButtonType.Reply, 0 );
			//			gump.AddLabel(  326, 72, 1153, "Edit Module" );
			//			gump.AddButton( 430, 70, 2445, 2445, 106, GumpButtonType.Reply, 0 );
			//			gump.AddLabel(  439, 72, 1153, "Delete Module" );

			if (subParams == null || !(subParams is CMGumpParams))
			{
				gump.AddHtml(215, 15, 300, 25, "<basefont size=7 color=white><center>Central Memory</center></font>",
					false, false);
				gump.AddHtml(140, 95, 450, 250,
					"<basefont color=white><center>Welcome to the Central Memory Admin Gump!</center><br>With this gump, you can see a list of all entries that the CM contains.  You can add new Modules or modify or delete existing Modules.<br><br>Make your selection from the top buttons, either List Mobiles or Items.  This will bring up a list of all Mobiles or Items that the CM is keeping track of.<br><br>You may then select one of the entries to list the Modules that are stored to that entry.  You can then add, modify or remove modules to that entry.</font>",
					false, false);
				return;
			}

			Params = subParams as CMGumpParams;

			switch ((int)Params.PageName)
			{
				#region ListMobiles

				case (int)Pages.ListMobiles:
					gump.AddLabel(120, 95, 1153, "Listing all Mobiles:");

					m_MobileList = GetMobiles();
					if (m_MobileList == null || m_MobileList.Count == 0)
						return;

					if (Params.PageIndex < 0)
						Params.PageIndex = 0;


					if (Params.PageIndex > 0)
						gump.AddButton(120, 332, 4014, 4015, 104, GumpButtonType.Reply, 0);
					if ((Params.PageIndex + 1) * 21 <= m_MobileList.Count)
						gump.AddButton(540, 332, 4005, 4006, 105, GumpButtonType.Reply, 0);

					for (int i = Params.PageIndex * 21, r = 0, c = 0; i < m_MobileList.Count; i++)
					{
						if (m_MobileList[i] == null)
							continue;
						gump.AddButton(120 + c * 155, 125 + r * 30, 2501, 2501, 1000 + i, GumpButtonType.Reply, 0);
						gump.AddLabel(130 + c * 155, 126 + r * 30, 1153,
							(m_MobileList[i].Name == null ? m_MobileList[i].Serial.ToString() : m_MobileList[i].Name));
					}

					break;

				#endregion //ListMobiles

				#region ListItems

				case (int)Pages.ListItems:
					gump.AddLabel(120, 95, 1153, "Listing all Items:");

					m_ItemList = GetItems();
					if (m_ItemList == null || m_ItemList.Count == 0)
						return;

					if (Params.PageIndex < 0)
						Params.PageIndex = 0;

					if (Params.PageIndex > 0)
						gump.AddButton(120, 332, 4014, 4015, 104, GumpButtonType.Reply, 0);
					if ((Params.PageIndex + 1) * 21 <= m_ItemList.Count)
						gump.AddButton(540, 332, 4005, 4006, 105, GumpButtonType.Reply, 0);

					for (int i = Params.PageIndex * 21, r = 0, c = 0; i < m_ItemList.Count; i++)
					{
						if (m_ItemList[i] == null)
							continue;
						gump.AddButton(120 + c * 155, 125 + r * 30, 2501, 2501, 1000 + i, GumpButtonType.Reply, 0);
						gump.AddLabel(130 + c * 155, 126 + r * 30, 1153,
							(m_ItemList[i].Name == null ? m_ItemList[i].Serial.ToString() : m_ItemList[i].Name));
					}

					break;

				#endregion //ListItems

				#region ListModulesFor

				case (int)Pages.ListModulesFor:
					if (!DictionaryOfModuleLists.ContainsKey(Params.SelectedSerial))
					{
						gump.AddLabel(120, 95, 1153, "This entity no longer exists in the Central Memory!");
						return;
					}

					if (DictionaryOfModuleLists[Params.SelectedSerial] == null ||
					    DictionaryOfModuleLists[Params.SelectedSerial].Count == 0)
					{
						gump.AddLabel(120, 95, 1153, "This entity has no Modules!");
						Remove(Params.SelectedSerial);
						return;
					}

					string name = "";
					if (Params.SelectedSerial.IsMobile)
						name = World.FindMobile(Params.SelectedSerial).Name;
					else if (Params.SelectedSerial.IsItem)
						name = World.FindItem(Params.SelectedSerial).Name;

					if (name == null || name.Length == 0)
						name = Params.SelectedSerial.ToString();

					gump.AddLabel(120, 95, 1153, String.Format("Listing all Modules for {0}:", name));

					m_ModuleList = DictionaryOfModuleLists[Params.SelectedSerial].GetListOfModules();

					if (m_ModuleList == null || m_ModuleList.Count == 0)
						return;

					if (Params.PageIndex < 0)
						Params.PageIndex = 0;
					if (Params.PageIndex * 21 >= m_ModuleList.Count)
						Params.PageIndex = m_ModuleList.Count - 21;

					if (Params.PageIndex > 0)
						gump.AddButton(120, 332, 4014, 4015, 104, GumpButtonType.Reply, 0);
					if ((Params.PageIndex + 1) * 21 <= m_ModuleList.Count)
						gump.AddButton(540, 332, 4005, 4006, 105, GumpButtonType.Reply, 0);

					gump.AddButton(331, 332, 4008, 4009, 106, GumpButtonType.Reply, 0);

					for (int i = Params.PageIndex * 21, r = 0, c = 0; i < m_ModuleList.Count; i++)
					{
						if (m_ModuleList[i] == null)
							continue;

						gump.AddButton(120 + c * 155, 125 + r * 30, 2501, 2501, 1000 + i, GumpButtonType.Reply, 0);
						gump.AddLabel(130 + c * 155, 126 + r * 30, 1153,
							(m_ModuleList[i].Name().Length == 0
								? m_ModuleList[i].Owner.ToString()
								: m_ModuleList[i].Name()));
					}

					break;

				#endregion //ListModulesFor
			}
		}

		public override void Help(Mobile from, Gump gump)
		{
		}

		/* ID's
		101   = Button	List Mobiles
		102   = Button	List Items
		103   = Button	List Types
		104   = Button	Previous
		105   = Button	Next
		106   = Button	Back to List
		1000+ = Button	Selections
		 */
		public override void OnResponse(NetState state, RelayInfo info, ACCGumpParams subParams)
		{
			if (!Running || info.ButtonID == 0)
				return;

			if (subParams is CMGumpParams)
				Params = subParams as CMGumpParams;

			CMGumpParams newParams = new CMGumpParams();

			//Switch to List Mobiles
			if (info.ButtonID == 101)
			{
				newParams.PageName = Pages.ListMobiles;
			}

			//Switch to List Items
			else if (info.ButtonID == 102)
			{
				newParams.PageName = Pages.ListItems;
			}

			//Previous Page
			else if (info.ButtonID == 104)
			{
				newParams.PageIndex = Params.PageIndex - 1;
			}

			//Next Page
			else if (info.ButtonID == 105)
			{
				newParams.PageIndex = Params.PageIndex + 1;
			}

			//Back to List
			else if (info.ButtonID == 106)
			{
				newParams.PageName = Pages.Home;
			}

			//Select Item from List
			else if (info.ButtonID >= 1000)
			{
				int selectedID = info.ButtonID - 1000;

				if (selectedID < 0 || selectedID >= DictionaryOfModuleLists.Count)
					return;

				switch ((int)Params.PageName)
				{
					//Change page to List Modules with serial of selected item
					case (int)Pages.ListItems:
					{
						if (selectedID < 0 || selectedID >= m_ItemList.Count)
							return;

						newParams.PageName = Pages.ListModulesFor;
						newParams.SelectedSerial = m_ItemList[selectedID].Serial;
						newParams.PageIndex = 0;

						break;
					}
					//Change page to List Modules with serial of selected mobile
					case (int)Pages.ListMobiles:
					{
						if (selectedID < 0 || selectedID >= m_MobileList.Count)
							return;

						newParams.PageName = Pages.ListModulesFor;
						newParams.SelectedSerial = m_MobileList[selectedID].Serial;
						newParams.PageIndex = 0;
						break;
					}
					//Open the Edit Module Gump 
					//Not implemented yet
					case (int)Pages.ListModulesFor:
					{
						if (selectedID < 0 || selectedID >= m_ModuleList.Count)
							return;

						if (m_ModuleList[selectedID] == null)
							return;

						Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(SendEMG), state.Mobile);
						break;
					}
				}
			}

			state.Mobile.SendGump(new ACCGump(state.Mobile, this.ToString(), newParams));
		}

		private void SendEMG(object state)
		{
			Mobile m = state as Mobile;
			if (m == null || m.Deleted)
				return;

			//m.SendMessage("Sent EMG for {0}", m_Module.Name());
			//m.SendGump( new EditModuleGump( m_Module ) );
		}

		#endregion //Gump

		public override void Save(GenericWriter idx, GenericWriter tdb, GenericWriter writer)
		{
			Flush();

			List<Module> fullList = new List<Module>();
			foreach (ModuleList ml in DictionaryOfModuleLists.Values)
			{
				foreach (Module m in ml.Values)
				{
					fullList.Add(m);
				}
			}

			writer.Write(0); //Version

			idx.Write(fullList.Count);
			foreach (Module m in fullList)
			{
				Type t = m.GetType();
				long start = writer.Position;
				idx.Write(m.m_TypeRef);
				idx.Write((int)m.Owner);
				idx.Write(start);

				m.Serialize(writer);

				idx.Write((int)(writer.Position - start));
			}

			tdb.Write(m_Types.Count);
			for (int i = 0; i < m_Types.Count; ++i)
				tdb.Write(m_Types[i].FullName);
		}

		public override void Load(BinaryReader idx, BinaryReader tdb, BinaryFileReader reader)
		{
			object[] ctorArgs = new object[1];
			Type[] ctorTypes = new Type[1] { typeof(Serial) };
			List<ModuleEntry> modules = new List<ModuleEntry>();
			DictionaryOfModuleLists = new Dictionary<Serial, ModuleList>();

			int version = reader.ReadInt();

			int count = tdb.ReadInt32();
			ArrayList types = new ArrayList(count);
			for (int i = 0; i < count; ++i)
			{
				string typeName = tdb.ReadString();
				Type t = ScriptCompiler.FindTypeByFullName(typeName);
				if (t == null)
				{
					Console.WriteLine("Type not found: {0}, remove?", typeName);
					if (Console.ReadKey(true).Key == ConsoleKey.Y)
					{
						types.Add(null);
						continue;
					}

					throw new Exception(String.Format("Bad type '{0}'", typeName));
				}

				ConstructorInfo ctor = t.GetConstructor(ctorTypes);
				if (ctor != null)
					types.Add(new object[] { ctor, null });
				else
					throw new Exception(String.Format("Type '{0}' does not have a serialization constructor", t));
			}

			int moduleCount = idx.ReadInt32();

			for (int i = 0; i < moduleCount; ++i)
			{
				int typeID = idx.ReadInt32();
				int serial = idx.ReadInt32();
				long pos = idx.ReadInt64();
				int length = idx.ReadInt32();

				object[] objs = (object[])types[typeID];
				if (objs == null)
					continue;

				Module m = null;
				ConstructorInfo ctor = (ConstructorInfo)objs[0];
				string typeName = (string)objs[1];

				try
				{
					ctorArgs[0] = new Serial(serial);
					m = (Module)(ctor.Invoke(ctorArgs));
				}
				catch
				{
				}

				if (m != null)
				{
					modules.Add(new ModuleEntry(m, typeID, typeName, pos, length));
					AddModule(m);
				}
			}

			bool failedModules = false;
			Type failedType = null;
			Exception failed = null;
			int failedTypeID = 0;

			for (int i = 0; i < modules.Count; ++i)
			{
				ModuleEntry entry = modules[i];
				Module m = entry.Module;

				if (m != null)
				{
					reader.Seek(entry.Position, SeekOrigin.Begin);

					try
					{
						m.Deserialize(reader);

						if (reader.Position != (entry.Position + entry.Length))
							throw new Exception(String.Format("Bad serialize on {0}", m.GetType()));
					}
					catch (Exception e)
					{
						modules.RemoveAt(i);

						failed = e;
						failedModules = true;
						failedType = m.GetType();
						failedTypeID = entry.TypeID;

						break;
					}
				}
			}

			if (failedModules)
			{
				Console.WriteLine("An error was encountered while loading a Module of Type: {0}", failedType);
				Console.WriteLine("Remove this type of Module? (y/n)");
				if (Console.ReadLine() == "y")
				{
					for (int i = 0; i < modules.Count;)
					{
						if (modules[i].TypeID == failedTypeID)
							modules.RemoveAt(i);
						else
							++i;
					}

					SaveIndex(modules);
				}

				Console.WriteLine("After pressing return an exception will be thrown and the server will terminate");
				Console.ReadLine();

				throw new Exception(String.Format("Load failed (type={0})", failedType), failed);
			}
		}

		private void SaveIndex<T>(List<T> list) where T : IEntityMod
		{
			string path = "";
			if (Running)
			{
				if (!Directory.Exists("ACC Backups/Central Memory/"))
					Directory.CreateDirectory("ACC Backups/Central Memory/");
				path = "ACC Backups/Central Memory/Central Memory.idx";
			}
			else
			{
				if (!Directory.Exists("Saves/ACC/Central Memory/"))
					Directory.CreateDirectory("Saves/ACC/Central Memory/");
				path = "Saves/ACC/Central Memory/Central Memory.idx";
			}

			using (FileStream idx = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				BinaryWriter idxWriter = new BinaryWriter(idx);

				idxWriter.Write(list.Count);
				for (int i = 0; i < list.Count; ++i)
				{
					T e = list[i];

					idxWriter.Write(e.TypeID);
					idxWriter.Write(e.Serial);
					idxWriter.Write(e.Position);
					idxWriter.Write(e.Length);
				}

				idxWriter.Close();
			}
		}

		private interface IEntityMod
		{
			Serial Serial { get; }
			int TypeID { get; }
			long Position { get; }
			int Length { get; }
		}

		private sealed class ModuleEntry : IEntityMod
		{
			public Module Module { get; }
			public Serial Serial { get { return Module == null ? Serial.MinusOne : Module.Owner; } }
			public int TypeID { get; }
			public string TypeName { get; }
			public long Position { get; }
			public int Length { get; }

			public ModuleEntry(Module module, int typeID, string typeName, long pos, int length)
			{
				Module = module;
				TypeID = typeID;
				TypeName = typeName;
				Position = pos;
				Length = length;
			}
		}
	}
}
