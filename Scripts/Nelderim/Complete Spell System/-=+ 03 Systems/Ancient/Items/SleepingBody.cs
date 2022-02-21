#region References

using System;
using System.Collections;
using System.IO;
using Server.Items;
using Server.Network;

#endregion

namespace Server.ACC.CSS.Systems.Ancient
{
	public class SleepingBody : Container
	{
		private string
			m_SleepingBodyName; // Value of the SleepingNameAttribute attached to the owner when he died -or- null if the owner had no SleepingBodyNameAttribute; use "the remains of ~name~"

		private ArrayList
			m_EquipItems; // List of items equiped when the owner died. Ingame, these items display /on/ the SleepingBody, not just inside

		private bool m_spell;
		private DateTime m_NextSnoreTrigger;

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Owner { get; private set; }

		public ArrayList EquipItems
		{
			get { return m_EquipItems; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Invuln { get; private set; }

		[Constructable]
		public SleepingBody(Mobile owner, bool blessed)
			: this(owner, blessed, true)
		{
		}

		[Constructable]
		public SleepingBody(Mobile owner, bool blessed, bool isSpell)
			: base(0x2006)
		{
			Stackable = true; // To supress console warnings, stackable must be true
			Amount = owner.Body; // protocol defines that for itemid 0x2006, amount=body
			Stackable = false;
			Invuln = blessed;
			Movable = false;

			Owner = owner;
			Name = Owner.Name;
			m_SleepingBodyName = GetBodyName(owner);
			Hue = Owner.Hue;
			Direction = Owner.Direction;
			m_spell = isSpell;

			m_EquipItems = new ArrayList();
			AddFromLayer(Owner, Layer.FirstValid, ref m_EquipItems);
			AddFromLayer(Owner, Layer.TwoHanded, ref m_EquipItems);
			AddFromLayer(Owner, Layer.Shoes, ref m_EquipItems);
			AddFromLayer(Owner, Layer.Pants, ref m_EquipItems);
			AddFromLayer(Owner, Layer.Shirt, ref m_EquipItems);
			AddFromLayer(Owner, Layer.Helm, ref m_EquipItems);
			AddFromLayer(Owner, Layer.Gloves, ref m_EquipItems);
			AddFromLayer(Owner, Layer.Ring, ref m_EquipItems);
			AddFromLayer(Owner, Layer.Neck, ref m_EquipItems);
			AddFromLayer(Owner, Layer.Hair, ref m_EquipItems);
			AddFromLayer(Owner, Layer.Waist, ref m_EquipItems);
			AddFromLayer(Owner, Layer.InnerTorso, ref m_EquipItems);
			AddFromLayer(Owner, Layer.Bracelet, ref m_EquipItems);
			AddFromLayer(Owner, Layer.FacialHair, ref m_EquipItems);
			AddFromLayer(Owner, Layer.MiddleTorso, ref m_EquipItems);
			AddFromLayer(Owner, Layer.Earrings, ref m_EquipItems);
			AddFromLayer(Owner, Layer.Arms, ref m_EquipItems);
			AddFromLayer(Owner, Layer.Cloak, ref m_EquipItems);
			AddFromLayer(Owner, Layer.OuterTorso, ref m_EquipItems);
			AddFromLayer(Owner, Layer.OuterLegs, ref m_EquipItems);
			AddFromLayer(Owner, Layer.LastUserValid, ref m_EquipItems);
		}

		private void AddFromLayer(Mobile from, Layer layer, ref ArrayList list)
		{
			if (list == null)
				list = new ArrayList();

			Item worn = from.FindItemOnLayer(layer);
			if (worn != null)
			{
				Item item = new Item();
				item.ItemID = worn.ItemID;
				item.Hue = worn.Hue;
				item.Layer = layer;
				DropItem(item);
				list.Add(item);
			}
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.SendLocalizedMessage(1001018); // You cannot perform negative acts on your target.
		}

		public override bool HandlesOnMovement { get { return true; } } // Tell the core that we implement OnMovement

		public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
		{
			from.SendLocalizedMessage(1005468, "", 0x8A5); // Me Sleepy.

			return false;
		}

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			from.SendLocalizedMessage(1005468, "", 0x8A5); // Me Sleepy.

			return false;
		}

		public override bool CheckContentDisplay(Mobile from)
		{
			return false;
		}

		public override bool DisplaysContent { get { return false; } }

		public override void OnAfterDelete()
		{
			if (Owner != null)
			{
				Owner.Z = this.Z;
				Owner.Blessed = this.Invuln;
			}

			for (int i = 0; i < m_EquipItems.Count; i++)
			{
				object o = m_EquipItems[i];
				if (o != null && o is Item)
				{
					Item item = (Item)o;
					item.Delete();
				}
			}

			base.OnAfterDelete();
		}

		public SleepingBody(Serial serial)
			: base(serial)
		{
		}

		public override void SendInfoTo(NetState state, bool sendOplPacket)
		{
			base.SendInfoTo(state);

			if (ItemID == 0x2006)
			{
				state.Send(new SleepingBodyContent(state.Mobile, this));
				state.Send(new SleepingBodyEquip(state.Mobile, this));
			}
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			if (m_SleepingBodyName != null)
				list.Add(m_SleepingBodyName);
			else
				list.Add(1049644, String.Format("Sleeping {0}", Name));
		}

		public override void OnAosSingleClick(Mobile from)
		{
			LabelTo(from, m_SleepingBodyName == null ? String.Format("Sleeping {0}", Name) : m_SleepingBodyName);
		}

		public static string GetBodyName(Mobile m)
		{
			Type t = m.GetType();

			object[] attrs = t.GetCustomAttributes(typeof(SleepingNameAttribute), true);

			if (attrs != null && attrs.Length > 0)
			{
				SleepingNameAttribute attr = attrs[0] as SleepingNameAttribute;

				if (attr != null)
					return attr.Name;
			}

			return m.Name;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1);

			writer.Write(m_spell); // version 1

			writer.Write(Owner); // version 0
			writer.Write(m_SleepingBodyName);
			writer.Write(Invuln);

			writer.WriteItemList(m_EquipItems, true);
		}

