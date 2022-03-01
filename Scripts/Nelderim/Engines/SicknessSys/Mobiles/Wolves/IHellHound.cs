#region References

using Server.Items;
using Server.Mobiles;

#endregion

namespace Server.SicknessSys.Mobiles
{
	[CorpseName("zwloki zarazonego piekielnego ogara")]
	public class IHellHound : InfectedWolf
	{
		[Constructable]
		public IHellHound() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "zarazony piekielny ogar";
			Body = 98;
			BaseSoundID = 229;

			SetStr(100, 350);
			SetDex(80, 300);
			SetInt(30, 180);

			SetHits(130, 300);

			SetDamage(11, 17);

			SetDamageType(ResistanceType.Physical, 20);
			SetDamageType(ResistanceType.Fire, 80);

			SetResistance(ResistanceType.Physical, 0, 56);
			SetResistance(ResistanceType.Fire, 30, 40);
			SetResistance(ResistanceType.Poison, 10, 20);
			SetResistance(ResistanceType.Energy, 10, 20);

			SetSkill(SkillName.Anatomy, 0, 5);
			SetSkill(SkillName.MagicResist, 0, 75);
			SetSkill(SkillName.Tactics, 0, 80);
			SetSkill(SkillName.Wrestling, 0, 80);
			SetSkill(SkillName.Necromancy, 18);
			SetSkill(SkillName.SpiritSpeak, 18);

			Fame = 3400;
			Karma = -3400;

			VirtualArmor = 30;

			Tamable = false;

			PackItem(new SulfurousAsh(5));
		}

		public IHellHound(Serial serial) : base(serial)
		{
		}

		public override int Meat
		{
			get
			{
				return 1;
			}
		}

		public override FoodType FavoriteFood
		{
			get
			{
				return FoodType.Meat;
			}
		}

		public override PackInstinct PackInstinct
		{
			get
			{
				return PackInstinct.Canine;
			}
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Average);
			AddLoot(LootPack.Meager);
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
