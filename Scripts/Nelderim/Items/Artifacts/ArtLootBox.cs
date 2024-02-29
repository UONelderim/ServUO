#region References

using System;
using Server.Items;

#endregion

namespace Server
{
	public class ArtLootBox : Item
	{
		public enum ArtLootType
		{
			Random = 0,
			Boss = 1,
			Miniboss = 2,
			Paragon = 3,
			Doom = 4,
			Hunter = 5,
			Cartography = 6,
			Fishing = 7
		}

		private ArtLootType m_LootType;

		[CommandProperty(AccessLevel.GameMaster)]
		public ArtLootType LootType
		{
			get { return m_LootType; }
			set
			{
				m_LootType = value;
				SetName();
			}
		}

		public override double DefaultWeight
		{
			get { return 0.01; }
		}

		[Constructable]
		public ArtLootBox() : this(ArtLootType.Random)
		{
		}

		[Constructable]
		public ArtLootBox(string type) : base(0x2DF3)
		{
			if (!Enum.TryParse(type, out m_LootType))
				m_LootType = ArtLootType.Paragon;
			SetName();
		}

		public ArtLootBox(ArtLootType type) : base(0x2DF3)
		{
			m_LootType = type;
			SetName();
		}

		public ArtLootBox(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (Parent != from.Backpack)
				from.SendMessage("Nie mozesz tego uzyc");
			else
			{
				switch (m_LootType)
				{
					case ArtLootType.Random:
						from.AddToBackpack(ArtifactHelper.CreateRandomArtifact());
						break;
					case ArtLootType.Boss:
						from.AddToBackpack(ArtifactHelper.CreateRandomBossArtifact());
						break;
					case ArtLootType.Miniboss:
						from.AddToBackpack(ArtifactHelper.CreateRandomMinibossArtifact());
						break;
					case ArtLootType.Paragon:
						from.AddToBackpack(ArtifactHelper.CreateRandomParagonArtifact());
						break;
					case ArtLootType.Hunter:
						from.AddToBackpack(ArtifactHelper.CreateRandomHunterArtifact());
						break;
					case ArtLootType.Cartography:
						from.AddToBackpack(ArtifactHelper.CreateRandomCartographyArtifact());
						break;
					case ArtLootType.Fishing:
						from.AddToBackpack(ArtifactHelper.CreateRandomFishingArtifact());
						break;
				}

				Delete();
			}

			base.OnDoubleClick(from);
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

		private void SetName()
		{
			switch (m_LootType)
			{
				case ArtLootType.Random:
					Name = "Skrzynia Artefaktu";
					break;
				case ArtLootType.Boss:
					Name = "Skrzynia Artefaktu Władcy Podziemi";
					break;
				case ArtLootType.Miniboss:
					Name = "Skrzynia Artefaktu Pomniejszego Władcy Podziemi";
					break;
				case ArtLootType.Paragon:
					Name = "Skrzynia Artefaktu Paragonów";
					break;
				case ArtLootType.Doom:
					Name = "Skrzynia Artefaktu Pana Mroku";
					break;
				case ArtLootType.Hunter:
					Name = "Skrzynia Artefaktu Myśliwego";
					break;
				case ArtLootType.Cartography:
					Name = "Skrzynia Artefaktu Poszukiwaczy Skarbów";
					break;
				case ArtLootType.Fishing:
					Name = "Skrzynia Artefaktu Leviathana";
					break;
			}

			InvalidateProperties();
		}
	}
}
