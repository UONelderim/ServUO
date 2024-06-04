#region References

using System;
using Server.Network;

#endregion

namespace Server.Gumps
{
	public class GeneralConfirmGump
	{
		protected Gump _gump;

		public event Action<NetState, RelayInfo> OnContinue;
		public event Action<NetState, RelayInfo> OnCancel;
		public event Action<NetState, RelayInfo> OnGumpClosed;

		protected string _text;

		public virtual string Text
		{
			get => _text;
			set
			{
				_text = value;
				CreateGump();
			}
		}

		protected Point2D _size;

		public virtual Point2D Size
		{
			get => _size;
			set
			{
				if (270 > value.X || 120 > value.Y)
					throw new ArgumentException(String.Format(
						"Minimalny rozmiar GeneralConfirmGump to (270, 60), a zostal podany ({0}, {1})", value.X,
						value.Y));
				_size = value;
				CreateGump();
			}
		}

		protected void Continue(NetState sender, RelayInfo info)
		{
			OnContinue?.Invoke(sender, info);
		}

		protected void Cancel(NetState sender, RelayInfo info)
		{
			OnCancel?.Invoke(sender, info);
		}

		protected void GumpClosed(NetState sender, RelayInfo info)
		{
			OnGumpClosed?.Invoke(sender, info);
		}

		public GeneralConfirmGump(Action<NetState, RelayInfo> onContinue = null,
			Action<NetState, RelayInfo> onCancel = null, Action<NetState, RelayInfo> onGumpClosed = null)
		{
			OnContinue += onContinue;
			OnCancel += onCancel;
			OnGumpClosed += onGumpClosed;

			_gump = new InternalGeneralConfirmGump(_size.X, _size.Y, this);
			Size = new Point2D(270, 120);
		}

		protected virtual void CreateGump()
		{
			_gump.Entries.Clear();

			_gump.AddPage(0);

			_gump.X = Size.X;
			_gump.Y = Size.Y;

			const int margin = 10;

			_gump.AddBackground(0, 0, _gump.X, _gump.Y, 5054);
			_gump.AddBackground(margin, margin, _gump.X - 2 * margin, _gump.Y - 2 * margin, 3000);

			Point2D htmlLoc = new Point2D(20, 15);
			Point2D htmlSize = new Point2D(_gump.X - 40, _gump.Y - 60);

			_gump.AddHtml(htmlLoc.X, htmlLoc.Y, htmlSize.X, htmlSize.Y, this.Text, true, true);

			int buttonsY = _gump.Y - 40;
			_gump.AddButton(20, buttonsY, 4005, 4007, 2, GumpButtonType.Reply, 0);
			_gump.AddHtmlLocalized(55, buttonsY, 75, 20, 1011011, false, false); // CONTINUE

			_gump.AddButton(135, buttonsY, 4005, 4007, 1, GumpButtonType.Reply, 0);
			_gump.AddHtmlLocalized(170, buttonsY, 75, 20, 1011012, false, false); // CANCEL
		}

		public static implicit operator Gump(GeneralConfirmGump gump)
		{
			gump.CreateGump();

			return gump._gump;
		}

		private class InternalGeneralConfirmGump : Gump
		{
			private readonly GeneralConfirmGump _parent;

			public override void OnResponse(NetState sender, RelayInfo info)
			{
				if (info.ButtonID == 2)
					_parent.Continue(sender, info);
				else if (info.ButtonID == 1)
					_parent.Cancel(sender, info);
				else
					_parent.GumpClosed(sender, info);
			}

			public InternalGeneralConfirmGump(int x, int y, GeneralConfirmGump parent)
				: base(x, y)
			{
				_parent = parent;
			}
		}
	}
}
