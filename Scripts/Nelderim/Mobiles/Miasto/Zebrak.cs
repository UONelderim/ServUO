#region References

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Server.Mobiles
{
	public class Zebrak : NBaseTalkingNPC
	{
		private static List<Action> _DefaultActions = new List<Action>
		{
			m => m.Say("Złota daj żebrakowi..."),
			m => m.Say("O Panocku o licu złotym, zlituj sie"),
			m => m.Say("Psia mać..."),
			m => {
				m.Say("Choć okrucha chleba... błagam");
				m.Emote("*Składa ręce błagalnie*");
			},
			m => m.Say("Mam chorą córkę..."),
			m => {
				m.Say("Wszystko mi zabrali... Wszystko...");
				m.Emote("*Szlocha*");
			},
			m => m.Say("Czy Ty przypadkiem ode mnie nie pożyczałeś pieniędzy?"),
			m => m.Say("Poratujcie w potrzebie..."),
			m => {
				m.Say("Kiedyś to sie żylo...");
				m.Emote("*Wzdycha ze smutkiem*");
			},
			m => m.Say("Ta straż ciągle tu tylko węszy."),
			m => m.Say("Za co ja dzieciaki wyżywie... Poratujcie błagam"),
			m => m.Say("Na chleb jeno..."),
			m => m.Emote("*Smarka w brudną chustę*"),
			m => {
				m.Say("Kiedyś to było...");
				m.Emote("*Wzdycha ciężko*");
			},
			m => m.Emote("*Poprawia łachmany*"),
			m => m.Say("Było sie młodym i głupim... Tak wyszło..."),
			m => {
				m.Emote("*Podnosi coś z ziemi*");
				m.Say("Ah.. jednak nie");
			},
			m => {
				m.Say("Cholipka...");
				m.Emote("*Rozgląda się powoli wzdychając*");
			},
		};
		
		private static readonly Dictionary<Race, List<Action>> _Actions = new Dictionary<Race, List<Action>>
		{
			{ Race.DefaultRace, new List<Action> { m => m.Emote("*Rozgląda się uważnie w koło*") } },
			{
				Race.NTamael, _DefaultActions.Concat(new List<Action>
				{
					m => m.Say("Wszystko co miałem dla kraju oddałem..."),
				}).ToList()
			},
			{
				Race.NJarling, _DefaultActions.Concat(new List<Action>
				{
					m => m.Say("Aż sie tu nie chce żyć... parszywe miasto"),
				}).ToList()
			},
		};

		protected override Dictionary<Race, List<Action>> NpcActions => _Actions;


		[Constructable]
		public Zebrak() : base("- Żebrak")
		{
		}

		public Zebrak(Serial serial) : base(serial)
		{
		}
		
		public override void OnGenderChanged(bool oldFemale)
		{
			base.OnGenderChanged(oldFemale);
			if (Female)
			{
				Title = "- Żebraczka";
			}
			else
			{
				Title = "- Żebrak";
			}
		}
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
