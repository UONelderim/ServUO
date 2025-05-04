using Server;
using Server.Items;
using Server.Network;
using Server.Regions;

[Flipable(0x278F, 0x27DA)]
public class NamelessHood : BaseHat
{
	public override int BasePhysicalResistance => 0;
	public override int BaseFireResistance => 0;
	public override int BaseColdResistance => 0;
	public override int BasePoisonResistance => 0;
	public override int BaseEnergyResistance => 0;

	public override int InitMinHits => 20;
	public override int InitMaxHits => 30;

	[Constructable]
	public NamelessHood()
		: this(0)
	{
	}

	[Constructable]
	public NamelessHood(int hue)
		: base(0x278F, hue)
	{
		Name = "Kaptur Ukrywajacy Tozsamosc";
		Layer = Layer.Earrings;
		Label1 = "(kaptur naciagany na uszy; kolczyki i nakrycie glowy moga przeszkadzac w jego noszeniu)";
	}

	public NamelessHood(Serial serial)
		: base(serial)
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
		reader.ReadInt(); // version
	}

	public override void OnAdded(IEntity parent)
	{
		base.OnAdded(parent);

		if (parent is Mobile mobile)
		{
			mobile.IdentityHidden = true;
		}
	}

	public override void OnRemoved(IEntity parent)
	{
		base.OnRemoved(parent);

		if (parent is Mobile mobile)
		{
			mobile.IdentityHidden = false;
		}
	}

	public override void OnParentLocationChange(Point3D oldLocation)
	{
		base.OnParentLocationChange(oldLocation);

		if (Parent is Mobile mobile)
		{
			if ((mobile.Region is CityRegion || mobile.Region is VillageRegion) && mobile.IdentityHidden)
			{
				mobile.SendMessage("Nie lubi się tutaj zakapturzonych typów, uważaj na straż.");
			}
		}
	}
}
