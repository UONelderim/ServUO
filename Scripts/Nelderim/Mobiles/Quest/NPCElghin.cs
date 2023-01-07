#region References

using System.Text.RegularExpressions;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class NPCElghin : BaseCreature
	{
		[Constructable]
		public NPCElghin()
			: base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			InitStats(100, 125, 25);

			Body = 400;
			CantWalk = true;
			Hue = 1641;
			Blessed = true;
			Name = "NPCElghin";

			Skills[SkillName.Anatomy].Base = 120.0;
			Skills[SkillName.Tactics].Base = 120.0;
			Skills[SkillName.Magery].Base = 120.0;
			Skills[SkillName.MagicResist].Base = 120.0;
			Skills[SkillName.DetectHidden].Base = 100.0;
		}

		public NPCElghin(Serial serial)
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
					Say("Hrrr... Czego tu szukasz? Nie powinno Cie tu byc... Lepiej zawroc... Chociaz, " +
					    "skoro tak bardzo chcesz zajrzec do krainy, ktorej nawet drowy sie obiawiaja... " +
					    "smialo. Przepuszcze Cie... ale najpierw przynies mi serca smoka, starozytnego ognistego i " +
					    "starozytnego lodowego smoka. Potrzebuje ich do mojego wywaru... Ruszaj wiec...");
				}
			}
		}


		private bool _Serce1;
		private bool _Serce2;
		private bool _Serce3;

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (dropped is DragonsHeart)
			{
				dropped.Delete();
				
				Say(true, " Pierwsza pieczec zostala otwarta, do otwarcia nastepnej potrzebuje Serce Lodowego Smoka! ");
				_Serce1 = true;
			}
			if (dropped is BlueDragonsHeart && !_Serce1)
			{
				if(!_Serce1)
					Say(true, "Glupcze najpierw potrzebuje serce zwyklego smoka!");
				else
				{
					dropped.Delete();

					Say(true,
						" Druga pieczec zostala otwarta, do otwarcia przedostateniej potrzebuje serce Ognistego Smoka ");

					_Serce2 = true;
				}
			}
			if (dropped is RedDragonsHeart)
			{
				if(!_Serce1)
					Say(true, "Glupcze najpierw potrzebuje serce zwyklego smoka!");
				else if (!_Serce2)
				{
					Say(true, "Glupcze, teraz potrzebuje serce Starozytnego Lodowego Smoka!");
				}
				else
				{
					dropped.Delete();

					_Serce3 = true;
					if (_Serce1 && _Serce2 && _Serce3)
					{
						Point3D loc = new Point3D(5914, 3227, 0);
						Map map = Map.Felucca;
						WrotaElghin portal = new WrotaElghin();
						portal.MoveToWorld(loc, map);
						
						Say(true,
							" Ooo tak! Wspaniale... Prosze oto portal, ktory zaprowadzi cie w szpony smierci... hahaha, ruszaj smialo, tylko spiesz sie! Za chwile go zamkne! ");

						_Serce1 = _Serce2 = _Serce3 = false;
					}
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
