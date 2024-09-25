#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class Tavti : BaseCreature
	{
		[Constructable]
		public Tavti() : base(AIType.AI_Mage, FightMode.None, 12, 5, 0.2, 0.4)
		{
			Race = Race.NTamael;
			Female = true;
			Name = "Lord Tavti";
			//Title = "- Wojownik";
			Body = 0x190;
			Hue = 0x389;
			HairItemID = 0x203B;
			HairHue = 842;

			Str = 160;
			Dex = 120;
			Int = 100;
			Hits = 400;

			SetSkill(SkillName.Anatomy, 100.0);
			SetSkill(SkillName.Healing, 100.0);
			SetSkill(SkillName.MagicResist, 90.0);
			SetSkill(SkillName.Macing, 120.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Parry, 100.0);


			BoneChest chest = new BoneChest();
			chest.Hue = 1510;
			EquipItem(chest);

			BoneLegs legs = new BoneLegs();
			legs.Hue = 1510;
			EquipItem(legs);

			BoneArms arms = new BoneArms();
			arms.Hue = 1510;
			EquipItem(arms);


			Sandals Boot = new Sandals();
			Boot.Hue = 1337;
			AddItem(Boot);

			Cloak Cloa = new Cloak();
			Cloa.Hue = 842;
			AddItem(Cloa);

			WarMace sword = new WarMace();
			sword.Hue = 2084;
			EquipItem(sword);


			Container pack = new Backpack();

			pack.Movable = false;

			AddItem(pack);
		}


		public Tavti(Serial serial)
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
