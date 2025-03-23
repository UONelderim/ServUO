using System;
using Server.ACC.CSS.Systems.Ancient;
using Server.ACC.CSS.Systems.Avatar;
using Server.ACC.CSS.Systems.Bard;
using Server.ACC.CSS.Systems.Cleric;
using Server.ACC.CSS.Systems.Druid;
using Server.ACC.CSS.Systems.Ranger;
using Server.ACC.CSS.Systems.Rogue;
using Server.ACC.CSS.Systems.Undead;
using Server.Items;

namespace Server.Engines.Craft
{
	public partial class DefInscription
	{
		public void SetMinChanceForScrollAtGM(double minSkill, ref double maxSkill)
		{
			// make sure we have at least 66.7% chance to make a scroll at GM skill:
			if ((100.0 - minSkill) / (maxSkill - minSkill) < 0.667)
				maxSkill = minSkill + (100.0 - minSkill) / 0.667;
		}
		
		private void AddBushidoSpell(int spell, int mana, double minSkill, Type type, int[] amounts, params Type[] regs)
		{
			double maxSkill = minSkill + 50;
			SetMinChanceForScrollAtGM(minSkill, ref maxSkill);

			AddSpell(spell, mana, minSkill, maxSkill, "Tradycja wojenna", 1060595, type, amounts, regs);
		}

		private void AddNinjitsuSpell(int spell, int mana, double minSkill, Type type, int[] amounts, params Type[] regs)
		{
			double maxSkill = minSkill + 50;
			SetMinChanceForScrollAtGM(minSkill, ref maxSkill);

			AddSpell(spell, mana, minSkill, maxSkill, "Skrytobojstwo", 1060610, type, amounts, regs);
		}

		private void AddChivalrySpell(int spell, int mana, double minSkill, Type type, int[] amounts, params Type[] regs)
		{
			double maxSkill = minSkill + 50;
			SetMinChanceForScrollAtGM(minSkill, ref maxSkill);

			AddSpell(spell, mana, minSkill, maxSkill, "Rycerstwo", 1060493, type, amounts, regs);
		}
		
		private void AddSpell(int spell, int mana, double minSkill, double maxSkill, TextDefinition group, TextDefinition start, Type type, int[] amounts, params Type[] regs)
		{
			if (amounts == null)
			{
				amounts = new int[regs.Length];
				for (int i = 0, count = amounts.Length; i < count; i++)
					amounts[i] = 1;
			}

			//int regIndex = Array.IndexOf( Reagent.Types, regs[0].Type );
			int index = AddCraft(type, group, start + spell, minSkill, maxSkill, regs[0], GetNameRes(regs[0]), amounts[0], 501627);

			for (int i = 1, count = regs.Length; i < count; i++)
			{
				Type regType = regs[i];
				AddRes(index, regType, GetNameRes(regType), amounts[i], 501627);
			}

			AddRes(index, typeof(BlankScroll), 1044377, 1, 1044378);

			SetManaReq(index, mana);
		}
		
		private TextDefinition GetNameRes(Type type)
		{
			int id = CraftItem.ItemIDOf(type);

			return id < 0x4000 ? 1020000 + id : 1078872 + id;
		}

