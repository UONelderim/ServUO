#region References

using Server.Items;
using Server.Mobiles;
using Server.Network;

#endregion

namespace Server.Gumps
{
	public class MainUOE : Gump
	{
		public Mobile m_Mob { get; set; }

		public Item i_Tool { get; set; }

		public MainUOE(Mobile m, int p) : base(0, 0)
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

			this.X = dd.x_Main;
			this.Y = dd.y_Main;

			this.AddPage(0);

			this.AddImage(1, 0, 100);
			this.AddLabel(41, 18, dd.Hue_T, @"UO Editor");
			this.AddLabel(24, 67, dd.c_Font, @"Ver 2.0 UL v97");

			if (dd.in_Prog == false)
				this.AddButton(39, 42, 247, 248, 1, GumpButtonType.Reply, 0);
			else
				this.AddButton(39, 42, 242, 241, 1, GumpButtonType.Reply, 0);
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
					if (pm.HasGump(typeof(GridUOE)))
						pm.CloseGump(typeof(GridUOE));

					if (dd.in_Prog == false)
						dd.in_Prog = true;
					else
						dd.in_Prog = false;

					if (dd.in_Prog == false)
					{
						dd.StcT = false;
						dd.LndT = false;

						dd.SendSYSBCK(pm, dd);

						if (dd.SndOn)
							pm.PlaySound(dd.Snd3);

						break;
					}

					if (dd.SndOn)
						pm.PlaySound(dd.Snd5);

					dd.SendSYSBCK(pm, dd);

					break;
				}
			}
		}
	}
}
