#region References

using Server.Mobiles;

#endregion

namespace Server.ACC.CSS.Systems.Ancient
{
	[CorpseName("a corpse")]
	public class CharmedMobile : BaseCreature
	{
		[Constructable]
		public CharmedMobile(BaseCreature owner)
			: base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			owner = Owner;
			Body = 777;
			Title = " The Mystic Lama Herder";
		}

		public CharmedMobile(Serial serial)
			: base(serial)
		{
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public BaseCreature Owner { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public override bool ClickTitle { get { return false; } }

		public override void GetProperties(ObjectPropertyList list)
		{
			list.Add(1042971, this.Name);

			list.Add(1049644, "charmed");
		}

		public override bool OnBeforeDeath()
		{
			BaseCreature m_Own = this.Owner;

			if (m_Own != null)
			{
				m_Own.Location = this.Location;
				m_Own.Blessed = false;
				m_Own.RevealingAction();
			}

			Delete();
			return false;
		}

		public override void OnAfterDelete()
		{
			BaseCreature m_Own = this.Owner;

			if (m_Own != null)
			{
				m_Own.Location = this.Location;
				m_Own.Blessed = false;
				m_Own.RevealingAction();
			}

			base.OnAfterDelete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
			writer.Write(Owner);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			Owner = reader.ReadMobile() as BaseCreature;

			Delete();
		}
	}
}
