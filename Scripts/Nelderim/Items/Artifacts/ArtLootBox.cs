#region References

using System;
using Server.Items;

#endregion

namespace Server
{
	public class ArtLootBox : Item
	{
		private ArtGroup _LootType;

		[CommandProperty(AccessLevel.GameMaster)]
		public ArtGroup LootType
		{
			get => _LootType;
			set
			{
				_LootType = value;
				SetName();
			}
		}

		public override double DefaultWeight => 0.01;

		[Constructable]
		public ArtLootBox() : this(ArtGroup.None)
		{
		}

		[Constructable]
		public ArtLootBox(string type) : base(0x2DF3)
		{
			if (!Enum.TryParse(type, out _LootType))
				_LootType = ArtGroup.Paragon;
			SetName();
		}

		public ArtLootBox(ArtGroup type) : base(0x2DF3)
		{
			_LootType = type;
			SetName();
		}

		public ArtLootBox(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (Parent != from.Backpack)
			{
				from.SendMessage("Nie mozesz tego uzyc");
				return;
			}

			from.AddToBackpack(ArtifactHelper.GetRandomArtifact(_LootType));
			Delete();

			base.OnDoubleClick(from);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		private void SetName()
		{
			switch (_LootType)
			{
				case ArtGroup.None:
					Name = "Skrzynia Artefaktu";
					break;
				case ArtGroup.Boss:
					Name = "Skrzynia Artefaktu Władcy Podziemi";
					break;
				case ArtGroup.Miniboss:
					Name = "Skrzynia Artefaktu Pomniejszego Władcy Podziemi";
					break;
				case ArtGroup.Paragon:
					Name = "Skrzynia Artefaktu Paragonów";
					break;
				case ArtGroup.Doom:
					Name = "Skrzynia Artefaktu Pana Mroku";
					break;
				case ArtGroup.Hunter:
					Name = "Skrzynia Artefaktu Myśliwego";
					break;
				case ArtGroup.Cartography:
					Name = "Skrzynia Artefaktu Poszukiwaczy Skarbów";
					break;
				case ArtGroup.Fishing:
					Name = "Skrzynia Artefaktu Leviathana";
					break;
			}

			InvalidateProperties();
		}
	}
}
