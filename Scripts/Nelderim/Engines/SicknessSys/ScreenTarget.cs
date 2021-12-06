using Server.Mobiles;
using Server.SicknessSys.Mobiles;
using Server.Targeting;

namespace Server.SicknessSys
{
    public class ScreenTarget : Target
    {
        private readonly VirusCell Cell;

        public ScreenTarget(VirusCell cell) : base(100, true, TargetFlags.None)
        {
            Cell = cell;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (Cell != null)
            {
                if (targeted == from)
                {
                    if (Cell.LastSkill == 0)
                    {
                        if (SicknessHelper.IsNight(from as PlayerMobile))
                        {
                            if (Cell.Illness == IllnessType.Vampirism)
                                SicknessSkills.BloodBath(Cell.PM, Cell);
                            else if (Cell.Illness == IllnessType.Lycanthropia)
                                SicknessSkills.RagePush(Cell.PM, Cell);
                            else
                                from.Say("Don't touch me, I'm sick!");

                            if (SicknessHelper.IsSpecialVirus(Cell))
                                Cell.LastSkill = 60;
                        }
                        else
                        {
                            from.SendMessage("Your skills do not work jurying the day!");
                        }
                    }
                    else
                    {
                        from.SendMessage("Your still recouping from the last skill use, please wait " + Cell.LastSkill + " secs");
                    }
                }
                else if (targeted == from.Combatant)
                {
                    Mobile m = targeted as Mobile;

                    if (Cell.LastSkill == 0)
                    {
                        if (SicknessHelper.IsNight(from as PlayerMobile))
                        {
                            if (Cell.Illness == IllnessType.Vampirism)
                                SicknessSkills.BloodBurn(Cell.PM, Cell);
                            else if (Cell.Illness == IllnessType.Lycanthropia)
                                SicknessSkills.RageStrike(Cell.PM, Cell);
                            else
                                m.Say("!!!");

                            if (SicknessHelper.IsSpecialVirus(Cell))
                                Cell.LastSkill = 60;
                        }
                        else
                        {
                            from.SendMessage("Your skills do not work jurying the day!");
                        }
                    }
                    else
                    {
                        from.SendMessage("Your still recouping from the last skill use, please wait " + Cell.LastSkill + " secs");
                    }
                }
                else if (targeted is PlayerMobile)
                {
                    PlayerMobile pm = targeted as PlayerMobile;

                    pm.Say("Don't touch me with your contaminated hands!");
                }
                else if (targeted is Mobile)
                {
                    Mobile m = targeted as Mobile;

                    m.Say("!!!");
                }
                else if (targeted is Item)
                {
                    if (!SicknessHelper.IsSpecialVirus(Cell) && Cell.Stage == 3)
                    {
                        double rndChance = Utility.RandomMinMax(1, 100);

                        if (targeted is Item)
                        {
                            Item item = targeted as Item;

                            bool itemDistCheck = from.InRange(item, 3);

                            if (itemDistCheck)
                            {
                                if (rndChance < 10 - (int)Cell.Illness)
                                {
                                    SpawnSuperSpreader(Cell, item.Location);
                                }
                                else
                                {
                                    from.Say("*failed to spread your illness*");
                                }
                            }
                            else
                            {
                                from.SendMessage("You need to be closer to that, to spread your illness");
                            }
                        }
                    }
                }
                else
                {
                        Cell.GumpX = 50;
                        Cell.GumpY = 50;
                }

                Cell.IsMovingGump = false;

                SicknessHelper.SendHeartGump(Cell);
            }
        }

        private static void SpawnSuperSpreader(VirusCell cell, Point3D point3D)
        {
            SuperSpreader SS = new SuperSpreader(true, (int)cell.Illness);

            SS.MoveToWorld(point3D, cell.Map);
        }

        public static void HealthStatus(Mobile from, VirusCell cell)
        {
            string condition = "Critical";

            if (cell.Stage < 3)
                condition = "Bad";
            if (cell.Stage < 2)
                condition = "Poor";

            int cellCNT = cell.MaxPower / cell.LevelMod;

            from.SendMessage(120, "††-†-†-†-†-†-†-†-†-††");
            from.SendMessage(55, cell.PM.Name + "'s ♥ Health Status [ " + condition + " ]");
            from.SendMessage(55, cell.PM.Name + "'s has " + cell.Sickness);
            from.SendMessage(55, "Viral Stage " + cell.Stage + " [ RO = " + cell.LevelMod + " ]");
            from.SendMessage(1153, "White Cell " + cell.Power/cellCNT + "/" + cell.LevelMod);
            from.SendMessage(1157, "Virus Cell " + (cell.LevelMod - cell.Power/cellCNT) + "/" + cell.LevelMod);
            from.SendMessage(120, "††-†-†-†-†-†-†-†-†-††");
            from.SendMessage(55, "World has " + cell.WorldInfectionLevel + " infected / " + World.Mobiles.Count + " total population!");
            from.SendMessage(120, "††-†-†-†-†-†-†-†-†-††");
        }
    }
}
