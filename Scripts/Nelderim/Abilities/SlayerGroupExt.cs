#region References

using System;
using System.Linq;
using Server.Mobiles;

#endregion

namespace Server.Items
{
	public partial class SlayerGroup
	{
		private static void FillNelderimSlayerEntries(ref SlayerGroup humanoid, ref SlayerGroup undead,
			ref SlayerGroup elemental,
			ref SlayerGroup abyss, ref SlayerGroup arachnid, ref SlayerGroup reptilian, ref SlayerGroup fey,
			ref SlayerGroup eodon,
			ref SlayerGroup eodonTribe, ref SlayerGroup dino, ref SlayerGroup myrmidex)
		{
			AddSuperTypes(humanoid, typeof(KapitanIIILegionuOrkow), typeof(KorahaTilkiDancer), typeof(KorahaTilkiLord), typeof(KorahaTilkiPeasant), typeof(KorahaTilkiPikador), typeof(KorahaTilkiShaman), typeof(KorahaTilkiSpearman), typeof(KorahaTilkiWarrior), typeof(KorahaTilkiXBowmen),
				typeof(HungaNekahiCavalry), typeof(HungaNekahiLord), typeof(HungaNekahiMage), typeof(HungaNekahiOverseer), typeof(HungaNekahiPirate), typeof(HungaNekahiServant), typeof(HungaNekahiWarrior), typeof(HungaNekahiXBowmen),
				typeof(BagusGagakArcher), typeof(BagusGagakFencer), typeof(BagusGagakLightCav), typeof(BagusGagakLord), typeof(BagusGagakLumberjack), typeof(BagusGagakNinja), typeof(BagusGagakShaman), typeof(BagusGagakWarrior),
				typeof(VitVarg), typeof(VitVargAmazon), typeof(VitVargArcher), typeof(VitVargBerserker), typeof(VitVargCook), typeof(VitVargCutler), typeof(VitVargLord), typeof(VitVargMage), typeof(VitVargWarrior), typeof(VitVargWorker),
				typeof(PSavage), typeof(PSavage1), typeof(PSavageRider), typeof(PSavageShaman), typeof(FieryGoblinSapper), typeof(GoblinSapper), typeof(Goblin), typeof(GoblinWarrior), typeof(LesserGoblinSapper),  
				typeof(PirateCaptain), typeof(PirateCrew), typeof(NPrzeklety), typeof(NZapomniany), typeof(MinotaurBoss), typeof(Minotaur), typeof(MinotaurCaptain), typeof(MinotaurLord), typeof(MinotaurMage), typeof(MinotaurScout), typeof(TormentedMinotaur), typeof(Meraktus), 
				typeof(LucznikMorrlok), typeof(KusznikMorrlok), typeof(LordMorrlok), typeof(MagMorrlok), typeof(JezdziecMorrlok), typeof(MordercaMorrlok), typeof(WojownikMorrlok));
			AddEntryTypes(humanoid, SlayerName.OrcSlaying, typeof(KapitanIIILegionuOrkow), typeof(FieryGoblinSapper), typeof(GoblinSapper), typeof(Goblin), typeof(GoblinWarrior), typeof(LesserGoblinSapper));
			AddEntryTypes(humanoid, SlayerName.TrollSlaughter, typeof(TrollLord));
			AddSuperTypes(undead, typeof(SaragAwatar), typeof(NSarag), typeof(MonstrousInterredGrizzle), typeof(Mummy2), typeof(Mummy3), typeof(Boner), typeof(RedDeath), typeof(UnfrozenMummy));
			AddSuperTypes(elemental, typeof(AgapiteColossus), typeof(BronzeColossus),
				typeof(BronzeColossus), typeof(DullCopperColossus), typeof(GoldenColossus), typeof(ShadowIronColossus),
				typeof(ValoriteColossus), typeof(VeriteColossus));
			AddEntryTypes(elemental, SlayerName.EarthShatter, typeof(AgapiteColossus), typeof(BronzeColossus),
				typeof(BronzeColossus), typeof(DullCopperColossus), typeof(GoldenColossus), typeof(ShadowIronColossus),
				typeof(ValoriteColossus), typeof(VeriteColossus));
			AddEntryTypes(undead, SlayerName.Exorcism, 
				typeof(LesserArcaneDaemon), 
				typeof(xCommonArcaneDaemon), 
				typeof(GreaterArcaneDaemon), 
				typeof(LesserHordeDaemon), 
				typeof(CommonHordeDaemon), 
				typeof(GreaterHordeDaemon), 
				typeof(LesserDaemon), 
				typeof(CommonDaemon), 
				typeof(GreaterDaemon), 
				typeof(LesserChaosDaemon), 
				typeof(xCommonChaosDaemon), 
				typeof(GreaterChaosDaemon), 
				typeof(xMoloch), 
				typeof(LesserMoloch), 
				typeof(CommonMoloch), 
				typeof(GreaterMoloch),
				// bossy:
				typeof(NDeloth), typeof(NDzahhar), typeof(NKatrill), typeof(WladcaDemonow), typeof(WladcaJezioraLawy));
			AddEntryTypes(arachnid, SlayerName.ArachnidDoom, typeof(NSzeol), 
				typeof(Arachne), typeof(PomiotPajaka), typeof(SkorpionKrolewski));	
			AddEntryTypes(arachnid, SlayerName.SpidersDeath, typeof(Arachne), typeof(NSzeol),typeof(PomiotPajaka));
			AddEntryTypes(reptilian, SlayerName.ReptilianDeath, typeof(NelderimDragon),
				typeof(LodowySmok), typeof(MlodyLodowySmok), typeof(PrastaryLodowySmok), typeof(StaryLodowySmok),
				typeof(NStarozytnyLodowySmok), // boss
				typeof(OgnistyNiewolnik), typeof(OgnistySzaman), typeof(OgnistyWojownik),
				typeof(MlodyOgnistySmok), typeof(OgnistySmok), typeof(PrastaryOgnistySmok), typeof(StaryOgnistySmok),
				typeof(NStarozytnySmok), // boss
				typeof(AmethystDragon), typeof(AmethystDrake), typeof(DiamondDragon), typeof(DiamondDrake),
				typeof(EmeraldDragon), typeof(EmeraldDrake), typeof(RubyDragon), typeof(RubyDrake),
				typeof(SapphireDragon), typeof(SapphireDrake), typeof(GreaterDragon),
				typeof(StarozytnyDiamentowySmok), //boss
				typeof(NelderimSkeletalDragon), // boss
				typeof(Rikktor), typeof(NChimera)); // champion
			AddEntryTypes(reptilian,SlayerName.DragonSlaying, typeof(NelderimDragon), typeof(Reptalon),
				typeof(LodowySmok),
				typeof(MlodyLodowySmok),
				typeof(PrastaryLodowySmok),
				typeof(StaryLodowySmok),
				typeof(MlodyOgnistySmok),
				typeof(OgnistySmok),
				typeof(PrastaryOgnistySmok),
				typeof(StaryOgnistySmok), typeof(GreaterDragon),  typeof(NStarozytnyLodowySmok), // boss
				typeof(NStarozytnySmok), // boss
				typeof(StarozytnyDiamentowySmok), //boss
				typeof(NelderimSkeletalDragon), // boss
				typeof(Rikktor), // champion
				typeof(AmethystDragon), typeof(AmethystDrake), typeof(DiamondDragon), typeof(DiamondDrake), typeof(EmeraldDragon), typeof(EmeraldDrake), typeof(RubyDragon), typeof(RubyDrake), typeof(SapphireDragon),  typeof(SapphireDrake)
			);
			AddEntryTypes(reptilian, SlayerName.LizardmanSlaughter, typeof(OgnistyNiewolnik), typeof(OgnistySzaman), typeof(OgnistyWojownik));
			AddEntryTypes(fey,SlayerName.Fey, typeof (UpadlyJednorozec), typeof (UpadlyKirin));

		}

		private static void AddSuperTypes(SlayerGroup group, params Type[] newTypes)
		{
			group.Super = new SlayerEntry(group.Super.Name, group.Super.Types.Concat(newTypes).ToArray());
		}

		private static void AddEntryTypes(SlayerGroup group, SlayerName slayerName, params Type[] newTypes)
		{
			for (int i = 0; i < group.Entries.Length; i++)
			{
				if (group.Entries[i].Name == slayerName)
				{
					var slayerEntry = group.Entries[i];
					group.Entries[i] = new SlayerEntry(slayerEntry.Name, slayerEntry.Types.Concat(newTypes).ToArray());
				}
			}
		}
	}
}
