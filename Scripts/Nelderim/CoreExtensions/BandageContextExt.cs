#region References

using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;

#endregion

namespace Server.Items
{
	public partial class BandageContext
	{
		public static bool AllowPetRessurection(Mobile healer, BaseCreature petPatient)
		{
			return AllowPetRessurection(healer, petPatient, true);
		}

		public static bool AllowPetRessurection(Mobile healer, BaseCreature petPatient, bool gump)
		{
			Mobile master = petPatient.ControlMaster;

			if (master != null && healer == master)
			{
				petPatient.ResurrectPet();

				for (int i = 0; i < petPatient.Skills.Length; ++i)
				{
					petPatient.Skills[i].Base -= 0.1;
				}

				return true;
			}

			if (master != null && master.InRange(petPatient, 3))
			{
				if (gump)
				{
					healer.SendLocalizedMessage(1049658, "",
						0x502); // The owner has been asked to sanctify the resurrection.

					master.CloseGump(typeof(PetResurrectGump));
					master.SendGump(new PetResurrectGump(healer, petPatient));
				}

				return true;
			}

			List<Mobile> friends = petPatient.Friends;

			for (int i = 0; friends != null && i < friends.Count; ++i)
			{
				Mobile friend = friends[i];

				if (friend.InRange(petPatient, 3))
				{
					if (gump)
					{
						healer.SendLocalizedMessage(1049658, "",
							0x502); // The owner has been asked to sanctify the resurrection.

						friend.CloseGump(typeof(PetResurrectGump));
						friend.SendGump(new PetResurrectGump(healer, petPatient));
					}

					return true;
				}
			}

			return false;
		}
	}
}