		private void InitNelderimSpells()
		{
			AddChivalrySpell(0, 10, 0, typeof(CleanseByFireScroll), new int[] { 10, 5, 5 }, typeof(SulfurousAsh), typeof(Amber), typeof(Ruby));
            AddChivalrySpell(1, 10, 5, typeof(CloseWoundsScroll), new int[] { 10, 5, 5 }, typeof(MandrakeRoot), typeof(Amethyst), typeof(Citrine));
            AddChivalrySpell(2, 10, 15, typeof(ConsecrateWeaponScroll), new int[] { 20, 5, 10 }, typeof(PigIron), typeof(Sapphire), typeof(Citrine));
            AddChivalrySpell(3, 15, 35, typeof(DispelEvilScroll), new int[] { 20, 10, 10 }, typeof(DaemonBlood), typeof(Ruby), typeof(Tourmaline));
            AddChivalrySpell(4, 15, 25, typeof(DivineFuryScroll), new int[] { 20, 10, 10 }, typeof(NoxCrystal), typeof(Emerald), typeof(Amethyst));
            AddChivalrySpell(5, 20, 45, typeof(EnemyOfOneScroll), new int[] { 20, 10, 15 }, typeof(BatWing), typeof(Diamond), typeof(Emerald));
            AddChivalrySpell(6, 15, 55, typeof(HolyLightScroll), new int[] { 20, 30, 10, 15 }, typeof(NoxCrystal), typeof(BlackPearl), typeof(Diamond), typeof(Sapphire));
            AddChivalrySpell(7, 20, 65, typeof(NobleSacrificeScroll), new int[] { 20, 30, 10, 20 }, typeof(GraveDust), typeof(MandrakeRoot), typeof(StarSapphire), typeof(Diamond));
            AddChivalrySpell(8, 20, 5, typeof(RemoveCurseScroll), new int[] { 20, 5, 5 }, typeof(Garlic), typeof(Tourmaline), typeof(Emerald));
            AddChivalrySpell(9, 20, 15, typeof(SacredJourneyScroll), new int[] { 20, 5, 10 }, typeof(GraveDust), typeof(StarSapphire), typeof(Ruby));

            AddBushidoSpell(0, 10, 25, typeof(HonorableExecutionScroll), new int[] { 10, 5, 5 }, typeof(Ginseng), typeof(Amber), typeof(Citrine));
            AddBushidoSpell(1, 10, 25, typeof(ConfidenceScroll), new int[] { 15, 5, 5 }, typeof(BlackPearl), typeof(Ruby), typeof(Amber));
            AddBushidoSpell(2, 10, 60, typeof(EvasionScroll), new int[] { 15, 10, 10 }, typeof(GraveDust), typeof(Sapphire), typeof(Emerald));
            AddBushidoSpell(3, 5, 40, typeof(CounterAttackScroll), new int[] { 15, 5, 10 }, typeof(NoxCrystal), typeof(Amethyst), typeof(Ruby));
            AddBushidoSpell(4, 5, 50, typeof(LightningStrikeScroll), new int[] { 15, 5, 10 }, typeof(NoxCrystal), typeof(Amethyst), typeof(Ruby));
            AddBushidoSpell(5, 10, 70, typeof(MomentumStrikeScroll), new int[] { 20, 10, 10 }, typeof(DaemonBlood), typeof(Diamond), typeof(StarSapphire));

            AddNinjitsuSpell(0, 20, 30, typeof(FocusAttackScroll), new int[] { 20, 20, 15}, typeof(PigIron), typeof(NoxCrystal), typeof(Amethyst));
            AddNinjitsuSpell(1, 30, 85, typeof(DeathStrikeScroll), new int[] { 20, 30, 20 }, typeof(DaemonBlood), typeof(Nightshade), typeof(Diamond));
            AddNinjitsuSpell(2, 10, 20, typeof(AnimalFormScroll), new int[] { 10, 10, 5 }, typeof(Nightshade), typeof(DaemonBlood), typeof(Citrine));
            AddNinjitsuSpell(3, 25, 80, typeof(KiAttackScroll), new int[] { 20, 20, 20 }, typeof(GraveDust), typeof(BatWing), typeof(StarSapphire));
            AddNinjitsuSpell(4, 20, 60, typeof(SurpriseAttackScroll), new int[] { 20, 10, 10 }, typeof(SpidersSilk), typeof(PigIron), typeof(Ruby));
            AddNinjitsuSpell(5, 30, 20, typeof(BackstabScroll), new int[] { 10, 20, 5 }, typeof(BatWing), typeof(Bloodmoss), typeof(Amber));
            AddNinjitsuSpell(6, 15, 50, typeof(ShadowJumpScroll), new int[] { 20, 20, 15 }, typeof(Ginseng), typeof(Amber), typeof(Citrine));
            AddNinjitsuSpell(7, 10, 40, typeof(MirrorImageScroll), new int[] { 20, 10, 10 }, typeof(BlackPearl), typeof(NoxCrystal), typeof(Tourmaline));
            
            //Custom Spelle
            int index = 0;
            //Okultyzm
            index = AddCraft( typeof( UndeadAngelicFaithScroll ), "Umiejetnosci specjalne", "Demoniczny Awatar", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            index = AddCraft( typeof( UndeadVolcanicEruptionScroll ), "Umiejetnosci specjalne", "Erupcja Wulkaniczna", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( UndeadSwarmOfInsectsScroll ), "Umiejetnosci specjalne", "Chmara Insektów", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( UndeadSeanceScroll ), "Umiejetnosci specjalne", "Seans", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( UndeadNaturesPassageScroll ), "Umiejetnosci specjalne", "Ścieżka Śmierci", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( ZoogiFungus ), "grzyby zoogi" , 50, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( UndeadMushroomGatewayScroll ), "Umiejetnosci specjalne", "Limbo", 90.0, 120.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( UndeadLureStoneScroll ), "Umiejetnosci specjalne", "Gnijące Zwłoki", 90.0, 120.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( UndeadLeafWhirlwindScroll ), "Umiejetnosci specjalne", "Piętno", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            index = AddCraft( typeof( UndeadHollowReedScroll ), "Umiejetnosci specjalne", "Hedonizm", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( UndeadHammerOfFaithScroll ), "Umiejetnosci specjalne", "Sierp Wiary Smierci", 90.0, 120.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( UndeadGraspingRootsScroll ), "Umiejetnosci specjalne", "Uchwyt Zza Grobu", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( UndeadCauseFearScroll ), "Umiejetnosci specjalne", "Strach", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );

            //Avatar
            index = AddCraft( typeof( AvatarSacredBoonScroll ), "Umiejetnosci specjalne", "Święty znak", 80.0, 110.0, typeof( Corruption ), "korupcja" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AvatarRestorationScroll ), "Umiejetnosci specjalne", "Odrodzenie", 80.0, 110.0, typeof( Corruption ), "korupcja" , 20, 1044253 );
            AddRes( index, typeof( DaemonBone ), "kości demona" , 50, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AvatarMarkOfGodsScroll ), "Umiejetnosci specjalne", "Znak Bogów", 80.0, 110.0, typeof( WyrmsHeart ), "Serce Wyrma" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ZoogiFungus ), "grzyby zoogi" , 50, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AvatarHeavensGateScroll ), "Umiejetnosci specjalne", "Niebiańska Brama", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( GateTravelScroll ), "zwoje bramy" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AvatarHeavenlyLightScroll ), "Umiejetnosci specjalne", "Niebiańskie Światło", 50.0, 70.0, typeof( WyrmsHeart ), "Serce Wyrma" , 2, 1044253 );
            AddRes( index, typeof( NightSightPotion ), "mikstura widzenia w ciemności" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
           index = AddCraft( typeof( AvatarEnemyOfOneSpell ), "Umiejetnosci specjalne", "Naznaczony", 25.0, 76.0, typeof( Corruption ), "korupcja" , 20, 1044253 );
          AddRes( index, typeof( DaemonBone ), "kości demona" , 50, 1044253 );
          AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
         AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AvatarArmysPaeonScroll  ), "Umiejetnosci specjalne", "Witalność Armii", 80.0, 110.0, typeof( WyrmsHeart ), "Serce Wyrma" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ZoogiFungus ), "grzyby zoogi" , 50, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AvatarAngelicFaithScroll ), "Umiejetnosci specjalne", "Awatar Pradawnego Mnicha", 60.0, 100.0, typeof( LuminescentFungi ), "lśniące grzyby", 20  , 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( ZoogiFungus ), "grzyby zoogi" , 50, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AvatarBallScroll ), "Umiejetnosci specjalne", "Kula Sniegu", 25.0, 76.0, typeof( Corruption ), "korupcja" , 20, 1044253 );
            AddRes( index, typeof( DaemonBone ), "kości demona" , 50, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes(index, typeof(Gold), "złoto", 2000, 104425);
            index = AddCraft( typeof( AvatarCurseRemovalScroll ), "Umiejetnosci specjalne", "Reka Mnicha", 60.0, 100.0, typeof( LuminescentFungi ), "lśniące grzyby", 20  , 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( ZoogiFungus ), "grzyby zoogi" , 50, 1044253 );
			

            //Druid
            index = AddCraft( typeof( DruidBlendWithForestScroll  ), "Umiejetnosci specjalne", "Jedność Z Lasem", 80.0, 110.0, typeof( LuminescentFungi ), "lśniące grzyby" , 20, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            index = AddCraft( typeof( DruidFamiliarScroll ), "Umiejetnosci specjalne", "Przywołanie Przyjaciela Lasu", 80.0, 110.0, typeof( LuminescentFungi ), "lśniące grzyby"  , 20, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( DragonsBlood ), "krew smoka" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( DruidEnchantedGroveScroll), "Umiejetnosci specjalne", "Zaklęty Gaj", 90.0, 120.0, typeof( LuminescentFungi ), "lśniące grzyby"  , 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( DruidGraspingRootsScroll), "Umiejetnosci specjalne", "Szalone Korzenie", 80.0, 110.0, typeof( LuminescentFungi ), "lśniące grzyby"  , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( DruidHollowReedScroll), "Umiejetnosci specjalne", "Siła Natury", 80.0, 110.0, typeof( LuminescentFungi ), "lśniące grzyby"  , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( ZoogiFungus ), "grzyby zoogi" , 50, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( DruidLeafWhirlwindScroll ), "Umiejetnosci specjalne", "Wir Liści", 90.0, 120.0, typeof( LuminescentFungi ), "lśniące grzyby", 20 , 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( DarkSapphire ), "ciemny szafir" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( DruidLureStoneScroll ), "Umiejetnosci specjalne", "Ciekawy kamie", 90.0, 120.0, typeof( LuminescentFungi ), "lśniące grzyby"  , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( DruidMushroomGatewayScroll ), "Umiejetnosci specjalne", "Przejście Natury", 80.0, 110.0, typeof( LuminescentFungi ), "lśniące grzyby"  , 20, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            index = AddCraft( typeof( DruidNaturesPassageScroll), "Umiejetnosci specjalne", "Naznaczenie", 80.0, 110.0, typeof( LuminescentFungi ), "lśniące grzyby"  , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( DruidPackOfBeastScroll), "Umiejetnosci specjalne", "Leśne Bestyje", 60.0, 90.0, typeof( LuminescentFungi ), "lśniące grzyby"  , 20, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( DragonsBlood ), "krew smoka" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( DruidRestorativeSoilScroll ), "Umiejetnosci specjalne", "Lecznicza Ziemia", 90.0, 120.0, typeof( LuminescentFungi ), "lśniące grzyby"  , 20, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( DruidShieldOfEarthScroll ), "Umiejetnosci specjalne", "Tarcza Ziemi", 80.0, 110.0, typeof( LuminescentFungi ), "lśniące grzyby"  , 20, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30,1044253 );
            AddRes( index, typeof( DragonsBlood ), "krew smoka" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( DruidSpringOfLifeScroll ), "Umiejetnosci specjalne", "Źródło życia", 80.0, 110.0, typeof( LuminescentFungi ), "lśniące grzyby"  , 20, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( DragonsBlood ), "krew smoka" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( DruidStoneCircleScroll ), "Umiejetnosci specjalne", "Kamienny Krąg", 90.0, 120.0, typeof( LuminescentFungi ), "lśniące grzyby"  , 20, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( DragonsBlood ), "krew smoka" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( DruidSwarmOfInsectsScroll ), "Umiejetnosci specjalne", "Chmara Insektów", 80.0, 110.0, typeof( LuminescentFungi ), "lśniące grzyby"  , 20, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( DruidVolcanicEruptionScroll ), "Umiejetnosci specjalne", "Erupcja Wulkaniczna", 90.0, 120.0, typeof( LuminescentFungi ), "lśniące grzyby"  ,20, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( DragonsBlood ), "krew smoka" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );

            //Ranger
            index = AddCraft( typeof( RangerSummonMountScroll ), "Umiejetnosci specjalne", "Przyzwanie Wierzcha", 80.0, 110.0, typeof( DryIce ), "suchy lód" , 20, 1044253 );
            AddRes( index, typeof( DreadHornMane ), "Grzywa Spaczonego" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( RangerPhoenixFlightScroll ), "Umiejetnosci specjalne", "Lot Feniksa", 80.0, 110.0, typeof( DryIce ), "suchy lód" , 20, 1044253 );
            AddRes( index, typeof( TeleportScroll ), "zwoje teleporatcji" , 20, 1044253 );
            AddRes( index, typeof( ZoogiFungus ), "grzyby zoogi" , 50, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( RangerFamiliarScroll ), "Umiejetnosci specjalne", "Zwierzęcy kompan", 80.0, 110.0, typeof( DryIce ), "suchy lód" , 20, 1044253 );
            AddRes( index, typeof( DreadHornMane ), "Grzywa Spaczonego" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( RangerNoxBowScroll ), "Umiejetnosci specjalne", "Wężowy Łuk", 80.0, 110.0, typeof( DryIce ), "suchy lód" , 20, 1044253 );
            AddRes( index, typeof( DreadHornMane ), "Grzywa Spaczonego" , 10, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( RangerLightningBowScroll ), "Umiejetnosci specjalne", "Umiejetnosci specjalne", 80.0, 110.0, typeof( DryIce ), "suchy lód" , 20, 1044253 );
            AddRes( index, typeof( DreadHornMane ), "Grzywa Spaczonego" , 10, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( RangerIceBowScroll ), "Umiejetnosci specjalne", "Lodowy Łuk", 80.0, 110.0, typeof( DryIce ), "suchy lód" , 20, 1044253 );
            AddRes( index, typeof( DreadHornMane ), "Grzywa Spaczonego" , 10, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( RangerFireBowScroll ), "Umiejetnosci specjalne", "Ognisty Łuk", 80.0, 110.0, typeof( DryIce ), "suchy lód" , 20, 1044253 );
            AddRes( index, typeof( DreadHornMane ), "Grzywa Spaczonego" , 10, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( RangerHuntersAimScroll ), "Umiejetnosci specjalne", "Celność łowcy", 80.0, 110.0, typeof( DryIce ), "suchy lód" , 20, 1044253 );
            AddRes( index, typeof( DreadHornMane ), "Grzywa Spaczonego" , 10, 1044253 );
            AddRes( index, typeof( ParasiticPlant ), "paraliżujący bluszcz" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );

            //Ancient
            index = AddCraft( typeof( AncientCloneScroll ), "Umiejetnosci specjalne", "Klonowanie", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            index = AddCraft( typeof( AncientDanceScroll ), "Umiejetnosci specjalne", "Taniec", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AncientDeathVortexScroll), "Umiejetnosci specjalne", "Wir Śmierci", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AncientSeanceScroll ), "Umiejetnosci specjalne", "Seans", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AncientDouseSpell), "Umiejetnosci specjalne", "Wygaszenie", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( ZoogiFungus ), "grzyby zoogi" , 50, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AncientFireRingScroll), "Umiejetnosci specjalne", "Pierścień Ognia", 90.0, 120.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AncientEnchantScroll ), "Umiejetnosci specjalne", "Magiczne Nasycenie", 90.0, 120.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AncientGreatDouseScroll ), "Umiejetnosci specjalne", "Większe Wygaszenie", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            index = AddCraft( typeof( AncientGreatIgniteScroll ), "Umiejetnosci specjalne", "Większe Podpalenie", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AncientIgniteScroll ), "Umiejetnosci specjalne", "Podpalenie", 90.0, 120.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AncientMassMightSpell ), "Umiejetnosci specjalne", "Masowa Potęga", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AncientCauseFearScroll ), "Umiejetnosci specjalne", "Strach", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AncientPeerScroll ), "Umiejetnosci specjalne", "Wizja", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( AncientSwarmScroll ), "Umiejetnosci specjalne", "Rój", 90.0, 120.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );

            //Bard
            index = AddCraft( typeof( BardArmysPaeonScroll ), "Umiejetnosci specjalne", "Śpiew Armii", 80.0, 110.0, typeof( Corruption ), "korupcja" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( BardEnchantingEtudeScroll), "Umiejetnosci specjalne", "Wzmacniająca Etiuda", 80.0, 110.0, typeof( Corruption ), "korupcja" , 20, 1044253 );
            AddRes( index, typeof( DaemonBone ), "kości demona" , 50, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( BardEnergyCarolScroll ), "Umiejetnosci specjalne", "Pobudzająca Pieśń", 80.0, 110.0, typeof( WyrmsHeart ), "Serce Wyrma" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ZoogiFungus ), "grzyby zoogi" , 50, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( BardEnergyThrenodyScroll ), "Umiejetnosci specjalne", "Porażający Tren", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( GateTravelScroll ), "zwoje bramy" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( BardFireThrenodyScroll ), "Umiejetnosci specjalne", "Palący Tren", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            index = AddCraft( typeof( BardFoeRequiemScroll ), "Umiejetnosci specjalne", "Soniczny Podmuch", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( BardIceCarolScroll), "Umiejetnosci specjalne", "Pieśń Lodu", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( BardIceThrenodyScroll ), "Umiejetnosci specjalne", "Lodowy Tren", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( BardKnightsMinneScroll ), "Umiejetnosci specjalne", "Wzmacniający Okrzyk", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( BardMagesBalladScroll ), "Umiejetnosci specjalne", "Pieśń Do Magów", 80.0, 110.0, typeof( Corruption ), "korupcja" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( BardMagicFinaleScroll ), "Umiejetnosci specjalne", "Magiczny Finał", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( BardPoisonCarolScroll), "Umiejetnosci specjalne", "Wężowa Pieśń", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( BardPoisonThrenodyScroll ), "Umiejetnosci specjalne", "Tren Jadu", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( BardSheepfoeMamboScroll ), "Umiejetnosci specjalne", "Pasterska Przyśpiewka", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( BardSinewyEtudeScroll ), "Umiejetnosci specjalne", "Przyśpiewka Górników", 80.0, 110.0, typeof( Taint ), "skaza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );

            //Cleric
            index = AddCraft( typeof( ClericAngelicFaithScroll ), "Umiejetnosci specjalne", "Anielska Wiara", 80.0, 110.0, typeof( Blight ), "zaraza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            index = AddCraft( typeof( ClericBanishEvilScroll ), "Umiejetnosci specjalne", "Wygnanie zła", 80.0, 110.0, typeof( Blight ), "zaraza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( ClericDampenSpiritScroll), "Umiejetnosci specjalne", "Stłumienie Ducha", 80.0, 110.0, typeof( Blight ), "zaraza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( ClericDivineFocusScroll ), "Umiejetnosci specjalne", "Boskie Skupienie", 80.0, 110.0, typeof( Blight ), "zaraza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( ClericHammerOfFaithScroll), "Umiejetnosci specjalne", "Topór Wiary", 80.0, 110.0, typeof( Blight ), "zaraza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( ZoogiFungus ), "grzyby zoogi" , 50, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( ClericPurgeScroll ), "Umiejetnosci specjalne", "Czystka", 90.0, 120.0, typeof( Blight ), "zaraza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( ClericRestorationScroll ), "Umiejetnosci specjalne", "Odrodzenie", 90.0, 120.0, typeof( Blight ), "zaraza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( ClericSacredBoonScroll ), "Umiejetnosci specjalne", "Święty znak", 80.0, 110.0, typeof( Blight ), "zaraza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            index = AddCraft( typeof( ClericSacrificeScroll ), "Umiejetnosci specjalne", "Poświęcenie", 80.0, 110.0, typeof( Blight ), "zaraza" , 20, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( ClericSmiteScroll ), "Umiejetnosci specjalne", "Smagnięcie", 90.0, 120.0, typeof( Blight ), "zaraza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( ClericTouchOfLifeScroll ), "Umiejetnosci specjalne", "Dotyk Życia", 80.0, 110.0, typeof( Blight ), "zaraza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft( typeof( ClericTrialByFireScroll ), "Umiejetnosci specjalne", "Próba Ognia", 80.0, 110.0, typeof( Blight ), "zaraza" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );

            //Rogue
            index = AddCraft( typeof( RogueFalseCoinScroll ), "Umiejetnosci specjalne", "Falszywa moneta", 50.0, 70.0, typeof( Gold ), "zloto" , 2000, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            index = AddCraft( typeof( RogueShieldOfEarthScroll ), "Umiejetnosci specjalne", "Klody pod nogi", 70.0, 100.0, typeof( Gold ), "zloto" , 2000, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 1, 1044253 );
            index = AddCraft( typeof( RogueCharmScroll), "Umiejetnosci specjalne", "Zaklecie", 70.0, 100.0, typeof( Gold ), "zloto" , 2000, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 1, 1044253 );
            index = AddCraft( typeof( RogueSlyFoxScroll ), "Umiejetnosci specjalne", "Przebiegła forma", 70.0, 100.0, typeof( Gold ), "zloto" , 2000, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 1, 1044253 );
            index = AddCraft( typeof( RogueShadowScroll ), "Umiejetnosci specjalne", "Cien", 70.0, 100.0, typeof( Gold ), "zloto" , 2000, 1044253 );
            AddRes( index, typeof( BladeSpiritsScroll ), "zwoj ducha ostrzy" , 5, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgnila kora" , 1, 1044253 );
            index = AddCraft( typeof( RogueIntimidationScroll ), "Umiejetnosci specjalne", "Zastraszenie", 70.0, 100.0, typeof( Gold ), "zloto" , 2000, 1044253 );
            AddRes( index, typeof( CapturedEssence ), "złapana esencja" , 2, 1044253 );
            AddRes( index, typeof( PowderOfTranslocation ), "proszek translokacji" , 10, 1044253 );
		}
	}
}
