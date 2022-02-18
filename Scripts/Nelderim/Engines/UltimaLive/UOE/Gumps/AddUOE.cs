#region References

using System;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using UltimaLive;

#endregion

namespace Server.Gumps
{
	public class AddUOE : Gump
	{
		public Mobile m_Mob { get; set; }

		public Item i_Tool { get; set; }

		public AddUOE(Mobile m, int p) : base(0, 0)
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

			this.X = dd.x_Add;
			this.Y = dd.y_Add;

			this.AddPage(0);

			this.AddBackground(0, 0, 100, 163, dd.s_Style);

			this.AddLabel(14, 5, dd.Hue_T, @"Add Tile");

			this.AddLabel(76, 29, dd.c_Font, @"ID");

			if (dd.StcT)
				this.AddTextEntry(9, 28, 58, 20, dd.Hue_G, 1, @"" + dd.StcID);
			if (dd.LndT)
				this.AddTextEntry(9, 28, 58, 20, dd.Hue_G, 1, @"" + dd.LndID);

			if (dd.MultiT == false)
			{
				if (dd.StcT)
				{
					this.AddLabel(76, 54, dd.c_Font, @"X");
					this.AddTextEntry(9, 55, 58, 20, dd.Hue_G, 2, @"" + dd.StcX);
					this.AddLabel(76, 82, dd.c_Font, @"Y");
					this.AddTextEntry(9, 82, 58, 20, dd.Hue_G, 3, @"" + dd.StcY);
					this.AddLabel(76, 108, dd.c_Font, @"Z");
					this.AddTextEntry(9, 109, 58, 20, dd.Hue_G, 4, @"" + dd.StcZ);
				}

				if (dd.LndT)
				{
					this.AddLabel(76, 54, dd.c_Font, @"X");
					this.AddTextEntry(9, 55, 58, 20, dd.Hue_G, 2, @"" + dd.LndX);
					this.AddLabel(76, 82, dd.c_Font, @"Y");
					this.AddTextEntry(9, 82, 58, 20, dd.Hue_G, 3, @"" + dd.LndY);
					this.AddLabel(76, 108, dd.c_Font, @"Z");
					this.AddTextEntry(9, 109, 58, 20, dd.Hue_G, 4, @"" + dd.LndZ);
				}
			}

			this.AddButton(7, 135, 247, 248, 1, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(NetState ns, RelayInfo info)
		{
			Mobile mob_m = ns.Mobile;

			PlayerMobile pm = mob_m as PlayerMobile;

			UOETool dd = i_Tool as UOETool;

			if (pm == null || dd == null)
				return;

			int si;

			if (dd.StcT)
			{
				TextRelay entry1 = info.GetTextEntry(1);
				string text1 = (entry1 == null ? "" : entry1.Text.Trim());
				bool r1 = Int32.TryParse(text1, out si);
				if (r1) { dd.StcID = si; }

				if (dd.MultiT == false)
				{
					TextRelay entry2 = info.GetTextEntry(2);
					string text2 = (entry2 == null ? "" : entry2.Text.Trim());
					bool r2 = Int32.TryParse(text2, out si);
					if (r2) { dd.StcX = si; }

					TextRelay entry3 = info.GetTextEntry(3);
					string text3 = (entry3 == null ? "" : entry3.Text.Trim());
					bool r3 = Int32.TryParse(text3, out si);
					if (r3) { dd.StcY = si; }

					TextRelay entry4 = info.GetTextEntry(4);
					string text4 = (entry4 == null ? "" : entry4.Text.Trim());
					bool r4 = Int32.TryParse(text4, out si);
					if (r4) { dd.StcZ = si; }
				}
			}

			if (dd.LndT)
			{
				TextRelay entry1 = info.GetTextEntry(1);
				string text1 = (entry1 == null ? "" : entry1.Text.Trim());
				bool r1 = Int32.TryParse(text1, out si);
				if (r1) { dd.LndID = si; }

				if (dd.MultiT == false)
				{
					TextRelay entry2 = info.GetTextEntry(2);
					string text2 = (entry2 == null ? "" : entry2.Text.Trim());
					bool r2 = Int32.TryParse(text2, out si);
					if (r2) { dd.LndX = si; }

					TextRelay entry3 = info.GetTextEntry(3);
					string text3 = (entry3 == null ? "" : entry3.Text.Trim());
					bool r3 = Int32.TryParse(text3, out si);
					if (r3) { dd.LndY = si; }

					TextRelay entry4 = info.GetTextEntry(4);
					string text4 = (entry4 == null ? "" : entry4.Text.Trim());
					bool r4 = Int32.TryParse(text4, out si);
					if (r4) { dd.LndZ = si; }
				}
			}

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
					bool MapCheck = dd.MapCKUOE(pm, dd);

					if (MapCheck == false)
					{
						pm.SendMessage(pm.Name + ", You entered improper values in the XYZ fields!");

						dd.SendSYSBCK(pm, dd);

						if (dd.SndOn)
							pm.PlaySound(dd.Snd7);

						break;
					}

					bool IDCheck = dd.IDCKUOE(pm, dd);

					if (IDCheck == false)
					{
						pm.SendMessage(pm.Name + ", You entered improper values in the ID field!");

						dd.SendSYSBCK(pm, dd);

						if (dd.SndOn)
							pm.PlaySound(dd.Snd7);

						break;
					}

					bool HueCK = dd.HueCKUOE(pm, dd);

					if (HueCK == false)
					{
						pm.SendMessage(pm.Name + ", You can only enter 1-3000 for the value!");

						dd.SendSYSBCK(pm, dd);

						if (dd.SndOn)
							pm.PlaySound(dd.Snd7);

						break;
					}

					if (dd.StcT)
					{
						if (dd.MultiT)
							CommandSystem.Handle(pm,
								String.Format("{0}m addStatic {1}", CommandSystem.Prefix, dd.StcID));
						if (dd.StcX == 0 || dd.StcY == 0)
							CommandSystem.Handle(pm, String.Format("{0}addStatic {1}", CommandSystem.Prefix, dd.StcID));
						else
							new AddStatic(pm.Map.MapID, dd.StcID, dd.StcZ, dd.StcX, dd.StcY, dd.Hue_S).DoOperation();

						dd.SendSYSBCK(pm, dd);

						if (dd.SndOn)
							pm.PlaySound(dd.Snd5);

						break;
					}

					if (dd.LndT)
					{
						if (dd.MultiT)
							CommandSystem.Handle(pm,
								String.Format("{0}m setLandId {1}", CommandSystem.Prefix, dd.LndID));
						if (dd.StcX == 0 || dd.StcY == 0)
							CommandSystem.Handle(pm, String.Format("{0}setLandId {1}", CommandSystem.Prefix, dd.LndID));
						if (IDCheck)
						{
							new SetLandID(dd.LndX, dd.LndY, pm.Map.MapID, dd.LndID).DoOperation();
							new SetLandAltitude(dd.LndX, dd.LndY, pm.Map.MapID, dd.LndZ).DoOperation();
						}

						dd.SendSYSBCK(pm, dd);

						if (dd.SndOn)
							pm.PlaySound(dd.Snd5);

						break;
					}

					dd.SendSYSBCK(pm, dd);

					break;
				}
			}
		}
	}
}
