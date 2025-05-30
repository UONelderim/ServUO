using System.Collections.Generic;
using Server.ContextMenus;
using Server.Items;
using Server.Nelderim.Engines.MailSystem;
using Server.Targeting;

namespace Server.Mobiles
{
	public class PostMaster : BaseVendor
	{
		private List<SBInfo> m_SBInfos = [];
		protected override List<SBInfo> SBInfos => m_SBInfos;

		public override NpcGuild NpcGuild => NpcGuild.MagesGuild;

		[Constructable]
		public PostMaster() : base("- posłaniec")
		{
			SetSkill(SkillName.EvalInt, 60.0, 83.0);
			SetSkill(SkillName.Inscribe, 90.0, 100.0);
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBPostMaster());
		}

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (TrySend(from, dropped))
			{
				return true;
			}
			return base.OnDragDrop(from, dropped);
		}

		private bool TrySend(Mobile from, Item item)
		{
			if (!PostSystemControl.Initialized)
			{
				from.SendMessage("Chwilowo nieczynne");
				return false;
			}

			if (item is not IMailItem mail)
			{
				from.SendMessage("Nie mozesz wyslac tego");
				return false;
			}

			from.SendGump(new SendMailGump(mail));
			return true;
		}

		public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.AddCustomContextEntries(from, list);
			list.Add(new SendMailContextMenuEntry(this, from));
		}

		public PostMaster(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		private class SendMailContextMenuEntry : ContextMenuEntry
		{
			private PostMaster _PostMaster;
			private Mobile _From;

			public SendMailContextMenuEntry(PostMaster postMaster, Mobile from) : base(3070075)
			{
				_PostMaster = postMaster;
				_From = from;
			}

			public override void OnClick()
			{
				_From.SendMessage("Co chcesz wysłać?");
				_From.BeginTarget(1,
					false,
					TargetFlags.None,
					((from, targeted) =>
						_PostMaster.TrySend(from, targeted as Item)));
			}
		} 
	}
}
