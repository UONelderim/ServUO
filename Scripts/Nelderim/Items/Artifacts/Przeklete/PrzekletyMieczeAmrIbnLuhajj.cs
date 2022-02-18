namespace Server.Items
{
	public class PrzekletyMieczeAmrIbnLuhajj : Daisho
	{
		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public PrzekletyMieczeAmrIbnLuhajj()
		{
			Hue = 2700;
			Name = "Miecze Amr Ibn Luhajj";
			LootType = LootType.Cursed;
			WeaponAttributes.MageWeapon = 60;
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.Luck = 50;
			Attributes.RegenMana = 10;
			Attributes.SpellDamage = 15;
			//Server.Engines.XmlSpawner2.XmlAttach.AttachTo(this, new Server.Engines.XmlSpawner2.TemporaryQuestObject("CursedArtifact", 20160));
		}

		public PrzekletyMieczeAmrIbnLuhajj(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
