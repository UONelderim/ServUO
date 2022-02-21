#region References

using Server.Items;
using Server.Network;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki wielkiego wojwnika czerwonych mrowek")]
	public class GreaterRedSolenWarrior : BaseCreature
	{
		public bool BurstSac { get; private set; }

		[Constructable]
		public GreaterRedSolenWarrior() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "wielki wojownik czerwonych mrowek";
			Body = 782;
			BaseSoundID = 959;
			Hue = 0x621;

			SetStr(220, 250);
			SetDex(120, 140);
			SetInt(36, 60);

			SetHits(120, 150);

			SetDamage(10, 18);

			SetDamageType(ResistanceType.Physical, 80);
			SetDamageType(ResistanceType.Poison, 20);

			SetResistance(ResistanceType.Physical, 30, 45);
			SetResistance(ResistanceType.Fire, 30, 40);
			SetResistance(ResistanceType.Cold, 20, 35);
			SetResistance(ResistanceType.Poison, 30, 40);
			SetResistance(ResistanceType.Energy, 20, 35);

			SetSkill(SkillName.MagicResist, 80.0);
			SetSkill(SkillName.Tactics, 90.0);
			SetSkill(SkillName.Wrestling, 90.0);

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 38;

			SolenHelper.PackPicnicBasket(this);
			PackItem(new ZoogiFungus((0.05 < Utility.RandomDouble()) ? 2 : 4));

			if (Utility.RandomDouble() < 0.05)
				PackItem(new BraceletOfBinding());
		}

		public override int GetAngerSound()
		{
			return 0xB5;
		}

		public override int GetIdleSound()
		{
			return 0xB5;
		}

		public override int GetAttackSound()
		{
			return 0x289;
		}

		public override int GetHurtSound()
		{
			return 0xBC;
		}

		public override int GetDeathSound()
		{
			return 0xE4;
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich);
		}

		public override bool IsEnemy(Mobile m)
		{
			if (SolenHelper.CheckRedFriendship(m))
				return false;
			return base.IsEnemy(m);
		}

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			SolenHelper.OnRedDamage(from);

			if (!willKill)
			{
				if (!BurstSac)
				{
					if (Hits < 50)
					{
						PublicOverheadMessage(MessageType.Regular, 0x3B2, true,
							"* The solen's acid sac is burst open! *");
						BurstSac = true;
					}
				}
				else if (from != null && from != this && InRange(from, 1))
				{
					SpillAcid(from, 1);
				}
			}

			base.OnDamage(amount, from, willKill);
		}

		public override bool OnBeforeDeath()
		{
			SpillAcid(4);

			return base.OnBeforeDeath();
		}

		public GreaterRedSolenWarrior(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(1);
			writer.Write(BurstSac);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
				{
					BurstSac = reader.ReadBool();
					break;
				}
			}
		}
	}
}
