#region References

using Nelderim;
using Server.Network;

#endregion

namespace Server.Items
{
	public partial class BaseQuiver : IWearableDurability
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public int MaxHitPoints
		{
			get => ItemHitPoints.Get(this).MaxHitPoints;
			set
			{
				ItemHitPoints.Get(this).MaxHitPoints = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int HitPoints
		{
			get => ItemHitPoints.Get(this).HitPoints;
			set
			{
				int curval = ItemHitPoints.Get(this).HitPoints;
				if (value != curval && MaxHitPoints > 0)
				{
					curval = ItemHitPoints.Get(this).HitPoints = value;

					if (curval < 0)
						Delete();
					else if (curval > MaxHitPoints)
						ItemHitPoints.Get(this).HitPoints = MaxHitPoints;

					InvalidateProperties();
				}
			}
		}

		public int InitMinHits => 90;
		public int InitMaxHits => 110;
		public virtual bool CanFortify => true;

		public void UnscaleDurability() { }

		public void ScaleDurability() { }


		public int OnHit(BaseWeapon weapon, int damageTaken)
		{
			double armorRating = 13; //Default for leather armor
			int Absorbed = (int)(armorRating * Utility.RandomDouble());

			damageTaken -= Absorbed;

			if (damageTaken < 0)
				damageTaken = 0;

			if (Absorbed < 2)
				Absorbed = 2;

			double chance = 25;

			if (chance >= Utility.Random(100)) // 25% chance to lower durability
			{
				int wear = 1;

				if (weapon.Type == WeaponType.Bashing)
					wear = Absorbed / 2;

				if (wear > 0 && MaxHitPoints > 0)
				{
					if (HitPoints >= wear)
					{
						HitPoints -= wear;
						wear = 0;
					}
					else
					{
						wear -= HitPoints;
						HitPoints = 0;
					}

					if (wear > 0)
					{
						if (MaxHitPoints > wear)
						{
							MaxHitPoints -= wear;

							if (Parent is Mobile)
								((Mobile)Parent).LocalOverheadMessage(MessageType.Regular, 0x3B2,
									1061121); // Your equipment is severely damaged.
						}
						else
						{
							Delete();
						}
					}
				}
			}

			return damageTaken;
		}
	}
}
