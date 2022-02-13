#region Header
//   Vorspire    _,-'/-'/  FireworkStarShadowIron.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using Server;
using Server.Items;
#endregion

namespace VitaNex.Items
{
	public class FireworkStarShadowIron : BaseFireworkStar
	{
		[Constructable]
		public FireworkStarShadowIron()
			: this(1)
		{ }

		[Constructable]
		public FireworkStarShadowIron(int amount)
			: base(CraftResource.ShadowIron, amount)
		{ }

		public FireworkStarShadowIron(Serial serial)
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