using System;
using System.Collections.Generic;

// szczaw :: 15.01.2013 :: Utowrzenie Cilloc dla wygodniejszego wysylania wiadomosci z cilloca np.
// Cilloc c = new Cilloc(1007000);
// c[3].SendTo(from); // wysyla wiadomosc 10070003 do from, przewaga nad tradycyjna metoda jest taka, 
//                    // ze nie musismy sie martwic o 1070000, a takze IntelliSense ma latwiejsze zadanie,
//                    // bo nie ma wyboru pomiedzy SendGump, SendMessage, SendLocalizedMessage...
// c[3].FillWith(itemCount, cost).SendTo(from) // przypisze dwa parametry do wiadomosc, tradycyjnie trzeba by...
// from.SendLocalizedMessage(1007003, String.Format( "{0}\t{1}", itemCount, cost );
namespace Server.Helpers
{

    public class Cliloc
    {
        public readonly int Offset;
        private int _index;
        private string[] _arguments;
        public Mobile To;

        public Cliloc( int offset )
        {
            this.Offset = offset;
            _arguments = null;
        }

        public Cliloc this[int index]
        {
            get
            {
                _index = index;
                _arguments = null;

                return this;
            }
        }

        public Cliloc FillWith( params object[] arguments )
        {
            List<string> args = new List<string>();
            foreach (object o in arguments)
            {
                args.Add(o.ToString());
            }
            _arguments = args.ToArray();
            return this;
        }

        public void Send()
        {
            SendTo(this.To);
        }

        public void SendTo( Mobile to )
        {
            if(to == null)
                throw new ArgumentNullException("SendTo mialo wyslac wiadomosc do null");

            int cilloc = this.Offset + _index;
            string cillocArgs = "";

            if(_arguments != null)
                foreach(var arg in _arguments)
                    cillocArgs += arg + "\t";

            if(cillocArgs != "")
                to.SendLocalizedMessage(cilloc, cillocArgs);
            else
                to.SendLocalizedMessage(cilloc);
        }
    }
}
