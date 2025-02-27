using System;

namespace Server.Items
{
	public class MajorArtifactRewardScroll : ArtifactRewardScroll
	{
		private static Type[] m_Artifacts = new Type[]
		{
			typeof(ArcaneShield), typeof(ArmorOfFortune), typeof(AxeOfTheHeavens), typeof(BladeOfInsanity),
			typeof(BoneCrusher), typeof(BraceletOfHealth), typeof(BreathOfTheDead), typeof(DivineCountenance),
			typeof(Frostbringer), typeof(GauntletsOfNobility), typeof(HatOfTheMagi), typeof(HelmOfInsight),
			typeof(HolyKnightsBreastplate), typeof(HuntersHeaddress), typeof(JackalsCollar),
			typeof(LegacyOfTheDreadLord), typeof(LeggingsOfBane), typeof(MidnightBracers),
			typeof(OrnamentOfTheMagician), typeof(OrnateCrownOfTheHarrower), typeof(RingOfTheElements),
			typeof(RingOfTheVile), typeof(SerpentsFang), typeof(ShadowDancerLeggings), typeof(SpiritOfTheTotem),
			typeof(StaffOfTheMagi), typeof(TheBeserkersMaul), typeof(TheDragonSlayer), typeof(TheDryadBow),
			typeof(TheTaskmaster), typeof(TunicOfFire), typeof(VoiceOfTheFallenKing), typeof(Aegis)
		};

		private static string[] m_ArtifactsNames = new string[]
		{
			" Arcane Shield ", " Armor Of Fortune ", " Axe Of The Heavens ", " Blade Of Insanity ",
			" Bone Crusher ", " Bracelet Of Health ", " Breath Of The Dead ", " Divine Countenance ",
			" Frostbringer ", " Gauntlets Of Nobility ", " Hat Of The Magi ", " Helm Of Insight ",
			" Holy Knight's Breastplate ", " Hunter's Headdress ", " Jackal's Collar ",
			" Legacy Of The Dread Lord ", " Leggings Of Bane ", " Midnight Bracers ", " Ornament Of The Magician ",
			" Ornate Crown Of The Harrower ", " Ring Of The Elements ", " Ring Of The Vile ", " Serpent's Fang ",
			" Shadow Dancer Leggings ", " Spirit Of The Totem ", " Staff Of The Magi ", " The Beserker's Maul ",
			" The Dragon Slayer ", " The Dryad Bow ", " The Taskmaster ", " Tunic Of Fire ",
			" Voice Of The Fallen King ", " Aegis "
		};

		[Constructable]
		public MajorArtifactRewardScroll(bool chosen) : base(chosen)
		{
			if (chosen)
				base.Hue = 1243;
			else
				base.Hue = 1237;
		}

		public MajorArtifactRewardScroll() : this(false)
		{
		}

		public MajorArtifactRewardScroll(Serial serial) : base(serial)
		{
		}

		public override Type[] Artifacts => m_Artifacts;
		public override string[] ArtifactsNames => m_ArtifactsNames;
		public override string RewardScrollInfo => "Zwoj Wiekszego Artefaktu";
		public override string RewardInfo => "Wiekszy Arefakt";

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();
		}
	}
}
