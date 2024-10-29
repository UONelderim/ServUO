using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Gumps
{
	public class PaintingGump : Gump
	{
		public PaintingGump() : base(321, 233)
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(1);
			AddBackground(245, 106, 321, 233, 2600);
			AddButton(320, 190, 1210, 1209, 0, GumpButtonType.Page, 2);
			AddButton(320, 240, 1210, 1209, 0, GumpButtonType.Page, 3);
			AddButton(320, 290, 1210, 1209, 0, GumpButtonType.Page, 4);
			AddLabel(360, 135, 0, "Menu Malarstwa");
			AddItem(307, 132, 3018);
			AddLabel(360, 290, 0, "Abstrakty");
			AddLabel(360, 190, 0, "Portrety");
			AddLabel(360, 240, 0, "Martwa natura");

			AddPage(2);
			AddBackground(245, 106, 321, 233, 2600);
			AddLabel(360, 135, 0, "Menu Portretów");
			AddLabel(380, 200, 0, "Namaluj");
			AddButton(395, 230, 1210, 1209, 1, GumpButtonType.Reply, 1);
			AddButton(265, 155, 5223, 5223, 0, GumpButtonType.Page, 1);
			
			AddPage(3);
			AddBackground(244, 106, 321, 233, 2600);
			AddLabel(345, 132, 0, "Menu Martwej Natury");
			AddItem(310, 180, 9229);
			AddItem(460, 180, 9231);
			AddItem(310, 255, 9233);
			AddItem(460, 255, 9235);
			AddButton(290, 195, 1210, 1209, 2, GumpButtonType.Reply, 1);
			AddButton(440, 195, 1210, 1209, 3, GumpButtonType.Reply, 1);
			AddButton(290, 285, 1210, 1209, 4, GumpButtonType.Reply, 1);
			AddButton(440, 285, 1210, 1209, 5, GumpButtonType.Reply, 1);
			AddButton(265, 155, 5223, 5223, 0, GumpButtonType.Page, 1);

			AddPage(4);
			AddBackground(245, 106, 321, 233, 2600);
			AddLabel(345, 131, 0, "Menu Abstraktów");
			AddButton(290, 300, 1210, 1209, 6, GumpButtonType.Reply, 1);
			AddButton(390, 300, 1210, 1209, 7, GumpButtonType.Reply, 1);
			AddButton(490, 305, 1210, 1209, 8, GumpButtonType.Reply, 1);
			AddItem(375, 190, 9237);
			AddItem(275, 180, 9239);
			AddItem(470, 160, 10375);
			AddButton(265, 155, 5223, 5223, 0, GumpButtonType.Page, 1);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			var from = sender.Mobile;

			if (!HasNeededItems(from))
			{
				from.SendMessage("Nie masz odpowiednich materialow");
				return;
			}
			if(info.ButtonID == 1)
			{
				from.SendMessage("Kogo chcialbys namalowac?");
			}
			else
			{
				from.SendMessage("Co chcialbys namalowac?");
			}

			from.Target = info.ButtonID switch
			{
				1 => new PortraitTarget(),
				2 => new ObjectTarget(typeof(StillLifeSmall1)),
				3 => new ObjectTarget(typeof(StillLifeSmall2)),
				4 => new ObjectTarget(typeof(StillLifeLarge1)),
				5 => new ObjectTarget(typeof(StillLifeLarge2)),
				6 => new ObjectTarget(typeof(AbstractPainting1)),
				7 => new ObjectTarget(typeof(AbstractPainting2)),
				8 => new ObjectTarget(typeof(AbstractPainting3)),
				_ => null
			};
		}

		private bool HasNeededItems(Mobile owner)
		{
			var canvasCount = owner.Backpack.GetAmount(typeof(Canvas), true);
			var bucketCount = owner.Backpack.GetAmount(typeof(PaintBucket), true);
			
			return canvasCount >= 1 && bucketCount >= 1;
		}

		public class PortraitTarget() : Target(10, false, TargetFlags.None)
		{
			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is PlayerMobile pm)
				{
					Item painting = pm.Female
						? new FemalePortrait(from.Name, pm.Name)
						: new MalePortrait(from.Name, pm.Name);
					from.AddToBackpack(painting);
					EventSink.InvokePaintingCreated(new PaintingCreatedEventArgs(from, painting));
					
					from.Backpack.FindItemByType<Canvas>().Consume(1);
					from.Backpack.FindItemByType<PaintBucket>().Consume(1);
				}
				else
				{
					from.SendMessage("Glupcze, tego nie namalujesz!");
				}
			}
		}

		public class ObjectTarget(Type paintingType) : Target(10, false, TargetFlags.None)
		{
			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Static || targeted is AddonComponent)
				{
					from.SendMessage("Glupcze, tego nie namalujesz!!");
					return;
				}
				if (targeted is Item item)
				{
					CreatePainting(from, String.IsNullOrEmpty(item.Name) ? item.GetType().Name : item.Name);
				}
				else if (targeted is Mobile m )
				{
					CreatePainting(from, String.IsNullOrEmpty(m.Name) ? m.GetType().Name : m.Name);
				}
				else
				{
					from.SendMessage("Glupcze, tego nie namalujesz!!");
				}
			}


			private void CreatePainting(Mobile from, string subject)
			{
				try
				{
					var painting = (Item)Activator.CreateInstance(paintingType, from.Name, subject);
					from.AddToBackpack(painting);
					EventSink.InvokePaintingCreated(new PaintingCreatedEventArgs(from, painting));

					from.Backpack.FindItemByType<Canvas>().Consume(1);
					from.Backpack.FindItemByType<PaintBucket>().Consume(1);
					
				}
				catch (Exception e)
				{
					Diagnostics.ExceptionLogging.LogException(e);
				}
			}
		}
	}
}
