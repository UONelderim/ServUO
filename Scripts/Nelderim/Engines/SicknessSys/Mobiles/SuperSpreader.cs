#region References

using Server.Mobiles;

#endregion

namespace Server.SicknessSys.Mobiles
{
	class SuperSpreader : BaseCreature
	{
		private IllnessType Illness { get; set; }
		private int InfectedCount;

		[Constructable]
		public SuperSpreader(bool IsDecided = false, int illness = 0) : base(AIType.AI_Melee, FightMode.Aggressor, 10,
			1, 0.2, 0.4)
		{
			int getIllness = Utility.Random(1, 3);

			if (IsDecided)
			{
				if (illness > 0 && illness < 4)
					getIllness = illness;
				else
					getIllness = 2;
			}

			if (getIllness > 3 || getIllness < 1)
				getIllness = 2;

			Illness = (IllnessType)getIllness;

			Name = "rozsiewacz" + Illness + "";

			Body = 238;

			InitStats(10, 10, 10);

			SetHits(10);
			SetDamage(0);

			Blessed = true;
			Hidden = true;
			CantWalk = true;

			InfectedCount = 0;
		}

		public override void OnMovement(Mobile m, Point3D oldLocation)
		{
			StayHidden();

			base.OnMovement(m, oldLocation);
		}

		public override void OnAfterMove(Point3D oldLocation)
		{
			StayHidden();

			base.OnAfterMove(oldLocation);
		}

		public override void OnThink()
		{
			StayHidden();

			foreach (Mobile mobile in GetMobilesInRange(3))
			{
				if (mobile is PlayerMobile)
				{
					PlayerMobile pm = mobile as PlayerMobile;

					Item HasVirus = pm.Backpack.FindItemByType(typeof(VirusCell));

					if (HasVirus == null)
					{
						int Chance = SicknessHelper.GetSickChance(pm, 0);

						if (Chance != 0)
						{
							int rndInfect = Utility.RandomMinMax(1, 10);

							if (Chance == 100)
							{
								SicknessInfect.Infect(pm, Illness);
								InfectedCount++;
							}
							else
							{
								if (rndInfect <= Chance)
								{
									SicknessInfect.Infect(pm, Illness);
									InfectedCount++;
								}
							}
						}
					}

					if (InfectedCount > 10)
					{
						Delete();
					}
				}
			}

			base.OnThink();
		}

		public SuperSpreader(Serial serial) : base(serial)
		{
		}

		private void StayHidden()
		{
			if (!Hidden)
				Hidden = true;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);

			writer.Write((int)Illness);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			Illness = (IllnessType)reader.ReadInt();
		}
	}
}
