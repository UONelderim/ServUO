using System;
using Server.Items;

namespace Server.Mobiles
{
	public class PublicVendor : RentedVendor
	{
		public PublicVendor(Mobile owner, VendorRentalDuration duration, int rentalPrice, int renewalPrice, bool landlordRenew) : base(owner, null, duration, rentalPrice, landlordRenew, 0)
		{
			Title = "- kupiec";
			RenewalPrice = renewalPrice;
		}

		public PublicVendor(Serial serial) : base(serial)
		{
		}

		[CommandProperty(AccessLevel.Seer)]
		public string EditableExpireTime
		{
			get
			{
				return m_RentalExpireTime.ToString();
			}
			set
			{
				try
				{
					m_RentalExpireTime = DateTime.Parse(value);
				}
				catch
				{ }
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

		public override void Destroy(bool toBackpack)
		{
			PublicVendorRentalContract pvrc = new PublicVendorRentalContract(this);
			pvrc.MoveToWorld(Location, Map);
			pvrc.Movable = false;

			base.Destroy(true);
		}
	}
}
