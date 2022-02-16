#region Header
//   Vorspire    _,-'/-'/  EquipmentSets_Init.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Network;

using VitaNex.Network;
#endregion

namespace VitaNex.Modules.EquipmentSets
{
	[CoreModule("Equipment Sets", "3.0.0.0")]
	public static partial class EquipmentSets
	{
		static EquipmentSets()
		{
			CMOptions = new EquipmentSetsOptions();

			SetTypes = TypeOfEquipmentSet.GetConstructableChildren();

			Sets = new Dictionary<Type, EquipmentSet>(SetTypes.Length);

			foreach (var t in SetTypes)
			{
				Sets[t] = t.CreateInstanceSafe<EquipmentSet>();
			}
		}

		private static void CMConfig()
		{
			EquipItemRequestParent = PacketHandlers.GetHandler(0x13);

			DropItemRequestParent = PacketHandlers.GetHandler(0x08);

			EquipItemParent = OutgoingPacketOverrides.GetHandler(0x2E);
			EquipItemParent6017 = OutgoingPacketOverrides.GetHandler(0x2E);

			PacketHandlers.Register(0x13, 10, true, EquipItemRequest);

			PacketHandlers.Register(0x08, 14, true, DropItemRequest);

			OutgoingPacketOverrides.Register(0x2E, EquipItem);
		}

		private static void CMInvoke()
		{
			EventSink.Login += OnLogin;
			ExtendedOPL.OnItemOPLRequest += GetProperties;
		}

		private static void CMEnabled()
		{
			if (World.Loaded)
			{
				World.Mobiles.Values.AsParallel()
					 .Where(m => m.Items != null && m.Items.Any(i => i.Layer.IsEquip()))
					 .ForEach(Invalidate);
			}
		}

		private static void CMDisabled()
		{
			if (World.Loaded)
			{
				foreach (var set in Sets.Values)
				{
					set.ActiveOwners.ForEachReverse(Invalidate);
				}
			}
		}

		private static void CMDispose()
		{
			if (EquipItemRequestParent != null && EquipItemRequestParent.OnReceive != null)
			{
				PacketHandlers.Register(0x13, 10, true, EquipItemRequestParent.OnReceive);
			}

			if (DropItemRequestParent != null && DropItemRequestParent.OnReceive != null)
			{
				PacketHandlers.Register(0x08, 14, true, DropItemRequestParent.OnReceive);
			}
		}
	}
}
