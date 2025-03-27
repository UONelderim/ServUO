using System;
using System.Collections.Generic;
using Server.Diagnostics;
using Server.Network;
using Server.Gumps;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidFamiliarSpell : DruidSpell
	{
		private static SpellInfo m_Info = new(
			"Przywołanie Przyjaciela Lasu",
			"Lore Sec En Sepa Ohm",
			203,
			9031,
			Reagent.MandrakeRoot,
			CReagent.SpringWater,
			CReagent.PetrafiedWood
		);

		public override SpellCircle Circle => SpellCircle.Sixth;
		public override double CastDelay => 3.0;
		public override double RequiredSkill => 30.0;
		public override int RequiredMana => 17;

		public DruidFamiliarSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			Scroll?.Consume();
		}

		public static Dictionary<Mobile, BaseCreature> Table = new();

		public override bool CheckCast()
		{
			if(Table.TryGetValue(Caster, out var bc) && !bc.Deleted){
				Caster.SendLocalizedMessage(1061605); // You already have a familiar.
				return false;
			}

			return base.CheckCast();
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				Caster.CloseGump(typeof(DruidFamiliarGump));
				Caster.SendGump(new DruidFamiliarGump(Caster, Entries));
			}
			FinishSequence();
		}

		public static DruidFamiliarEntry[] Entries =
		{
			new(typeof(SkitteringHopperFamiliar), "leśny robak", 30.0, 30.0),
			new(typeof(PixieFamiliar), "wróżka", 50.0, 50.0),
			new(typeof(EagleFamiliar), "duchowy orzeł", 60.0, 60.0),
			new(typeof(QuagmireFamiliar), "trzesawisko", 80.0, 80.0),
			new(typeof(SummonedTreefellow), "drzewiec", 90.0, 90.0),
			new(typeof(DryadFamiliar), "driada", 100.0, 100.0)
		};
	}

	public class DruidFamiliarEntry
	{
		private Type m_Type;
		private object m_Name;
		private double _reqMagery;
		private double _reqHerbalism;

		public Type Type => m_Type;
		public object Name => m_Name;
		public double ReqMagery => _reqMagery;
		public double ReqHerbalism => _reqHerbalism;

		public DruidFamiliarEntry(Type type, object name, double reqMagery, double reqHerbalism)
		{
			m_Type = type;
			m_Name = name;
			_reqMagery = reqMagery;
			_reqHerbalism = reqHerbalism;
		}
	}

	public class DruidFamiliarGump : Gump
	{
		private Mobile m_From;
		private DruidFamiliarEntry[] m_Entries;

		private const int EnabledColor16 = 0x0F20;
		private const int DisabledColor16 = 0x262A;

		private const int EnabledColor32 = 0x18CD00;
		private const int DisabledColor32 = 0x4A8B52;

		public DruidFamiliarGump(Mobile from, DruidFamiliarEntry[] entries) : base(200, 100)
		{
			m_From = from;
			m_Entries = entries;

			AddPage(0);

			AddBackground(10, 10, 250, 178, 9270);
			AddAlphaRegion(20, 20, 230, 158);

			AddImage(220, 20, 10464);
			AddImage(220, 72, 10464);
			AddImage(220, 124, 10464);

			AddItem(188, 16, 6883);
			AddItem(198, 168, 6881);
			AddItem(8, 15, 6882);
			AddItem(2, 168, 6880);

			AddHtmlLocalized(30, 26, 200, 20, 1060147, EnabledColor16, false, false); // Chose thy familiar...

			double lore = from.Skills[SkillName.Magery].Base;
			double taming = from.Skills[SkillName.Herbalism].Base;

			for (int i = 0; i < entries.Length; ++i)
			{
				object name = entries[i].Name;

				bool enabled = (lore >= entries[i].ReqMagery && taming >= entries[i].ReqHerbalism);

				AddButton(27, 53 + (i * 21), 9702, 9703, i + 1, GumpButtonType.Reply, 0);

				if (name is int)
					AddHtmlLocalized(50,
						51 + (i * 21),
						150,
						20,
						(int)name,
						enabled ? EnabledColor16 : DisabledColor16,
						false,
						false);
				else if (name is string)
					AddHtml(50,
						51 + (i * 21),
						150,
						20,
						String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>",
							enabled ? EnabledColor32 : DisabledColor32,
							name),
						false,
						false);
			}
		}


		public override void OnResponse(NetState sender, RelayInfo info)
		{
			int index = info.ButtonID - 1;

			if (index >= 0 && index < m_Entries.Length)
			{
				DruidFamiliarEntry entry = m_Entries[index];

				BaseCreature familiar = DruidFamiliarSpell.Table[m_From];

				if (familiar != null && !familiar.Deleted)
				{
					m_From.SendLocalizedMessage(1061605); // You already have a familiar.
				}
				else if (m_From.Skills[SkillName.Magery].Base < entry.ReqMagery || m_From.Skills[SkillName.Herbalism].Base < entry.ReqHerbalism)
				{
					m_From.SendMessage($"Musisz mieć {entry.ReqMagery:F1} Zielarstwa i {entry.ReqHerbalism:F1} Magii.");

					m_From.CloseGump(typeof(DruidFamiliarGump));
					m_From.SendGump(new DruidFamiliarGump(m_From, DruidFamiliarSpell.Entries));
				}
				else if (entry.Type == null)
				{
					m_From.SendMessage("Tego nie można przyzwać.");

					m_From.CloseGump(typeof(DruidFamiliarGump));
					m_From.SendGump(new DruidFamiliarGump(m_From, DruidFamiliarSpell.Entries));
				}
				else
				{
					try
					{
						BaseCreature bc = (BaseCreature)Activator.CreateInstance(entry.Type);

						bc.Skills.MagicResist = m_From.Skills.MagicResist;

						if (BaseCreature.Summon(bc, m_From, m_From.Location, -1, TimeSpan.FromDays(1.0)))
						{
							m_From.FixedParticles(0x3728, 1, 10, 9910, EffectLayer.Head);
							bc.PlaySound(bc.GetIdleSound());
							DruidFamiliarSpell.Table[m_From] = bc;
						}
					}
					catch(Exception e)
					{
						ExceptionLogging.LogException(e);
					}
				}
			}
			else
			{
				m_From.SendLocalizedMessage(1061825); // You decide not to summon a familiar.
			}
		}
	}
}
