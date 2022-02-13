#region Header
//   Vorspire    _,-'/-'/  BlockBuster.cs
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
#endregion

namespace VitaNex.Items
{
	public class BlockBusterRocket : BaseFireworkRocket
	{
		public override FireworkStars DefStarsEffect { get { return FireworkStars.BloomFlower; } }

		[Constructable]
		public BlockBusterRocket()
			: this(Utility.RandomMetalHue())
		{ }

		[Constructable]
		public BlockBusterRocket(int hue)
			: base(9242, hue)
		{
			Name = "Block Buster";
			Weight = 8.0;
		}

		public BlockBusterRocket(Serial serial)
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