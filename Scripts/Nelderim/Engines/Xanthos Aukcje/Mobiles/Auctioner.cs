#region AuthorHeader

//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

#endregion AuthorHeader

#region References

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Server;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;

#endregion

namespace Arya.Auction
{
	#region Context Menu

	public class TradeHouseEntry : ContextMenuEntry
	{
		private readonly Auctioner m_Auctioner;


		public TradeHouseEntry(Auctioner auctioner) : base(6103, 10)
		{
			m_Auctioner = auctioner;
		}

		public override void OnClick()
		{
			Mobile m = Owner.From;

			if (!m.CheckAlive())
				return;

			if (AuctionSystem.Running)
			{
				m.SendGump(new AuctionGump(m));
			}
			else if (m_Auctioner != null)
			{
				m_Auctioner.SayTo(m, AuctionSystem.ST[145]);
			}
		}
	}

	#endregion

	/// <summary>
	///     Summary description for Auctioner.
	/// </summary>
	public class Auctioner : BaseVendor
	{
		[Constructable]
		public Auctioner() : base("the Auctioner")
		{
			RangePerception = 10;
		}

		protected override List<SBInfo> SBInfos { get; } = new List<SBInfo>();

		public override void InitOutfit()
		{
			AddItem(new LongPants(GetRandomHue()));
			AddItem(new Boots(GetRandomHue()));
			AddItem(new FeatheredHat(GetRandomHue()));

			if (Female)
			{
				AddItem(new Kilt(GetRandomHue()));
				AddItem(new Shirt(GetRandomHue()));
				AddItem(new GoldBracelet { Hue = GetRandomHue() });
				AddItem(new GoldNecklace { Hue = GetRandomHue() });
			}
			else
			{
				AddItem(new FancyShirt(GetRandomHue()));
				AddItem(new Doublet(GetRandomHue()));
			}
		}

		public Auctioner(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();
		}

		public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
		{
			list.Add(new TradeHouseEntry(this));
		}

		public override void InitSBInfo()
		{
		}

		public override void OnSpeech(SpeechEventArgs e)
		{
			if (Regex.IsMatch(e.Speech, "aukcj", RegexOptions.IgnoreCase))
			{
				e.Handled = true;

				if (!e.Mobile.CheckAlive())
				{
					SayTo(e.Mobile, "Czy ja slysze glosy?");
				}
				else if (AuctionSystem.Running)
				{
					e.Mobile.SendGump(new AuctionGump(e.Mobile));
				}
				else
				{
					SayTo(e.Mobile, "Przykro mi, aktualnie nieczynne, sprobuj pozniej.");
				}
			}

			base.OnSpeech(e);
		}
	}
}
