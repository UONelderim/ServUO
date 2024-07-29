#region References

using System;
using System.Collections.Generic;
using Server.ACC.CSS;
using Server.Targeting;

#endregion

namespace Server.Items
{
	public enum PigmentTarget
	{
		Metal = 0,
		Leather = 1,
		Wood = 2,
		Spellbook = 3,
		Cloth = 4
	}

	public class BasePigment : Item, IUsesRemaining
	{
		private PigmentTarget m_Target;

		[CommandProperty(AccessLevel.GameMaster)]
		public PigmentTarget Target
		{
			get { return m_Target; }
			set
			{
				m_Target = value;
				InvalidateProperties();
			}
		}

		private int m_UsesRemaining;

		public override int LabelNumber
		{
			get
			{
				switch (m_Target)
				{
					case PigmentTarget.Wood:
						return 1064860; // Pigment do drewna
					case PigmentTarget.Metal:
						return 1064861; // Pigment do metalu
					case PigmentTarget.Leather:
						return 1064862; // Pigment do skory
					case PigmentTarget.Spellbook:
						return 1064863; // Pigment do ksiag
					case PigmentTarget.Cloth:
						return 1064869; // Pigment do tkanin
				}

				return 1064860;
			}
		}

		[Constructable]
		public BasePigment(int level)
			: this(Utility.RandomList(0, 1, 2, 3), 3, level)
		{
		}

		[Constructable]
		public BasePigment()
			: this(Utility.RandomList(0, 1, 2, 3), 3, Utility.RandomList(0, 1))
		{
		}

		[Constructable]
		public BasePigment(int target, int uses, int level)
			: base(0xEFF)
		{
			Weight = 1.0;
			m_Target = (PigmentTarget)target;
			SetPigment(uses, level);
		}

		public BasePigment(PigmentTarget target, int uses, int hue)
			: base(0xEFF)
		{
			Weight = 1.0;
			m_Target = target;
			m_UsesRemaining = uses;
			Hue = hue;
		}

