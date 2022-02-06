using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Gumps
{
	public class MajorArtifactGump : ArtifactGump
	{
		public MajorArtifactGump(Mobile from, Item deed) : base(from, deed)
		{
		}

		protected override Type gumpType => typeof(MajorArtifactGump);

		protected override SortedList<string, Type[]> artifacts => new SortedList<string, Type[]>
		{
			{
				"Weapons",
				new[]
				{
					typeof(AxeOfTheHeavens), typeof(BladeOfInsanity), typeof(BladeOfTheRighteous),
					typeof(BoneCrusher), typeof(BreathOfTheDead), typeof(Frostbringer),
					typeof(LegacyOfTheDreadLord), typeof(SerpentsFang), typeof(StaffOfTheMagi),
					typeof(TheBeserkersMaul), typeof(TheDragonSlayer), typeof(TitansHammer), typeof(TheTaskmaster),
					typeof(ZyronicClaw),
				}
			},
			{
				"Armor",
				new[]
				{
					typeof(ArmorOfFortune), typeof(GauntletsOfNobility), typeof(HelmOfInsight),
					typeof(HolyKnightsBreastplate), typeof(JackalsCollar), typeof(LeggingsOfBane),
					typeof(MidnightBracers), typeof(OrnateCrownOfTheHarrower), typeof(ShadowDancerLeggings),
					typeof(InquisitorsResolution), typeof(TunicOfFire), typeof(VoiceOfTheFallenKing),
				}
			},
			{
				"Jewelery",
				new[]
				{
					typeof(BraceletOfHealth), typeof(OrnamentOfTheMagician), typeof(RingOfTheElements),
					typeof(RingOfTheVile),
				}
			},
			{ "Shields", new[] { typeof(Aegis), typeof(ArcaneShield), } },
			{
				"Hats & Masks",
				new[]
				{
					typeof(DivineCountenance), typeof(HatOfTheMagi), typeof(HuntersHeaddress),
					typeof(SpiritOfTheTotem)
				}
			}
		};
	}
}
