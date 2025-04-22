using System;
using System.Collections;
using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerFamiliarSpell : RangerSpell
	{
		private static readonly SpellInfo m_Info = new(
			"Zwierzęcy kompan", "Sinta Ner Arda Moina",
			//SpellCircle.Sixth,
			203,
			9031,
			CReagent.DestroyingAngel,
			CReagent.SpringWater,
			CReagent.PetrafiedWood
		);

		public override SpellCircle Circle => SpellCircle.Sixth;

		public override double CastDelay => 3.0;
		public override double RequiredSkill => 30.0;
		public override int RequiredMana => 17;

		public RangerFamiliarSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			if (Scroll != null)
				Scroll.Consume();
		}

		public static Dictionary<Mobile, BaseCreature> Table { get; } = new();

		public override bool CheckCast()
		{
			BaseCreature check = Table[Caster];

			if (check != null && !check.Deleted)
			{
				Caster.SendLocalizedMessage(1061605); // You already have a familiar.
				return false;
			}

			return base.CheckCast();
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				Caster.CloseGump(typeof(RangerFamiliarGump));
				Caster.SendGump(new RangerFamiliarGump(Caster, Entries, Caster.Skills[CastSkill].Base, Caster.Skills[DamageSkill].Base));
			}

			FinishSequence();
		}

		public static RangerFamiliarEntry[] Entries { get; } =
		{
			new(typeof(PackRatFamiliar), "Szczur z plecakiem", 30.0, 30.0),
			new(typeof(IceHoundFamiliar), "Lodowy wilk", 50.0, 50.0),
			new(typeof(ThunderHoundFamiliar), "Piorunujący wilk", 60.0, 60.0),
			new(typeof(HellHoundFamiliar), "Ognisty Ogar", 80.0, 80.0),
			new(typeof(VampireWolfFamiliar), "wilk-wampir", 100.0, 100.0),
			new(typeof(TigerFamiliar), "Tygrys szablozębny", 115.0, 115.0)
		};
	}

	public class RangerFamiliarEntry
	{
		public Type Type { get; }
		public string Name { get; }
		public double ReqCastSkill { get; }
		public double ReqDmgSkill { get; }

		public RangerFamiliarEntry(Type type, string name, double reqCastSkill, double reqDmgSkill)
		{
			Type = type;
			Name = name;
			ReqCastSkill = reqCastSkill;
			ReqDmgSkill = reqDmgSkill;
		}
	}

	public class RangerFamiliarGump : Gump
	{
		private readonly Mobile m_From;
		private readonly RangerFamiliarEntry[] m_Entries;
		private readonly double _castSkill;
		private readonly double _dmgSkill;

		private const int EnabledColor16 = 0x0F20;
		private const int DisabledColor16 = 0x262A;

		private const int EnabledColor32 = 0x18CD00;
		private const int DisabledColor32 = 0x4A8B52;

		public RangerFamiliarGump(Mobile from, RangerFamiliarEntry[] entries, double castSkill, double dmgSkill) : base(200, 100)
		{
			m_From = from;
			m_Entries = entries;
			_castSkill = castSkill;
			_dmgSkill = dmgSkill;

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
			
			for (int i = 0; i < entries.Length; ++i)
			{
				var name = entries[i].Name;

				bool enabled = (castSkill >= entries[i].ReqCastSkill && dmgSkill >= entries[i].ReqDmgSkill);

				AddButton(27, 53 + (i * 21), 9702, 9703, i + 1, GumpButtonType.Reply, 0);

				AddHtml(50, 51 + (i * 21), 150, 20,
					$"<BASEFONT COLOR=#{(enabled ? EnabledColor32 : DisabledColor32):X6}>{name}</BASEFONT>", false, false);
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			int index = info.ButtonID - 1;

			if (index >= 0 && index < m_Entries.Length)
			{
				RangerFamiliarEntry entry = m_Entries[index];
				
				BaseCreature check = RangerFamiliarSpell.Table[m_From];

				if (check != null && !check.Deleted)
				{
					m_From.SendLocalizedMessage(1061605); // You already have a familiar.
				}
				else if (_castSkill < entry.ReqCastSkill || _dmgSkill < entry.ReqDmgSkill)
				{
					m_From.SendMessage($"Ten przywołaniec wymaga {entry.ReqCastSkill:F1} Taktyki i {entry.ReqDmgSkill:F1} Łucznictwa.");

					m_From.CloseGump(typeof(RangerFamiliarGump));
					m_From.SendGump(new RangerFamiliarGump(m_From, RangerFamiliarSpell.Entries, _castSkill, _dmgSkill));
				}
				else if (entry.Type == null)
				{
					m_From.SendMessage("Ten przywołaniec nie został zdefiniowany.");

					m_From.CloseGump(typeof(RangerFamiliarGump));
					m_From.SendGump(new RangerFamiliarGump(m_From, RangerFamiliarSpell.Entries, _castSkill, _dmgSkill));
				}
				else
				{
					BaseCreature bc = (BaseCreature)Activator.CreateInstance(entry.Type);

					bc.Skills.MagicResist = m_From.Skills.MagicResist;

					if (BaseCreature.Summon(bc, m_From, m_From.Location, -1, TimeSpan.FromDays(1.0)))
					{
						m_From.FixedParticles(0x3728, 1, 10, 9910, EffectLayer.Head);
						bc.PlaySound(bc.GetIdleSound());
						RangerFamiliarSpell.Table[m_From] = bc;
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
