#region References

using Server.Items;

#endregion

namespace Server.ACC.CSS.Systems.Cleric
{
	public class PlayerEvent
	{
		public delegate void OnWeaponHit(Mobile attacker, Mobile defender, int damage, WeaponAbility a);

		public static event OnWeaponHit HitByWeapon;

		public static void InvokeHitByWeapon(Mobile attacker, Mobile defender, int damage, WeaponAbility a)
		{
			if (HitByWeapon != null)
				HitByWeapon(attacker, defender, damage, a);
		}
	}
}
