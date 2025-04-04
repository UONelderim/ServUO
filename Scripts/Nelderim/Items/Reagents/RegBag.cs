using Server;
using Server.Items;
using System;
using Server.ACC.CSS;
using Server.Engines.Craft;

namespace Items.RegBag
{
    public class RegBag : Container, ICraftable
    {
       private int m_WeightReduction = 50;
       private bool m_IsBeingLoaded = false;

       [CommandProperty(AccessLevel.GameMaster)]
       public int Reduction
       {
          get { return m_WeightReduction; }
          set
          {
             if (value < 0)
                m_WeightReduction = 0;
             else if (value >= 100)
                m_WeightReduction = 100;
             else
                m_WeightReduction = value;
             InvalidateProperties();
          }
       }

       private static int m_Capacity = int.MaxValue;
       public int Capacity
       {
          get { return m_Capacity; }
          set { m_Capacity = value; InvalidateProperties(); }
       }

       [Constructable]
       public RegBag() : base(0xE76)
       {
          Name = "worek na reagenty";
          Weight = 1;
       }

       public RegBag(Serial serial) : base(serial)
       {
       }

       public override void GetProperties(ObjectPropertyList list)
       {
          base.GetProperties(list);

          if (m_WeightReduction != 0)
             list.Add(1072210, m_WeightReduction.ToString()); // Weight reduction: ~1_PERCENTAGE~%  
       }
       public override void UpdateTotal(Item sender, TotalType type, int delta)
       {
          InvalidateProperties();

          base.UpdateTotal(sender, type, delta);
       }

       public override int GetTotal(TotalType type)
       {
          int total = base.GetTotal(type);

          if (type == TotalType.Weight)
             total -= total * m_WeightReduction / 100;

          return total;
       }

       private static Type[] m_Ammo = new Type[]
       {
          typeof( Arrow ), typeof( Bolt )
       };

       public bool CheckType(Item item)
       {
          return item is BaseReagent || item is Kindling || item is CReagent || item is BaseTobacco;
       }

       public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, bool checkWeight, int plusItems, int plusWeight)
       {
          // Skip type validation during world load/deserialization
          if (m_IsBeingLoaded)
             return base.CheckHold(m, item, message, checkItems, checkWeight, plusItems, plusWeight);

          if (!CheckType(item))
          {
             if (message)
                m.SendLocalizedMessage(1074836); // The container can not hold that type of object.

             return false;
          }

          return base.CheckHold(m, item, message, checkItems, checkWeight, plusItems, plusWeight);
       }

       public override void AddItem(Item dropped)
       {
          base.AddItem(dropped);

          InvalidateWeight();
       }

       public override void RemoveItem(Item dropped)
       {
          base.RemoveItem(dropped);

          InvalidateWeight();
       }

       public override void Serialize(GenericWriter writer)
       {
          base.Serialize(writer);
          writer.Write((int)2); // version 

          writer.Write(m_WeightReduction);
       }

       public override void Deserialize(GenericReader reader)
       {
          // Set flag to bypass type checking during deserialization
          m_IsBeingLoaded = true;
          
          base.Deserialize(reader);

          int version;

          int num = reader.ReadInt();
          if (num == 0)
             version = reader.ReadInt();    // num is from BaseContainer serialization (deprecated parent of RegBag)
          else
             version = num; // num is from RegBag serialization

          if (version == 1)
          {
             double reduction = reader.ReadDouble();

             m_WeightReduction = (int)(reduction * 100);
          }

          if (version >= 2)
             m_WeightReduction = reader.ReadInt();
             
          // Reset flag after deserialization is complete
          // Use CallPriority to ensure this runs after all contents are restored
          Timer.DelayCall(TimeSpan.Zero, () => m_IsBeingLoaded = false);
       }

       public void InvalidateWeight()
       {
          if (RootParent is Mobile)
          {
             Mobile m = (Mobile)RootParent;

             m.UpdateTotals();
          }
       }

       #region ICraftable
       public int OnCraft(int quality,
          bool makersMark,
          Mobile from,
          CraftSystem craftSystem,
          Type typeRes,
          ITool tool,
          CraftItem craftItem,
          int resHue)
       {
          return quality;
       }
       #endregion
    }
}
