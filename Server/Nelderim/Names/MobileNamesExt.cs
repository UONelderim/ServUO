using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Nelderim;
using Nelderim.Configuration;
using Server.Network;

namespace Server
{
	public partial class Mobile
	{
		public Dictionary<Mobile, string> NameFor
		{
			get => Names.Get(this).Names;
			set
			{
				Names.Get(this).Names = value;
				Delta(MobileDelta.Name);
				InvalidateProperties();
			}
		}
		
		public string NGetName(Mobile m)
		{
			if (NConfig.NameSystemEnabled && !UseRealName(m) )
			{
				return NameFor.TryGetValue(m, out var assignedName) ? assignedName : DefaultName;
			}
			return Name;
		}

		public virtual bool UseRealName(Mobile m)
		{
			return m == this || IsStaff() || m.IsStaff();
		}

		public virtual string DefaultName => "nieznajomy " + (Race == Race.DefaultRace ? "" : Race.GetName().ToLower());

		public Dictionary<Mobile, ObjectPropertyList> OPLs = new Dictionary<Mobile, ObjectPropertyList>();
		public Dictionary<Mobile, OPLInfo> OPLInfos = new Dictionary<Mobile, OPLInfo>();
		
		[MethodImpl(MethodImplOptions.Synchronized)]
		public OPLInfo NGetOPLPacket(Mobile m)
		{
			if (!NConfig.NameSystemEnabled)
				return OPLPacket;
			if (OPLInfos.TryGetValue(m, out var oplInfo))
			{
				return oplInfo;
			}
			var newOplInfo = new OPLInfo(NGetPropertyList(m));

			newOplInfo.SetStatic();

			OPLInfos[m] = newOplInfo;
			return newOplInfo;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public ObjectPropertyList NGetPropertyList(Mobile m)
		{
			if (!NConfig.NameSystemEnabled)
				return PropertyList;
			if (OPLs.TryGetValue(m, out var opl))
			{
				return opl;
			}
			
			var newOpl = new ObjectPropertyList(this);

			NGetProperties(newOpl, m);

			newOpl.Terminate();
			newOpl.SetStatic();

			OPLs[m] = newOpl;

			return newOpl;
		}
		
		public virtual void NGetProperties(ObjectPropertyList list, Mobile m)
		{
			NAddNameProperties(list, m);
			AddNameProperties(list); //We still call original method for extra labels, but just 'Name' should be disabled

			if (Spawner != null)
			{
				Spawner.GetSpawnProperties(this, list);
			}
		}
		
		public virtual void NAddNameProperties(ObjectPropertyList list, Mobile m)
		{
			var name = NGetName(m);

			var prefix = ""; // still needs to be defined due to cliloc. Only defined in PlayerMobile. BaseCreature and BaseVendor require the suffix for the title and use the same cliloc.

			var suffix = "";

			if (PropertyTitle && !String.IsNullOrEmpty(Title))
			{
				suffix = Title;
			}

			suffix = ApplyNameSuffix(suffix);

			list.Add(1050045, "{0} \t{1}\t {2}", prefix, name, suffix); // ~1_PREFIX~~2_NAME~~3_SUFFIX~           
		}
		
		public void NInvalidateProperties()
		{
			if (!ObjectPropertyList.Enabled)
			{
				return;
			}

			foreach (var serial in OPLs.Keys)
			{
				var opl = OPLs[serial];
				Packet.Release(ref opl);
				if (OPLInfos.TryGetValue(serial, out var info))
				{
					Packet.Release(ref info);
					Delta(MobileDelta.Properties);
				}
			}
			OPLs.Clear();
			OPLInfos.Clear();
		}
	}
}
