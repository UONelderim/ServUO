using Nelderim.Towns;
using Server.Nelderim;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public partial class TownCrier
    {
        public static void UpdateNews()
        {
            foreach ( TownCrier tc in Instances )
                tc.Update();
        }

        private void Update()
        {
            List<RumorRecord> news = RumorsSystem.GetRumors( this as Mobile, NewsType.News );

            if ( news != null && news.Count != 0 )
                ForceBeginAutoShout();
        }

		[CommandProperty(AccessLevel.Administrator)]
		public Towns TownAssigned
		{
			get { return TownsVendor.Get(this).TownAssigned; }
			set { TownsVendor.Get(this).TownAssigned = value; }
		}
	}
}
