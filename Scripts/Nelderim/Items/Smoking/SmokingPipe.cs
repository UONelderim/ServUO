using Server.ContextMenus;
using Server.Engines.Harvest;
using Server.Mobiles;
using Server.Network;
using Server.SkillHandlers;
using Server.Targeting;
using System;
using System.Collections.Generic;

namespace Server.Items
{
	public class SmokingPipe : BaseEarrings
	{
		private class TobaccoInfo
		{
			public delegate void OnSmokeCallback(Mobile m);

			public OnSmokeCallback OnSmoke;
			public string Description;
			public TobaccoInfo(string description, OnSmokeCallback onSmoke)
			{
				Description = description;
				OnSmoke = onSmoke;
			}
		}
		private static readonly Dictionary<Type, TobaccoInfo> TobaccoDescription = new Dictionary<Type, TobaccoInfo>
		{
			{ typeof(PlainTobacco), new TobaccoInfo("tyton zwyczajny", PlainTobacco.OnSmoke) },
			{ typeof(PlainTobaccoApple), new TobaccoInfo("tyton zwyczajny aromatyzowany jablkiem", PlainTobaccoApple.OnSmoke) },
			{ typeof(PlainTobaccoPear), new TobaccoInfo("tyton zwyczajny aromatyzowany gruszka", PlainTobaccoPear.OnSmoke) },
			{ typeof(PlainTobaccoLemon), new TobaccoInfo("tyton zwyczajny aromatyzowany cytryna", PlainTobaccoLemon.OnSmoke) },

			{ typeof(NobleTobacco), new TobaccoInfo("tyton szlachetny", NobleTobacco.OnSmoke) },
			{ typeof(NobleTobaccoApple), new TobaccoInfo("tyton szlachetny aromatyzowany jablkiem", NobleTobaccoApple.OnSmoke) },
			{ typeof(NobleTobaccoPear), new TobaccoInfo("tyton szlachetny aromatyzowany gruszka", NobleTobaccoPear.OnSmoke) },
			{ typeof(NobleTobaccoLemon), new TobaccoInfo("tyton szlachetny aromatyzowany cytryna", NobleTobaccoLemon.OnSmoke) },

			{ typeof(SwampTobacco), new TobaccoInfo("bagienne ziele", SwampTobacco.OnSmoke) },
		};

		private Type TobaccoType { get; set; }

		private int TobaccoQuantity { get; set; }

		private int TobaccoQuantityLimit => 100;

		public int UsesRemaining { get; set; }
		private bool ShowUsesRemaining { get; set; }

		private class SmokeTimer : Timer
		{
			private readonly Mobile m_Mobile;
			private readonly SmokingPipe m_Fajka;

			private int TobaccoRequired => 4;

			public SmokeTimer(SmokingPipe fajka, Mobile mobile) : base(TimeSpan.FromSeconds(1), TimeSpan.Zero)
			{
				m_Mobile = mobile;
				m_Fajka = fajka;
				Priority = TimerPriority.OneSecond;

				Start();
			}

			protected override void OnTick()
			{
				Stop();

				if (m_Fajka.TobaccoQuantity == 0)
					return;

				if (m_Fajka.TobaccoType != null && TobaccoDescription.ContainsKey(m_Fajka.TobaccoType))
				{
					TobaccoDescription[m_Fajka.TobaccoType].OnSmoke(m_Mobile);

					m_Fajka.UsesRemaining--;
					m_Fajka.TobaccoQuantity--;
					if (m_Fajka.TobaccoQuantity == 0)
						m_Fajka.TobaccoType = null;
					m_Fajka.InvalidateProperties();
				}
			}
		}

		[Constructable]
		public SmokingPipe() : base(0x17B3)
		{
			Weight = 0.1;
			Name = "Fajka do palenia";
			Light = LightType.Circle150;
			UsesRemaining = 800;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			//base.GetProperties(list);
			AddNameProperties(list);

			list.Add(1060584, UsesRemaining.ToString()); // uses remaining: ~1_val~

			if (TobaccoQuantity > 0)
			{
				if (TobaccoType != null && TobaccoDescription.ContainsKey(TobaccoType))
					list.Add(3006247, TobaccoDescription[TobaccoType].Description);  // Zawiera ~1_val~
				else
					list.Add(3006247, "jakies ziolo");  // Zawiera ~1_val~

				list.Add(3006248, TobaccoQuantity.ToString());  // Pozostalo ~1_val~ porcji tytoniu
			}
			else
				list.Add("Pusta");
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (UsesRemaining > 0)
			{
				if (TobaccoType != null && TobaccoQuantity > 0)
				{
					new SmokeTimer(this, from);
					from.SendMessage("Zaczynasz zaciagac sie dymem z fajki.");
					from.Emote("*wciaga powietrze przez fajke*");
					if (from is PlayerMobile pm)
					{
						pm.Statistics.TobaccoSmoked.Increment(TobaccoType);
					}
				}
				else
					from.SendMessage("Fajka jest pusta.");
			}
			else
			{
				from.SendMessage("Fajka jest zniszczona.");
				from.Emote("*spoglada na zatkana fajke*");
			}
		}

