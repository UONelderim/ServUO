#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class Renny : BaseCreature
	{
		[Constructable]
		public Renny() : base(AIType.AI_Animal, FightMode.None, 12, 5, 0.2, 0.4)
		{
			Race = Race.NTamael;
			Female = true;
			Name = "Renny";
			Title = "- Wojownik";
			Body = 0x190;
			Hue = 0x905;
			HairItemID = 0x203D;
			HairHue = 0x457;

			Str = 200;
			Dex = 120;
			Int = 60;
			Hits = 400;

			SetSkill(SkillName.Anatomy, 100.0);
			SetSkill(SkillName.Healing, 100.0);
			SetSkill(SkillName.MagicResist, 90.0);
			SetSkill(SkillName.Swords, 120.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Parry, 100.0);


			StuddedChest chest = new StuddedChest();
			chest.Hue = 926;
			chest.Movable = false;
			EquipItem(chest);

			StuddedLegs legs = new StuddedLegs();
			legs.Hue = 926;
			legs.Movable = false;
			EquipItem(legs);

			StuddedArms arms = new StuddedArms();
			arms.Hue = 926;
			arms.Movable = false;
			EquipItem(arms);

			StuddedGloves gloves = new StuddedGloves();
			gloves.Hue = 926;
			gloves.Movable = false;
			EquipItem(gloves);

			Bandana band = new Bandana();
			band.Hue = 570;
			band.Movable = false;
			AddItem(band);

			Boots Boot = new Boots();
			Boot.Hue = 926;
			AddItem(Boot);

			Cloak Cloa = new Cloak();
			Cloa.Hue = 563;
			Cloa.Movable = false;
			AddItem(Cloa);

			PaladinSword sword = new PaladinSword();
			sword.Movable = false;
			sword.Hue = 570;
			EquipItem(sword);


			Container pack = new Backpack();

			pack.Movable = false;

			AddItem(pack);
		}


		public Renny(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
