using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Items
{
    public class DeathKnightLantern : BaseShield
    {
	    private int _TrappedSouls;

	    public static void Initialize()
	    {
		    EventSink.CreatureDeath += CreatureDeath;
	    }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TrappedSouls
        {
            get => _TrappedSouls;
            set
            {
                _TrappedSouls = value;
                if (_TrappedSouls > 100000)
	                _TrappedSouls = 100000;
                InvalidateProperties();
            }
        }

        [Constructable]
        public DeathKnightLantern() : base(0xA18)
        {
            Name = "Latarnia dusz";
            Hue = 2980;
            Light = LightType.Circle300;
            Weight = 2.0;
            
			Attributes.AttackChance = 10;
			Attributes.DefendChance = 10;
			Attributes.ReflectPhysical = 30;
			Attributes.SpellChanneling = 1;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1049644, "Wiezienie dla dusz");

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
        
        public override bool OnEquip(Mobile from)
        {
	        ItemID = 0xA15;
	        return base.OnEquip(from);
        }

        public override void OnRemoved(IEntity parent)
        {
	        ItemID = 0xA18;
	        base.OnRemoved(parent);
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

        public DeathKnightLantern(Serial serial) : base(serial)
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

        public static void CreatureDeath(CreatureDeathEventArgs e)
        {
	        if(e.Creature is BaseCreature bc && e.Corpse != null)
            {
	            //if (SlayerGroup.GetEntryByName(SlayerName.Repond).Slays(killed))
                var deathknight = e.Killer;
                if (deathknight is BaseCreature killerBc)
                    deathknight = killerBc.GetMaster();
                
                if (deathknight is PlayerMobile && e.Corpse.TotalGold > 0)
                {
                    if (deathknight.FindItemOnLayer(Layer.TwoHanded) is DeathKnightLantern soulLantern) 
                    {
                        var soulsToAdd = e.Corpse.TotalGold;
                        soulLantern.TrappedSouls += soulsToAdd;
                        
                        foreach (var gold in e.Corpse.FindItemsByType<Gold>())
                        {
	                        gold.Delete();
                        }
                        deathknight.SendMessage("Odebrano duszę.");
                        Effects.SendLocationParticles(
                            EffectItem.Create(deathknight.Location, deathknight.Map,
                                EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                        Effects.PlaySound(deathknight.Location, deathknight.Map, 0x1ED);
                        
                        BaseQuest quest = QuestHelper.GetQuest((PlayerMobile)deathknight, typeof(DeathKnightPhase2Quest));
                        if (quest != null)
                        {
                            foreach (var objective in quest.Objectives)
	                            objective.Update(soulsToAdd);
                        }
                    }
                }
            }
        }
    }
}
