// 05.12.13 :: troyan
// 05.12.17 :: troyan :: naprawa bledu z # w tekscie plotki

#region References

using System;
using Server.Gumps;
using Server.Network;

#endregion

namespace Server.Nelderim
{
	public class SayRumorGump : Gump
	{
		public const int Black = 0x0000;
		public const int White = 0x7FFF;
		public const int DarkGreen = 10000;
		public const int LightGreen = 90000;
		public const int Blue = 19777215;

		private readonly RumorRecord m_Rumor;
		private readonly string m_Message;

		public SayRumorGump(Mobile from, RumorRecord rr) : base(75, 25)
		{
			try
			{
				m_Rumor = rr;
				m_Message = "";

				AddPage(0);

				AddImageTiled(50, 20, 400, 400, 2624);
				AddAlphaRegion(50, 20, 400, 400);

				AddImage(90, 33, 9005);
				AddHtml(130, 45, 270, 20,
					Format(String.Format("{0} opowiada o {1}", from.Name, m_Rumor.Title), 0xFFFFFF), false, false);
				AddImageTiled(130, 65, 175, 1, 9101);

				string[] phrases = rr.Text.Split('#');

				foreach (string txt in phrases)
					m_Message += txt + "<BR>";

				AddHtml(98, 140, 312, 260, Format(m_Message, White), false, true);

				AddImageTiled(50, 29, 30, 390, 10460);
				AddImageTiled(34, 140, 17, 279, 9263);

				AddImage(48, 135, 10411);
				AddImage(-16, 285, 10402);
				AddImage(0, 10, 10421);
				AddImage(25, 0, 10420);

				AddImageTiled(83, 15, 350, 15, 10250);

				AddImage(34, 419, 10306);
				AddImage(442, 419, 10304);
				AddImageTiled(51, 419, 392, 17, 10101);

				AddImageTiled(415, 29, 44, 390, 2605);
				AddImageTiled(415, 29, 30, 390, 10460);
				AddImage(425, 0, 10441);

				AddImage(370, 50, 1417);
				AddImage(379, 60, 0x15C8);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
		}

		public string Format(string text, int color)
		{
			return String.Format("<CENTER><BASEFONT COLOR=#{0:X6}>{1}</BASEFONT></CENTER>", color, text);
		}
	}
}
