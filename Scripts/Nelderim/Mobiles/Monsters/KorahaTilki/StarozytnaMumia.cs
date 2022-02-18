#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class StarozytnaMumia : BaseCreature
	{
		[Constructable]
		public StarozytnaMumia()
			: base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			InitStats(100, 125, 25);

			Body = 400;
			CantWalk = true;
			Hue = 1641;
			Blessed = true;
			Name = "Kapłan Tilki";

			Skills[SkillName.Anatomy].Base = 120.0;
			Skills[SkillName.Tactics].Base = 120.0;
			Skills[SkillName.Magery].Base = 120.0;
			Skills[SkillName.MagicResist].Base = 120.0;
			Skills[SkillName.DetectHidden].Base = 100.0;
		}

		public StarozytnaMumia(Serial serial)
			: base(serial)
		{
		}


		public override void OnSpeech(SpeechEventArgs e)
		{
			base.OnSpeech(e);

			Mobile from = e.Mobile;

			if (from.InRange(this, 2))
			{
				if (e.Speech.ToLower().IndexOf("zadan") >= 0 || e.Speech.ToLower().IndexOf("witaj") >= 0)
				{
					string message1 =
						"Witajcie! Ta pluskwa czai sie nieopodal, przyniescie mi posoke jej pomniejszej siostry by udowodnic ze sprostacie jej mocy!";
					this.Say(message1);
				}
			}
		}


		public int posoka;

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (dropped is PosokaPluskwy)
			{
				dropped.Delete();
				posoka++;

				if (posoka > 0)
				{
					Point3D loc = new Point3D(3791, 2240, 0);
					Map map = Map.Felucca;
					WrotaTilki portal = new WrotaTilki();
					portal.MoveToWorld(loc, map);

					Say(true,
						" Portal do Leza Wielka Pluskwy zostal otwarty, ale spieszcie sie gdyz za dwie i pol klepsydry sie zamknie! ");

					posoka = 0;
				}
			}

			return base.OnDragDrop(from, dropped);
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
		}
	}
}
