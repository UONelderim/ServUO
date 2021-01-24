using Server.Gumps;
using Server.Helpers;
using Server.Mobiles;
using System;
using System.Collections;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class QuestShrunkPet : ShrunkPet
    {
        private int m_PetExpireTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public override bool RequiresAnimalTrainer
        {
            get { return false; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PetExpireTime
        {
            get { return m_PetExpireTime; }
            set { m_PetExpireTime = value; }
        }

        [Constructable]
        public QuestShrunkPet(string typeName, int statueExpireTime) : base()
        {
            Name = "Podejrzana statuetka zwierzecia";

            try
            {
                Type creatureType = ScriptCompiler.FindTypeByName(typeName);

                if (creatureType == null)
                {
                    Console.WriteLine("ERROR: QuestShrunkPet(): FindTypeByName({0})", typeName);
                }
                else if( !creatureType.IsSubclassOf(typeof(BaseCreature)))
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

            m_PetExpireTime = 60 * 5; // 5h default

            // This item is gonna be deleted after specified amount of minutes:
            XmlAttachment attachment = (XmlAttachment)(new TemporaryQuestObject("Nazwa", statueExpireTime));
            if (attachment == null)
            {
                Console.WriteLine("ERROR: QuestShrunkPet(): Unable to construct TemporaryQuestObject with specified args");
            }
            else
            {
                if (XmlAttach.AttachTo(this, attachment))
                {
                    //Console.WriteLine("QuestShrunkPet(): Added TemporaryQuestObject to {0}", this);
                }
                else
                    Console.WriteLine("ERROR: QuestShrunkPet(): Attachment TemporaryQuestObject not added to {0}", this);
            }
        }

        public override void OnAfterUnshrink()
        {
            // This creature is gonna be deleted after specified amount of minutes:
            XmlAttachment attachment = (XmlAttachment)(new TemporaryQuestObject("Nazwaaa", m_PetExpireTime));

            if (attachment == null)
            {
                Console.WriteLine("ERROR: QuestShrunkPet.OnAfterUnshrink(): Unable to construct TemporaryQuestObject with specified args");
            }
            else
            {
                if (XmlAttach.AttachTo(Pet, attachment))
                {
                    //Console.WriteLine("QuestShrunkPet.OnAfterUnshrink(): Added TemporaryQuestObject to {0}", Pet);
                }
                else
                    Console.WriteLine("ERROR: QuestShrunkPet.OnAfterUnshrink(): Attachment TemporaryQuestObject not added to {0}", Pet);
            }
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);

            writer.Write(0); // version

            writer.Write(m_PetExpireTime);
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_PetExpireTime = reader.ReadInt();
        }

        public QuestShrunkPet( Serial serial ) : base(serial)
        {
        }
    }
}
