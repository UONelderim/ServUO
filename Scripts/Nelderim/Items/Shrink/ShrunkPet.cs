#region References

using Server.ContextMenus;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

#endregion

namespace Server.Items
{
	public class ShrunkPet : Item
	{
		private static readonly int[] _cost = { 50, 200, 800, 2000, 5000 };

		[CommandProperty(AccessLevel.Counselor)]
		public virtual bool RequiresAnimalTrainer => true;

		[CommandProperty(AccessLevel.GameMaster)]
		public int PetHue
		{
			get => Pet?.Hue ?? 0;
			set
			{
				if (Pet != null)
					Pet.Hue = value;
			}
		}

		// In case pet disappears
		[CommandProperty(AccessLevel.GameMaster, true)]
		public Serial LastSerial { get; private set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public BaseCreature Pet
		{
			get;
			private set;
		}

		public static void Shrink(BaseVendor trainer, PlayerMobile from, BaseCreature target, bool confirmed = false)
		{
			if (from == null)
				return;

			if (target == null || target.ControlSlots > 5 || 1 > target.ControlSlots || target.Allured)
				from.SendLocalizedMessage(1070041); // Nie mozesz tego uwiazac.
			else if (target.ControlMaster != from)
				from.SendLocalizedMessage(1070042); // Musisz kontrolowac istote ktora chcesz uwiazac
			else if (target.Summoned)
				from.SendLocalizedMessage(1070043); // Nie mozesz uwiazac przywolanca.
			else if (target.Combatant != null && target.InRange(target.Combatant, 20))
				from.SendLocalizedMessage(1070044); // Nie mozesz przywiazac zwierzecia ktore walczy
			else if (target.Hits < target.HitsMax || target.Poisoned)
				from.SendLocalizedMessage(1070045); // Nie mozesz przywiaza rannego zwierzecia
			else if ((target is PackLlama || target is PackHorse || target is Beetle)
			         && target.Backpack != null && target.Backpack.Items.Count > 0)
				from.SendLocalizedMessage(1070046); // Wpierw musisz rozladowac jego juki
			else if (!target.IsNearBy(trainer, 5))
				from.SendLocalizedMessage(1070049); // Jestes zbyt daleko od tresera zwierzat
			else
			{
				int cost = _cost[target.ControlSlots - 1];

				if (from.TotalGold < cost)
					from.SendLocalizedMessage(1070047, cost.ToString()); // Nie stac Cie, potrzebujesz {0} zlota
				else
				{
					if (confirmed)
					{
						if (from.Backpack.ConsumeTotal(typeof(Gold), cost))
						{
							from.Backpack.AddItem(new ShrunkPet(target));
							target.AIObject.DoOrderRelease();
							target.Map = Map.Internal;
							target.Blessed = true;

							from.SendLocalizedMessage(1070048);
						}
						else
							from.SendLocalizedMessage(1070047, cost.ToString()); // Nie stac Cie, potrzebujesz {0} zlota
					}
					else
					{
						var g = new GeneralConfirmGump();
						g.Text =
							$"Usluga bedzie Cie kosztowac {cost} centarow.<br /> Po pomniejszeniu zwierze straci pamiec. Czy jestes pewien ze chcesz to zrobic?";
						g.Size = new Point2D(300, 160);
						g.OnContinue += (ns, ri) => Shrink(trainer, from, target, true);
						from.SendGump(g);
					}
				}
			}
		}

		public void AttachPet(BaseCreature newPet)
		{
			if (Pet != null)
			{
				Pet.Delete();
				Pet = null;
				LastSerial = Serial.Zero;
			}

			if (newPet != null)
			{
				Pet = newPet;
				LastSerial = newPet.Serial;
				Hue = PetHue;
				ItemID = ShrinkTable.Lookup(Pet.Body);
			}
		}

		public override void OnDelete()
		{
			Pet?.Delete();
			base.OnDelete();
		}

		public ShrunkPet(BaseCreature pet) : base(ShrinkTable.Lookup(pet.Body))
		{
			Name = "Statuetka zwierzecia";
			LootType = LootType.Blessed;

			AttachPet(pet);
		}

		public virtual void OnAfterUnshrink()
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (Pet == null || Pet.Deleted)
			{
				from.SendMessage(
					"Wyglada na to, ze dusza tego zwierzecia zdolala sie uwolnic ze statuetki jakis czas temu.");
				return;
			}

			if (!IsChildOf(from.Backpack))
				from.SendLocalizedMessage(1070051); // Przedmiot musi znajdowac sie w Twoim plecaku
			else if (!Pet.CanBeControlledBy(from))
				from.SendLocalizedMessage(1070052); // Nie potrafisz zapanowac nad ta istota
			else if (Pet.ControlSlots + from.Followers > from.FollowersMax)
				from.SendLocalizedMessage(1070053); // Masz zbyt wiele zwierzat pod swoja opieka
			else if (!from.Alive)
				from.SendLocalizedMessage(1070054); // Nie mozesz tego zrobic gdy jestes duchem
			else
			{
				if (from.IsNearBy(typeof(AnimalTrainer)) || !RequiresAnimalTrainer)
				{
					Pet.SetControlMaster(from);
					Pet.Location = from.Location;
					Pet.Map = from.Map;
					Pet.Blessed = false;

					OnAfterUnshrink();

					Pet = null; // Separate pet from the statue to avoid deleting pet along with the statue
					Delete();

					from.SendLocalizedMessage(1070055); // Powiekszyles zwierze
				}
				else
					from.SendLocalizedMessage(1070049); // Jestes zbyt daleko od tresera zwierzat
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (Pet == null || Pet.Deleted)
			{
				list.Add("(Pusta statuetka)");
				return;
			}

			list.Add(Pet.Name);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(3); //version

			writer.Write(Pet);
			writer.Write(LastSerial.Value);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();
			Pet = (BaseCreature)reader.ReadMobile();

			if (version < 3)
			{
				reader.ReadBool(); //Deprecated prop
			}

			LastSerial = reader.ReadSerial();
		}

		public ShrunkPet(Serial serial) : base(serial)
		{
		}
	}

	public class PetShrinkEntry : ContextMenuEntry
	{
		private BaseVendor m_Trainer;
		private Mobile m_From;

		public PetShrinkEntry(BaseVendor trainer, Mobile from)
			: base(6065, 12)
		{
			m_Trainer = trainer;
			m_From = from;
		}

		public override void OnClick()
		{
			if (!Owner.From.CheckAlive())
				return;

			if (m_Trainer.CheckVendorAccess(m_From))
			{
				m_From.SendLocalizedMessage(1070058);
				m_From.Target = new ShrinkTarget(m_Trainer);
			}
		}
	}

	public class ShrinkTarget : Target
	{
		private readonly BaseVendor m_Trainer;

		public ShrinkTarget(BaseVendor trainer) : base(12, false, TargetFlags.None)
		{
			m_Trainer = trainer;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			ShrunkPet.Shrink(m_Trainer, from as PlayerMobile, targeted as BaseCreature);
		}
	}
}
