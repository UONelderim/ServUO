#region References

using System;
using Server.Engines.XmlSpawner2;
using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class QuestShrunkPet : ShrunkPet
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public override bool RequiresAnimalTrainer
		{
			get { return false; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int PetExpireTime { get; set; }

		[Constructable]
		public QuestShrunkPet(string typeName, int statueExpireTime)
		{
			Name = "Podejrzana statuetka zwierzecia";

			try
			{
				Type creatureType = ScriptCompiler.FindTypeByName(typeName);

				if (creatureType == null)
				{
					Console.WriteLine("ERROR: QuestShrunkPet(): FindTypeByName({0})", typeName);
				}
				else if (!creatureType.IsSubclassOf(typeof(BaseCreature)))
				{
					Console.WriteLine("ERROR: QuestShrunkPet(): This is not BaseCreature: {0}", typeName);
				}
				else
				{
					BaseCreature bc = (BaseCreature)Activator.CreateInstance(creatureType);

					if (bc != null)
						AttachPet(bc);
					else
						Console.WriteLine("ERROR: QuestShrunkPet(): CreateInstance({0})", typeName);
				}
			}
			catch
			{
				Console.WriteLine("ERROR: QuestShrunkPet(): CreateInstance({0})", typeName);
			}

			PetExpireTime = 60 * 5; // 5h default

			// This item is gonna be deleted after specified amount of minutes:
			XmlAttachment attachment = new TemporaryQuestObject("Nazwa", statueExpireTime);
			if (attachment == null)
			{
				Console.WriteLine(
					"ERROR: QuestShrunkPet(): Unable to construct TemporaryQuestObject with specified args");
			}
			else
			{
				if (XmlAttach.AttachTo(this, attachment))
				{
					//Console.WriteLine("QuestShrunkPet(): Added TemporaryQuestObject to {0}", this);
				}
				else
					Console.WriteLine("ERROR: QuestShrunkPet(): Attachment TemporaryQuestObject not added to {0}",
						this);
			}
		}

		public override void OnAfterUnshrink()
		{
			// This creature is gonna be deleted after specified amount of minutes:
			XmlAttachment attachment = new TemporaryQuestObject("Nazwaaa", PetExpireTime);

			if (attachment == null)
			{
				Console.WriteLine(
					"ERROR: QuestShrunkPet.OnAfterUnshrink(): Unable to construct TemporaryQuestObject with specified args");
			}
			else
			{
				if (XmlAttach.AttachTo(Pet, attachment))
				{
					//Console.WriteLine("QuestShrunkPet.OnAfterUnshrink(): Added TemporaryQuestObject to {0}", Pet);
				}
				else
					Console.WriteLine(
						"ERROR: QuestShrunkPet.OnAfterUnshrink(): Attachment TemporaryQuestObject not added to {0}",
						Pet);
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version

			writer.Write(PetExpireTime);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			PetExpireTime = reader.ReadInt();
		}

		public QuestShrunkPet(Serial serial) : base(serial)
		{
		}
	}
}
