// 06.01.05 :: troyan :: lokalizacja

using System;
using Server;

namespace Arya.Jail
{
	/// <summary>
	///  Stores data about a quick jailing
	/// </summary>
	public class QuickJailEntry
	{
		private DateTime m_End;
		private Mobile m_Mobile;
		private string m_Reason = string.Empty;
		private JailType m_Jail;

		/// <summary>
		/// Gets the reason for this jailing
		/// </summary>
		public string Reason
		{
			get
			{
				if ( m_Reason == null || m_Reason.Length == 0 )
					return "Unspecified";
				else
					return m_Reason;
			}
		}

		/// <summary>
		/// Gets the end time for this jailing
		/// </summary>
		public DateTime End
		{
			get { return m_End; }
		}
		
		public JailType Type_Jail
		{
			get { return m_Jail; }
		}

		/// <summary>
		/// Gets the jailed mobile
		/// </summary>
		public Mobile Mobile
		{
			get { return m_Mobile; }
		}

		/// <summary>
		/// States whether the quick jailing has expired and the offender should be released
		/// </summary>
		public bool Expired
		{
			get { return DateTime.Now >= m_End; }
		}

		private QuickJailEntry( Mobile m, int minutes, string reason , JailType jail  )
		{
			m_Mobile = m;
			m_End = DateTime.Now + TimeSpan.FromMinutes( minutes );
			m_Jail = jail;
		}

		/// <summary>
		/// Performs a quick jail action
		/// </summary>
		/// <param name="offender">The mobile being jailed</param>
		/// <param name="staff">The GM performing the jailing</param>
		/// <param name="minutes">The length of the jailing</param>
		/// <param name="reason">The reason of the jailing</param>
		public static void Jail( Mobile offender, Mobile staff, int minutes, string reason , JailType jailtype )
		{
			if ( minutes <= 0 )
			{
				staff.SendMessage( 0x40, "Duration too short" );
				return;
			}

			if ( minutes >= 60 )
			{
				if ( JailSystem.InitJail( staff, offender ) )
					JailSystem.BeginJail( offender );

				return;
			}

			if ( ! JailSystem.CanBeJailed( offender ) )
			{
				staff.SendMessage( 0x40, "You can't jail them" );
				return;
			}

			QuickJailEntry jail = new QuickJailEntry( offender, minutes, reason , jailtype );

			JailSystem.QuickJailings.Add( jail );
			JailSystem.FinalizeJail( offender );

			string args = String.Format( "{0}\t{1}\t{2}", staff.Name, minutes, jail.Reason );
			
			offender.SendLocalizedMessage( 505615, args, 0x40 );
		}

		/// <summary>
		/// Releases the jailed mobile
		/// </summary>
		public void Release()
		{
			if ( !m_Mobile.Deleted )
			{
				
				JailSystem.SetFree( m_Mobile );
			}
		}
	}
}
