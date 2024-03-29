﻿using System;
using Nelderim;

namespace Server.Mobiles
{
	public partial class PlayerMobile
	{
		[Flags]
		public enum NPlayerFlag
		{
			None = 0x00000000,
			Mysticism = 0x00000001,
		}
		
		public bool NGetFlag(NPlayerFlag flag)
		{
			return ((PlayerFlagsExt.Get(this).Flags & flag) != 0);
		}

		public void NSetFlag(NPlayerFlag flag, bool value)
		{
			if (value)
			{
				PlayerFlagsExt.Get(this).Flags |= flag;
			}
			else
			{
				PlayerFlagsExt.Get(this).Flags &= ~flag;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Mysticism { 
			get => NGetFlag(NPlayerFlag.Mysticism);
			set => NSetFlag(NPlayerFlag.Mysticism, value);
		}
	}
	
	class PlayerFlagsExt : NExtension<PlayerFlagsExtInfo>
	{
		public static string ModuleName = "PlayerFlags";

		public static void Initialize()
		{
			EventSink.WorldSave += Save;
			Load(ModuleName);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Save(args, ModuleName);
		}
	}

	class PlayerFlagsExtInfo : NExtensionInfo
	{
		public PlayerMobile.NPlayerFlag Flags { get; set; }

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 ); //version
			writer.Write((int)Flags);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = 0;
			if (Fix)
				version = reader.ReadInt(); //version
			Flags = (PlayerMobile.NPlayerFlag)reader.ReadInt();
		}
	}
}
