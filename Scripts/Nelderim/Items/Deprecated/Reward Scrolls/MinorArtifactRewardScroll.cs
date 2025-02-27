using System;

namespace Server.Items
{
	public class MinorArtifactRewardScroll : ArtifactRewardScroll
	{
		private static Type[] m_Artifacts = new Type[]
		{
			typeof(GoldBricks), typeof(PhillipsWoodenSteed), typeof(AlchemistsBauble), typeof(ArcticDeathDealer),
			typeof(BlazeOfDeath), typeof(BowOfTheJukaKing), typeof(BurglarsBandana), typeof(CavortingClub),
			typeof(EnchantedTitanLegBone), typeof(GwennosHarp), typeof(IolosLute), typeof(LunaLance),
			typeof(NightsKiss), typeof(NoxRangersHeavyCrossbow), typeof(OrcishVisage), typeof(PolarBearMask),
			typeof(ShieldOfInvulnerability), typeof(StaffOfPower), typeof(VioletCourage), typeof(HeartOfTheLion),
			typeof(WrathOfTheDryad), typeof(PixieSwatter), typeof(GlovesOfThePugilist)
		};

		private static string[] m_ArtifactsNames = new string[]
		{
			" Gold Bricks ", " Phillip's Wooden Steed ", " Alchemist's Bauble ", " Arctic Death Dealer ",
			" Blaze Of Death ", " Bow Of The Juka King ", " Burglar's Bandana ", " Cavorting Club ",
			" Enchanted Titan Leg Bone ", " Gwenno's Harp ", " Iolo's Lute ", " Luna Lance ", " Night's Kiss ",
			" Nox Rangers Heavy Crossbow ", " Orcish Visage ", " Polar Bear Mask ", " Shield Of Invulnerability ",
			" Staff Of Power ", " Violet Courage ", " Heart Of The Lion ", " Wrath Of The Dryad ",
			" Pixie Swatter ", " Gloves Of The Pugilist "
		};

		[Constructable]
		public MinorArtifactRewardScroll(bool chosen) : base(chosen)
		{
			if (chosen)
				base.Hue = 1258;
			else
				base.Hue = 1260;
		}

		public MinorArtifactRewardScroll() : this(false)
		{
		}

		public MinorArtifactRewardScroll(Serial serial) : base(serial)
		{
		}

		public override Type[] Artifacts => m_Artifacts;
		public override string[] ArtifactsNames => m_ArtifactsNames;
		public override string RewardScrollInfo => "Zwoj Mniejszego Artefaktu";
		public override string RewardInfo => "Pomniejszy Arefakt";

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
