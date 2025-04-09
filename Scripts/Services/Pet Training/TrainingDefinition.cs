using Server.Items;
using System;
using System.Linq;

namespace Server.Mobiles
{
    public class TrainingDefinition
    {
        public Type CreatureType { get; private set; }
        public Class Class { get; private set; }
        public MagicalAbility MagicalAbilities { get; private set; }
        public SpecialAbility[] SpecialAbilities { get; private set; }
        public WeaponAbility[] WeaponAbilities { get; private set; }
        public AreaEffect[] AreaEffects { get; private set; }

        public int ControlSlotsMin { get; private set; }
        public int ControlSlotsMax { get; private set; }

        public TrainingDefinition(
            Type type,
            Class classificaion,
            MagicalAbility magicalAbility,
            SpecialAbility[] specialAbility,
            WeaponAbility[] weaponAbility,
            AreaEffect[] areaEffect,
            int controlmin,
            int controlmax)
        {
            CreatureType = type;
            Class = classificaion;
            MagicalAbilities = magicalAbility;
            SpecialAbilities = specialAbility;
            WeaponAbilities = weaponAbility;
            AreaEffects = areaEffect;

            ControlSlotsMin = controlmin;
            ControlSlotsMax = controlmax;
        }
        
        public TrainingDefinition(
	        Type type,
	        Class classificaion,
	        MagicalAbility magicalAbility,
	        SpecialAbility[] specialAbility,
	        WeaponAbility[] weaponAbility,
	        AreaEffect[] areaEffect,
	        int controlSlots)
        {
	        CreatureType = type;
	        Class = classificaion;
	        MagicalAbilities = magicalAbility;
	        SpecialAbilities = specialAbility;
	        WeaponAbilities = weaponAbility;
	        AreaEffects = areaEffect;

	        ControlSlotsMin = controlSlots;
	        ControlSlotsMax = Math.Min(5, controlSlots + 2);
        }

        public bool HasSpecialAbility(SpecialAbility ability)
        {
            return SpecialAbilities != null && SpecialAbilities.Any(a => a == ability);
        }

        public bool HasAreaEffect(AreaEffect ability)
        {
            return AreaEffects != null && AreaEffects.Any(a => a == ability);
        }
    }
}