		public override bool OnDragDrop(Mobile from, Item item)
		{
			return Fill(from, item);
		}

		private bool Fill(Mobile from, Item item, bool checkItemOwnership = false)
		{
			if (checkItemOwnership && !item.IsChildOf(from.Backpack))
			{
				from.SendMessage("Musisz miec tyton w plecaku, aby moc nim nabic fajke.");
				return false;
			}
			if (this.RootParent != from)
			{
				from.SendMessage("Musisz miec fajke przy sobie, aby mojc ja nabic tytoniem.");
				return false;
			}

			if (item is ISmokable)
			{
				if (TobaccoType == item.GetType())
				{
					int space = TobaccoQuantityLimit - TobaccoQuantity;
					int amount = Math.Min(space, item.Amount);
					if (amount > 0)
					{
						TobaccoQuantity += amount;
						InvalidateProperties();

						item.Consume(amount);

						from.SendMessage("Nabiles fajke nowa porcja tytoniu.");

						return true;
					}
					else
						from.SendMessage("Fajka nie pomiesci juz wiecej tytoniu.");
				}
				else if (TobaccoType == null)
				{
					int amount = Math.Min(TobaccoQuantityLimit, item.Amount);

					TobaccoType = item.GetType();
					TobaccoQuantity += amount;
					InvalidateProperties();

					item.Consume(amount);

					from.SendMessage("Nabiles fajke nowa porcja tytoniu.");

					return true;
				}
				else
					from.SendMessage("Fajka jest juz nabita czyms innym. Oproznij ja wpierw.");
			}
			else
				from.SendMessage("Nie mozesz nabic tym fajki.");

			return false;
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);

			if (from.Alive && this.RootParent == from)
			{
				//PlayerMobile pm = from as PlayerMobile;
				//if (pm != null)
				//{
				if (TobaccoQuantity > 0 && TobaccoType != null)
					list.Add(new ExtractTobacco(from, this));
				else
					list.Add(new AddTobacco(from, this));
				//}
			}
		}

		private class ExtractTobacco : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private SmokingPipe m_Pipe;

			public ExtractTobacco(Mobile mobile, SmokingPipe pipe) : base(6245)
			{
				m_Mobile = mobile;
				m_Pipe = pipe;
			}

			public override void OnClick()
			{
				if (m_Pipe.TobaccoType == null || m_Pipe.TobaccoQuantity <= 0)
					return;

				Item[] tobaccos = m_Mobile.Backpack.FindItemsByType(m_Pipe.TobaccoType);
				if (tobaccos.Length > 0)
				{
					tobaccos[0].Amount += m_Pipe.TobaccoQuantity;

					m_Pipe.TobaccoType = null;
					m_Pipe.TobaccoQuantity = 0;
					m_Pipe.InvalidateProperties();
				}
				else
				{
					try
					{
						Item tobacco = Activator.CreateInstance(m_Pipe.TobaccoType) as Item;
						if (tobacco != null)
						{
							tobacco.Amount = m_Pipe.TobaccoQuantity;
							m_Mobile.Backpack.DropItem(tobacco);

							m_Pipe.TobaccoType = null;
							m_Pipe.TobaccoQuantity = 0;
							m_Pipe.InvalidateProperties();
						}
						else
							m_Mobile.SendMessage("Cos poszlo nie tak...");
					}
					catch (Exception e)
					{
					}
				}
			}
		}

		private class AddTobacco : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private SmokingPipe m_Pipe;

			public AddTobacco(Mobile mobile, SmokingPipe pipe) : base(6246)
			{
				m_Mobile = mobile;
				m_Pipe = pipe;
			}

			public override void OnClick()
			{
				if (m_Mobile != null && m_Pipe.TobaccoQuantity == 0)
					m_Mobile.Target = new TobaccoChoiceTarget(m_Pipe);
			}
		}

		class TobaccoChoiceTarget : Target
		{
			private SmokingPipe m_Pipe;

			public TobaccoChoiceTarget(SmokingPipe pipe) : base(-1, true, TargetFlags.None)
			{
				m_Pipe = pipe;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Item)
					m_Pipe.Fill(from, (Item)targeted, true);
			}
		}

		public SmokingPipe(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
			writer.Write((int)UsesRemaining);
			writer.Write((string)(TobaccoType == null ? "" : TobaccoType.FullName));
			writer.Write((int)TobaccoQuantity);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			UsesRemaining = reader.ReadInt();
			if (version >= 1)
			{
				string typeName = reader.ReadString();
				if (typeName != "")
					TobaccoType = Type.GetType(typeName);
				TobaccoQuantity = reader.ReadInt();

				if (TobaccoType == null)
					TobaccoQuantity = 0;
			}
		}
	}
}
