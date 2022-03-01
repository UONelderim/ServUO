#region References

using Server.Mobiles;

#endregion

namespace Server.SicknessSys.Mobiles
{
	[CorpseName("zwloki zakazonego nietoperza wampira")]
	public class InfectedBat : BaseCreature
	{
		[Constructable]
		public InfectedBat() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "zakazony nietoperz wampir";
			Body = 317;
			BaseSoundID = 0x270;

			SetStr(91, 110);
			SetDex(91, 115);
			SetInt(26, 50);

			SetHits(55, 66);

			SetDamage(7, 9);

			SetDamageType(ResistanceType.Physical, 80);
			SetDamageType(ResistanceType.Poison, 20);

			SetResistance(ResistanceType.Physical, 35, 45);
			SetResistance(ResistanceType.Fire, 15, 25);
			SetResistance(ResistanceType.Cold, 15, 25);
			SetResistance(ResistanceType.Poison, 60, 70);
			SetResistance(ResistanceType.Energy, 40, 50);

			SetSkill(SkillName.MagicResist, 70.1, 95.0);
			SetSkill(SkillName.Tactics, 55.1, 80.0);
			SetSkill(SkillName.Wrestling, 30.1, 55.0);

			Fame = 1000;
			Karma = -1000;

			VirtualArmor = 14;
		}

		public InfectedBat(Serial serial) : base(serial)
		{
		}

		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.Poor);
		}

		public override int GetIdleSound()
		{
			return 0x29B;
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			if (defender is PlayerMobile && defender.Alive)
			{
				int getRnd = Utility.Random(1, 10);

				if (getRnd == 7)
				{
					Item cell = defender.Backpack.FindItemByType(typeof(VirusCell));

					if (cell == null)
					{
						Say("*ugryzienie*");

						PlayerMobile pm = defender as PlayerMobile;

						SicknessInfect.Infect(pm, IllnessType.Vampirism);

						Kill();
					}
				}
			}

			base.OnGaveMeleeAttack(defender);
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
