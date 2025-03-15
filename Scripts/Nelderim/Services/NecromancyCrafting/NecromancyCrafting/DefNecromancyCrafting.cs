using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Craft
{
    public class DefNecromancyCrafting : CraftSystem
    {
        public static readonly double PowderDropChance = 0.05;
        public override SkillName MainSkill
        {
            get { return SkillName.Necromancy; }
        }

        public override string GumpTitleString
        {
            get { return "<CENTER><BASEFONT COLOR=#FFFFFF>MENU TWORZENIA NIEUMARLYCH</BASEFONT></CENTER>"; } // <CENTER>INSCRIPTION MENU</CENTER>
        }

        private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem
        {
            get
            {
                if (m_CraftSystem == null)
                {
	                m_CraftSystem = new DefNecromancyCrafting();
                }

                return m_CraftSystem;
            }
        }

        public override double GetChanceAtMin(CraftItem item)
        {
            return 0.0; // 0%
        }

        private DefNecromancyCrafting() : base(1, 1, 1.25) // base( 1, 2, 1.7 )
        {
        }

        public override int CanCraft(Mobile from, ITool tool, Type itemType)
        {
	        if ( from.Mounted )
	        {
		        return 1072018;  // Nie mozesz wykonywac tej czynnosci bedac konno!
	        }
	        
	        var num = 0;
            if (tool == null || tool.Deleted || tool.UsesRemaining < 0)
            {
	            return 1044038; // You have worn out your tool!
            }
            if (!(from is PlayerMobile && from.Skills[SkillName.Necromancy].Base >= 20.0))
            {
	            return 1044153; // You don't have the required skill
            }
            if (!tool.CheckAccessible(from, ref num))
            {
	            return 1044263; // The tool must be on your person to use.
            }

            return num;
        }

        public override void PlayCraftEffect(Mobile from)
        {
            from.PlaySound(0x247); // magic
            
        }

        public override int PlayEndingEffect(Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality,
            bool makersMark, CraftItem item)
        {
            if (toolBroken)
            {
	            from.SendLocalizedMessage(1044038); // You have worn out your tool
            }

            if (failed)
            {
                from.PlaySound(65); // rune breaking
                if (lostMaterial)
                {
	                return 1044043; // You failed to create the item, and some of your materials are lost.
                }
                else
                {
	                return 1044157; // You failed to create the item, but no materials were lost.
                }
            }
            else
            {
                from.PlaySound(65); // rune breaking
                if (quality == 0)
                {
	                return 502785; // You were barely able to make this item.  It's quality is below average.
                }
                else if (makersMark && quality == 2)
                {
	                return 1044156; // You create an exceptional quality item and affix your maker's mark.
                }
                else if (quality == 2)
                {
	                return 1044155; // You create an exceptional quality item.
                }
                else
                {
	                return 1044154; // You create the item.
                }
            }
        }
        
        public override void InitCraftList()
        {
            int index = AddCraft(typeof(SkeletonCrystal), "Krysztaly", "Krysztal Szkieleta", 20.0, 120.0,
                typeof(NoxCrystal), "Krysztal Trucizny", 1, "Nie masz wystarczajaco duzo krysztalow trucizny.");
            AddRes(index, typeof(SkeletonPowder), "Proch szkieleta", 1,
                "Nie masz wystarczajaco duzej ilosci prochu szkieleta.");

            index = AddCraft(typeof(BoneKnightCrystal), "Krysztaly", "Krysztal Koscianego Rycerza", 30.0, 120.0,
                typeof(CrystallineFragments), "Fragmenty Krysztalow", 1,
                "Nie masz wystarczajaco duzo fragmentow krysztalow.");
            AddRes(index, typeof(BoneKnightPowder), "Proch koscianego rycerza", 1,
                "Nie masz wystarczajaco duzej ilosci prochu koscianego rycerza.");

            index = AddCraft(typeof(BoneMagiCrystal), "Krysztaly", "Krysztal Koscianego Maga", 40.0, 120.0,
                typeof(Ruby), "rubin", 1, "Nie masz wystarczajaco duzej ilosci rubinow.");
            AddRes(index, typeof(BoneMagiPowder), "Proch kościanego maga", 1,
                "Nie masz wystarczajaco duzej ilosci prochu kościanego maga.");

            index = AddCraft(typeof(MummyCrystal), "Krysztaly", "Krysztal Mumii", 50.0, 120.0,
                typeof(Emerald), "szmaragd", 1, "Nie masz wystarczajaco duzej ilosci szmaragdow.");
            AddRes(index, typeof(MummyPowder), "Proch mumii", 1,
                "Nie masz wystarczajaco duzej ilosci prochu mumii.");

            index = AddCraft(typeof(ZombieCrystal), "Krysztaly", "Krysztal Zombie", 20.0, 120.0,
                typeof(Amber), "Bursztyn", 1,
                "Nie masz wystarczajaco duzej ilosci bursztynów.");
            AddRes(index, typeof(ZombiePowder), "Proch zombie", 1,
                "Nie masz wystarczajaco duzej ilosci prochu zombie.");

            index = AddCraft(typeof(LichCrystal), "Krysztaly", "Krysztal Licza", 70.0, 120.0,
                typeof(ReceiverCrystal), "Krysztal Komunikacyjny - Sluchacz", 1,
                "Nie masz wystarczajaco duzej ilosci krysztalow komunikacyjnych.");
            AddRes(index, typeof(LichPowder), "Proch licza", 1,
                "Nie masz wystarczajaco duzej ilosci prochu licza.");

            index = AddCraft(typeof(GhoulCrystal), "Krysztaly", "Krysztal Ghoula", 80.0, 120.0,
                typeof(PowerCrystal), "Krysztal Mocy", 1, "Nie masz wystarczajaco duzej ilosci krysztalow mocy.");
            AddRes(index, typeof(GhoulPowder), "Proch ghoula", 1,
                "Nie masz wystarczajaco duzej ilosci prochu ghoula.");

            index = AddCraft(typeof(BonerCrystal), "Krysztaly", "Krysztal Koscieja", 110.0, 130.0,
                typeof(ShimmeringCrystals), "Skrzący Się Kryształ", 1, "Nie masz wystarczajaco duzej ilosci skrzących się kryształów..");
            AddRes(index, typeof(BonerPowder), "Proch koscieja", 1,
                "Nie masz wystarczajaco duzej ilosci prochu koscieja.");

            index = AddCraft(typeof(AncientLichCrystal), "Krysztaly", "Krysztal Starozytnego Licza", 100.0, 120.0,
                typeof(SkeletonCrystal), "Krysztal Szkieleta", 1, "Nie masz wystarczajaco duzej ilosci krysztalow zla.");
            AddRes(index, typeof(AncientLichPowder), "Porch starozytnego licza", 1,
                "Nie masz wystarczajaco duzej ilosci prochu straozytnego licza.");

            //Skeleton Crafts
            index = AddCraft(typeof(SkeletonLegs), "Szkielety", "Nogi Szkieleta", 20.0, 120.0,
                typeof(Bone), "Kosc", 20, "Nie masz wystarczajaco duzej ilosci kosci.");
            AddRes(index, typeof(AnimateDeadScroll), "Zwoj wskrzeszenia zwlok (Nekromancja)", 1,
                "Nie masz wystarczajaco duzej ilosci zwojow nekromanckich.");
            AddSkill(index, SkillName.Anatomy, 20.0, 50.0);

            index = AddCraft(typeof(SkeletonTorso), "Szkielety", "Tułów szkieleta", 20.0, 120.0,
                typeof(Bone), "Kosc", 20, "Nie masz wystarczajaco duzej ilosci kosci.");
            AddRes(index, typeof(Skull), "czaszka", 1, "Nie masz wystarczajaco duzej ilosci czaszek.");
            AddRes(index, typeof(RibCage), "Klatka piersiowa", 1,
                "Nie masz wystarczajaco duzej ilosci klatek piersiowych.");
            AddRes(index, typeof(Spine), "Kregoslup", 1, "Nie masz wystarczajaco duzej ilosci kregoslupow.");
            AddSkill(index, SkillName.Anatomy, 20.0, 50.0);

            index = AddCraft(typeof(SkeletonMageTorso), "Szkielety", "Tułów szkieleta maga", 30.0, 130.0,
                typeof(SkeletonTorso), "Tułów szkieleta", 1, "Nie masz wystarczajaco duzej ilosci tułowiów.");
            AddRes(index, typeof(Jawbone), "szczęka", 1, "Nie masz wystarczajaco duzej ilosci szczęk.");
            AddSkill(index, SkillName.Anatomy, 30.0, 60.0);

            // Rotting Crafts
            index = AddCraft(typeof(RottingTorso), "Gnijące", "gnijący tułów", 20.0, 120.0,
                typeof(Head), "głowa", 1, "Nie masz wystarczajaco duzej ilosci głów.");
            AddRes(index, typeof(Torso), "Tułów", 1, "Nie masz wystarczajaco duzej ilosci tułowiów.");
            AddRes(index, typeof(RightArm), "prawa ręka", 1,
                "Nie masz wystarczajaco duzej ilosci prawych rąk (bo masz dwie lewe he he).");
            AddRes(index, typeof(LeftArm), "lewa ręka", 1,
                "Nie masz wystarczajaco duzej ilosci lewych rąk (a to też źle he he).");
            AddSkill(index, SkillName.Anatomy, 25.0, 50.0);

            index = AddCraft(typeof(RottingLegs), "Gnijące", "gnijące nogi", 20.0, 120.0,
                typeof(LeftLeg), "lewa noga", 1, "Nie masz wystarczajaco duzej ilosci lewych nóg.");
            AddRes(index, typeof(RightLeg), "Prawa Noga", 1, "Nie masz wystarczajaco duzej ilosci prawych nóg.");
            AddRes(index, typeof(AnimateDeadScroll), "Zwoj wskrzeszenia zwlok (Nekromancja)", 1,
                "Nie masz wystarczajaco duzej ilosci zwojow nekromanckich.");
            AddSkill(index, SkillName.Anatomy, 25.0, 50.0);

            index = AddCraft(typeof(ToxicTorso), "Gnijące", "toksyczne ciało", 20.0, 120.0,
                typeof(RottingTorso), "gnijący tułów", 1, "Nie masz wystarczajaco duzej ilosci gnijących tułowiów.");
            AddRes(index, typeof(GreaterPoisonPotion), "Butelka Mocnej Trucizny", 10,
                "Nie masz wystarczajaco duzej ilosci butelek mocnej trucizny");
            AddRes(index, typeof(AnimateDeadScroll), "Zwoj wskrzeszenia zwlok (Nekromancja)", 1,
                "Nie masz wystarczajaco duzej ilosci zwojow nekromanckich.");
            AddSkill(index, SkillName.Anatomy, 25.0, 50.0);

            //Mummy Crafts

            index = AddCraft(typeof(WrappedLegs), "Owinięte", "Zmumifikowane nogi", 40.0, 130.0,
                typeof(SkeletonLegs), "Nogi Szkieleta", 1, "Nie masz wystarczajaco duzej ilosci nóg szkieleta.");
            AddRes(index, typeof(Bandage), "Bandaż", 100, "Nie masz wystarczajaco duzej ilosci bandaży.");
            AddRes(index, typeof(AnimateDeadScroll), "Zwoj wskrzeszenia zwlok (Nekromancja)", 1,
                "Nie masz wystarczajaco duzej ilosci zwojow nekromanckich.");
            AddSkill(index, SkillName.Anatomy, 40.0, 80.0);

            index = AddCraft(typeof(WrappedTorso), "Owinięte", "Zmumifikowany tułów", 40.0, 130.0,
                typeof(SkeletonTorso), "Tułów szkieleta", 1, "Nie masz wystarczajaco duzej ilosci tułowiów.");
            AddRes(index, typeof(Bandage), "Bandaż", 100, "Nie masz wystarczajaco duzej ilosci bandaży.");
            AddSkill(index, SkillName.Anatomy, 40.0, 80.0);

            index = AddCraft(typeof(WrappedMageTorso), "Owinięte", "Zmumifikowany tułów oznaczony runami", 50.0, 140.0,
                typeof(SkeletonMageTorso), "Tułów szkieleta maga", 1, "Nie masz wystarczajaco duzej ilosci tułowiów.");
            AddRes(index, typeof(Bandage), "Bandaż", 100, "Nie masz wystarczajaco duzej ilosci bandaży.");
            AddRes(index, typeof(RecallRune), "Czysta Runa", 10, "Nie masz wystarczajaco duzej ilosci czystych run.");
            AddSkill(index, SkillName.Anatomy, 50.0, 80.0);

            // Phylacery
            index = AddCraft(typeof(Phylacery), "Filakterium", "Filakterium", 100.0, 130.0,
                typeof(Soul), "Dusza", 1, "Nie masz duszy potrzebnej do związania w filakterium.");
            AddRes(index, typeof(ArcaneGem), "Tajemniczy kamień", 6,
                "Nie masz wystarczajaco duzej ilosci tajemniczych kamieni.");
            AddRes(index, typeof(AnimateDeadScroll), "Zwoj wskrzeszenia zwlok (Nekromancja)", 1,
                "Nie masz wystarczajaco duzej ilosci zwojow nekromanckich.");
            AddRes(index, typeof(WoodenChest), "Drewniana skrzynia", 1,
                "Nie masz wystarczajaco duzej ilosci drewnianych skrzyń.");
            AddSkill(index, SkillName.Anatomy, 100.0, 120.0);
            
            //Death Knight Skulls
            index = AddCraft(typeof(BanishSkull), "Mroczny Rycerz", "Wygnanie", 50.0, 140.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( Pumice ), "pumeks" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(DemonicTouchSkull), "Mroczny Rycerz", "Dotyk Demona", 54.0, 144.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ZoogiFungus ), "grzyby zoogi" , 50, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(DevilPactSkull), "Mroczny Rycerz", "Pakt Ze Smiercia", 60.0, 151.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(GrimReaperSkull), "Mroczny Rycerz", "Ponury Zniwiarz", 60.0, 151.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( DaemonBone ), "kości demona" , 50, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(HagHandSkull), "Mroczny Rycerz", "Reka Wiedzmy", 62.0, 153.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( DaemonBone ), "kości demona" , 50, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(HellfireSkull), "Mroczny Rycerz", "Ogien Piekielny", 62.0, 153.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(LucifersBoltSkull), "Mroczny Rycerz", "Promien Smierci", 67.0, 157.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( DiseasedBark ), "zgniła kora" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(OrbOfOrcusSkull), "Mroczny Rycerz", "Kula Smierci", 67.0, 157.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(ShieldOfHateSkull), "Mroczny Rycerz", "Tarcza Nienawisci", 69.0, 159.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( DaemonBone ), "kości demona" , 50, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(SoulReaperSkull), "Mroczny Rycerz", "Zniwiarz Dusz", 70.0, 160.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( FireRuby ), "ognisty rubin" , 10, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(StrengthOfSteelSkull), "Mroczny Rycerz", "Wytrzymalosc Stali", 71.0, 161.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( DaemonBone ), "kości demona" , 50, 1044253 );
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(StrikeSkull), "Mroczny Rycerz", "Uderzenie", 72.0, 162.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( DaemonBone ), "kości demona" , 50, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(SuccubusSkinSkull), "Mroczny Rycerz", "Skora Sukkuba", 73.0, 163.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(WeakSpotSkull), "Mroczny Rycerz", "Slaby Punkt", 73.5, 163.5, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( SpringWater ), "wiosenna woda" , 30, 1044253 );
            AddRes( index, typeof( DragonsBlood ), "krew smoka" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );
            index = AddCraft(typeof(WrathSkull), "Mroczny Rycerz", "Gniew", 80.0, 170.0, typeof(Soul), "Dusza", 1,
	            "Nie masz duszy potrzebnej do stworzenia czaszki.");
            AddRes( index, typeof( GrizzledBones ), "blade kości" , 10, 1044253 );
            AddRes( index, typeof( ObsidianStone ), "obysdian" , 20, 1044253 );
            AddRes( index, typeof( Gold ), "złoto" , 2000, 1044253 );

            Recycle = true;
            RecycleAction = NecroRecycle.Do;
            RecycleText = "ROZŁÓŻ PRZYWOŁAŃCA";
        }
    }
}
