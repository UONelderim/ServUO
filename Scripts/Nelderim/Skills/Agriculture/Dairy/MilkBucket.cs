using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class MilkBucket : Item
	{
		private int _Held; 
		private MilkType _MilkType; 

		[CommandProperty(AccessLevel.GameMaster)]
		public int Held
		{
			get => _Held;
			set
			{
				_Held = Math.Max(0, value);
				if (_Held == 0)
				{
					_MilkType = MilkType.None;
				}
				InvalidateProperties();
			}
		}

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

		[Constructable]
		public MilkBucket() : base(0x0FFA)
		{
			Weight = 10.0;
			Name = "Pojemnik Na Mleko";
			Hue = 1001;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add(_MilkType.GetPropertyString());
			if(Held > 0)
				list.Add($"{_Held}/50 litrów");
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(this.GetWorldLocation(), 1))
			{
				from.LocalOverheadMessage(MessageType.Regular, 906, 1019045); // I can't reach that.
			}
			else
			{
				from.Target = new CowTarget(this);
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
			writer.Write(_Held);
			writer.Write((int)_MilkType);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			_Held = reader.ReadInt();
			_MilkType = (MilkType)reader.ReadInt();
		}
	}

	public class CowTarget(MilkBucket milkBucket) : Target(2, false, TargetFlags.None)
	{
		protected override void OnTarget(Mobile from, object o)
		{
			if (o is Mobile m)
			{
				if (milkBucket.Held == 0)
				{
					milkBucket.MilkType = m switch
					{
						Sheep => MilkType.Sheep,
						Goat => MilkType.Goat,
						Cow => MilkType.Cow,
						_ => MilkType.None
					};
				}
				if (milkBucket.MilkType == MilkType.None)
				{
						from.SendMessage(0x96D, "Nie mozesz tego wydoic!");
						from.CloseGump(typeof(MilkBucketHelpGump));
						from.SendGump(new MilkBucketHelpGump());
				}
				else if (milkBucket.Held >= 50)
				{
					from.SendMessage(0x84B, "Pojemnik jest pelny!");
				}
				else
				{
					if (m.Stam <= 3)
					{
						from.SendMessage(0x84B, "To zwierze jest zbyt zmeczone by dac ci wiecej mleka!");
					}
					else
					{
						if (((m is Sheep) && (milkBucket.MilkType == MilkType.Sheep)) ||
						    ((m is Goat) && (milkBucket.MilkType == MilkType.Goat)) ||
						    ((m is Cow) && (milkBucket.MilkType == MilkType.Cow)))
						{
							++milkBucket.Held;
							m.Stam -= 3;
							from.PlaySound(0X4D1);
							from.SendMessage(0x96D, "Zebrales litr mleka.");
						}
						else
						{
							from.SendMessage(0x84B, "Nie mozesz tego wydoic!");
							from.CloseGump(typeof(MilkBucketHelpGump));
							from.SendGump(new MilkBucketHelpGump());
						}
					}
				}
			}
			else if ((o is Bottle) && milkBucket.MilkType != MilkType.None && milkBucket.Held > 0 && from.Backpack.ConsumeTotal(typeof(Bottle), 1))
			{
				milkBucket.Held--;
				Item newBottle = milkBucket.MilkType switch
				{
					MilkType.Cow => new BottleCowMilk(),
					MilkType.Goat => new BottleGoatMilk(),
					MilkType.Sheep => new BottleSheepMilk(),
					_ => null
				};
				from.SendMessage(0x96D, "Napelniles butelke mlekiem.");
				from.PlaySound(0X240);
				from.AddToBackpack(newBottle);
			}
			else if (o is BaseBeverage beverage)
			{
				if (milkBucket.Held >= beverage.MaxQuantity && beverage.Quantity == 0)
				{
					beverage.Content = BeverageType.Milk;
					beverage.Quantity = beverage.MaxQuantity;
					milkBucket.Held -= beverage.MaxQuantity;
					from.SendMessage(0x96D, "Napelniles pojemnik mlekiem.");
					from.PlaySound(0X240);
				}
			}
			else if (o is CheeseForm cheeseForm)
			{

				if (milkBucket.Held >= 15 && cheeseForm.MilkType == MilkType.None)
				{
					milkBucket.Held -= 15;
					cheeseForm.MilkType = milkBucket.MilkType;
				}
			}
			else
			{
				from.CloseGump(typeof(MilkBucketHelpGump));
				from.SendGump(new MilkBucketHelpGump());
			}
		}
	}
	
	public class MilkBucketHelpGump : Gump
	{
		private object m_State;

		public MilkBucketHelpGump() : base(30, 30)
		{
			Closable = true;
			Dragable = true;

			AddPage(0);

			AddBackground(0, 0, 400, 300, 5054);

			Add(new GumpHtml(10, 10, 380, 280,
				"<p><b><u>Mleczny System : Pojemnik Na Mleko</u></b></p>" +
				"<p>Pomoc : </p>" +
				"<p>1 - Dwoklik na pusty pojemnik i zaznaczenie zwierzecia rozpoczyna dojenie.Kiedy rozpoczniesz dojenie nie mzoesz mieszac mleka.<br>" +
				"Nie mzoesz wydoic krow owiec i koz do jednego pojemnika.</p>" +
				"<p>2 - Pojemnik jest teraz gotow do napelnienia.<br>" +
				"Zwierze mozna wydoic podwojnym kliknieciem na nim.<br>" +
				"Dojenie meczy zwierze wiec potrzebuja troche czasu na odnowienie zapasow mleka. Zwierze potrzebuje krotkiej przerwy podczas dojenia.</p>" +
				"<p>3 - Kiedy juz pojemnik jest pelen mleko mozna przelac do formy na ser, butelek i dzbanow.<br>Rozne typy mleka daja roznego rodzaju sery. Na napelnienie formy na ser potrzeba 15 litrow.</p>" +
				"<p>Jesli to czytasz to dlatego ze:</p>" +
				"<p>- Pojemnik jest pusty a Ty prubujesz go napelnic .<br>" +
				"- Pojemnik jest pelny a Ty probujesz go napelnic<br>" +
				"- Wybrales niedozwolony cel.",
				true, true));
		}
	}
}
