using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Engines.XmlSpawner2
{
	public class XmlAttachmentScroll : Item
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public string AttachType { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public string Args { get; set; }


		[CommandProperty(AccessLevel.GameMaster)]
		public bool Reusable { get; set; }

		[Constructable]
		public XmlAttachmentScroll() : this("", "")
		{
		}

		[Constructable]
		public XmlAttachmentScroll(string attachType, string args) : base(0x14F0)
		{
			AttachType = attachType;
			Args = args;
			Reusable = false;
		}

		public XmlAttachmentScroll(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!this.IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // Musisz miec przedmiot w plecaku, zeby go uzyc.
				return;
			}

			Target t = new InternalTarget(this);

			from.SendMessage("Wybierz cel zalacznika.");
			from.Target = t;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			writer.Write(AttachType);
			writer.Write(Args);
			writer.Write(Reusable);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			AttachType = reader.ReadString();
			Args = reader.ReadString();
			Reusable = reader.ReadBool();
		}

		private class InternalTarget : Target
		{
			private XmlAttachmentScroll m_Scroll;

			public InternalTarget(XmlAttachmentScroll scroll) : base(3, false, TargetFlags.None)
			{
				m_Scroll = scroll;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				Type attachtype = SpawnerType.GetType(m_Scroll.AttachType);
				string[] args = m_Scroll.Args.Split(' ');

				if (attachtype == null || !attachtype.IsSubclassOf(typeof(XmlAttachment)))
				{
					from.SendMessage("Invalid attachment type " + m_Scroll.AttachType);
					return;
				}

				XmlAttachment o = (XmlAttachment)XmlSpawner.CreateObject(attachtype, args, false, true);

				if (o == null)
				{
					from.SendMessage(String.Format("Unable to construct {0} with specified args", attachtype.Name));
					return;
				}

				if (XmlAttach.AttachTo(targeted, o))
				{
					from.SendMessage("Sukces");
					if (!m_Scroll.Reusable)
						m_Scroll.Delete();
				}
				else
					from.SendMessage("Niepowodzenie");
			}
		}
	}
}
