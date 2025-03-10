﻿#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class PublicVendor : RentedVendor
	{
		public override bool ChargeCommision => true;
		public override bool ChargeDaily => true;

		public PublicVendor(Mobile owner, VendorRentalDuration duration, int rentalPrice, int renewalPrice,
			bool landlordRenew) : base(owner, null, duration, rentalPrice, landlordRenew, 0)
		{
			Title = "- kupiec";
			RenewalPrice = renewalPrice;
		}

		public PublicVendor(Serial serial) : base(serial)
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Renew => LandlordRenew && RenterRenew;

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
			
			var vendorItems = GetItems();
			Container backpack = new Backpack();
			
			foreach (Item item in vendorItems)
			{
				backpack.DropItem(item);
			}

			Owner?.BankBox.DropItem(backpack);

			base.Destroy(true);
		}
	}
}
