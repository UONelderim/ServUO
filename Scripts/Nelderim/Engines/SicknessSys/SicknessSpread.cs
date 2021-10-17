using Server.Mobiles;
using Server.SicknessSys.Items;

namespace Server.SicknessSys
{
    static class SicknessSpread
    {
        public static void SpreadSickness(VirusCell cell)
        {
            if (cell.Level < 99)
            {
                TryToDamage(cell);

                if (cell.LevelMod == 1)
                    cell.PM.SendMessage(53, "You have lost [ 1 ] white cell!");
                else
                    cell.PM.SendMessage(53, "You have lost [ " + cell.LevelMod + " ] white cells!");

                cell.Level = cell.Level + cell.LevelMod;

                if (cell.Level > 100)
                    cell.Level = 99;
            }
            else
            {
                if (cell.Stage < 3)
                {
                    cell.Stage++;

                    cell.Level = 1;

                    UpdateSickness(cell);

                    cell.PM.SendMessage(37, "Your illness has caused your body to change!");

                    AddVampireRobe(cell);
                }
                else if (!SicknessHelper.IsSpecialVirus(cell))
                {
                    cell.Level = 100;

                    TryToDamage(cell);

                    int chanceTOdie = Utility.RandomMinMax(1, 90000/(int)cell.Illness);

                    if (chanceTOdie >= 3)
                        cell.PM.SendMessage(37, "Your illness consumes you, see a medic before you die!");
                    else
                        cell.PM.Kill();
                }
                else
                {
                    if (cell.Level == 99)
                    {
                        if (cell.Illness == IllnessType.Vampirism)
                            cell.PM.SendMessage("Your virus has fully matured, you are now a Vampire");
                        else
                            cell.PM.SendMessage("Your virus has fully matured, you are now a Werewolf");

                        cell.Level = 100;

                        SicknessAnimate.RunMutateAnimation(cell.PM);

                        AddVampireRobe(cell);
                    }
                    else
                    {
                        int age = Utility.RandomMinMax(900, 999);

                        if (cell.Level < age)
                        {
                            cell.PM.SendMessage("The virus has aged!");

                            SicknessAnimate.RunMutateAnimation(cell.PM);

                            if (cell.Level > 99)
                                cell.Level++;
                        }
                        else
                        {
                            cell.PM.SendMessage("The virus has faded from your body!");

                            SicknessCure.Cure(cell.PM, cell);
                        }
                    }
                }
            }
        }

        private static void TryToDamage(VirusCell cell)
        {
            bool getBool = Utility.RandomBool();

            if (getBool)
            {
                int rnd = Utility.RandomMinMax(1, 9);
                RunRandomAnimation(cell.PM, rnd);

                int dmg = (((cell.Level / rnd) * cell.Stage) * cell.Damage) / rnd;

                if (dmg > cell.PM.Hits)
                    dmg = (cell.PM.Hits / 10) * cell.Stage;

                if (dmg < 1)
                    dmg = 1;

                cell.PM.Damage(dmg);
            }
        }

        private static void AddVampireRobe(VirusCell cell)
        {
            if (cell.Illness == IllnessType.Vampirism)
            {
                switch (cell.Stage)
                {
                    case 2:
                        cell.PM.AddToBackpack(new VampireRobe(cell.PM, 0x2687, 2));
                        break;
                    case 3:
                        if (cell.Level < 100)
                            cell.PM.AddToBackpack(new VampireRobe(cell.PM, 0x7816, 3));
                        else
                            cell.PM.AddToBackpack(new VampireRobe(cell.PM, 0x4B9D, 0));
                        break;
                    default:
                        break;
                }
            }
        }

        public static void UpdateSickness(VirusCell cell)
        {
            IllnessType illnessType = cell.Illness;

            switch (illnessType)
            {
                case IllnessType.Cold: Illnesses.Cold.UpdateBody(cell);
                    break;
                case IllnessType.Flu: Illnesses.Flu.UpdateBody(cell);
                    break;
                case IllnessType.Virus: Illnesses.Virus.UpdateBody(cell);
                    break;
                case IllnessType.Vampirism: Illnesses.Vampirism.UpdateBody(cell);
                    break;
                case IllnessType.Lycanthropia: Illnesses.Lycanthropia.UpdateBody(cell);
                    break;
                default:
                    break;
            }
        }

        public static void RunRandomAnimation(PlayerMobile pm, int anim)
        {
            if (anim == 1)
            {
                SicknessAnimate.RunBlowNoseAnimation(pm);
            }
            if (anim == 2)
            {
                SicknessAnimate.RunClearThroatAnimation(pm);
            }
            if (anim == 3)
            {
                SicknessAnimate.RunCoughAnimation(pm);
            }
            if (anim == 4)
            {
                SicknessAnimate.RunGaspAnimation(pm);
            }
            if (anim == 5)
            {
                SicknessAnimate.RunGroanAnimation(pm);
            }
            if (anim == 6)
            {
                SicknessAnimate.RunPukeAnimation(pm);
            }
            if (anim == 7)
            {
                SicknessAnimate.RunSiffAnimation(pm);
            }
            if (anim == 8)
            {
                SicknessAnimate.RunSighAnimation(pm);
            }
            if (anim == 9)
            {
                SicknessAnimate.RunSneezeAnimation(pm);
            }
        }
    }
}
