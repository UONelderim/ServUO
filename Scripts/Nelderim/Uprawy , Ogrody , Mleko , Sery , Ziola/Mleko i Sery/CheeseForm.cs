/********************************************************************************************
**Lait et fromage Crystal/Sna/Cooldev 2003 (laitage.cs,laitage_items.cs et fromage.cs)     **
**le script comprend 1 seau pour traire vache , sheep , chevre . 3 bouteilles de laits    ** 
**et 3 moule plein de fromages (je prefere les bouteilles que les pichets c'est stackable) **
**               http://invisionfree.com/forums/Hyel_dev/index.php                         **
********************************************************************************************/

#region References

using System;
using Server.Gumps;
using Server.Network;
using Server.Targeting;

#endregion

namespace Server.Items

{
	public class CheeseForm : Item
	{
		public int m_FromBonusSkill; // ajout du bonus de skill cooking .

		[CommandProperty(AccessLevel.GameMaster)]
		public int m_FromAfaire { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int m_FromageQual { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int m_StadeFermentation { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool m_MoulePlein { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool m_Fermentation { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool m_ContientUnFromton { get; set; }

		[Constructable]
		public CheeseForm() : base(0x0E78)
		{
			Weight = 10.0;
			Name = "Forma Na Ser: Pusta";
			Hue = 0x481;
		}

		public override void OnDoubleClick(Mobile from)
		{
			Container pack = from.Backpack;
			m_FromBonusSkill = (m_FromageQual + ((int)(from.Skills[SkillName.Cooking].Value) / 5));
			if (!from.InRange(this.GetWorldLocation(), 2))
			{
				from.LocalOverheadMessage(MessageType.Regular, 906, 1019045); // I can't reach that.
			}
			else
			{
				if ((m_Fermentation == false) && (m_MoulePlein == false) && (m_ContientUnFromton == false))
				{
					from.Target = new OnRempliMouleFromton(this);
					from.SendMessage(0x84C, "Wybierz pojemnik na mleko aby uzupelnic forme.");
				}
				else if (m_MoulePlein && (m_Fermentation == false) && (m_ContientUnFromton == false))
				{
					new FromQuiFermente(this).Start();
					m_Fermentation = true;
					from.SendMessage("Rozpoczeles proces fermetacji.");
					if (from.CheckSkill(SkillName.Cooking, 0, 100))
						m_StadeFermentation = 5;
					else
						m_StadeFermentation = 0;
				}
				else if (m_Fermentation && (m_ContientUnFromton == false) && m_MoulePlein)
				{
					this.PublicOverheadMessage(MessageType.Regular, 1151, false,
						String.Format("Proces fermetacji: " + m_StadeFermentation + " %"));
				}
				else if ((m_Fermentation == false) && m_ContientUnFromton && m_MoulePlein)
				{
					if (m_FromBonusSkill < 10)
					{
						this.PublicOverheadMessage(MessageType.Regular, 1152, false,
							"Fermetacja sie nie udala, mleko przepadlo.");
						m_ContientUnFromton = false;
						m_MoulePlein = false;
						m_FromageQual = 0;
						m_FromAfaire = 0;
						this.Name = "Forma Na Ser: Pusta";
					}
					else if ((m_FromBonusSkill > 95) && Utility.RandomBool()) // magique reward
					{
						// m_FromBonusSkill is random 1-100 + cooking/5
						if (m_FromAfaire == 1)
						{
							from.SendMessage(0x84C, "Otrzymales wspanialy twarog z formy.");
							from.AddToBackpack(new FromageDeBrebisMagic());
							m_ContientUnFromton = false;
							m_MoulePlein = false;
							m_FromageQual = 0;
							m_FromAfaire = 0;
							this.Name = "Forma Na Ser: Pusta";
						}
						else if (m_FromAfaire == 2)
						{
							from.SendMessage(0x84C, "Uzyskales wspanialy Chavignol z formy.");
							from.AddToBackpack(new FromageDeChevreMagic());
							m_ContientUnFromton = false;
							m_MoulePlein = false;
							m_FromageQual = 0;
							m_FromAfaire = 0;
							this.Name = "Forma Na Ser: Pusta";
						}
						else
						{
							from.SendMessage(0x84C, "Uzyskales wspanialy Maroille z formy.");
							from.AddToBackpack(new FromageDeVacheMagic());
							m_ContientUnFromton = false;
							m_MoulePlein = false;
							m_FromageQual = 0;
							m_FromAfaire = 0;
							this.Name = "Forma Na Ser: Pusta";
						}
					}
					else // ((m_FromBonusSkill >= 10 )&& (m_FromBonusSkill < 95 ))
					{
						if (m_FromAfaire == 1)
						{
							from.SendMessage(0x84C, "Uzyskales wspanialy owczy ser z formy.");
							from.AddToBackpack(new FromageDeBrebis());
							m_ContientUnFromton = false;
							m_MoulePlein = false;
							m_FromageQual = 0;
							m_FromAfaire = 0;
							this.Name = "Forma Na Ser: Pusta";
						}
						else if (m_FromAfaire == 2)
						{
							from.SendMessage(0x84C, "Uzyskales wspanialy kozi ser z formy.");
							from.AddToBackpack(new FromageDeChevre());
							m_ContientUnFromton = false;
							m_MoulePlein = false;
							m_FromageQual = 0;
							m_FromAfaire = 0;
							this.Name = "Forma Na Ser: Pusta";
						}
						else
						{
							from.SendMessage(0x84C, "Uzyskales wspanialy krowi ser z formy.");
							from.AddToBackpack(new FromageDeVache());
							m_ContientUnFromton = false;
							m_MoulePlein = false;
							m_FromageQual = 0;
							m_FromAfaire = 0;
							this.Name = "Forma Na Ser: Pusta";
						}
					}
				}
				else
				{
					from.SendMessage(0x84C, "*gasp*!");
				}
			}
		}

		public CheeseForm(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
			writer.Write(m_FromAfaire);
			writer.Write(m_FromageQual);
			writer.Write(m_StadeFermentation);
			writer.Write(m_MoulePlein);
			writer.Write(m_Fermentation);
			writer.Write(m_ContientUnFromton);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			switch (version)
			{
				case 0:
				{
					m_FromAfaire = reader.ReadInt();
					m_FromageQual = reader.ReadInt();
					m_StadeFermentation = reader.ReadInt();
					m_MoulePlein = reader.ReadBool();
					m_Fermentation = reader.ReadBool();
					m_ContientUnFromton = reader.ReadBool();

					if (m_Fermentation)
						new FromQuiFermente(this).Start();

					break;
				}
			}
		}
	}

	public class OnRempliMouleFromton : Target
	{
		private readonly CheeseForm m_MouleVar;
		MilkBucket m_SautFromage;

		public OnRempliMouleFromton(CheeseForm m_CheeseForm) : base(3, false, TargetFlags.None)
		{
			m_MouleVar = m_CheeseForm;
		}

		protected override void OnTarget(Mobile from, object o)
		{
			if (o is MilkBucket)
			{
				m_SautFromage = (MilkBucket)o;
				if (m_SautFromage.Laitage >= 15)
				{
					m_SautFromage.Laitage = m_SautFromage.Laitage - 15;

					if (m_SautFromage.m_bestiole == 1)
					{
						m_SautFromage.Name =
							"Pojemnik na mleko: " + m_SautFromage.Laitage + "/50 litrow owczego mleka.";
						m_MouleVar.m_FromAfaire = 1;
						m_MouleVar.Name = "Forma Na Ser: Owczy Ser";
						m_MouleVar.m_MoulePlein = true;
					}
					else if (m_SautFromage.m_bestiole == 2)
					{
						m_SautFromage.Name =
							"Pojemnik na mleko: " + m_SautFromage.Laitage + "/50 litrow koziego mleka.";
						m_MouleVar.m_FromAfaire = 2;
						m_MouleVar.Name = "Forma Na Ser: Kozi Ser";
						m_MouleVar.m_MoulePlein = true;
					}
					else if (m_SautFromage.m_bestiole == 3)
					{
						m_SautFromage.Name =
							"Pojemnik na mleko: " + m_SautFromage.Laitage + "/50 litrow krowiego mleka.";
						m_MouleVar.m_FromAfaire = 3;
						m_MouleVar.Name = "Forma Na Ser: Krowi Ser";
						m_MouleVar.m_MoulePlein = true;
					}
					else
					{
						from.SendMessage(0x84C, "To nie jest pojemnik na mleko.");
						from.CloseGump(typeof(CheeseFormHelpGump));
						from.SendGump(new CheeseFormHelpGump(from));
					}
				}
				else
				{
					from.SendMessage(0x84C, "Pojemnik nie zawiera wystarczajacej ilosci mleka.");
					from.CloseGump(typeof(CheeseFormHelpGump));
					from.SendGump(new CheeseFormHelpGump(from));
				}

				if (m_SautFromage.Laitage <= 0)
				{
					m_SautFromage.Laitage = 0;
					m_SautFromage.m_bestiole = 0;
					m_SautFromage.Name = "Pojemnik na mleko: (Pusty)";
				}
			}
			else
			{
				from.SendMessage(0x84C, "Urzyj pojemnika na mleko z 15 litrami.");
				from.CloseGump(typeof(CheeseFormHelpGump));
				from.SendGump(new CheeseFormHelpGump(from));
			}
		}
	}
}
