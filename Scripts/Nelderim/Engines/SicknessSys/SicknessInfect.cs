using Server.Mobiles;
using Server.SicknessSys.Items;
using System.Collections.Generic;
using System.Linq;

namespace Server.SicknessSys
{
    static class SicknessInfect
    {
        public static void Infect(PlayerMobile pm, IllnessType type, string sickness = null)
        {
            Item cell = pm.Backpack.FindItemByType(typeof(VirusCell));

            bool IsImmune = false;

            if (pm.Backpack.FindItemByType(typeof(WhiteCell)) is WhiteCell whitecell && type != IllnessType.Vampirism && type != IllnessType.Lycanthropia)
            {
                int chance = SicknessHelper.GetSickChance(pm, 0);

                if (chance < whitecell.ViralResistance)
                    IsImmune = true;
            }

            if (!IsImmune)
            {
                if (cell == null && pm != null)
                {
                    SicknessAnimate.RunInfectedAnimation(pm);

                    pm.AddToBackpack(new VirusCell(pm, type, sickness));

                    VirusCell vc = pm.Backpack.FindItemByType(typeof(VirusCell)) as VirusCell;
                    SicknessCore.VirusCellList.Add(vc);

                    if (type == IllnessType.Vampirism)
                        pm.AddToBackpack(new VampireRobe(pm, 0x1F03, 1));

                    if (type == IllnessType.Lycanthropia)
                        pm.AddToBackpack(new WereClaws(pm));

                    if (!(pm.Backpack.FindItemByType(typeof(WhiteCell)) is WhiteCell wc))
                        pm.AddToBackpack(new WhiteCell(pm, vc));
                    else if (Utility.RandomBool())
                        wc.ViralResistance++;
                }
            }
        }

        public static void SpreadVirus(PlayerMobile pm, VirusCell cell)
        {
            int rnd = Utility.RandomMinMax(1, 100);

            IEnumerable<PlayerMobile> result = from c in pm.GetMobilesInRange(3)
                                               where c is PlayerMobile
                                               select c as PlayerMobile;

            if (result.Any())
            {
                foreach (PlayerMobile player in result)
                {
                    Item cellCheck = player.Backpack.FindItemByType(typeof(VirusCell));

                    if (cellCheck == null && rnd < (int)cell.Illness * 10)
                        Infect(pm, cell.Illness, cell.Sickness);
                }
            }
        }

        public static void OutBreak(List<VirusCell> CellList)
        {
            try
            {
                foreach (VirusCell cell in CellList)
                {
                    int Chance = SicknessHelper.GetSickChance(cell.PM, 0);

                    if (Chance != 0)
                    {
                        int Illness = Utility.RandomMinMax(1, 3);

                        int rndInfect = Utility.RandomMinMax(1, 50000 * Illness);

                        if (Chance == 100)
                        {
                            Infect(cell.PM, (IllnessType)Illness);
                        }
                        else
                        {
                            if (rndInfect <= Chance * 10)
                            {
                                Infect(cell.PM, (IllnessType)Illness);
                            }
                        }
                    }
                }
            }
            catch
            {
                //do nothing : failed attempt
            }
        }
    }
}
