#region References

using Server.Mobiles;

#endregion

namespace Server.SicknessSys.Mobiles
{
	public class InfectedWolf : BaseCreature
	{
		public InfectedWolf(Serial serial) : base(serial)
		{
		}

		public InfectedWolf(AIType aI_Melee, FightMode closest, int v1, int v2, double v3, double v4) : base(aI_Melee,
			closest, v1, v2, v3, v4)
		{
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

						SicknessInfect.Infect(pm, IllnessType.Lycanthropia);

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
