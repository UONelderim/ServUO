#region References

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class NPCSzachy : BaseCreature
	{
		[Constructable]
		public NPCSzachy()
			: base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			InitStats(100, 125, 25);

			Body = 400;
			CantWalk = true;
			Hue = 1641;
			Blessed = true;
			Name = "Zenobiusz z Tafroel";
			Title = "- Mistrz Szachów";

			Skills[SkillName.Anatomy].Base = 120.0;
			Skills[SkillName.Tactics].Base = 120.0;
			Skills[SkillName.Magery].Base = 120.0;
			Skills[SkillName.MagicResist].Base = 120.0;
			Skills[SkillName.DetectHidden].Base = 100.0;
		}

		public NPCSzachy(Serial serial)
			: base(serial)
		{
		}

		public override void OnSpeech(SpeechEventArgs e)
		{
			base.OnSpeech(e);

			Mobile from = e.Mobile;

			if (from.InRange(this, 2))
			{
				if (Regex.IsMatch(e.Speech,"zadan|witaj", RegexOptions.IgnoreCase))
				{
					Say("No, No, No... Widze, ze mamy tu amatora szachow! *zaciera rece* Moze chcesz sprobowac " +
					    "sie w Szachach Bojowych? Kosztuje to jedyne 5000 centarow, 100 sztuk srebra i " +
					    "1 krysztal komunikacyjny! Przekaz mi te pieniadze, a pozwole Ci zagrac w Szachy Bojowe! " +
					    "*wyciaga rece po pieniadze*");
				}
			}
		}


		private bool _Gold;
		private bool _Silver;
		private bool _Crystal;

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			var delivered = false;
			if (dropped is Gold && dropped.Amount == 5000)
			{
				dropped.Delete();
				Say(true, "Dziekuje bardzo!");
				_Gold = true;
				delivered = true;
			}
			if (dropped is Silver && dropped.Amount == 100)
			{
				dropped.Delete();
				Say(true, "Dziekuje bardzo!");
				_Silver = true;
				delivered = true;
			}

			if (dropped is BroadcastCrystal)
			{
				dropped.Delete();
				Say(true, "Dziekuje bardzo!");
				_Silver = true;
				delivered = true;
			}

			if (_Gold && _Silver && _Crystal)
			{
				Point3D loc = new Point3D(5914, 3227, 0);
				Map map = Map.Felucca;
				WrotaSzachy portal = new WrotaSzachy();
				portal.MoveToWorld(loc, map);
						
				Say(true, "Ooo tak! Wspaniale... Prosze oto portal, ktory zaprowadzi cie do zwyciestwa... hahaha, " +
				          "ruszaj smialo, tylko spiesz sie! Za chwile go zamkne!");

				_Gold = _Silver = _Crystal = false;
			}
			else if(delivered)
			{
				var needs = new List<string>();
				if(!_Gold)
					needs.Add("5000 zlota");
				if(!_Silver)
					needs.Add("100 srebra");
				if(!_Crystal)
					needs.Add("krysztalu komunikacyjnego");

				Say($"Potrzebuje jeszcze {String.Join(" i ", needs)}");
			}
			else
			{
				Say("Nie potrzebuje tego!");
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
