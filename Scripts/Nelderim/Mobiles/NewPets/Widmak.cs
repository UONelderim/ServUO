using Server.ContextMenus;
using Server.Network;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Server.Mobiles
{
	[CorpseName("zwloki widmaka")]
	public class Widmak : BaseMount
	{
		[Constructable]
		public Widmak() : base("widmak", 0x74, 0x3EA7, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			BaseSoundID = 0xA8;

			// Spectral
			Hue = 1 | HueTransparentFlag;

			SetStr(400);
			SetDex(120);
			SetInt(50);

			SetHits(250);
			SetMana(0);

			SetDamage(6, 8);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Fire, 55, 65);
			SetResistance(ResistanceType.Cold, 25, 35);
			SetResistance(ResistanceType.Poison, 25, 35);
			SetResistance(ResistanceType.Energy, 25, 35);

			SetSkill(SkillName.MagicResist, 30.0, 40.0);
			SetSkill(SkillName.Tactics, 40.0, 50.0);
			SetSkill(SkillName.Wrestling, 40.0, 50.0);

			SetSkill(SkillName.Hiding, 100.0);

			Fame = 500;
			Karma = 500;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 29.1;
		}

		DateTime m_NextHideUsage = DateTime.MinValue;

		public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.AddCustomContextEntries(from, list);

			if (from.Alive && Alive && Controlled && from == ControlMaster && from.InRange(this, 14))
			{
				// TODO: context menu entries for Hide and Reveal
			}
		}

		public override void OnSpeech(SpeechEventArgs e)
		{
			if (e.Mobile.Alive && Alive && Controlled && e.Mobile == ControlMaster)
			{
				string speech = e.Speech.ToLower();

				if (Regex.IsMatch(e.Speech, "ukryjcie sie", RegexOptions.IgnoreCase) && CheckControlChance(e.Mobile))
				{
					Hide();
					return;
				}
				if (Regex.IsMatch(e.Speech, "pokazcie sie", RegexOptions.IgnoreCase) && CheckControlChance(e.Mobile))
				{
					Unhide();
					return;
				}
				if ((WasNamed(speech) && CheckControlChance(e.Mobile)) && Regex.IsMatch(e.Speech, "ukryj sie", RegexOptions.IgnoreCase))
				{
					Hide();
					return;
				}
				if ((WasNamed(speech) && CheckControlChance(e.Mobile)) && Regex.IsMatch(e.Speech, "pokaz sie", RegexOptions.IgnoreCase))
				{
					Unhide();
					return;
				}

			}

			base.OnSpeech(e);
		}

		private void DoHide()
		{
			Hidden = true;
			Warmode = false;
			ControlTarget = null;
			ControlOrder = OrderType.Stay;
			Spells.Sixth.InvisibilitySpell.RemoveTimer(this);
		}

		private static bool HasNearbyEnemies(Mobile from)
		{
			if (from == null)
				return false;

			// Ponizszy kod zaczerpnieto z Hiding.cs
			int range = Math.Min((int)((100 - from.Skills[SkillName.Hiding].Value) / 2) + 8, 18);

			bool badCombat = (from.Combatant is Mobile m && from.InRange(m.Location, range) && m.InLOS(from));
			if (!badCombat)
			{
				IPooledEnumerable eable = from.GetMobilesInRange(range);
				foreach (Mobile check in eable)
				{
					if (check.InLOS(from) && check.Combatant == from)
					{
						badCombat = true;
						break;
					}
				}
				eable.Free();
			}

			return badCombat;
		}

		private void Hide()
		{
			if (Hidden)
				return;

			if (DateTime.Now < m_NextHideUsage)
			{
				if (ControlMaster != null)
					ControlMaster.SendMessage("Zwierze " + Name + " musi chwile poczekac, aby moc sie ponownie ukryc.");

				return;
			}

			if (HasNearbyEnemies(this) || HasNearbyEnemies(ControlMaster))
			{
				if (ControlMaster != null)
					PrivateOverheadMessage(MessageType.Regular, 0x22, true, "Nie moze sie teraz ukryc", ControlMaster.NetState);

				m_NextHideUsage = DateTime.Now.AddSeconds(1.0);
			}
			else
			{
				DoHide();

				if (ControlMaster != null)
					PrivateOverheadMessage(MessageType.Regular, 0x1F4, true, "Ukrylo sie", ControlMaster.NetState);

				m_NextHideUsage = DateTime.Now.AddSeconds(10.0);
			}
		}

		private void Unhide()
		{
			Hidden = false;
		}

		public override void OnAfterResurrect()
		{
			if (Skills[SkillName.Hiding].Value < 100.0)
				SetSkill(SkillName.Hiding, 100.0); // prevent from dropping Hide skill

			base.OnAfterResurrect();
		}

		public virtual bool WasNamed(string speech)
		{
			return Name != null && Insensitive.StartsWith(speech, Name);
		}

		public override PackInstinct PackInstinct => PackInstinct.Equine;

		public override FoodType FavoriteFood => FoodType.Meat;

		public override bool BardImmune => false;

		public override double GetControlChance(Mobile m, bool useBaseSkill)
		{
			AbilityProfile profile = PetTrainingHelper.GetAbilityProfile(this);

			if (profile != null && profile.HasCustomized())
			{
				return base.GetControlChance(m, useBaseSkill);
			}
			return 1.0;
		}

		public Widmak(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
