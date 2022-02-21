#region References

using System;
using System.Collections;
using Server.Items;
using Server.Spells;

#endregion

namespace Server.ACC.CSS
{
	public class SpellInfoRegistry
	{
		public static Hashtable Registry { get; set; } = new Hashtable();

		public static void Register(Type type, string name, string desc, string regs, string info, int icon, int back,
			School school)
		{
			if (!Registry.ContainsKey(school))
				Registry.Add(school, new ArrayList());

			if (((ArrayList)Registry[school]).Count < 64)
			{
				CSpellInfo inf = new CSpellInfo(type, name, desc, regs, info, icon, back, school, true);

				((ArrayList)Registry[school]).Add(inf);
			}
			else
				Console.WriteLine("You cannot register more than 64 spells to the {0} school.", school);
		}

		public static void DisEnable(School school, Type type, bool enabled)
		{
			foreach (CSpellInfo info in (ArrayList)Registry[school])
			{
				if (info.Type == type)
					info.Enabled = enabled;
			}
		}

		public static bool CheckRegistry(School school, Type type)
		{
			if (!CSS.Running)
				return false;

			if (school == School.Invalid)
				return true;

			if (Registry.ContainsKey(school))
			{
				foreach (CSpellInfo info in (ArrayList)Registry[school])
				{
					if (info.Type == type && info.Enabled)
						return true;
				}

				return false;
			}

			return false;
		}

		public static bool CheckBooksForSchool(Mobile mobile, Type type)
		{
			Item item = mobile.FindItemOnLayer(Layer.OneHanded);
			if (item is CSpellbook && CheckRegistry(((CSpellbook)item).School, type))
				return true;

			Container pack = mobile.Backpack;
			if (pack == null)
				return false;

			for (int i = 0; i < pack.Items.Count; i++)
			{
				item = pack.Items[i];
				if (item is CSpellbook && CheckRegistry(((CSpellbook)item).School, type))
					return true;
			}

			return false;
		}

		public static CSpellInfo GetInfo(School school, Type type)
		{
			if (Registry.ContainsKey(school))
			{
				foreach (CSpellInfo info in (ArrayList)Registry[school])
				{
					if (info.Type == type && info.Enabled)
						return info;
				}
			}

			return null;
		}

		public static ArrayList GetSpellsForSchool(School school)
		{
			ArrayList ret = new ArrayList();
			if (Registry.ContainsKey(school))
			{
				foreach (CSpellInfo info in (ArrayList)Registry[school])
				{
					ret.Add(info.Type);
				}
			}

			return ret;
		}

		private static readonly object[] m_Params = new object[2];

		public static Spell NewSpell(Type type, School school, Mobile caster, Item scroll)
		{
			if (!CSS.Running)
				return null;
			if (school == School.Invalid && scroll == null && !CheckBooksForSchool(caster, type))
				return null;
			if (!CheckRegistry(school, type))
				return null;
			if (!SpellRestrictions.UseRestrictions && SpellRestrictions.CheckRestrictions(caster, type))
				return null;

			m_Params[0] = caster;
			m_Params[1] = scroll;

			return (Spell)Activator.CreateInstance(type, m_Params);
			;
		}
	}

	public class CSpellInfo
	{
		public Type Type { get; private set; }
		public string Name { get; }
		public string Desc { get; }
		public string Regs { get; }
		public string Info { get; }
		public int Icon { get; }
		public int Back { get; }
		public School School { get; private set; }
		public bool Enabled { get; set; }

		public CSpellInfo(Type type, string name, string desc, string regs, string info, int icon, int back,
			School school, bool enabled)
		{
			Type = type;
			Name = name;
			Desc = desc;
			Regs = regs;
			Info = info;
			Icon = icon;
			Back = back;
			School = school;
			Enabled = enabled;
		}

		public CSpellInfo(GenericReader reader)
		{
			Deserialize(reader);
		}

		public void Serialize(GenericWriter writer)
		{
			writer.Write(0); //version

			writer.Write(Type.Name);
			writer.Write((int)School);
			writer.Write(Enabled);
		}

		public void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					Type = ScriptCompiler.FindTypeByName(reader.ReadString());
					School = (School)reader.ReadInt();
					Enabled = reader.ReadBool();
					break;
				}
			}
		}
	}
}
