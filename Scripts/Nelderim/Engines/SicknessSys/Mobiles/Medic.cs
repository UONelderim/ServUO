#region References

using Server.Items;
using Server.Mobiles;

#endregion

namespace Server.SicknessSys.Mobiles
{
	public class Medic : BaseCreature
	{
		[Constructable]
		public Medic() : base(AIType.AI_Healer, FightMode.None, 10, 1, 0.2, 0.4)
		{
			Title = "- medyk";

			InitStats(31, 41, 51);

			SpeechHue = 53;

			Hue = Race.RandomSkinHue();

			if (Female = Utility.RandomBool())
			{
				Body = 0x191;
				Name = NameList.RandomName("female");
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName("male");
			}

			AddItem(new LongPants(Utility.RandomNeutralHue()));
			AddItem(new Robe(Utility.RandomDyedHue()));
			AddItem(new Cap(Utility.RandomDyedHue()));

			AddItem(new ThighBoots(Utility.RandomNeutralHue()));

			Utility.AssignRandomHair(this);

			Container pack = new Backpack();

			pack.DropItem(new Gold(10, 50));

			pack.Movable = false;

			AddItem(pack);

			ThinkDelay = 0;
		}

		public Medic(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from is PlayerMobile)
			{
				PlayerMobile pm = from as PlayerMobile;

				Item cell = pm.Backpack.FindItemByType(typeof(VirusCell));
				Item illcure = pm.Backpack.FindItemByType(typeof(IllnessCure));

				if (cell != null && illcure == null)
				{
					VirusCell Cell = cell as VirusCell;

					//if (SicknessHelper.IsSpecialVirus(Cell)) //Uncomment to stop Vampire/Werewolf auto cure!
					//{
					//    SpeechHue = 53;

					//    SayTo(pm, "That virus is beyond my skills to cure, " + pm.Name);
					//}
					//else
					//{
					IllnessCure cure = new IllnessCure(Cell);

					pm.AddToBackpack(cure);

					SpeechHue = 53;

					SayTo(pm, "Zdrowiej, " + pm.Name);

					SicknessAnimate.RunMedicGiveCureAnimation(pm, this);
					//}
				}
				else
				{
					SpeechHue = 38;

					if (illcure == null)
						SayTo(pm, pm.Name + ", Nie wygladasz na chorego!");
					else
						SayTo(pm, pm.Name + ", Otrzymales ode mnie miksture. Sprawdz swoj plecak!");

					base.OnDoubleClick(from);
				}
			}
		}

		private int ThinkDelay { get; set; }

		public override void OnThink()
		{
			if (ThinkDelay < 1)
			{
				ThinkDelay = 25;

				foreach (Mobile mobile in GetMobilesInRange(3))
				{
					if (mobile is PlayerMobile)
					{
						PlayerMobile pm = mobile as PlayerMobile;

						Item cell = pm.Backpack.FindItemByType(typeof(VirusCell));

						if (cell != null)
						{
							SpeechHue = 53;

							SayTo(pm, pm.Name + ", Czy jestes chory? (aby otrzymac odtrutke, 2xkliknij na mnie)!");

							SicknessAnimate.RunMedicAnimation(pm, this);
						}
					}
				}
			}
			else
			{
				ThinkDelay--;

				base.OnThink();
			}
		}

		public override bool ClickTitle
		{
			get
			{
				return false;
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version 
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			ThinkDelay = 0;
		}
	}
}
