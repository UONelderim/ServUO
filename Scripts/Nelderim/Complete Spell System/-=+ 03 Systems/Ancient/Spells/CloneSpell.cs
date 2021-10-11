using System;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;

namespace Server.ACC.CSS.Systems.Ancient
{
	public class AncientCloneSpell : AncientSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
			"Klonowanie", "In Quas Xen",
			//SpellCircle.Sixth,
			230,
			9022,
			Reagent.SulfurousAsh,
			Reagent.SpidersSilk,
			Reagent.Bloodmoss,
			Reagent.Ginseng,
			Reagent.Nightshade,
			Reagent.MandrakeRoot
		);

		public override SpellCircle Circle
		{
			get { return SpellCircle.Sixth; }
		}

		public override double RequiredSkill { get { return 71.1; } }
		public override int RequiredMana { get { return 33; } }

		public AncientCloneSpell(Mobile caster, Item scroll)
			: base(caster, scroll, m_Info)
		{
		}

		public override bool CheckCast()
		{
			if (Caster.Mounted)
			{
				Caster.SendLocalizedMessage(1063132); // You cannot use this ability while mounted.
				return false;
			}
			else if ((Caster.Followers + 1) > Caster.FollowersMax)
			{
				Caster.SendLocalizedMessage(
					1063133); // You cannot summon a mirror image because you have too many followers.
				return false;
			}
			else if (TransformationSpellHelper.UnderTransformation(Caster, typeof(HorrificBeastSpell)))
			{
				Caster.SendLocalizedMessage(1061091); // You cannot cast that spell in this form.
				return false;
			}

			return base.CheckCast();
		}

		public override bool CheckDisturb(DisturbType type, bool firstCircle, bool resistable)
		{
			return false;
		}

		public override void OnBeginCast()
		{
			base.OnBeginCast();

			Caster.SendLocalizedMessage(1063134); // You begin to summon a mirror image of yourself.
		}

		public override void OnCast()
		{
			if (Caster.Mounted)
			{
				Caster.SendLocalizedMessage(1063132); // You cannot use this ability while mounted.
			}
			else if ((Caster.Followers + 1) > Caster.FollowersMax)
			{
				Caster.SendLocalizedMessage(
					1063133); // You cannot summon a mirror image because you have too many followers.
			}
			else if (TransformationSpellHelper.UnderTransformation(Caster, typeof(HorrificBeastSpell)))
			{
				Caster.SendLocalizedMessage(1061091); // You cannot cast that spell in this form.
			}
			else if (CheckSequence())
			{
				Caster.FixedParticles(0x376A, 1, 14, 0x13B5, EffectLayer.Waist);
				Caster.PlaySound(0x511);

				new Clone(Caster).MoveToWorld(Caster.Location, Caster.Map);
			}

			FinishSequence();
		}
	}
}

namespace Server.Mobiles
{
	public class AncientCloneSpell : BaseCreature
	{
		private Mobile m_Caster;

		public AncientCloneSpell(Mobile caster) : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
		{
			m_Caster = caster;

			Body = caster.Body;

			Race = caster.Race;
			Hue = caster.Hue;
			Female = caster.Female;
			Name = caster.Name;
			NameHue = caster.NameHue;
			Label1 = caster.Label1;
			Label2 = caster.Label2;

			Guild = caster.Guild;
			GuildFealty = caster.GuildFealty;
			GuildTitle = caster.GuildTitle;

			Title = caster.Title;
			if (caster.Guild != null)
				Title += String.Format(" [{0}]", caster.Guild.Abbreviation);

			Kills = caster.Kills;

			HairItemID = caster.HairItemID;
			HairHue = caster.HairHue;

			FacialHairItemID = caster.FacialHairItemID;
			FacialHairHue = caster.FacialHairHue;

			for (int i = 0; i < caster.Skills.Length; ++i)
			{
				Skills[i].Base = caster.Skills[i].Base;
				Skills[i].Cap = caster.Skills[i].Cap;
			}

			for (int i = 0; i < caster.Items.Count; i++)
			{
				AddItem(CloneItem(caster.Items[i]));
			}

			Warmode = true;

			Summoned = true;
			SummonMaster = caster;

			ControlOrder = OrderType.Follow;
			ControlTarget = caster;

			TimeSpan duration = TimeSpan.FromSeconds(30 + caster.Skills.Ninjitsu.Fixed / 40);

			new UnsummonTimer(caster, this, duration).Start();
			SummonEnd = DateTime.Now + duration;

			MirrorImage.AddClone(m_Caster);
		}

		//	protected override BaseAI ForcedAI { get { return new CloneAI( this ); } }

		public override bool IsHumanInTown() { return false; }

		private Item CloneItem(Item item)
		{
			Item newItem = new Item(item.ItemID);
			newItem.Hue = item.Hue;
			newItem.Layer = item.Layer;

			return newItem;
		}

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			Delete();
		}

		public override bool DeleteCorpseOnDeath { get { return true; } }

		public override void OnDelete()
		{
			Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 15,
				5042);

			base.OnDelete();
		}

		public override void OnAfterDelete()
		{
			MirrorImage.RemoveClone(m_Caster);
			base.OnAfterDelete();
		}

		public override bool IsDispellable { get { return false; } }
		public override bool Commandable { get { return false; } }

		public AncientCloneSpell(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version

			writer.Write(m_Caster);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();

			m_Caster = reader.ReadMobile();

			MirrorImage.AddClone(m_Caster);
		}
	}
}
