namespace Server.Gumps
{
	public partial class Gump
	{
		public string B(string text) => $"<B>{text}</B>";
		public string CENTER(string text) => $"<CENTER>{text}</CENTER>";

		public string FONT(string text, int color = 0x00000, int fontId = 0) =>
			$"<BASEFONT SIZE={fontId} COLOR=#{color:X6}>{text}</BASEFONT>";
	}
}
