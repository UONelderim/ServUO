using Server;
using System;

public interface ICustomWeaponAbilities
{
    int LegacyPrimaryWeaponAbilityIndex { get; }
    int LegacySecondaryWeaponAbilityIndex { get; }
    int CustomPrimaryWeaponAbilityIndex { get; }
    int CustomSecondaryWeaponAbilityIndex { get; }
}