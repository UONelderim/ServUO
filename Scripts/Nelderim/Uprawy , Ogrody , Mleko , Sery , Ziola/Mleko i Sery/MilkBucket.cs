/********************************************************************************************
**Lait et fromage Crystal/Sna/Cooldev 2003 (laitage.cs,laitage_items.cs et fromage.cs)     **
**le script comprend 1 seau pour traire vache , brebis , chevre . 3 bouteilles de laits    **
**et 3 moule plein de fromages (je prefere les bouteilles que les pichets c'est stackable) **
**               http://invisionfree.com/forums/Hyel_dev/index.php                         **
********************************************************************************************/

// google translates
/**********************************************************************************************
** Milk and Cheese by Crystal/Sna/Cooldev 2003 (laitage.cs, laitage_items.cs and fromage.cs) **
** script includes/understands 1 bucket to milk cow, ewe, goat.  3 milk bottles              **
** and 3 mould full of cheeses (I prefere bottles who the jugs it is stackable)              **
*                   http://invisionfree.com/forums/Hyel_dev/index.php                        **
**********************************************************************************************/

#region References

using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

#endregion

namespace Server.Items

{
	public class MilkBucket : Item
	{
		public int Laitage; //quantité de lait de base du seau
		public int Bestiole; // 1-brebis 2-chevre 3-vache

		[CommandProperty(AccessLevel.GameMaster)]
		public int m_laitage
		{
			get { return Laitage; }
			set { Laitage = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int m_bestiole
		{
			get { return Bestiole; }
			set { Bestiole = value; }
		}

		[Constructable]
		public MilkBucket() : base(0x0FFA)
		{
			Weight = 10.0;
			Name = "Pojemnik Na Mleko: (Pusty)";
			Hue = 1001; // added by Alari - makes the item more distinctive. (also I use buckets for wells and drinking well water. =)
		}

		public override void OnSingleClick(Mobile from)
		{
			base.OnSingleClick(from);
			this.LabelTo(from, Laitage.ToString());
		}


		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(this.GetWorldLocation(), 1))
			{
				from.LocalOverheadMessage(MessageType.Regular, 906, 1019045); // I can't reach that.
			}
			else
			{
				from.Target = new OnVaVoirLesVaches(this);
				from.SendMessage(0x96D, "Na czym chcesz tego uzyc?");
			}
		}

		public MilkBucket(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
			writer.Write(Laitage);
			writer.Write(Bestiole);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			switch (version)
			{
				case 0:
				{
					Laitage = reader.ReadInt();
					Bestiole = reader.ReadInt();
					break;
				}
			}
		}
	}

	public class OnVaVoirLesVaches : Target
	{
		private readonly MilkBucket m_varsaut;
		Mobile m_mobile;

		public OnVaVoirLesVaches(MilkBucket m_saut) : base(2, false, TargetFlags.None)
		{
			m_varsaut = m_saut;
		}
		/* le target aura 2 effets , si c'est un animal a traire , il extrait du lait en echange de 2 point de Bouffe
		* si c'est une bouteille il extrait 1L du seau et fait une bouteille pleine , chaque lait a une propriété soif
		* allant de 1 a 3 pour le lait de chevre , on pourra transformez cela en faim avec les fromages */

		// google translates
		/* the target will have 2 effects, if it is an animal has to milk, it extracts from milk in exchange of 2 point from Puffs out if it is a bottle it extracts 1L from the bucket and makes a full bottle, each milk has a property thirst going for 1 A 3 for the goat's milk, one will be able transform that into hunger with cheeses */


