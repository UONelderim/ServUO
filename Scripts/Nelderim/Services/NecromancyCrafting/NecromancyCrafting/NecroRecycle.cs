using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Engines.Craft
{
    public class NecroRecycle
    {
        public static void Do(Mobile from, CraftSystem craftSystem, ITool tool)
        {
            int num = craftSystem.CanCraft(from, tool, null);

            if (num > 0)
            {
                from.SendGump(new CraftGump(from, craftSystem, tool, num));
            }
            else
            {
                from.Target = new InternalTarget(craftSystem, tool);
                from.SendMessage("Wskaz przywolanca do rozłożenia");
            }
        }

        private class InternalTarget : Target
        {
            private CraftSystem m_CraftSystem;
            private ITool m_Tool;

            public InternalTarget(CraftSystem craftSystem, ITool tool) : base(2, false, TargetFlags.None)
            {
                m_CraftSystem = craftSystem;
                m_Tool = tool;
            }

            private bool Disassemble(Mobile from, BaseCreature bc)
            {
                try
                {
                    Type crystalType = ScriptCompiler.FindTypeByName(bc.GetType().Name + "Crystal");
                    if (crystalType != null && crystalType.IsSubclassOf(typeof(BaseNecroCraftCrystal)))
                    {
                        List<Item> resources = new List<Item>();
                        BaseNecroCraftCrystal crystal = (BaseNecroCraftCrystal)Activator.CreateInstance(crystalType);
                        resources.Add(crystal);

                        foreach (Type bodyPartType in crystal.RequiredBodyParts)
                        {
                            Item bodyPart = (Item)Activator.CreateInstance(bodyPartType);
                            resources.Add(bodyPart);
                        }

                        foreach (Item resource in resources)
                        {
                            if (!from.AddToBackpack(resource))
                            {
                                from.SendMessage("Jeden z materiałów nie zmieścił się do plecaka");
                                Console.WriteLine(DateTime.Now + " Upuszczam przedmiot " + resource.Name + " " + from.Name);
                            }
                        }

                        bc.Delete();

                        from.PlaySound(0x5A9);
                        return true;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return false;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                int num = m_CraftSystem.CanCraft(from, m_Tool, null);

                if (num > 0)
                {
                    from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, num));
                }
                else
                {
                    bool success = false;

                    if (targeted is BaseCreature)
                    {
                        BaseCreature bc = (BaseCreature)targeted;
                        if (bc.Allured && bc.ControlMaster == from && bc.Hits == bc.HitsMax)
                        {
                            success = Disassemble(from, (BaseCreature)targeted);
                        }
                    }

                    if (success)
                        from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, "Rozkladasz przywolanca na elementy"));
                    else
                        from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, "Nie mozesz tego rozlozyc"));
                }
            }
        }
    }
}
