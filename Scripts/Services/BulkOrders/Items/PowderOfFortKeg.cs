using System;

namespace Server.Items
{
    public class PowderOfFortKeg : Item
    {
        private int _Charges;
        private Type _CurrentPowderType;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Charges 
        { 
            get { return _Charges; } 
            set { _Charges = value; InvalidateProperties(); } 
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Type CurrentPowderType 
        { 
            get { return _CurrentPowderType; } 
            set { _CurrentPowderType = value; InvalidateProperties(); } 
        }

        public override int LabelNumber => 1157221;  // A specially lined keg for powder of fortification.

        [Constructable]
        public PowderOfFortKeg() : this(0, null) { }

        [Constructable]
        public PowderOfFortKeg(int charges, Type powderType) : base(0x1940)
        {
            _Charges = charges;
            _CurrentPowderType = powderType;
            Hue = 2419;
            Weight = 15.0;
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is SpecializedPowderOfTemperament powder)
            {
                if (_CurrentPowderType == null)
                {
                    _CurrentPowderType = powder.GetType();
                }
                else if (_CurrentPowderType != powder.GetType())
                {
                    from.SendMessage("Ten keg zawiera juz jeden rodzaj proszkow wzocnienia. Aby wlozyc tu inny rodzaj proszkow, musisz go oproznic.");
                    return false;
                }

                if (_Charges < 250)
                {
                    if (powder.UsesRemaining + _Charges > 250)
                    {
                        int add = 250 - _Charges;
                        powder.UsesRemaining -= add;
                        _Charges = 250;
                    }
                    else
                    {
                        _Charges += powder.UsesRemaining;
                        powder.Delete();
                    }

                    from.PlaySound(0x247);
                    InvalidateProperties();
                    return true;
                }
                else
                {
	                from.SendLocalizedMessage(502233); // The keg will not hold any more!
                }
            }
            from.SendLocalizedMessage(502232); // The keg is not designed to hold that type of object.
            return false;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Backpack != null && IsChildOf(from.Backpack))
            {
                if (_Charges > 0 && _CurrentPowderType != null)
                {
                    var powder = (SpecializedPowderOfTemperament)Activator.CreateInstance(_CurrentPowderType);

                    if (powder != null && from.Backpack.TryDropItem(from, powder, false))
                    {
                        _Charges--;
                        from.PlaySound(0x247);
                        from.SendMessage($"Wyciagasz 1 {_CurrentPowderType.Name} z kega.");

                        if (_Charges == 0)
                        {
                            _CurrentPowderType = null;
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(1080016); // That container cannot hold more weight.
                        powder?.Delete();
                    }

                    InvalidateProperties();
                }
                else
                {
	                from.SendLocalizedMessage(502246); // The keg is empty.
                }
            }
            else
            {
	            from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (_CurrentPowderType != null)
            {
                list.Add($"{_CurrentPowderType.Name}: {_Charges}");
            }
            else
            {
                list.Add("*keg nalezy wypelnic jednym z rodzajow proszkow wzmocnienia*");
            }

            int number = GetFullnessNumber(_Charges);
            list.Add(number);
        }

        private int GetFullnessNumber(int charges)
        {
            int percentage = (int)((double)charges / 250 * 100);

            if (percentage <= 0) return 502246; // The keg is empty.
            if (percentage < 5) return 502248; // The keg is nearly empty.
            if (percentage < 20) return 502249; // The keg is not very full.
            if (percentage < 30) return 502250; // The keg is about one quarter full.
            if (percentage < 40) return 502251; // The keg is about one third full.
            if (percentage < 47) return 502252; // The keg is almost half full.
            if (percentage < 54) return 502254; // The keg is approximately half full.
            if (percentage < 70) return 502253; // The keg is more than half full.
            if (percentage < 80) return 502255; // The keg is about three quarters full.
            if (percentage < 90) return 502256; // The keg is very full.
            if (percentage < 100) return 502257; // The liquid is almost to the top of the keg.
            return 502258; // The keg is completely full.
        }

        public PowderOfFortKeg(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(2); // version

            writer.WriteObjectType(_CurrentPowderType);
            writer.Write(_Charges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 2)
            {
	            _CurrentPowderType = reader.ReadObjectType();
            }
            if (version >= 1)
            {
	            _Charges = reader.ReadInt();
            }
            if (version < 2 && _Charges > 0)
            {
	            _CurrentPowderType = typeof(BlacksmithyPowderOfTemperament);
            }

            if (version == 0)
                ItemID = 0x1940;
        }
    }
}
