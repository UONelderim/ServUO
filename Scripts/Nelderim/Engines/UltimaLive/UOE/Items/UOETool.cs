#region References

using Server.Gumps;
using Server.Mobiles;

#endregion

namespace Server.Items
{
	[Flipable(0x14F5, 0x14F6)]
	public class UOETool : Item
	{
		[CommandProperty(AccessLevel.Administrator)]
		public bool in_Prog { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int p_Page { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int s_Style { get; set; } = 9200;

		[CommandProperty(AccessLevel.Administrator)]
		public int c_Font { get; set; } = 48;

		[CommandProperty(AccessLevel.Administrator)]
		public int Hue_G { get; set; } = 75;

		[CommandProperty(AccessLevel.Administrator)]
		public int Hue_T { get; set; } = 50;

		[CommandProperty(AccessLevel.Administrator)]
		public bool SndOn { get; set; } = true;

		[CommandProperty(AccessLevel.Administrator)]
		public int Snd1 { get; set; } = 0x565;

		[CommandProperty(AccessLevel.Administrator)]
		public int Snd2 { get; set; } = 0x568;

		[CommandProperty(AccessLevel.Administrator)]
		public int Snd3 { get; set; } = 0x0F3;

		[CommandProperty(AccessLevel.Administrator)]
		public int Snd4 { get; set; } = 0x543;

		[CommandProperty(AccessLevel.Administrator)]
		public int Snd5 { get; set; } = 0x239;

		[CommandProperty(AccessLevel.Administrator)]
		public int Snd6 { get; set; } = 0x100;

		[CommandProperty(AccessLevel.Administrator)]
		public int Snd7 { get; set; } = 0x040;

		[CommandProperty(AccessLevel.Administrator)]
		public int Snd8 { get; set; } = 0x027;

		[CommandProperty(AccessLevel.Administrator)]
		public string GmpN { get; set; } = "Gump Name";

		[CommandProperty(AccessLevel.Administrator)]
		public int GmpX { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int GmpY { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Add { get; set; } = 805;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Add { get; set; } = 95;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Alt { get; set; } = 905;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Alt { get; set; } = 258;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Cir { get; set; } = 100;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Cir { get; set; } = 100;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Del { get; set; } = 905;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Del { get; set; } = 95;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Info { get; set; } = 805;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Info { get; set; } = 25;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Hue { get; set; } = 905;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Hue { get; set; } = 125;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_List { get; set; } = 100;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_List { get; set; } = 100;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Main { get; set; } = 660;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Main { get; set; } = 505;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Move { get; set; } = 905;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Move { get; set; } = 197;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Multi { get; set; } = 805;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Multi { get; set; } = 258;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Pick { get; set; } = 400;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Pick { get; set; } = 300;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Pos { get; set; } = 805;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Pos { get; set; } = 292;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Reset { get; set; } = 685;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Reset { get; set; } = 605;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Rnd { get; set; } = 100;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Rnd { get; set; } = 100;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_SetId { get; set; } = 905;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_SetId { get; set; } = 160;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_SetLoc { get; set; } = 100;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_SetLoc { get; set; } = 100;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Setting { get; set; } = 805;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Setting { get; set; } = 422;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Sub { get; set; } = 683;

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Sub { get; set; } = 455;

		[CommandProperty(AccessLevel.Administrator)]
		public int x_Sel { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int y_Sel { get; set; } = 605;

		[CommandProperty(AccessLevel.Administrator)]
		public bool StcT { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public bool LndT { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public bool MultiT { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public bool ResetT { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public string TempN { get; set; } = "tile";

		[CommandProperty(AccessLevel.Administrator)]
		public int TempID { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int TempX { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int TempY { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int TempZ { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int TempH { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int StcX { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int StcY { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int StcZ { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int StcID { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int LndX { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int LndY { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int LndZ { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int LndID { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int M_Val { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int A_Val { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public bool Cir_T { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int Cir_V { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public bool Rnd_T { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int Rnd_V { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int Hue_S { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public bool ListT { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int List1 { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int List2 { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int List3 { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int List4 { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int List5 { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int List6 { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int List7 { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int List8 { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int List9 { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int List0 { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int CntBG { get; set; } = 18;

		[CommandProperty(AccessLevel.Administrator)]
		public int CntGN { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public string PassW { get; set; } = "";

		[Constructable]
		public UOETool() : base(0x14F5)
		{
			Name = "Ultima Live Editor";
			Hue = 1153;
			Weight = 0;
			LootType = LootType.Blessed;
		}

		public override void OnDoubleClick(Mobile m)
		{
			PlayerMobile pm = m as PlayerMobile;

			if (pm == null)
				return;

			if (PassW == "")
				PassW = pm.Name;

			if (PassW != pm.Name)
			{
				pm.SendMessage(pm.Name + ", You not the owner of this tool, REMOVING!");

				this.Delete();

				if (SndOn)
					pm.PlaySound(Snd1);

				return;
			}

			if (!IsChildOf(pm.Backpack))
			{
				pm.SendLocalizedMessage(1042001);

				pm.SendMessage(pm.Name + ", You picked up the Ultima Live Editor & placed it into your backpack!");

				pm.PlaceInBackpack(this);

				if (SndOn)
					pm.PlaySound(Snd2);

				return;
			}

			if (pm.HasGump(typeof(MainUOE)))
			{
				if (in_Prog)
				{
					pm.SendMessage(pm.Name + ", Ultima Live Editor is already running!");


					if (SndOn)
						pm.PlaySound(Snd1);

					return;
				}

				pm.CloseGump(typeof(MainUOE));

				Movable = true;

				if (SndOn)
					pm.PlaySound(Snd3);

				return;
			}

			in_Prog = false;
			StcT = false;
			LndT = false;
			Movable = false;

			pm.SendMessage(pm.Name + ", Welcome to the Ultima Live Editor!");

			pm.SendGump(new MainUOE(pm, 0));

			if (SndOn)
				pm.PlaySound(Snd4);
		}

		public void ResendPick(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return;

			if (CntBG > 18)
				CntBG = 1;
			if (CntBG <= 0)
				CntBG = 18;
			if (CntBG == 1)
				dd.s_Style = 2600;
			if (CntBG == 2)
				dd.s_Style = 2620;
			if (CntBG == 3)
				dd.s_Style = 3000;
			if (CntBG == 4)
				dd.s_Style = 3500;
			if (CntBG == 5)
				dd.s_Style = 3600;
			if (CntBG == 6)
				dd.s_Style = 5100;
			if (CntBG == 7)
				dd.s_Style = 5120;
			if (CntBG == 8)
				dd.s_Style = 5054;
			if (CntBG == 9)
				dd.s_Style = 9250;
			if (CntBG == 10)
				dd.s_Style = 9260;
			if (CntBG == 11)
				dd.s_Style = 9270;
			if (CntBG == 12)
				dd.s_Style = 9300;
			if (CntBG == 13)
				dd.s_Style = 9350;
			if (CntBG == 14)
				dd.s_Style = 9400;
			if (CntBG == 15)
				dd.s_Style = 9450;
			if (CntBG == 16)
				dd.s_Style = 9550;
			if (CntBG == 17)
				dd.s_Style = 9559;
			if (CntBG == 18)
				dd.s_Style = 9200;

			if (pm.HasGump(typeof(PickUOE)))
				pm.CloseGump(typeof(PickUOE));
			pm.SendGump(new PickUOE(pm, dd.p_Page));


			if (dd.SndOn)
				pm.PlaySound(dd.Snd5);
		}

		public void ResetUOE(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return;

			if (dd.SndOn)
				pm.PlaySound(dd.Snd6);

			CloseUOE(pm, dd);

			if (pm.HasGump(typeof(GridUOE)))
				pm.CloseGump(typeof(GridUOE));

			dd.in_Prog = false;
			dd.p_Page = 0;
			dd.s_Style = 9200;

			dd.c_Font = 48;
			dd.Hue_G = 75;
			dd.Hue_T = 50;

			dd.SndOn = true;
			dd.Snd1 = 0x565;
			dd.Snd2 = 0x568;
			dd.Snd3 = 0x0F3;
			dd.Snd4 = 0x543;
			dd.Snd5 = 0x239;
			dd.Snd6 = 0x100;
			dd.Snd7 = 0x040;
			dd.Snd8 = 0x027;

			dd.GmpN = "Gump Name";
			dd.GmpX = 0;
			dd.GmpY = 0;

			dd.x_Add = 805;
			dd.y_Add = 95;
			dd.x_Alt = 905;
			dd.y_Alt = 258;
			dd.x_Cir = 100;
			dd.y_Cir = 100;
			dd.x_Del = 905;
			dd.y_Del = 95;
			dd.x_Hue = 905;
			dd.y_Hue = 125;
			dd.x_Info = 805;
			dd.y_Info = 25;
			dd.x_List = 100;
			dd.y_List = 100;
			dd.x_Main = 660;
			dd.y_Main = 505;
			dd.x_Move = 905;
			dd.y_Move = 197;
			dd.x_Multi = 805;
			dd.y_Multi = 258;
			dd.x_Pick = 400;
			dd.y_Pick = 300;
			dd.x_Pos = 805;
			dd.y_Pos = 292;
			dd.x_Reset = 685;
			dd.y_Reset = 605;
			dd.x_Rnd = 100;
			dd.y_Rnd = 100;
			dd.x_SetId = 905;
			dd.y_SetId = 160;
			dd.x_SetLoc = 100;
			dd.y_SetLoc = 100;
			dd.x_Setting = 805;
			dd.y_Setting = 422;
			dd.x_Sub = 683;
			dd.y_Sub = 455;
			dd.x_Sel = 0;
			dd.y_Sel = 605;

			dd.StcT = false;
			dd.LndT = false;

			dd.MultiT = false;
			dd.ResetT = false;

			dd.TempN = "tile";
			dd.TempID = 0;
			dd.TempX = 0;
			dd.TempY = 0;
			dd.TempZ = 0;
			dd.TempH = 0;

			dd.StcX = 0;
			dd.StcY = 0;
			dd.StcZ = 0;
			dd.StcID = 0;

			dd.LndX = 0;
			dd.LndY = 0;
			dd.LndZ = 0;
			dd.LndID = 0;

			dd.M_Val = 0;
			dd.A_Val = 0;

			dd.Cir_T = false;
			dd.Cir_V = 0;

			dd.Rnd_T = false;
			dd.Rnd_V = 0;

			dd.Hue_S = 0;

			dd.ListT = false;
			dd.List1 = 0;
			dd.List2 = 0;
			dd.List3 = 0;
			dd.List4 = 0;
			dd.List5 = 0;
			dd.List6 = 0;
			dd.List7 = 0;
			dd.List8 = 0;
			dd.List9 = 0;
			dd.List0 = 0;

			dd.CntBG = 18;
			dd.CntGN = 0;

			pm.SendMessage(pm.Name + ", You have reset the UOE Tool!");

			pm.SendGump(new MainUOE(pm, 0));
		}

		public void CloseUOE(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return;

			if (pm.HasGump(typeof(AddUOE)))
				pm.CloseGump(typeof(AddUOE));
			if (pm.HasGump(typeof(AltUOE)))
				pm.CloseGump(typeof(AltUOE));
			if (pm.HasGump(typeof(CirUOE)))
				pm.CloseGump(typeof(CirUOE));
			if (pm.HasGump(typeof(DelUOE)))
				pm.CloseGump(typeof(DelUOE));
			if (pm.HasGump(typeof(HueUOE)))
				pm.CloseGump(typeof(HueUOE));
			if (pm.HasGump(typeof(InfoUOE)))
				pm.CloseGump(typeof(InfoUOE));
			if (pm.HasGump(typeof(ListUOE)))
				pm.CloseGump(typeof(ListUOE));
			if (pm.HasGump(typeof(MainUOE)))
				pm.CloseGump(typeof(MainUOE));
			if (pm.HasGump(typeof(MoveUOE)))
				pm.CloseGump(typeof(MoveUOE));
			if (pm.HasGump(typeof(MultiUOE)))
				pm.CloseGump(typeof(MultiUOE));
			if (pm.HasGump(typeof(PosUOE)))
				pm.CloseGump(typeof(PosUOE));
			if (pm.HasGump(typeof(ResetUOE)))
				pm.CloseGump(typeof(ResetUOE));
			if (pm.HasGump(typeof(RndUOE)))
				pm.CloseGump(typeof(RndUOE));
			if (pm.HasGump(typeof(SetIdUOE)))
				pm.CloseGump(typeof(SetIdUOE));
			if (pm.HasGump(typeof(SetLocUOE)))
				pm.CloseGump(typeof(SetLocUOE));
			if (pm.HasGump(typeof(SettingUOE)))
				pm.CloseGump(typeof(SettingUOE));
			if (pm.HasGump(typeof(SubUOE)))
				pm.CloseGump(typeof(SubUOE));
			if (pm.HasGump(typeof(GumpSelUOE)))
				pm.CloseGump(typeof(GumpSelUOE));
			if (pm.HasGump(typeof(HelpUOE)))
				pm.CloseGump(typeof(HelpUOE));
		}

		public void SendSYSBCK(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return;

			CloseUOE(pm, dd);

			pm.SendGump(new MainUOE(pm, dd.p_Page));

			if (dd.in_Prog)
			{
				pm.SendGump(new SubUOE(pm, dd.p_Page));

				if (dd.StcT)
				{
					pm.SendGump(new AddUOE(pm, dd.p_Page));

					pm.SendGump(new AltUOE(pm, dd.p_Page));

					//pm.SendGump( new CirUOE( pm, dd.p_Page ) ); //Coming Soon

					pm.SendGump(new DelUOE(pm, dd.p_Page));

					pm.SendGump(new HueUOE(pm, dd.p_Page));

					pm.SendGump(new InfoUOE(pm, dd.p_Page));

					//pm.SendGump( new ListUOE( pm, dd.p_Page ) ); //Coming Soon

					pm.SendGump(new MoveUOE(pm, dd.p_Page));

					pm.SendGump(new MultiUOE(pm, dd.p_Page));

					pm.SendGump(new PosUOE(pm, dd.p_Page));

					pm.SendGump(new ResetUOE(pm, dd.p_Page));

					//pm.SendGump( new RndUOE( pm, dd.p_Page ) ); //Coming Soon

					pm.SendGump(new SetIdUOE(pm, dd.p_Page));

					//pm.SendGump( new SetLocUOE( pm, dd.p_Page ) ); //Coming Soon

					pm.SendGump(new SettingUOE(pm, dd.p_Page));

					pm.SendGump(new GumpSelUOE(pm, dd.p_Page));

					return;
				}

				if (dd.LndT)
				{
					pm.SendGump(new AddUOE(pm, dd.p_Page));

					pm.SendGump(new AltUOE(pm, dd.p_Page));

					//pm.SendGump( new CirUOE( pm, dd.p_Page ) ); //Coming Soon

					pm.SendGump(new InfoUOE(pm, dd.p_Page));

					//pm.SendGump( new ListUOE( pm, dd.p_Page ) ); //Coming Soon

					pm.SendGump(new MultiUOE(pm, dd.p_Page));

					//pm.SendGump( new PosUOE( pm, dd.p_Page ) ); //Coming Soon

					pm.SendGump(new ResetUOE(pm, dd.p_Page));

					//pm.SendGump( new RndUOE( pm, dd.p_Page ) ); //Coming Soon

					pm.SendGump(new SetIdUOE(pm, dd.p_Page));

					//pm.SendGump( new SetLocUOE( pm, dd.p_Page ) ); //Coming Soon

					pm.SendGump(new SettingUOE(pm, dd.p_Page));

					pm.SendGump(new GumpSelUOE(pm, dd.p_Page));
				}
			}
		}

		public bool MapCKUOE(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return false;

			int ckx = 0;
			int cky = 0;

			if (pm.Map == Map.Felucca)
			{
				ckx = 7168;
				cky = 4096;
			}

			if (pm.Map == Map.Trammel)
			{
				ckx = 7168;
				cky = 4096;
			}

			if (pm.Map == Map.Ilshenar)
			{
				ckx = 2304;
				cky = 1600;
			}

			if (pm.Map == Map.Malas)
			{
				ckx = 2560;
				cky = 2048;
			}

			if (pm.Map == Map.Tokuno)
			{
				ckx = 1448;
				cky = 1448;
			}

			if (pm.Map == Map.TerMur)
			{
				ckx = 1280;
				cky = 4096;
			}
			/*if ( pm.Map == Map.NewWorld )
			{
				ckx = 7168;
				cky = 4096;
			}*/

			if (dd.StcT)
			{
				if (dd.StcX > ckx || dd.StcX < 0)
					return false;
				if (dd.StcY > cky || dd.StcY < 0)
					return false;
				return true;
			}

			if (dd.LndT)
			{
				if (dd.LndX > ckx || dd.LndX < 0)
					return false;
				if (dd.LndY > cky || dd.LndY < 0)
					return false;
				return true;
			}

			if (dd.SndOn)
				pm.PlaySound(dd.Snd7);

			return false;
		}

		public bool MapAltUOE(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return false;

			int Hmin = 1;
			int Hmax = 100;

			if (dd.StcT)
			{
				if (dd.A_Val > Hmax || dd.A_Val < Hmin)
					return false;
				return true;
			}

			if (dd.LndT)
			{
				if (dd.A_Val > Hmax || dd.A_Val < Hmin)
					return false;
				return true;
			}

			if (dd.SndOn)
				pm.PlaySound(dd.Snd7);

			return false;
		}

		public bool CirCKUOE(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return false;

			int Cmin = 1;
			int Cmax = 10;

			if (dd.Cir_V > Cmax || dd.Cir_V < Cmin)
				return false;
			return true;
		}

		public bool RndCKUOE(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return false;

			int Rmin = 1;
			int Rmax = 10;

			if (dd.Rnd_V > Rmax || dd.Rnd_V < Rmin)
				return false;
			return true;
		}

		public bool MovCKUOE(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return false;

			int Mmin = 1;
			int Mmax = 15;

			if (dd.M_Val > Mmax || dd.M_Val < Mmin)
				return false;
			return true;
		}

		public bool HueCKUOE(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return false;

			int Umin = 0;
			int Umax = 3000;

			if (dd.c_Font > Umax || dd.c_Font < Umin)
				return false;
			if (dd.Hue_S > Umax || dd.Hue_S < Umin)
				return false;
			if (dd.Hue_G > Umax || dd.Hue_G < Umin)
				return false;
			if (dd.Hue_T > Umax || dd.Hue_T < Umin)
				return false;
			return true;
		}

		public bool IDCKUOE(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return false;


			int Smin = 2;
			int Smax = 16383;

			int Lmin = 3;
			int Lmax = 16383;

			if (StcT)
			{
				if (dd.StcID > Smax || dd.StcID < Smin)
					return false;
				return true;
			}

			if (LndT)
			{
				if (dd.LndID > Lmax || dd.LndID < Lmin)
					return false;
				return true;
			}

			if (dd.SndOn)
				pm.PlaySound(dd.Snd7);

			return false;
		}

		public void GumpNameUOE(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return;

			if (CntGN < 0)
				CntGN = 19;

			if (CntGN > 19)
				CntGN = 0;

			if (CntGN == 0)
				GmpN = "Gump Name";

			if (CntGN == 1)
				GmpN = "Add Gump";

			if (CntGN == 2)
				GmpN = "Alt Gump";

			if (CntGN == 3)
				GmpN = "Cir Gump";

			if (CntGN == 4)
				GmpN = "Del Gump";

			if (CntGN == 5)
				GmpN = "Hue Gump";

			if (CntGN == 6)
				GmpN = "Info Gump";

			if (CntGN == 7)
				GmpN = "List Gump";

			if (CntGN == 8)
				GmpN = "Main Gump";

			if (CntGN == 9)
				GmpN = "Move Gump";

			if (CntGN == 10)
				GmpN = "Multi Gump";

			if (CntGN == 11)
				GmpN = "Pick Gump";

			if (CntGN == 12)
				GmpN = "POS Gump";

			if (CntGN == 13)
				GmpN = "Reset Gump";

			if (CntGN == 14)
				GmpN = "Rnd Gump";

			if (CntGN == 15)
				GmpN = "SetID Gump";

			if (CntGN == 16)
				GmpN = "SetLoc Gump";

			if (CntGN == 17)
				GmpN = "Set Gump";

			if (CntGN == 18)
				GmpN = "Sub Gump";

			if (CntGN == 19)
				GmpN = "This Gump";

			GetGumpNameUOE(pm, dd);
		}

		public void GetGumpNameUOE(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return;

			pm.SendGump(new GridUOE(pm, dd.p_Page));

			if (GmpN == "Gump Name")
			{
				GmpX = 0;
				GmpY = 0;
			}

			if (GmpN == "Add Gump")
			{
				GmpX = x_Add;
				GmpY = y_Add;
			}

			if (GmpN == "Alt Gump")
			{
				GmpX = x_Alt;
				GmpY = y_Alt;
			}

			if (GmpN == "Cir Gump")
			{
				GmpX = x_Cir;
				GmpY = y_Cir;
			}

			if (GmpN == "Del Gump")
			{
				GmpX = x_Del;
				GmpY = y_Del;
			}

			if (GmpN == "Hue Gump")
			{
				GmpX = x_Hue;
				GmpY = y_Hue;
			}

			if (GmpN == "Info Gump")
			{
				GmpX = x_Info;
				GmpY = y_Info;
			}

			if (GmpN == "List Gump")
			{
				GmpX = x_List;
				GmpY = y_List;
			}

			if (GmpN == "Main Gump")
			{
				GmpX = x_Main;
				GmpY = y_Main;
			}

			if (GmpN == "Move Gump")
			{
				GmpX = x_Move;
				GmpY = y_Move;
			}

			if (GmpN == "Multi Gump")
			{
				GmpX = x_Multi;
				GmpY = y_Multi;
			}

			if (GmpN == "Pick Gump")
			{
				GmpX = x_Pick;
				GmpY = y_Pick;
			}

			if (GmpN == "POS Gump")
			{
				GmpX = x_Pos;
				GmpY = y_Pos;
			}

			if (GmpN == "Reset Gump")
			{
				GmpX = x_Reset;
				GmpY = y_Reset;
			}

			if (GmpN == "Rnd Gump")
			{
				GmpX = x_Rnd;
				GmpY = y_Rnd;
			}

			if (GmpN == "SetID Gump")
			{
				GmpX = x_SetId;
				GmpY = y_SetId;
			}

			if (GmpN == "SetLoc Gump")
			{
				GmpX = x_SetLoc;
				GmpY = y_SetLoc;
			}

			if (GmpN == "Set Gump")
			{
				GmpX = x_Setting;
				GmpY = y_Setting;
			}

			if (GmpN == "Sub Gump")
			{
				GmpX = x_Sub;
				GmpY = y_Sub;
			}

			if (GmpN == "This Gump")
			{
				GmpX = x_Sel;
				GmpY = y_Sel;
			}

			SendSYSBCK(pm, dd);
		}

		public void SetGumpNameUOE(Mobile m, Item i)
		{
			PlayerMobile pm = m as PlayerMobile;

			UOETool dd = i as UOETool;

			if (pm == null || dd == null)
				return;

			if (GmpN == "Gump Name")
			{
				GmpX = 0;
				GmpY = 0;
			}

			if (GmpN == "Add Gump")
			{
				x_Add = GmpX;
				y_Add = GmpY;
			}

			if (GmpN == "Alt Gump")
			{
				x_Alt = GmpX;
				y_Alt = GmpY;
			}

			if (GmpN == "Cir Gump")
			{
				x_Cir = GmpX;
				y_Cir = GmpY;
			}

			if (GmpN == "Del Gump")
			{
				x_Del = GmpX;
				y_Del = GmpY;
			}

			if (GmpN == "Hue Gump")
			{
				x_Hue = GmpX;
				y_Hue = GmpY;
			}

			if (GmpN == "Info Gump")
			{
				x_Info = GmpX;
				y_Info = GmpY;
			}

			if (GmpN == "List Gump")
			{
				x_List = GmpX;
				y_List = GmpY;
			}

			if (GmpN == "Main Gump")
			{
				x_Main = GmpX;
				y_Main = GmpY;
			}

			if (GmpN == "Move Gump")
			{
				x_Move = GmpX;
				y_Move = GmpY;
			}

			if (GmpN == "Multi Gump")
			{
				x_Multi = GmpX;
				y_Multi = GmpY;
			}

			if (GmpN == "Pick Gump")
			{
				x_Pick = GmpX;
				y_Pick = GmpY;
			}

			if (GmpN == "POS Gump")
			{
				x_Pos = GmpX;
				y_Pos = GmpY;
			}

			if (GmpN == "Reset Gump")
			{
				x_Reset = GmpX;
				y_Reset = GmpY;
			}

			if (GmpN == "Rnd Gump")
			{
				x_Rnd = GmpX;
				y_Rnd = GmpY;
			}

			if (GmpN == "SetID Gump")
			{
				x_SetId = GmpX;
				y_SetId = GmpY;
			}

			if (GmpN == "SetLoc Gump")
			{
				x_SetLoc = GmpX;
				y_SetLoc = GmpY;
			}

			if (GmpN == "Set Gump")
			{
				x_Setting = GmpX;
				y_Setting = GmpY;
			}

			if (GmpN == "Sub Gump")
			{
				x_Sub = GmpX;
				y_Sub = GmpY;
			}

			if (GmpN == "This Gump")
			{
				x_Sel = GmpX;
				y_Sel = GmpY;
			}

			if (pm.HasGump(typeof(GridUOE)))
				pm.CloseGump(typeof(GridUOE));

			SendSYSBCK(pm, dd);
		}

		public UOETool(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version

			writer.Write(in_Prog);
			writer.Write(p_Page);
			writer.Write(s_Style);

			writer.Write(c_Font);
			writer.Write(Hue_G);
			writer.Write(Hue_T);

			writer.Write(SndOn);
			writer.Write(Snd1);
			writer.Write(Snd2);
			writer.Write(Snd3);
			writer.Write(Snd4);
			writer.Write(Snd5);
			writer.Write(Snd6);
			writer.Write(Snd7);
			writer.Write(Snd8);

			writer.Write(GmpN);
			writer.Write(GmpX);
			writer.Write(GmpY);

			writer.Write(x_Add);
			writer.Write(y_Add);
			writer.Write(x_Alt);
			writer.Write(y_Alt);
			writer.Write(x_Cir);
			writer.Write(y_Cir);
			writer.Write(x_Del);
			writer.Write(y_Del);
			writer.Write(x_Info);
			writer.Write(y_Info);
			writer.Write(x_Hue);
			writer.Write(y_Hue);
			writer.Write(x_List);
			writer.Write(y_List);
			writer.Write(x_Main);
			writer.Write(y_Main);
			writer.Write(x_Move);
			writer.Write(y_Move);
			writer.Write(x_Multi);
			writer.Write(y_Multi);
			writer.Write(x_Pick);
			writer.Write(y_Pick);
			writer.Write(x_Pos);
			writer.Write(y_Pos);
			writer.Write(x_Reset);
			writer.Write(y_Reset);
			writer.Write(x_Rnd);
			writer.Write(y_Rnd);
			writer.Write(x_SetId);
			writer.Write(y_SetId);
			writer.Write(x_SetLoc);
			writer.Write(y_SetLoc);
			writer.Write(x_Setting);
			writer.Write(y_Setting);
			writer.Write(x_Sub);
			writer.Write(y_Sub);
			writer.Write(x_Sel);
			writer.Write(y_Sel);

			writer.Write(StcT);
			writer.Write(LndT);

			writer.Write(MultiT);
			writer.Write(ResetT);

			writer.Write(TempN);
			writer.Write(TempID);
			writer.Write(TempX);
			writer.Write(TempY);
			writer.Write(TempZ);
			writer.Write(TempH);

			writer.Write(StcX);
			writer.Write(StcY);
			writer.Write(StcZ);
			writer.Write(StcID);

			writer.Write(LndX);
			writer.Write(LndY);
			writer.Write(LndZ);
			writer.Write(LndID);

			writer.Write(M_Val);
			writer.Write(A_Val);

			writer.Write(Cir_T);
			writer.Write(Cir_V);

			writer.Write(Rnd_T);
			writer.Write(Rnd_V);

			writer.Write(Hue_S);

			writer.Write(ListT);
			writer.Write(List1);
			writer.Write(List2);
			writer.Write(List3);
			writer.Write(List4);
			writer.Write(List5);
			writer.Write(List6);
			writer.Write(List7);
			writer.Write(List8);
			writer.Write(List9);
			writer.Write(List0);

			writer.Write(CntBG);
			writer.Write(CntGN);

			writer.Write(PassW);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			in_Prog = reader.ReadBool();
			p_Page = reader.ReadInt();
			s_Style = reader.ReadInt();

			c_Font = reader.ReadInt();
			Hue_G = reader.ReadInt();
			Hue_T = reader.ReadInt();

			SndOn = reader.ReadBool();
			Snd1 = reader.ReadInt();
			Snd2 = reader.ReadInt();
			Snd3 = reader.ReadInt();
			Snd4 = reader.ReadInt();
			Snd5 = reader.ReadInt();
			Snd6 = reader.ReadInt();
			Snd7 = reader.ReadInt();
			Snd8 = reader.ReadInt();

			GmpN = reader.ReadString();
			GmpX = reader.ReadInt();
			GmpY = reader.ReadInt();

			x_Add = reader.ReadInt();
			y_Add = reader.ReadInt();
			x_Alt = reader.ReadInt();
			y_Alt = reader.ReadInt();
			x_Cir = reader.ReadInt();
			y_Cir = reader.ReadInt();
			x_Del = reader.ReadInt();
			y_Del = reader.ReadInt();
			x_Info = reader.ReadInt();
			y_Info = reader.ReadInt();
			x_Hue = reader.ReadInt();
			y_Hue = reader.ReadInt();
			x_List = reader.ReadInt();
			y_List = reader.ReadInt();
			x_Main = reader.ReadInt();
			y_Main = reader.ReadInt();
			x_Move = reader.ReadInt();
			y_Move = reader.ReadInt();
			x_Multi = reader.ReadInt();
			y_Multi = reader.ReadInt();
			x_Pick = reader.ReadInt();
			y_Pick = reader.ReadInt();
			x_Pos = reader.ReadInt();
			y_Pos = reader.ReadInt();
			x_Reset = reader.ReadInt();
			y_Reset = reader.ReadInt();
			x_Rnd = reader.ReadInt();
			y_Rnd = reader.ReadInt();
			x_SetId = reader.ReadInt();
			y_SetId = reader.ReadInt();
			x_SetLoc = reader.ReadInt();
			y_SetLoc = reader.ReadInt();
			x_Setting = reader.ReadInt();
			y_Setting = reader.ReadInt();
			x_Sub = reader.ReadInt();
			y_Sub = reader.ReadInt();
			x_Sel = reader.ReadInt();
			y_Sel = reader.ReadInt();

			StcT = reader.ReadBool();
			LndT = reader.ReadBool();

			MultiT = reader.ReadBool();
			ResetT = reader.ReadBool();

			TempN = reader.ReadString();
			TempID = reader.ReadInt();
			TempX = reader.ReadInt();
			TempY = reader.ReadInt();
			TempZ = reader.ReadInt();
			TempH = reader.ReadInt();

			StcX = reader.ReadInt();
			StcY = reader.ReadInt();
			StcZ = reader.ReadInt();
			StcID = reader.ReadInt();

			LndX = reader.ReadInt();
			LndY = reader.ReadInt();
			LndZ = reader.ReadInt();
			LndID = reader.ReadInt();

			M_Val = reader.ReadInt();
			A_Val = reader.ReadInt();

			Cir_T = reader.ReadBool();
			Cir_V = reader.ReadInt();

			Rnd_T = reader.ReadBool();
			Rnd_V = reader.ReadInt();

			Hue_S = reader.ReadInt();

			ListT = reader.ReadBool();
			List1 = reader.ReadInt();
			List2 = reader.ReadInt();
			List3 = reader.ReadInt();
			List4 = reader.ReadInt();
			List5 = reader.ReadInt();
			List6 = reader.ReadInt();
			List7 = reader.ReadInt();
			List8 = reader.ReadInt();
			List9 = reader.ReadInt();
			List0 = reader.ReadInt();

			CntBG = reader.ReadInt();
			CntGN = reader.ReadInt();

			PassW = reader.ReadString();
		}
	}
}
