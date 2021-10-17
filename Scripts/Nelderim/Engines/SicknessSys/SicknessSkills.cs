using Server.Mobiles;

namespace Server.SicknessSys
{
    public static class SicknessSkills 
    {
        //Vampirism
        public static void BloodDrain(PlayerMobile pm, VirusCell cell) //Auto
        {
            int damage = cell.Damage;

            if (cell.MaxPower > cell.Power + damage / 2)
            {
                cell.Power = cell.Power + damage / 2;

                pm.Combatant.Damage(damage / 2, pm);

                SicknessAnimate.RunBloodDrainAnimation(pm);
            }
            else
            {
                pm.Say("*miss*");
                pm.SendMessage("You are at max power!");
            }
        }

        public static void BloodBurn(PlayerMobile pm, VirusCell cell) //Attack
        {
            int damage = cell.Damage;

            if (0 < cell.Power - damage * 2)
            {
                cell.Power = cell.Power - damage * 2;

                pm.Combatant.Damage(damage, pm);

                SicknessAnimate.RunBloodBurnAnimation(pm);
            }
            else
            {
                pm.Say("*miss*");
                pm.SendMessage("You do not have enough power to perform Blood Burn");
            }
        }

        public static void BloodBath(PlayerMobile pm, VirusCell cell) //Defense
        {
            int manaGain = pm.ManaMax - pm.Mana;

            if (cell.Power > manaGain)
            {
                pm.SpeechHue = 47;

                pm.Say("Mana + " + manaGain.ToString());

                pm.Mana = cell.PM.ManaMax;

                pm.SendMessage("Your mind has been replenished!");

                SicknessAnimate.RunBloodBathAnimation(pm);
            }
            else
            {
                pm.Say("*fizzle*");
                pm.SendMessage("You do not have enough power to perform Blood Bath");
            }
        }

        //Lycanthropia
        public static void RageFeed(PlayerMobile pm, VirusCell cell) //Auto
        {
            int damage = cell.Damage;

            if (cell.MaxPower < cell.Power + damage / 2)
            {
                cell.Power = cell.Power + damage / 2;

                pm.Combatant.Damage(damage / 2, pm);
            }
            else
            {
                pm.Say("*miss*");
                pm.SendMessage("You are full and refuse to eat!");
            }
        }

        public static void RageStrike(PlayerMobile pm, VirusCell cell) //Attack
        {
            int damage = cell.Damage;

            if (0 < cell.Power - damage * 2)
            {
                cell.Power = cell.Power - damage * 2;

                pm.Combatant.Damage(damage, pm);
            }
            else
            {
                pm.Say("*miss*");
                pm.SendMessage("You do not have enough power to perform Rage Strike");
            }
        }

        public static void RagePush(PlayerMobile pm, VirusCell cell) //Defense
        {
            int hitsGain = pm.HitsMax - pm.Hits;

            if (cell.Power > hitsGain)
            {
                pm.SpeechHue = 47;

                pm.Say("HP + " + hitsGain.ToString());

                pm.Hits = cell.PM.HitsMax;

                pm.SendMessage("Your body has been rejuvenated!");
            }
            else
            {
                pm.Say("*fizzle*");
                pm.SendMessage("You do not have enough power to perform Rage Push");
            }
        }
    }
}
