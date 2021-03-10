using Server.Gumps;
using Server.Helpers;
using Server.Mobiles;
using System;
using System.Collections;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
    public class ShrunkPet : Item
    {
        private static Cliloc _cliloc = new Cliloc(1070040);

        private static int[] _cost = { 50, 200, 800, 2000, 5000 };

        [CommandProperty(AccessLevel.Counselor)]
        public virtual bool RequiresAnimalTrainer
        {
            get { return true; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PetHue
        {
            get
            {
                if (Pet == null)
                    return 0;
                return Pet.Hue;
            }
            set
            {
                if (Pet != null)
                    Pet.Hue = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string PetName
        {
            get
            {
                if (Pet == null)
                    return "";
                return Pet.Name;
            }
            set
            {
                if (Pet != null)
                    Pet.Name = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string PetLabel1
        {
            get
            {
                if (Pet == null)
                    return "";
                return Pet.Label1;
            }
            set
            {
                if (Pet != null)
                    Pet.Label1 = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string PetLabel2
        {
            get
            {
                if (Pet == null)
                    return "";
                return Pet.Label2;
            }
            set
            {
                if (Pet != null)
                    Pet.Label2 = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string PetLabel3
        {
            get
            {
                if (Pet == null)
                    return "";
                return Pet.Label3;
            }
            set
            {
                if (Pet != null)
                    Pet.Label3 = value;
            }
        }

        // Przydatne tylko w celach debug, w razie gdyby zwierze powiazane ze statuetka zniknelo przechowujemy jego Serial osobno.
        private Serial m_LastSerial;
        [CommandProperty(AccessLevel.GameMaster)]
        public int LastSerial
        {
            get { return m_LastSerial; }
        }

        // Aby odroznic statuetki stworzone przed naprawa bledu ze znikajacymi zwierzakami:
        private bool m_Deprecated;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Deprecated
        {
            get { return m_Deprecated; }
        }

        private BaseCreature m_Pet;
        [CommandProperty(AccessLevel.GameMaster)]
        public BaseCreature Pet
        {
            get { return m_Pet; }
            /*
            set
            {
                if (m_Pet == value)
                    return;

                if (m_Pet != null)
                    m_Pet.Delete();
             
                m_LastSerial = value.Serial;
                m_Pet = value;
            }
            */
        }

        public static void Shrink( BaseVendor trainer, PlayerMobile from, BaseCreature target, bool confirmed = false )
        {
			// <workaround na bug z miniatorka nie majaca polaczenia z petem>
			//from.SendMessage("System zminiaturyzowanych zwierzat chwilowo wylaczony, przepraszamy.");
			//return;
			// </workaround>
            if (from == null)
                return;
            
            _cliloc.To = from;

            if (target == null || target.ControlSlots > 5 || 1 > target.ControlSlots)
                _cliloc[1].Send(); // Nie mozesz tego uwiazac.
            else if (target.ControlMaster != from)
                _cliloc[2].Send(); // Musisz kontrolowac istote ktora chcesz uwiazac
            else if (target.Summoned == true)
                _cliloc[3].Send(); // Nie mozesz uwiazac przywolanca.
            else if (target.Combatant != null && target.InRange(target.Combatant, 20))
                _cliloc[4].Send(); // Nie mozesz przywiazac zwierzecia ktore walczy
            else if (target.Hits < target.HitsMax || target.Poisoned)
                _cliloc[5].Send(); // Nie mozesz przywiaza rannego zwierzecia

            else if ((target is PackLlama || target is PackHorse || target is Beetle)
                  && target.Backpack != null && target.Backpack.Items.Count > 0)
                _cliloc[6].Send(); // Wpierw musisz rozladowac jego juki
            else if (!target.IsNearBy(trainer, 5))
                _cliloc[9].Send(); // Jestes zbyt daleko od tresera zwierzat
            else
            {

                int cost = _cost[target.ControlSlots - 1];

                if (from.TotalGold < cost)
                    _cliloc[7].FillWith(cost).Send(); // Nie stac Cie, potrzebujesz {0} zlota
                else
                {
                    if (confirmed)
                    {
                        if (from.Backpack.ConsumeTotal(typeof(Gold), cost))
                        {
                            from.Backpack.AddItem(new ShrunkPet(target));
                            target.AIObject.DoOrderRelease();
                            target.Map = Map.Internal;
                            target.Blessed = true;

                            _cliloc[8].Send();
                        }
                        else
                            _cliloc[7].FillWith(cost).Send(); // Nie stac Cie, potrzebujesz {0} zlota
                    }
                    else
                    {
                        var g = new GeneralConfirmGump();
                        g.Text = String.Format("Usluga bedzie Cie kosztowac {0} centarow.<br /> Po pomniejszeniu zwierze straci pamiec. Czy jestes pewien ze chcesz to zrobic?", cost);
                        g.Size = new Point2D(300, 160);
                        g.OnContinue += (ns, ri) => Shrink(trainer, from, target, true);
                        from.SendGump(g);
                    }
                }
            }
        }

        public void AttachPet( BaseCreature newPet )
        {
            if (m_Pet != null)
            {
                m_Pet.Delete();
                m_Pet = null;
                m_LastSerial = Serial.Zero;
            }

            if (newPet != null)
            {
                m_Pet = newPet;
                m_LastSerial = newPet.Serial;
                m_Deprecated = false;
                this.Hue = PetHue;
                ItemID = ShrinkTable.Lookup(m_Pet.Body);
            }
        }

        public override void OnDelete()
        {
            if (m_Pet != null)
                m_Pet.Delete();

            base.OnDelete();
        }

        // Separate pet from the statue to avoid deleting pet along with the statue
        // Call this method to avoid deleting pet in ShrunkPet.Delete()
        public void DetachPet()
        {
            m_Pet = null;
        }

        protected ShrunkPet() : base()
        {
            Name = "Statuetka zwierzecia";
        }

        public ShrunkPet( BaseCreature pet ) : base(ShrinkTable.Lookup(pet.Body))
        {
            Name = "Statuetka zwierzecia";
            LootType = Server.LootType.Blessed;

            AttachPet(pet);
        }

        public virtual void OnAfterUnshrink()
        {            
        }

        public override void OnDoubleClick( Mobile from )
        {
            if (Pet == null || Pet.Deleted)
			{
                // Na wszelki wypadek.
                // Ponadto, statuetki utworzone przed naprawa bledu ze znikajacymi petami pozostaly puste.
				from.SendMessage("Wyglada na to, ze dusza tego zwierzecia zdolala sie uwolnic ze statuetki jakis czas temu.");

				return;
			}

            _cliloc.To = from;

            if(!this.IsChildOf(from.Backpack))
                _cliloc[11].Send(); // Przedmiot musi znajdowac sie w Twoim plecaku
            else if(!Pet.CanBeControlledBy(from))
                _cliloc[12].Send(); // Nie potrafisz zapanowac nad ta istota
            else if(Pet.ControlSlots + from.Followers > from.FollowersMax)
                _cliloc[13].Send(); // Masz zbyt wiele zwierzat pod swoja opieka
            else if(!from.Alive)
                _cliloc[14].Send(); // Nie mozesz tego zrobic gdy jestes duchem
            else
            {
                if(from.IsNearBy(typeof(AnimalTrainer)) || !RequiresAnimalTrainer)
                {
                    m_Pet.SetControlMaster(from);
                    m_Pet.Location = from.Location;
                    m_Pet.Map = from.Map;
                    m_Pet.Blessed = false;

                    OnAfterUnshrink();

                    DetachPet(); // Separate pet from the statue to avoid deleting pet along with the statue
                    this.Delete();

                    _cliloc[15].Send(); // Powiekszyles zwierze
                }
                else
                    _cliloc[9].Send(); // Jestes zbyt daleko od tresera zwierzat
            }
        }

        public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

            if (Deprecated)
                list.Add("(Przestarzaly przedmiot)");

            if (Pet == null || Pet.Deleted)
            {
                // Na wszelki wypadek.
                // Ponadto, statuetki utworzone przed naprawa bledu ze znikajacymi petami pozostaly puste.
                list.Add("(Pusta statuetka)");
                return;
            }

			list.Add( Pet.Name );
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);

            int version = 2;
            writer.Write(version);
            writer.Write(Pet);
            writer.Write(m_Deprecated);
            writer.Write(m_LastSerial.Value);
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Pet = (BaseCreature)reader.ReadMobile();

            m_Deprecated = true;
            if (version >= 2)
            {
                m_Deprecated = reader.ReadBool();
                m_LastSerial = reader.ReadSerial();
            }
        }

        public ShrunkPet( Serial serial )
            : base(serial)
        {

        }
    }
}
