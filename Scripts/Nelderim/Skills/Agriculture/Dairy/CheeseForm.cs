using System;
using Server.Targeting;
using Server.Network;
using Server.Gumps;

namespace Server.Items
{

	public class CheeseForm : Item
	{
		private MilkType _MilkType;
		private int _CheeseQuality;
		private int _FermentationProgress;

		[CommandProperty(AccessLevel.GameMaster)]
		public MilkType MilkType
		{
			get => _MilkType;
			set
			{
				_MilkType = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int CheeseQuality
		{
			get => _CheeseQuality;
			set => _CheeseQuality = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int FermentationProgress
		{
			get => _FermentationProgress;
			set => _FermentationProgress = value;
		}
		

		[Constructable]
		public CheeseForm() : base(0x0E78)
		{
			Weight = 10.0;
			Name = "Forma Na Ser";
			Hue = 0x481;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(GetWorldLocation(), 2))
			{
				from.LocalOverheadMessage(MessageType.Regular, 906, 1019045); // I can't reach that.
			}
			else
			{
				if (MilkType == MilkType.None)
				{
					from.Target = new MilkBucketTarget(this);
					from.SendMessage(0x84C, "Wybierz pojemnik na mleko aby uzupelnic forme.");
				}
				else if (MilkType != MilkType.None && FermentationProgress < 100)
				{
					from.SendMessage(0x84C, $"Proces fermetacji: {_FermentationProgress}%");
				}
				else if (MilkType != MilkType.None && FermentationProgress >= 100)
				{
					var skillBonus = (CheeseQuality + ((int)(from.Skills[SkillName.Cooking].Value) / 5));
					if (skillBonus < 10)
					{
						from.SendMessage(0x84C, "Fermetacja sie nie udala, mleko przepadlo.");
					}
					if ((skillBonus > 95) && Utility.RandomBool())
					{
						if (_MilkType == MilkType.Sheep)
						{
							from.SendMessage(0x84C, "Otrzymales wspanialy twarog z formy.");
							from.AddToBackpack(new FromageDeBrebisMagic());
						}
						else if (_MilkType == MilkType.Goat)
						{
							from.SendMessage(0x84C, "Uzyskales wspanialy Chavignol z formy.");
							from.AddToBackpack(new FromageDeChevreMagic());
						}
						else
						{
							from.SendMessage(0x84C, "Uzyskales wspanialy Maroille z formy.");
							from.AddToBackpack(new FromageDeVacheMagic());
						}
					}
					else
					{
						if (_MilkType == MilkType.Sheep)
						{
							from.SendMessage(0x84C, "Uzyskales wspanialy owczy ser z formy.");
							from.AddToBackpack(new FromageDeBrebis());
						}
						else if (_MilkType == MilkType.Goat)
						{
							from.SendMessage(0x84C, "Uzyskales wspanialy kozi ser z formy.");
							from.AddToBackpack(new FromageDeChevre());
						}
						else
						{
							from.SendMessage(0x84C, "Uzyskales wspanialy krowi ser z formy.");
							from.AddToBackpack(new FromageDeVache());
						}
					}
					FermentationProgress = 0;
					CheeseQuality = 0;
					MilkType = MilkType.None;
				}
				else
				{
					from.SendMessage(0x84C, "*wzdycha*");
				}
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add(MilkType.GetPropertyString());
		}

		public CheeseForm(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); //version
			writer.Write((int)_MilkType);
			writer.Write(_CheeseQuality);
			writer.Write(_FermentationProgress);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			_MilkType = (MilkType)reader.ReadInt();
			_CheeseQuality = reader.ReadInt();
			_FermentationProgress = reader.ReadInt();
			if (version < 1)
			{
				reader.ReadBool(); //IsFull
				reader.ReadBool(); //IsFermenting
				reader.ReadBool(); //IsFermentationDone
			}
			
			if (_MilkType != MilkType.None && _FermentationProgress < 100)
				new FermentationTimer(this).Start();
		}
	}

	public class MilkBucketTarget : Target
	{
		private CheeseForm _CheeseForm;

		public MilkBucketTarget(CheeseForm cheeseForm) : base(3, false, TargetFlags.None)
		{
			_CheeseForm = cheeseForm;
		}

		protected override void OnTarget(Mobile from, object o)
		{
			if (o is MilkBucket milkBucket)
			{
				if (milkBucket.Held >= 15)
				{
					milkBucket.Held -= 15;
					_CheeseForm.MilkType = milkBucket.MilkType;
					if (from.CheckSkill(SkillName.Cooking, 0, 100))
						_CheeseForm.FermentationProgress = 5;
					new FermentationTimer(_CheeseForm).Start();
				}
				else
				{
					from.SendMessage(0x84C, "Pojemnik nie zawiera wystarczajacej ilosci mleka.");
					from.CloseGump(typeof(CheeseFormHelpGump));
					from.SendGump(new CheeseFormHelpGump());
				}
			}
			else
			{
				from.SendMessage(0x84C, "To nie jest pojemnik na mleko.");
				from.CloseGump(typeof(CheeseFormHelpGump));
				from.SendGump(new CheeseFormHelpGump());
			}
		}
	}
	
	public class FermentationTimer : Timer
	{
		private readonly CheeseForm _CheeseForm;

		public FermentationTimer(CheeseForm cheeseForm) :
			base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(72))
		{
			_CheeseForm = cheeseForm;
		}

		protected override void OnTick()
		{
			_CheeseForm.FermentationProgress++;
			if (_CheeseForm.FermentationProgress >= 100)
			{
				_CheeseForm.CheeseQuality = Utility.Random(1, 100);
				Stop();
			}
		}
	}
	
	public class CheeseFormHelpGump : Gump
	{
		public CheeseFormHelpGump() : base(30, 30)
		{
			Closable = true;
			Dragable = true;

			AddPage(0);

			AddBackground(0, 0, 400, 300, 5054);

			Add(new GumpHtml(10, 10, 380, 280,
				"<p><b><u>Mleczny System : Forma Na Ser</u></b></p>" +
				"<p>Pomoc : </p>" +
				"<p>1 - Dwuklik na forme i zaznaczenie pojemnika z mlekiem powoduje rozpoczecie fermetacji.<br>" +
				"Mozesz zrobic ser z krowiego ,koziego i owczego mleka. Potrzebujesz 15 litrow mleka aby zrobic ser.</p>" +
				"<p>2 - Ser jest gotowy do fermetacji.<br>Dwoklik na forme rozpoczyna proces fermetacji.<br>" +
				"Jesli proces sie rozpocza ,jego stan pokazany jest w %.</p>" +
				"<p>3 - Kiedy ser juz jest gotow kliknij na forme aby uzyskac nowy ser.<br>" +
				"Masz szanse na uzyskanie magicznego sera. Masz takze szanse ze nie uda ci sie zrobic zadnego sera. Niewielki skill gotowania pomoze osiagnac ci lepsze efekty.</p>" +
				"<p>Jesli to czytasz to dlatego ze:</p>" +
				"<p>- Forma na ser jest pelna a TY prubujesz ja napelnic.<br>" +
				"- W pojemniku na mleko nie ma wystarczajacej ilosci mleka.<br>" +
				"- Wybrales niedozwolony cel.</p>",
				true, true));
		}
	}
}
