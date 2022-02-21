#region References

using Server.Items;
using Server.Mobiles;
using Server.Network;

#endregion

namespace Server.Gumps
{
	public class MultiUOE : Gump
	{
		public Mobile m_Mob { get; set; }

		public Item i_Tool { get; set; }

		public MultiUOE(Mobile m, int p) : base(0, 0)
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

			this.X = dd.x_Multi;
			this.Y = dd.y_Multi;

			this.AddPage(0);

			this.AddBackground(0, 0, 100, 34, dd.s_Style);

			if (dd.MultiT == false)
				this.AddLabel(24, 6, dd.c_Font, @"Multi CMD");
			else
				this.AddLabel(24, 6, 1153, @"Multi CMD");

			if (dd.MultiT == false)
				this.AddButton(5, 9, 1209, 1210, 1, GumpButtonType.Reply, 0);
			else
				this.AddButton(5, 9, 1210, 1209, 1, GumpButtonType.Reply, 0);
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
					if (dd.MultiT == false)
						dd.MultiT = true;
					else
						dd.MultiT = false;

					if (dd.MultiT)
						dd.Cir_T = false;

					dd.SendSYSBCK(pm, dd);

					if (dd.SndOn)
						pm.PlaySound(dd.Snd5);

					break;
				}
			}
		}
	}
}
