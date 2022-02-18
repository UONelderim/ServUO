#region References

using Server.Items;
using static Server.Mobiles.DragonBreath;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki lodowej jaszczurki")]
	[TypeAlias("Server.Mobiles.IceLizard")]
	public class IceLizard : BaseCreature
	{
		public static void Initialize()
		{
			DragonBreathDefinition.Definitions.Add(new DragonBreathDefinition(
				0.16,
				1.0,
				1.3,
				1.0,
				0, 0, 100, 0, 0, 0, 0,
				30.0, 45.0,
				0x37C4,
				5,
				0,
				false,
				false,
				196,
				0,
				0x227,
				12,
				false,
				new[] { typeof(IceLizard) }));
		}

		[Constructable]
		public IceLizard()
			: base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "lodowa jaszczurka";
			Body = 0xCE;
			Hue = 196;
			BaseSoundID = 0x5A;

			SetStr(126, 150);
			SetDex(56, 75);
			SetInt(11, 20);

			SetHits(76, 90);
			SetMana(0);

			SetDamage(6, 24);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 35, 45);
			SetResistance(ResistanceType.Cold, 30, 45);
			SetResistance(ResistanceType.Poison, 25, 35);
			SetResistance(ResistanceType.Energy, 25, 35);

			SetSkill(SkillName.MagicResist, 55.1, 70.0);
			SetSkill(SkillName.Tactics, 60.1, 80.0);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 40;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 80.7;

			PackItem(new SulfurousAsh(Utility.Random(4, 10)));

			SetSpecialAbility(SpecialAbility.DragonBreath);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Meager);
		}

		public override int Hides { get { return 3; } }
		public override HideType HideType { get { return HideType.Spined; } }

		public IceLizard(Serial serial)
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
