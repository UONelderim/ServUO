using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Gumps
{
	public class MinorArtifactGump : ArtifactGump
	{
		public MinorArtifactGump(Mobile from, Item deed) : base(from, deed)
		{
		}

		protected override Type gumpType => typeof(MinorArtifactGump);

		protected override SortedList<string, Type[]> artifacts => new SortedList<string, Type[]>
		{
			{
				"Weapons",
				new[]
				{
					typeof(ArcticDeathDealer), typeof(BlazeOfDeath), typeof(BowOfTheJukaKing),
					typeof(CaptainQuacklebushsCutlass), typeof(CavortingClub), typeof(ColdBlood),
					typeof(EnchantedTitanLegBone), typeof(LunaLance), typeof(NightsKiss),
					typeof(NoxRangersHeavyCrossbow), typeof(PixieSwatter), typeof(WrathOfTheDryad),
					typeof(TheTaskmaster), typeof(StaffOfPower),
				}
			},
			{
				"Armor",
				new[]
				{
					typeof(BurglarsBandana), typeof(DreadPirateHat), typeof(HeartOfTheLion), typeof(OrcishVisage),
					typeof(PolarBearMask), typeof(HuntersHeaddress), typeof(SpiritOfTheTotem),
					typeof(VioletCourage), typeof(GlovesOfThePugilist)
				}
			},
			{
				"Misc",
				new[]
				{
					typeof(AlchemistsBauble), typeof(IolosLute), typeof(GwennosHarp),
					typeof(ShieldOfInvulnerability),
				}
			}
		};
	}
}
