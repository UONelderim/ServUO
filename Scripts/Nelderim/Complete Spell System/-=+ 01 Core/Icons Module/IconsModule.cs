#region References

using System;
using System.Collections.Generic;
using Server.ACC.CM;

#endregion

namespace Server.ACC.CSS.Modules
{
	public class IconsModule : Module
	{
		public Dictionary<Type, IconInfo> Icons { get; private set; }
		//private Hashtable m_Icons;
		//public Hashtable Icons{ get{ return m_Icons; } }

		public override string Name() { return "Spell Icons"; }

		public IconsModule(Serial serial)
			: this(serial, null)
		{
		}

		public IconsModule(Serial serial, IconInfo info)
			: base(serial)
		{
			//m_Icons = new Hashtable();
			Icons = new Dictionary<Type, IconInfo>();

			Add(info);
		}

		public void Add(IconInfo info)
		{
			if (info == null)
				return;

			if (Icons.ContainsKey(info.SpellType))
				Icons[info.SpellType] = info;

			else
				Icons.Add(info.SpellType, info);
		}

		public IconInfo Get(Type type)
		{
			return Icons[type];
		}

		public bool Contains(Type type)
		{
			return Icons.ContainsKey(type);
		}

		public void Remove(Type type)
		{
			Icons.Remove(type);

			if (Icons.Count == 0)
				CentralMemory.RemoveModule(this.Owner, this);
		}

		public override void Append(Module mod, bool negatively)
		{
			IconsModule im = mod as IconsModule;
			List<Type> removeList = new List<Type>();

			foreach (KeyValuePair<Type, IconInfo> kvp in im.Icons)
			{
				if (negatively)
					removeList.Add(kvp.Value.SpellType);
				else
					Add(kvp.Value);
			}

			foreach (Type t in removeList)
			{
				Remove(t);
			}

			removeList.Clear();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // version

			writer.Write(Icons.Count);
			foreach (KeyValuePair<Type, IconInfo> kvp in Icons)
			{
				kvp.Value.Serialize(writer);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			Icons = new Dictionary<Type, IconInfo>();

			int count = reader.ReadInt();
			for (int i = 0; i < count; i++)
			{
				IconInfo ii = new IconInfo(reader);
				if (ii.SpellType != null)
					Icons.Add(ii.SpellType, ii);
			}
		}
	}
}
