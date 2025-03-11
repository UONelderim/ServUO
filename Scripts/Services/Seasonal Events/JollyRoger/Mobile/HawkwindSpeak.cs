using System;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.JollyRoger
{
    public class HawkwindSpeak : BaseQuester
    {
        private MessageTimer _Timer;

        public static HawkwindSpeak Instance { get; set; }

        [Constructable]
        public HawkwindSpeak()
            : base()
        {
            Name = "Hawkwind";

            _Timer = new MessageTimer(this);
            _Timer.Start();
        }

        public override bool DisallowAllMoves => true;

        public override void InitBody()
        {
            base.InitBody();

            Body = 0x2b1;
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.AccessLevel > AccessLevel.Player && m.Hidden)
            {
                return;
            }

            if (m is PlayerMobile && m.InRange(this, 10) && _Timer == null)
            {
                _Timer = new MessageTimer(this);
                _Timer.Start();
            }
        }

        public override void OnAfterDelete()
        {
            if (_Timer != null)
                _Timer.Stop();

            _Timer = null;

            base.OnAfterDelete();
        }

        public class MessageTimer : Timer
        {
            private static readonly string[] m_Messages =
            {
	            "Kodeks Ostatecznej Madrosci dal mi informacje, ktorych potrzebuje.",
	            "Ostroznie przeniose magie zaklecia do Stonegate i tam dokoncze rytual.",
	            "Dotknij Bramy Czasu, zanim opuscisz to miejsce.",
	            "Twoj duch wzmocni zaklecie, ktore razem rzucimy.",
	            "Czesc Esencji Bramy Czasu CI Nascent pojdzie z toba.",
	            "Bedzie czescia ciebie, a zatem czescia magii tego odlamka.",
	            "Jesli wystarczajaca liczba osob zrobi to na kazdym odlamku, bede w stanie naprawic polaczenie.",
	            "Wtedy Brama Czasu bedzie wystarczajaco stabilna, aby przez nia przejsc!",
	            "Porozmawiamy ponownie, gdy to wszystko sie skonczy. Dziekuje, Legendo Nelderimu!"
            };

            private readonly HawkwindSpeak _Mobile;
            private int m_State;

            public MessageTimer(HawkwindSpeak m)
                : base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(10.0))
            {
                _Mobile = m;
            }

            protected override void OnTick()
            {
                if (m_State < m_Messages.Length)
                {
                    _Mobile.Say(m_Messages[m_State++], 946);
                }

                if (m_State == m_Messages.Length && _Mobile._Timer != null)
                {
                    _Mobile._Timer.Stop();
                    _Mobile._Timer = null;
                }
            }
        }

        public override void OnTalk(PlayerMobile player, bool contextMenu)
        {
        }

        public override bool CanTalkTo(PlayerMobile to)
        {
            return false;
        }

        public HawkwindSpeak(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            Instance = this;
        }
    }
}