		protected override void OnTarget(Mobile from, object o)
		{
			Container pack = from.Backpack;
			if (o is Mobile)
				m_mobile = (Mobile)o;

			if ((m_varsaut.m_laitage == 0) && (m_varsaut.m_bestiole == 0) && m_mobile != null)
			{
				if (m_mobile is Sheep)
				{
					m_varsaut.m_bestiole = 1;
					//m_varsaut.Name ="Pojemnik Na Mleko: " + m_varsaut.m_laitage.ToString() + "/50 litrow owczego mleka";
					//from.SendMessage (0x84B,"Pojemnik jest gotowy na owcze mleko.");
					++m_varsaut.m_laitage;
					m_mobile.Stam = m_mobile.Stam - 3;
					from.PlaySound(0X4D1);
					m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow owczego mleka";
					from.SendMessage(0x96D, "Zebrales 1 litr owczego mleka.");
				}
				else if (m_mobile is Goat)
				{
					m_varsaut.m_bestiole = 2;
					//m_varsaut.Name ="Pojemnik Na Mleko: " + m_varsaut.m_laitage.ToString() + "/50 litrow koziego mleka";
					//from.SendMessage (0x84B,"Pojemnik jest gotowy na kozie mleko.");
					++m_varsaut.m_laitage;
					m_mobile.Stam = m_mobile.Stam - 3;
					from.PlaySound(0X4D1);
					m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow koziego mleka.";
					from.SendMessage(0x96D, "Zebrales 1 litr koziego mleka.");
				}
				else if (m_mobile is Cow)
				{
					m_varsaut.m_bestiole = 3;
					//m_varsaut.Name ="Pojemnik Na Mleko: " + m_varsaut.m_laitage.ToString() + "/50 litrow krowiego mleka";
					//from.SendMessage (0x84B,"Pojemnik jest gotow na korowie mleko.");
					++m_varsaut.m_laitage;
					m_mobile.Stam = m_mobile.Stam - 3;
					from.PlaySound(0X4D1);
					m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow krowiego mleka.";
					from.SendMessage(0x96D, "Zebrales 1 litr krowiego mleka.");
				}
				else
				{
					from.SendMessage(0x96D, "Mozesz wydoic tylko krowy, kozy i owce!");
					from.CloseGump(typeof(LaitageHelpGump));
					from.SendGump(new LaitageHelpGump(from));
				}
			}
			else if (m_mobile != null && (m_varsaut.m_laitage <= 49))
			{
				if (m_mobile.Stam > 3)
				{
					if ((m_mobile is Cow) && (m_varsaut.m_bestiole == 3))
					{
						++m_varsaut.m_laitage;
						m_mobile.Stam = m_mobile.Stam - 3;
						from.PlaySound(0X4D1);
						m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow krowiego mleka.";
						from.SendMessage(0x96D, "Zebrales 1 litr krowiego mleka.");
					}
					else if ((m_mobile is Goat) && (m_varsaut.m_bestiole == 2))
					{
						++m_varsaut.m_laitage;
						m_mobile.Stam = m_mobile.Stam - 3;
						from.PlaySound(0X4D1);
						m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow koziego mleka.";
						from.SendMessage(0x96D, "Zebrales 1 litr koziego mleka.");
					}
					else if ((m_mobile is Sheep) && (m_varsaut.m_bestiole == 1))
					{
						++m_varsaut.m_laitage;
						m_mobile.Stam = m_mobile.Stam - 3;
						from.PlaySound(0X4D1);
						m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow owczego mleka.";
						from.SendMessage(0x96D, "Zebrales 1 litr owczego mleka.");
					}
					else
					{
						from.SendMessage(0x84B, "Nie mzoesz tego wydoic!");
						from.CloseGump(typeof(LaitageHelpGump));
						from.SendGump(new LaitageHelpGump(from));
					}
				}
				else
				{
					from.SendMessage(0x84B, "To zwierze jest zbyt zmeczone by dac ci wiecej mleka!");
				}
			}
			else if ((o is Bottle) && (m_varsaut.m_laitage > 0) && pack.ConsumeTotal(typeof(Bottle), 1))
			{
				if (m_varsaut.m_bestiole == 3)
				{
					m_varsaut.m_laitage = m_varsaut.m_laitage - 1;
					from.SendMessage(0x96D, "Napelniles butelke krowim mlekiem.");
					m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow krowiego mleka";
					from.PlaySound(0X240);
					from.AddToBackpack(new BottleCowMilk());
				}
				else if (m_varsaut.m_bestiole == 2)
				{
					m_varsaut.m_laitage = m_varsaut.m_laitage - 1;
					from.SendMessage(0x96D, "Napelniles butelke kozim mlekiem.");
					from.PlaySound(0X240);
					m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow koziego mleka.";
					from.AddToBackpack(new BottleGoatMilk());
				}
				else if (m_varsaut.m_bestiole == 1)
				{
					m_varsaut.m_laitage = m_varsaut.m_laitage - 1;
					from.SendMessage(0x96D, "Napelniles butelke owczym mlekiem.");
					m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow owczego mleka.";
					from.PlaySound(0X240);
					from.AddToBackpack(new BottleSheepMilk());
				}
				else
				{
					from.SendMessage(0x84B, "To nie jest butelka lub pojemnik na mleko jest pusty.");
					from.CloseGump(typeof(LaitageHelpGump));
					from.SendGump(new LaitageHelpGump(from));
				}

				if (m_varsaut.m_laitage <= 0)
				{
					m_varsaut.m_bestiole = 0;
					m_varsaut.Name = "Pojemnik Na Mleko: (Pusty)";
				}
			}
			else if ((o is BaseBeverage)) // added by Alari
			{
				BaseBeverage p = (BaseBeverage)o;

				if ((m_varsaut.m_laitage >= p.MaxQuantity) && (p.Quantity == 0))
				{
					if (m_varsaut.m_bestiole == 3)
					{
						p.Content = BeverageType.Milk;
						p.Quantity = p.MaxQuantity;
						m_varsaut.m_laitage = m_varsaut.m_laitage - p.MaxQuantity;
						from.SendMessage(0x96D, "Napelniles pojemnik krowim mlekiem.");
						m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow krowiego mleka";
						from.PlaySound(0X240);
					}
					else if (m_varsaut.m_bestiole == 2)
					{
						p.Content = BeverageType.Milk;
						p.Quantity = p.MaxQuantity;
						m_varsaut.m_laitage = m_varsaut.m_laitage - p.MaxQuantity;
						from.SendMessage(0x96D, "Napelniles pojemnik kozim mlekiem.");
						from.PlaySound(0X240);
						m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow koziego mleka.";
					}
					else if (m_varsaut.m_bestiole == 1)
					{
						p.Content = BeverageType.Milk;
						p.Quantity = p.MaxQuantity;
						m_varsaut.m_laitage = m_varsaut.m_laitage - p.MaxQuantity;
						from.SendMessage(0x96D, "Napelniles pojemnik owczym mlekiem.");
						m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow owczego mleka.";
						from.PlaySound(0X240);
					}
					else
					{
						from.SendMessage(0x84B, "To nie jest pojemnik z mlekiem!.");
						from.CloseGump(typeof(LaitageHelpGump));
						from.SendGump(new LaitageHelpGump(from));
					}
				}

				if (m_varsaut.m_laitage <= 0)
				{
					m_varsaut.m_laitage = 0;
					m_varsaut.m_bestiole = 0;
					m_varsaut.Name = "Pojemnik Na Mleko: (Pusty)";
				}
			}

			else if (o is CheeseForm) // added by Alari
			{
				CheeseForm m_MouleVar = (CheeseForm)o;

				if (m_varsaut.m_laitage >= 15 && m_MouleVar.m_MoulePlein == false)
				{
					m_varsaut.m_laitage = m_varsaut.m_laitage - 15;

					if (m_varsaut.m_bestiole == 1)
					{
						m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow owczego mleka";
						m_MouleVar.m_FromAfaire = 1;
						m_MouleVar.Name = "Forma na ser: owczy ser";
						m_MouleVar.m_MoulePlein = true;
					}
					else if (m_varsaut.m_bestiole == 2)
					{
						m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow koziego mleka.";
						m_MouleVar.m_FromAfaire = 2;
						m_MouleVar.Name = "Formana na ser: kozi ser";
						m_MouleVar.m_MoulePlein = true;
					}
					else if (m_varsaut.m_bestiole == 3)
					{
						m_varsaut.Name = "Pojemnik Na Mleko: " + m_varsaut.m_laitage + "/50 litrow krowiego mleka.";
						m_MouleVar.m_FromAfaire = 3;
						m_MouleVar.Name = "Forma na ser: krowi ser";
						m_MouleVar.m_MoulePlein = true;
					}
					else
					{
						from.SendMessage(0x84C, "Ten pojemnik na mleko jest zly.");
						from.CloseGump(typeof(CheeseFormHelpGump));
						from.SendGump(new CheeseFormHelpGump(from));
					}

					if (m_varsaut.Laitage <= 0)
					{
						m_varsaut.m_bestiole = 0;
						m_varsaut.Name = "Pojemnik Na Mleko: (Pusty)";
					}
				}
				else
				{
					from.SendMessage(0x84C, "Ten pojemnik na mleko nie zawiera wystarczajacej ilosci mleka.");
					from.CloseGump(typeof(CheeseFormHelpGump));
					from.SendGump(new CheeseFormHelpGump(from));
				}
			}

			else
			{
				from.CloseGump(typeof(LaitageHelpGump));
				from.SendGump(new LaitageHelpGump(from));
			}
		}
	}
}
