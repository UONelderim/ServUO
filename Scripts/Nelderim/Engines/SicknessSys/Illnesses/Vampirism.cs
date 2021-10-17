using Server.Items;
using Server.Regions;
using System.Collections.Generic;
using System.Linq;

namespace Server.SicknessSys.Illnesses
{
    public static class Vampirism
    {
        public static int IllnessType { get => 101; }
        public static string Name { get => BaseVirus.GetRandomName(IllnessType); }
        public static int StatDrain { get => BaseVirus.GetRandomDamage(IllnessType); }
        public static int BaseDamage { get => BaseVirus.GetRandomDamage(IllnessType); }
        public static int PowerDegenRate { get => BaseVirus.GetRandomDegen(IllnessType); }

        public static void UpdateBody(VirusCell cell)
        {
            IllnessMutationLists.SetMutation(cell);
        }

        public static void VampireWeakness(VirusCell cell)
        {
            bool DoDamage = false;
            bool DoMinDamage = false;

            if (cell.Level < 100)
            {
                IEnumerable<Garlic> result = from c in cell.PM.GetItemsInRange(3)
                                             where c is Garlic
                                             select c as Garlic;


                DoMinDamage = result.Any();

                Item resultBP = cell.PM.Backpack.FindItemByType(typeof(Garlic));
                if (resultBP != null)
                    DoMinDamage = true;
            }

            if (!SicknessHelper.IsNight(cell.PM) && !cell.PM.Region.IsPartOf<DungeonRegion>())
            {
                if (!SicknessHelper.InDoors(cell.PM))
                {
                    if (!SicknessHelper.IsFullyCovered(cell.PM))
                    {
                        DoDamage = true;
                    }
                }
            }

            if (DoDamage || DoMinDamage)
            {
                int damage = BaseDamage * cell.Stage;

                if (DoMinDamage && damage != 0)
                    damage = damage / 10;

                cell.PM.Damage(damage);

                if (DoMinDamage)
                    cell.PM.SendMessage("You feel the pain of being exposed to garlic!");
                else
                    cell.PM.SendMessage("You are exposed to light, cover up or die!");

                SicknessAnimate.RunScreamAnimation(cell.PM);
            }
        }
    }
}
