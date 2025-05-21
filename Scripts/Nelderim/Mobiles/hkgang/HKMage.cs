#region References

using Server.Mobiles;

#endregion

namespace Server.Engines.HunterKiller
{
	public class HKMage : HKMobile
	{
		[Constructable]
		public HKMage() : base(AIType.AI_Mage, FightMode.Closest, HunterKillerType.MageType)
		{
			SetStr(50, 80);
			SetDex(80, 100);
			SetInt(100, 150);

			SetHits(110, 120);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Fire, 30, 40);
			SetResistance(ResistanceType.Cold, 25, 35);
			SetResistance(ResistanceType.Poison, 10, 20);
			SetResistance(ResistanceType.Energy, 10, 20);

			SetSkill(SkillName.EvalInt, 90.0, 100.0);
			SetSkill(SkillName.Magery, 90.0, 120.0);
			SetSkill(SkillName.Meditation, 80.0, 100.0);
			SetSkill(SkillName.MagicResist, 100.0, 120.0);
			SetSkill(SkillName.Tactics, 65.0, 80.0);
			SetSkill(SkillName.Wrestling, 60.3, 100.0);

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 22;

			new Horse().Rider = this;

			AddLoot(LootPack.MageryRegs, 20);
			AddLoot(LootPack.MageryScrolls, 2, 7);
		}

		public HKMage(Serial serial) : base(serial)
		{
		}

		#region DetectHidden RedBeard

		public override void OnMovement(Mobile m, Point3D oldLocation)
		{
			if (m is PlayerMobile)
			{
				PlayerMobile pm = m as PlayerMobile;

				if (pm != null)
				{
					if (pm.AccessLevel == AccessLevel.Player && pm.Hidden)
					{
						this.UseSkill(SkillName.DetectHidden);
						pm.RevealingAction();
						pm.FixedParticles(0x375A, 9, 20, 5049, EffectLayer.Head);
						pm.PlaySound(0x1FD);
						pm.SendLocalizedMessage(500814, Title, 33); // You have been revealed!

						base.OnMovement(pm, oldLocation);
					}
				}
			}
		}

		#endregion

		public override bool AutoDispel => false;

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
