#region References

using Server.Items;
using Server.Mobiles;

#endregion

namespace Server.SicknessSys.Illnesses
{
	public static class Lycanthropia
	{
		public static int IllnessType { get => 102; }
		public static string Name { get => BaseVirus.GetRandomName(IllnessType); }
		public static int StatDrain { get => BaseVirus.GetRandomDamage(IllnessType); }
		public static int BaseDamage { get => BaseVirus.GetRandomDamage(IllnessType); }
		public static int PowerDegenRate { get => BaseVirus.GetRandomDegen(IllnessType); }

		public static void UpdateBody(VirusCell cell)
		{
			IllnessMutationLists.SetMutation(cell);
		}

		public static void LycanthropiaWeakness(VirusCell cell)
		{
			bool DoDamage = false;

			//if (cell.Level < 100)
			//{
			//    IEnumerable<Silver> result = from c in cell.PM.GetItemsInRange(3)
			//                                 where c is Silver
			//                                 select c as Silver;

			//    DoDamage = result.Any();

			//    Item resultBP = cell.PM.Backpack.FindItemByType(typeof(Silver));
			//    if (resultBP != null)
			//        DoDamage = true;
			//}

			bool IsSilverSlayer = false;

			if (cell.PM.Combatant != null && cell.PM.Combatant is PlayerMobile m)
			{
				if (m.FindItemOnLayer(Layer.OneHanded) is BaseWeapon bw1)
				{
					if (bw1.Slayer == SlayerName.Silver || bw1.Slayer2 == SlayerName.Silver)
						IsSilverSlayer = true;
				}


				if (m.FindItemOnLayer(Layer.TwoHanded) is BaseWeapon bw2)
				{
					if (bw2.Slayer == SlayerName.Silver || bw2.Slayer2 == SlayerName.Silver)
						IsSilverSlayer = true;
				}
			}


			if (DoDamage || IsSilverSlayer)
			{
				int damage = BaseDamage * cell.Stage;

				cell.PM.Damage(damage);

				SicknessAnimate.RunGrowlAnimation(cell.PM);
			}
		}
	}
}
