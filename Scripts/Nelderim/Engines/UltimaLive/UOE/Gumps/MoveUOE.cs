#region References

using System;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Network;

#endregion

namespace Server.Gumps
{
	public class MoveUOE : Gump
	{
		public Mobile m_Mob { get; set; }

		public Item i_Tool { get; set; }

		public MoveUOE(Mobile m, int p) : base(0, 0)
		{
			PlayerMobile pm = m as PlayerMobile;

			if (pm == null || pm.Backpack == null)
				return;

			m_Mob = pm;

			Item check = pm.Backpack.FindItemByType(typeof(UOETool));

			if (check == null)
			{
				pm.SendMessage(pm.Name + ", Contact Draco, System Error : Check Failed {0}/{1}", check);

				return;
			}

			UOETool dd = check as UOETool;

			i_Tool = dd;

			this.Closable = false;
			this.Disposable = false;
			this.Dragable = false;
			this.Resizable = false;

			this.X = dd.x_Move;
			this.Y = dd.y_Move;

			this.AddPage(0);

			this.AddBackground(0, 0, 100, 35, dd.s_Style);

			this.AddLabel(27, 7, dd.c_Font, @"Move Tile");
			this.AddButton(6, 10, 1209, 1210, 1, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(NetState ns, RelayInfo info)
		{
			Mobile mob_m = ns.Mobile;

			PlayerMobile pm = mob_m as PlayerMobile;

			UOETool dd = i_Tool as UOETool;

			if (pm == null || dd == null)
				return;

			switch (info.ButtonID)
			{
				case 0:
				{
					pm.SendMessage(pm.Name + ", Thanks for using the UO Editor!");

					dd.SendSYSBCK(pm, dd);

					break;
				}
				case 1:
				{
					if (dd.StcT)
						CommandSystem.Handle(pm, String.Format("{0}MoveStatic", CommandSystem.Prefix));

					dd.SendSYSBCK(pm, dd);

					if (dd.SndOn)
						pm.PlaySound(dd.Snd5);

					break;
				}
			}
		}
	}
}
