using Server.Items;
using System;

namespace Server.Engines.Quests
{
	public class Oogan : MondainQuester
	{
		[Constructable]
		public Oogan()
			: base("Oogan", "- Mnich")
		{
		}

		public Oogan(Serial serial)
			: base(serial)
		{
		}

		public override Type[] Quests => new Type[] { typeof(AvatarQuest) };

		public override void InitBody()
		{
			InitStats(100, 100, 25);

			Female = true;
			Race = Race.NJarling;
		}

		public override void InitOutfit()
		{
			SetWearable(new Boots(), 0, 1);
			SetWearable(new MonkRobe(), 1355, dropChance: 1);
			SetWearable(new QuarterStaff(), 0, dropChance: 1);
			SetWearable(new LeatherNinjaBelt(), 0, dropChance: 1);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
