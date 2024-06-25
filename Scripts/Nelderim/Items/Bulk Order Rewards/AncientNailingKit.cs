#region References

using Server.Engines.Craft;

#endregion

namespace Server.Items
{
	public class AncientNailingKit : BaseTool
	{
		private int m_Bonus;
		private SkillMod m_SkillMod;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Bonus
		{
			get
			{
				return m_Bonus;
			}
			set
			{
				m_Bonus = value;
				InvalidateProperties();

				if (m_Bonus == 0)
				{
					if (m_SkillMod != null)
						m_SkillMod.Remove();

					m_SkillMod = null;
				}
				else if (m_SkillMod == null && Parent is Mobile)
				{
					m_SkillMod = new DefaultSkillMod(SkillName.Carpentry, true, m_Bonus);
					((Mobile)Parent).AddSkillMod(m_SkillMod);
				}
				else if (m_SkillMod != null)
				{
					m_SkillMod.Value = m_Bonus;
				}
			}
		}

		public override void OnAdded(IEntity parent)
		{
			base.OnAdded(parent);

			if (m_Bonus != 0 && parent is Mobile)
			{
				if (m_SkillMod != null)
					m_SkillMod.Remove();

				m_SkillMod = new DefaultSkillMod(SkillName.Carpentry, true, m_Bonus);
				((Mobile)parent).AddSkillMod(m_SkillMod);
			}
		}

		public override void OnRemoved(IEntity parent)
		{
			base.OnRemoved(parent);

			if (m_SkillMod != null)
				m_SkillMod.Remove();

			m_SkillMod = null;
		}

		public override CraftSystem CraftSystem { get { return DefCarpentry.CraftSystem; } }
		public override int LabelNumber { get { return 3060081; } } 

		[Constructable]
		public AncientNailingKit(int bonus) : this(bonus, 600)
		{
		}

		[Constructable]
		public AncientNailingKit(int bonus, int uses) : base(uses, 0x1034)
		{
			m_Bonus = bonus;
			Weight = 8.0;
			Layer = Layer.OneHanded;
			Hue = 0x482;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (m_Bonus != 0)
				list.Add(3060099, "3060099", m_Bonus.ToString()); // ~1_skillname~ +~2_val~
		}

		public AncientNailingKit(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version

			writer.Write(m_Bonus);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					m_Bonus = reader.ReadInt();
					break;
				}
			}

			if (m_Bonus != 0 && Parent is Mobile)
			{
				if (m_SkillMod != null)
					m_SkillMod.Remove();

				m_SkillMod = new DefaultSkillMod(SkillName.Carpentry, true, m_Bonus);
				((Mobile)Parent).AddSkillMod(m_SkillMod);
			}

			if (Hue == 0)
				Hue = 0x482;
		}
	}
}
