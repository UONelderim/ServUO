using System;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class ArtifactRewardScroll : Item
	{
		public bool m_Chosen;

		public ArtifactRewardScroll(bool chosen) : base(0x14F0)
		{
			Weight = 0.1;
			LootType = LootType.Blessed;
			m_Chosen = chosen;
		}

		public ArtifactRewardScroll(Serial serial) : base(serial)
		{
		}


		public virtual Type[] Artifacts => new Type[] { };
		public virtual string[] ArtifactsNames => new string[] { };

		public virtual string RewardScrollInfo => "";

		public virtual string RewardInfo => "";

		public override void AddNameProperty(ObjectPropertyList list)
		{
			list.Add(RewardScrollInfo + " " + (m_Chosen ? "(wybierany)" : "(losowy)"));
		}

		public override void OnSingleClick(Mobile from)
		{
			LabelTo(from, RewardScrollInfo + " " + (m_Chosen ? "(wybierany)" : "(losowy)"));
		}


		public override void OnDoubleClick(Mobile from)
		{
			if (Deleted || !(from is PlayerMobile) || !from.Alive)
				return;

			if (IsChildOf(from.Backpack))
			{
				if (m_Chosen)
					from.SendGump(new ArtChooseGump(from, Artifacts, ArtifactsNames, this));
				else
					Use(from, true);
			}
			else
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
		}

		public void Use(Mobile from, bool firstStage)
		{
			if (Deleted || !IsChildOf(from.Backpack)) return;
			if (firstStage)
			{
				from.CloseGump(typeof(InternalGump));
				from.SendGump(new InternalGump(from, this));
			}
			else
			{
				var reward = Utility.Random(Artifacts.Length);
				var art = (Item)Activator.CreateInstance(Artifacts[reward]);
				art.LabelOfCreator = "ArtifactRewardScroll " + Serial;
				from.Backpack.DropItem(art);

				from.SendLocalizedMessage(505596); // Nagroda zmaterializwoała się w plecaku.

				from.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
				from.PlaySound(0x1E0);

				Delete();
			}
		}

		public class InternalGump : Gump
		{
			private Mobile m_Mobile;
			private ArtifactRewardScroll m_Scroll;

			public InternalGump(Mobile mobile, ArtifactRewardScroll scroll) : base(25, 50)
			{
				m_Mobile = mobile;
				m_Scroll = scroll;

				AddPage(0);

				AddBackground(25, 10, 420, 200, 5054);

				AddImageTiled(33, 20, 401, 181, 2624);
				AddAlphaRegion(33, 20, 401, 181);

				AddHtml(40,
					48,
					387,
					100,
					String.Format("<basefont color=#FFFFFF>Uzywajac tego zwoju, otrzymasz losowy {0}.</basefont>",
						m_Scroll.RewardInfo),
					true,
					true);

				AddHtmlLocalized(125, 148, 200, 20, 1049478, 0xFFFFFF, false, false); // Do you wish to use this scroll?

				AddButton(100, 172, 4005, 4007, 1, GumpButtonType.Reply, 0);
				AddHtmlLocalized(135, 172, 120, 20, 1046362, 0xFFFFFF, false, false); // Yes

				AddButton(275, 172, 4005, 4007, 0, GumpButtonType.Reply, 0);
				AddHtmlLocalized(310, 172, 120, 20, 1046363, 0xFFFFFF, false, false); // No

				AddHtml(40,
					20,
					260,
					20,
					String.Format("<basefont color=#FFFFFF>{0}:</basefont>", m_Scroll.RewardScrollInfo),
					false,
					false);
			}

			public override void OnResponse(NetState state, RelayInfo info)
			{
				if (info.ButtonID == 1)
					m_Scroll.Use(m_Mobile, false);
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			writer.Write(m_Chosen);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();
			m_Chosen = reader.ReadBool();
		}
	}
}
