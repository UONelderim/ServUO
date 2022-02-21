#region References

using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

#endregion

namespace Server.Items
{
	public class PublicVendorRentalContract : VendorRentalContract
	{
		[Constructable]
		public PublicVendorRentalContract()
		{
			Hue = 0x675;
			Name = "stanowisko kupieckie";
			Price = 5000; // cena wynajecia kupca na targowisku

			LandlordRenew = true;
		}

		public PublicVendorRentalContract(RentedVendor vendor) : this()
		{
			Price = vendor.RenewalPrice;
			LandlordRenew = vendor.LandlordRenew;
		}

		public PublicVendorRentalContract(Serial serial) : base(serial)
		{
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (Offeree != null)
				list.Add(1062368, Offeree.Name); // Being Offered To ~1_NAME~
		}

		public override void OnDelete()
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (Offeree != null)
			{
				from.SendLocalizedMessage(1062343); // That item is currently in use.
			}
			else if (Movable)
			{
				if (!IsChildOf(from.Backpack))
				{
					from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
					return;
				}

				from.Target = new RentTarget(this);
			}
			else if (IsLandlord(from))
			{
				if (from.InRange(this, 5))
				{
					from.CloseGump(typeof(VendorRentalContractGump));
					from.SendGump(new VendorRentalContractGump(this, from));
				}
				else
				{
					from.SendLocalizedMessage(501853); // Target is too far away.
				}
			}
			else
			{
				if (from == null || !from.Player || !from.Alive)
					return;

				if (!from.InRange(this, 5))
				{
					from.SendLocalizedMessage(501853); // Target is too far away.
				}
				else
				{
					from.SendGump(new PublicVendorRentalOfferGump(this));

					Offeree = from;
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}

		private class RentTarget : Target
		{
			private readonly VendorRentalContract m_Contract;

			public RentTarget(VendorRentalContract contract) : base(-1, true, TargetFlags.None)
			{
				m_Contract = contract;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (!m_Contract.IsUsableBy(from, false, true, true, true))
					return;

				IPoint3D location = targeted as IPoint3D;
				if (location == null)
					return;

				Point3D pLocation = new Point3D(location);
				Map map = from.Map;

				if (!map.CanFit(pLocation, 16, false, false))
				{
					from.SendLocalizedMessage(1062486); // A vendor cannot exist at that location.  Please try again.
				}
				else
				{
					m_Contract.MoveToWorld(pLocation, map);
					m_Contract.Movable = false;
				}
			}

			protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
			{
				from.SendLocalizedMessage(1062336); // You decide not to place the contract at this time.
			}
		}

		public class PublicVendorRentalOfferGump : BaseVendorRentalGump
		{
			private readonly PublicVendorRentalContract m_Contract;

			public PublicVendorRentalOfferGump(PublicVendorRentalContract contract) : base(
				GumpType.Offer, contract.Duration, contract.Price, contract.Price,
				null, null, contract.LandlordRenew, false, false)
			{
				m_Contract = contract;
			}

			protected override bool IsValidResponse(Mobile from)
			{
				return from.CheckAlive() && m_Contract.Offeree == from;
			}

			protected override void AcceptOffer(Mobile from)
			{
				m_Contract.Offeree = null;

				if (!m_Contract.Map.CanFit(m_Contract.Location, 16, false, false))
				{
					from.SendLocalizedMessage(1062486); // A vendor cannot exist at that location.  Please try again.
					return;
				}

				if (m_Contract.Price > 0)
				{
					if (Banker.Withdraw(from, m_Contract.Price))
						from.SendLocalizedMessage(1060398,
							m_Contract.Price.ToString()); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
					else
					{
						from.SendLocalizedMessage(
							1062378); // You do not have enough gold in your bank account to cover the cost of the contract.

						return;
					}
				}

				PlayerVendor vendor = new PublicVendor(from, m_Contract.Duration, m_Contract.Price, 0,
					m_Contract.LandlordRenew);
				vendor.MoveToWorld(m_Contract.Location, m_Contract.Map);

				m_Contract.Delete();

				from.SendLocalizedMessage(
					1062377); // You have accepted the offer and now own a vendor in this house.  Rental contract options and details may be viewed on this vendor via the 'Contract Options' context menu.	
			}

			protected override void Cancel(Mobile from)
			{
				m_Contract.Offeree = null;

				from.SendLocalizedMessage(1062375); // You decline the offer for a vendor space rental.
			}
		}
	}
}
