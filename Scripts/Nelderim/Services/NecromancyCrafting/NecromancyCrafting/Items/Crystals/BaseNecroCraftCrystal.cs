using System;
using System.Collections.Generic;
using System.Linq;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class BaseNecroCraftCrystal : Item
	{
		private static Dictionary<Type, string> BodyPartName = new()
		{
			{ typeof(RottingLegs), "gnijące nogi" },
			{ typeof(RottingTorso), "gnijący tułów " },
			{ typeof(SkeletonLegs), "nogi szkieleta" },
			{ typeof(SkeletonMageTorso), "tulow szkieleta maga" },
			{ typeof(SkeletonTorso), "tulow szkieleta" },
			{ typeof(WrappedLegs), "zmumifikowane nogi" },
			{ typeof(WrappedMageTorso), "zmumifikowany tułów oznaczony runami" },
			{ typeof(WrappedTorso), "zmumifikowany tułów" },
			{ typeof(Phylacery), "filakterium" },
			{ typeof(Brain), "mozg" },
		};

		public abstract double RequiredNecroSkill { get; }

		public abstract Type[] RequiredBodyParts { get; }

		public int[] RequiredBodyPartsAmounts => RequiredBodyParts.Select(x => 1).ToArray();

		public abstract Type SummonType { get; }

		public BaseNecroCraftCrystal() : base(0x1F19)
		{
			Weight = 1.0;
			Stackable = false;
		}

		public BaseNecroCraftCrystal(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return;
			}

			double NecroSkill = from.Skills[SkillName.Necromancy].Value;

			if (NecroSkill < RequiredNecroSkill)
			{
				from.SendMessage(String.Format(
					"Musisz mieć przynajmniej {0:F1} umiejętności nekromancji, by stworzyć szkieleta.",
					RequiredNecroSkill));
				return;
			}


			var pack = from.Backpack;

			if (pack == null)
				return;
			var bc = (BaseCreature)Activator.CreateInstance(SummonType);
			if (from.Followers + bc.ControlSlots > from.FollowersMax)
			{
				from.SendLocalizedMessage(1049607); // You have too many followers to control that creature.
				bc.Delete();
				return;
			}

			int res = pack.ConsumeTotal(RequiredBodyParts, RequiredBodyPartsAmounts);
			if (res != -1)
			{
				if (BodyPartName.ContainsKey(RequiredBodyParts[res]))
				{
					from.SendMessage("Musisz miec " + BodyPartName[RequiredBodyParts[res]]);
				}
				else
				{
					from.SendMessage("Musisz miec " + RequiredBodyParts[res].Name);
				}

				if (from.AccessLevel > AccessLevel.Player)
				{
					from.SendMessage("Boskie moce pomagają ci stworzyć przywołańca bez wszystkich części ciała");
				}
				else
				{
					return;
				}
			}

			if (bc.SetControlMaster(from))
			{
				bc.Allured = true;
				Scale(bc, NecroSkill);
				bc.MoveToWorld(from.Location, from.Map);
				from.PlaySound(0x241);
				Consume();
			}
			else
			{
				bc.Delete();
			}
		}

		private void Scale(BaseCreature bc, double skillValue)
		{
			int scalar = (int)(skillValue - RequiredNecroSkill);

			bc.RawStr += AOS.Scale(bc.RawStr, scalar);
			bc.RawDex += AOS.Scale(bc.RawDex, scalar);
			bc.RawInt += AOS.Scale(bc.RawInt, scalar);

			bc.HitsMaxSeed += AOS.Scale(bc.HitsMaxSeed, scalar);
			bc.StamMaxSeed += AOS.Scale(bc.StamMaxSeed, scalar);
			bc.ManaMaxSeed += AOS.Scale(bc.ManaMaxSeed, scalar);

			bc.Hits = (int)(bc.HitsMax * 0.5);
			bc.Stam = bc.StamMax;
			bc.Mana = bc.ManaMax;

			for (int i = 0; i < bc.Skills.Length; i++)
			{
				Skill skill = bc.Skills[i];

				if (skill.Base > 0.0)
					skill.BaseFixedPoint += AOS.Scale(skill.BaseFixedPoint, scalar);
			}

			bc.DamageMin += AOS.Scale(bc.DamageMin, scalar);
			bc.DamageMax += AOS.Scale(bc.DamageMax, scalar);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
