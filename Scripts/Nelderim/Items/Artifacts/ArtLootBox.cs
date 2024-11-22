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

			var artifact = ArtifactHelper.GetRandomArtifact(_LootType);
			ArtifactHelper.GiveArtifact(from, artifact);
			from.SendMessage($"Skrzynia Artefaktu zawierała {artifact.Name ?? artifact.GetType().Name}");
			Delete();

			base.OnDoubleClick(from);
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
			writer.Write((int)_LootType);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			if (version >= 1)
			{
				_LootType = (ArtGroup) reader.ReadInt();
			}
		}

		private void SetName()
		{
			Name = _LootType switch
			{
				ArtGroup.None => "Skrzynia Artefaktu",
				ArtGroup.Boss => "Skrzynia Artefaktu Władcy Podziemi",
				ArtGroup.Miniboss => "Skrzynia Artefaktu Pomniejszego Władcy Podziemi",
				ArtGroup.Paragon => "Skrzynia Artefaktu Paragonów",
				ArtGroup.Doom => "Skrzynia Artefaktu Pana Mroku",
				ArtGroup.Hunter => "Skrzynia Artefaktu Myśliwego",
				ArtGroup.Cartography => "Skrzynia Artefaktu Poszukiwaczy Skarbów",
				ArtGroup.Fishing => "Skrzynia Artefaktu Leviathana",
				_ => Name
			};
		}
	}
}
