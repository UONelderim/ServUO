namespace Server.Items
{
	public class DeathKnightSkull750 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull750() : base( 750, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Czaszka Rycerza Smierci";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Swiety Kargoth");
            list.Add( 1049644, "Wygnanie");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull750( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new BanishSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull751 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull751() : base( 751, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Czaszka Rycerza Smierci";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Lord Monduiz Dephaar");
            list.Add( 1049644, "Dotyk Demona");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull751( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new DemonicTouchSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull752 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull752() : base( 752, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Czaszka Rycerza Smierci";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Fron z Talas");
            list.Add( 1049644, "Pakt Ze Smiercia");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull752( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new DevilPactSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull753 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull753() : base( 753, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Czaszka Rycerza Smierci";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Ksiaze Myrhal z Thila");
            list.Add( 1049644, "Ponury Zniwiarz");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull753( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new GrimReaperSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull754 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull754() : base( 754, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Czaszka Rycerza Smierci";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Sir Maeril z Tasandory");
            list.Add( 1049644, "Reka Wiedzmy");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull754( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new HagHandSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull755 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull755() : base( 755, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Czaszka Rycerza Smierci";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Sir Farian z Tasandory");
            list.Add( 1049644, "Ogien Piekielny");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull755( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new HellfireSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull756 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull756() : base( 756, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Czaszka Rycerza Smierci";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Halrand Wulfrost z Garlan");
            list.Add( 1049644, "Promien Smierci");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull756( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new LucifersBoltSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull757 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull757() : base( 757, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Czaszka Rycerza Smierci";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Sir Oslan Knarren");
            list.Add( 1049644, "Kula Smierci");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull757( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new OrbOfOrcusSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull758 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull758() : base( 758, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Czaszka Rycerza Smierci";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Lorda Tasandora");
            list.Add( 1049644, "Tarcza Nienawisci");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull758( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new ShieldOfHateSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull759 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull759() : base( 759, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Death Knight Skull";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Lord Thyrian Zlotobrody");
            list.Add( 1049644, "Zniwiarz Dusz");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull759( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new SoulReaperSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull760 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull760() : base( 760, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Czaszka Rycerza Smierci";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Lady Nolens");
            list.Add( 1049644, "Wytrzymalosc Stali");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull760( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new StrengthOfSteelSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull761 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull761() : base( 761, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Czaszka Rycerza Smierci";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Ksiaze Yngvinrill z Podrmoku");
            list.Add( 1049644, "Uderzenie");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull761( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new StrikeSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull762 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull762() : base( 762, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Czaszka Smierci";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Sir Luren");
            list.Add( 1049644, "Skora Sukkuba");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull762( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new SuccubusSkinSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull763 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull763() : base( 763, 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Hue = 1153;
			Name = "Death Knight Skull";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Lord Khayven");
            list.Add( 1049644, "Gniew");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu." );
		}

		public DeathKnightSkull763( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ReplaceWith(new WrathSkull());
		}
	}
	///////////////////////////////////////////////////////////////////////////////////////////////
	public class DeathKnightSkull764 : SpellScroll
	{
		[Constructable]
		public DeathKnightSkull764() : base(764, 0x1AE0)
		{
			ItemID = Utility.RandomList(0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3);
			Hue = 1153;
			Name = "Czaszka Smierci";
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add(1070722, "Soterios Lowca Nekromantow");
			list.Add(1049644, "Slaby Punkt");
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.SendMessage("Ta czaszka nalezy do rycerza, ktory odszedl z tego swiata dawno temu.");
		}

		public DeathKnightSkull764(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			ReplaceWith(new WeakSpotSkull());
		}
	}
}
