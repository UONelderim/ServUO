namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarBag : ScrollBag
	{
		[Constructable]
		public AvatarBag()
		{
			Hue = 1174;
			PlaceItemIn(30, 35, new AvatarHeavenlyLightScroll());
			PlaceItemIn(50, 35, new AvatarHeavensGateScroll());
			PlaceItemIn(70, 35, new AvatarMarkOfGodsScroll());
			PlaceItemIn(50, 35, new AvatarRestorationScroll());
			PlaceItemIn(70, 35, new AvatarSacredBoonScroll());
			PlaceItemIn(70, 35, new AvatarAngelicFaithScroll());
			PlaceItemIn(70, 35, new AvatarArmysPaeonScroll());
		}

		public AvatarBag(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
