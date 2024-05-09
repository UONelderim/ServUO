using Server.Mobiles;

namespace Server.Items
{
    public class SoulLantern : MagicLantern
    {
	    private int _TrappedSouls;

        [CommandProperty(AccessLevel.GameMaster)]
        public int TrappedSouls
        {
            get => _TrappedSouls;
            set
            {
                _TrappedSouls = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public SoulLantern()
        {
            Name = "Latarnia dusz";
            Hue = 2980;
			Attributes.AttackChance = 10;
			Attributes.DefendChance = 10;
			Attributes.ReflectPhysical = 30;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if (ItemID == 0xA15)
            {
                list.Add(1049644, "Wiezienie dla dusz czystych");
            }
            else
            {
                list.Add(1049644, "Wiezienie dla dusz potepionych");
            }

            if (Owner != null)
            {
                list.Add(1070722, $"Dusze pochwycone przez {Owner.Name}: {_TrappedSouls:n0}");
            }
        }

        public override void OnAdded(IEntity parent)
        {
            if (Owner == null && parent is Item itemParent)
            {
	            if (itemParent.RootParent is PlayerMobile)
	            {
		            Owner = (Mobile)itemParent.RootParent;
	            }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            Item lantern = from.FindItemOnLayer(Layer.TwoHanded);
            if (lantern == this)
            {
                from.AddToBackpack(this);
                ItemID = 0xA18;
                from.PlaySound(0x4BB);
                base.OnRemoved(from);
            }
            else if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else if (Owner == from)
            {
                if (from.FindItemOnLayer(Layer.TwoHanded) != null)
                {
                    from.AddToBackpack(from.FindItemOnLayer(Layer.TwoHanded));
                }

                from.SendMessage("Wkładasz latarnię do lewej ręki.");
                from.AddItem(this);
                ItemID = 0xA15;
                from.PlaySound(0x47);
                base.OnEquip(from);
            }
            else
            {
                from.SendMessage("To nie jest twoja latarnia!");
            }
        }

        public SoulLantern(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int) 1); // version
            writer.Write((Mobile) Owner);
            writer.Write(_TrappedSouls);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Owner = reader.ReadMobile();
            _TrappedSouls = reader.ReadInt();
        }

        public static void OnBeforeDeath(BaseCreature killed)
        {
            //if (SlayerGroup.GetEntryByName(SlayerName.Repond).Slays(killed))
            {
                Mobile deathknight = killed.LastKiller;
                if (deathknight is BaseCreature)
                    deathknight = ((BaseCreature) deathknight).GetMaster();
                
                if (deathknight != null && deathknight is PlayerMobile && killed.TotalGold > 0)
                {
                    Item lantern = deathknight.FindItemOnLayer(Layer.TwoHanded);

                    if (lantern is SoulLantern)
                    {
                        SoulLantern soulLantern = (SoulLantern) lantern;
                        soulLantern._TrappedSouls += killed.TotalGold;
                        if (soulLantern._TrappedSouls > 100000)
                            soulLantern._TrappedSouls = 100000;

                        soulLantern.InvalidateProperties();

                        Item deathpack = killed.FindItemOnLayer(Layer.Backpack);
                        if (deathpack != null)
                        {
                            Item gold = killed.Backpack.FindItemByType(typeof(Gold));
                            gold.Delete();
                            deathknight.SendMessage("Odebrano duszę.");
                            Effects.SendLocationParticles(
                                EffectItem.Create(deathknight.Location, deathknight.Map,
                                    EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                            Effects.PlaySound(deathknight.Location, deathknight.Map, 0x1ED);
                        }
                    }
                }
            }
        }
    }
}
