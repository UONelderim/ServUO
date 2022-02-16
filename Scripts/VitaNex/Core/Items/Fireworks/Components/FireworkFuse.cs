#region Header
//   Vorspire    _,-'/-'/  FireworkFuse.cs
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

using Server;
#endregion

namespace VitaNex.Items
{
	public class FireworkFuse : FireworkComponent
	{
		[Constructable]
		public FireworkFuse()
			: this(1)
		{ }

		[Constructable]
		public FireworkFuse(int amount)
			: base(3613)
		{
			Name = "Firework Fuse";
			Hue = 85;
			Stackable = true;
			Amount = Math.Max(1, Math.Min(60000, amount));
		}

		public FireworkFuse(Serial serial)
			: base(serial)
		{ }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();
		}
	}
}