		// Czesc hue w hex czesc w int
		private void SetPigment(int uses, int level)
		{
			switch (m_Target)
			{
				case PigmentTarget.Metal:
					if (level == 0)
					{
						m_UsesRemaining = uses;
						Hue = Utility.RandomList(0x07A1, 0x079E, 0x054D, 0x0622, 0x08A4, 0x0552);
					}
					else if (level == 1)
					{
						m_UsesRemaining = uses + 1;
						Hue = Utility.RandomList(0x07A1, 0x079E, 0x054D, 0x0622, 0x08A4, 0x0552, 1158, 1109, 1644, 33,
							1166, 1366);
					}

					break;
				case PigmentTarget.Leather:
					if (level == 0)
					{
						m_UsesRemaining = uses;
						Hue = Utility.RandomList(0x07A1, 0x079E, 0x054D, 0x055A, 0x08A4, 0x0552);
					}
					else if (level == 1)
					{
						m_UsesRemaining = uses + 1;
						Hue = Utility.RandomList(0x07A1, 0x079E, 0x054D, 0x055A, 0x08A4, 0x0552, 1158, 1420, 1270, 1364,
							1165);
					}

					break;
				case PigmentTarget.Wood:
					if (level == 0)
					{
						m_UsesRemaining = uses;
						Hue = Utility.RandomList(0x07A1, 0x079E, 0x0842, 0x0552);
					}
					else if (level == 1)
					{
						m_UsesRemaining = uses + 1;
						Hue = Utility.RandomList(0x07A1, 0x079E, 0x0842, 0x0552, 1158, 1150, 1167, 1428, 1548);
					}

					break;
				case PigmentTarget.Spellbook:
					if (level == 0)
					{
						m_UsesRemaining = 1;
						Hue = Utility.RandomList(0x07A1, 0x079E, 0x054D, 0x0622, 0x08A4, 0x0552);
					}
					else if (level == 1)
					{
						m_UsesRemaining = 2;
						Hue = Utility.RandomList(0x07A1, 0x079E, 0x054D, 0x0622, 0x08A4, 0x0552, 1158, 1266, 1359, 1161,
							2460);
					}

					break;
				default:
					Hue = 0;
					break;
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(1060584, m_UsesRemaining.ToString()); // Pozostalo uzyc: ~1_val~
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (IsAccessibleTo(from) && from.InRange(GetWorldLocation(), 3))
			{
				from.SendLocalizedMessage(1064864); // Wybierz przedmiot do zafarbowania.
				from.BeginTarget(3, false, TargetFlags.None, new TargetStateCallback(InternalCallback),
					this);
			}
			else
				from.SendLocalizedMessage(502436); // To nie jest dostepne.
		}

		private void InternalCallback(Mobile from, object targeted, object state)
		{
			BasePigment pigment = (BasePigment)state;

			if (pigment.Deleted || pigment.UsesRemaining <= 0 || !from.InRange(pigment.GetWorldLocation(), 3) ||
			    !pigment.IsAccessibleTo(from))
				return;

			Item i = targeted as Item;

			if (i == null)
				from.SendLocalizedMessage(1064865); // Pigmentem mozesz zafarbowac jedynie przedmioty.
			else if (!i.IsChildOf(from.Backpack))
				from.SendLocalizedMessage(1064867); // Przedmiot do zafarbowania musi sie znajdowac w plecaku.
			else if (!from.InRange(i.GetWorldLocation(), 3) || !IsAccessibleTo(from))
				from.SendLocalizedMessage(502436); // To nie jest dostepne.
			else if (i is PigmentsOfTokuno)
				from.SendLocalizedMessage(1042417); // Nie mozesz tego zafarbowac.
			else if (!IsValidItem(i, m_Target))
				from.SendLocalizedMessage(1064866); // Nie mozesz tego zafarbowac tym pigmentem.
			else
			{
				i.Hue = Hue;
				from.SendLocalizedMessage(1064868); // Przedmiot zostal zafarbowany.
				if (--pigment.UsesRemaining <= 0)
					pigment.Delete();
			}
		}

		public static bool IsValidItem(Item i, PigmentTarget target)
		{
			if (i is PigmentsOfTokuno)
				return false;

			Type t = i.GetType();
			List<Type> listOfCorrectTypes = new List<Type>();
			List<CraftResource> resourcesAccepted = new List<CraftResource>();

			switch (target)
			{
				case PigmentTarget.Metal:
					resourcesAccepted.Add(CraftResource.Iron);
					resourcesAccepted.Add(CraftResource.DullCopper);
					resourcesAccepted.Add(CraftResource.ShadowIron);
					resourcesAccepted.Add(CraftResource.Copper);
					resourcesAccepted.Add(CraftResource.Bronze);
					resourcesAccepted.Add(CraftResource.Gold);
					resourcesAccepted.Add(CraftResource.Agapite);
					resourcesAccepted.Add(CraftResource.Verite);
					resourcesAccepted.Add(CraftResource.Valorite);
					resourcesAccepted.Add(CraftResource.Platinum);
					return IsInResourceList(resourcesAccepted, i);
				case PigmentTarget.Leather:
					resourcesAccepted.Add(CraftResource.RegularLeather);
					resourcesAccepted.Add(CraftResource.SpinedLeather);
					resourcesAccepted.Add(CraftResource.HornedLeather);
					resourcesAccepted.Add(CraftResource.BarbedLeather);
					return IsInResourceList(resourcesAccepted, i) || IsInType(t, typeof(BaseQuiver)) || IsInType(t, typeof(DeerMask)) || IsInType(t, typeof(BearMask)) || IsInType(t, typeof(HornedTribalMask)) || IsInType(t, typeof(TribalMask));
				case PigmentTarget.Wood:
					resourcesAccepted.Add(CraftResource.RegularWood);
					resourcesAccepted.Add(CraftResource.OakWood);
					resourcesAccepted.Add(CraftResource.AshWood);
					resourcesAccepted.Add(CraftResource.YewWood);
					resourcesAccepted.Add(CraftResource.Heartwood);
					resourcesAccepted.Add(CraftResource.Bloodwood);
					resourcesAccepted.Add(CraftResource.Frostwood);
					return IsInResourceList(resourcesAccepted, i);
				case PigmentTarget.Spellbook:
					return IsInType(t, typeof(Spellbook)) || IsInType(t, typeof(CSpellbook)) || IsInType(t, typeof(BaseTalisman));
				case PigmentTarget.Cloth:
					return IsInType(t, typeof(BaseClothing));
				default:
					return false;
			}
		}

		private static bool IsInResourceList(List<CraftResource> resourcesAccepted, Item i)
		{
			Type t = i.GetType();
			if (IsInType(t, typeof(BaseWeapon)))
				return (resourcesAccepted.Contains(((BaseWeapon)i).Resource) ||
				        resourcesAccepted.Contains(((BaseWeapon)i).Resource2));
			if (IsInType(t, typeof(BaseArmor)))
				return (resourcesAccepted.Contains(((BaseArmor)i).Resource) ||
				        resourcesAccepted.Contains(((BaseArmor)i).Resource2));
			if (IsInType(t, typeof(BaseClothing)))
				return (resourcesAccepted.Contains(((BaseClothing)i).Resource));
			if (IsInType(t, typeof(BaseJewel)))
				return (resourcesAccepted.Contains(((BaseJewel)i).Resource));
			return false;
		}

		private static bool IsInTypeList(Type t, List<Type> listOfTypes)
		{
			for (int i = 0; i < listOfTypes.Count; i++)
			{
				if (IsInType(t, listOfTypes[i])) return true;
			}

			return false;
		}

		private static bool IsInType(Type t, Type oneTypes)
		{
			return (oneTypes == t || t.IsSubclassOf(oneTypes));
		}

		public BasePigment(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.WriteEncodedInt((int)m_Target);
			writer.WriteEncodedInt(m_UsesRemaining);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Target = (PigmentTarget)reader.ReadEncodedInt();
			m_UsesRemaining = reader.ReadEncodedInt();
		}

		#region IUsesRemaining Members

		[CommandProperty(AccessLevel.GameMaster)]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set
			{
				m_UsesRemaining = value;
				InvalidateProperties();
			}
		}

		public bool ShowUsesRemaining
		{
			get { return true; }
			set { }
		}

		#endregion
	}
}