		public override void Deserialize(GenericReader reader)
		{
			m_spell = true;
			base.Deserialize(reader);

			int version = reader.ReadInt();
			switch (version)
			{
				case 1:
				{
					m_spell = reader.ReadBool();
					goto case 0;
				}
				case 0:
				{
					Owner = reader.ReadMobile();
					m_SleepingBodyName = reader.ReadString();
					Invuln = reader.ReadBool();

					m_EquipItems = reader.ReadItemList();
					break;
				}
			}

			m_NextSnoreTrigger = DateTime.Now;

			// Delete on Server restart if spell action
			if (m_spell)
				this.Delete();
		}

		public bool CheckRange(Point3D loc, Point3D oldLoc, int range)
		{
			return CheckRange(loc, range) && !CheckRange(oldLoc, range);
		}

		public bool CheckRange(Point3D loc, int range)
		{
			return ((this.Z + 8) >= loc.Z && (loc.Z + 16) > this.Z)
			       && Utility.InRange(GetWorldLocation(), loc, range);
		}

		public override void OnMovement(Mobile m, Point3D oldLocation)
		{
			base.OnMovement(m, oldLocation);

			if (m.Location == oldLocation)
				return;

			if (CheckRange(m.Location, oldLocation, 5) && DateTime.Now >= m_NextSnoreTrigger)
			{
				m_NextSnoreTrigger = DateTime.Now + TimeSpan.FromSeconds(Utility.Random(5, 10));

				if (this != null && this.Owner != null)
				{
					this.PublicOverheadMessage(0, Owner.SpeechHue, false, "zZz");
					Owner.PlaySound(Owner.Female ? 819 : 1093);
				}
			}
		}
	}

	public sealed class SleepingBodyEquip : Packet
	{
		public SleepingBodyEquip(Mobile beholder, SleepingBody beheld)
			: base(0x89)
		{
			ArrayList list = beheld.EquipItems;

			EnsureCapacity(8 + (list.Count * 5));

			m_Stream.Write(beheld.Serial);

			for (int i = 0; i < list.Count; ++i)
			{
				Item item = (Item)list[i];

				if (!item.Deleted && beholder.CanSee(item) && item.Parent == beheld)
				{
					m_Stream.Write((byte)(item.Layer + 1));
					m_Stream.Write(item.Serial);
				}
			}

			m_Stream.Write((byte)Layer.Invalid);
		}
	}

	public sealed class SleepingBodyContent : Packet
	{
		public SleepingBodyContent(Mobile beholder, SleepingBody beheld)
			: base(0x3C)
		{
			ArrayList items = beheld.EquipItems;
			int count = items.Count;

			EnsureCapacity(5 + (count * 19));

			long pos = m_Stream.Position;

			int written = 0;

			m_Stream.Write((ushort)0);

			for (int i = 0; i < count; ++i)
			{
				Item child = (Item)items[i];

				if (!child.Deleted && child.Parent == beheld && beholder.CanSee(child))
				{
					m_Stream.Write(child.Serial);
					m_Stream.Write((ushort)child.ItemID);
					m_Stream.Write((byte)0); // signed, itemID offset
					m_Stream.Write((ushort)child.Amount);
					m_Stream.Write((short)child.X);
					m_Stream.Write((short)child.Y);
					m_Stream.Write(beheld.Serial);
					m_Stream.Write((ushort)child.Hue);

					++written;
				}
			}

			m_Stream.Seek(pos, SeekOrigin.Begin);
			m_Stream.Write((ushort)written);
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class SleepingNameAttribute : Attribute
	{
		public string Name { get; }

		public SleepingNameAttribute(string name)
		{
			Name = name;
		}
	}
}
